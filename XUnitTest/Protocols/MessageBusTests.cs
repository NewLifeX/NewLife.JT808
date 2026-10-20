using System.Collections.Concurrent;
using System.ComponentModel;
using NewLife.Caching;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>消息总线单元测试</summary>
public class MessageBusTests
{
    private static async Task<Boolean> WaitAsync(Func<Boolean> condition, Int32 timeout = 3000)
    {
        var end = DateTime.Now.AddMilliseconds(timeout);
        while (DateTime.Now < end)
        {
            if (condition()) return true;
            await Task.Delay(50);
        }

        return condition();
    }

    /// <summary>以内存队列模拟 Redis 队列服务，验证总线契约</summary>
    private sealed class FakeCacheProvider : ICacheProvider
    {
        private readonly ConcurrentDictionary<String, Object> _queues = new(StringComparer.Ordinal);

        public ICache Cache { get; set; } = new MemoryCache();

        public ICache InnerCache
        {
            get => Cache;
            set => Cache = value;
        }

        public IProducerConsumer<T> GetQueue<T>(String topic, String? group = null) =>
            (IProducerConsumer<T>)_queues.GetOrAdd(topic + "|" + group, _ => new FakeQueue<T>());

        public IProducerConsumer<T> GetInnerQueue<T>(String topic) => GetQueue<T>(topic);

        public IDisposable AcquireLock(String lockKey, Int32 msTimeout) => null!;
    }

    /// <summary>伪造队列：内存存放，按需阻塞等待</summary>
    private sealed class FakeQueue<T> : IProducerConsumer<T>
    {
        private readonly ConcurrentQueue<T> _queue = new();

        public Int32 Count => _queue.Count;

        public Boolean IsEmpty => _queue.IsEmpty;

        public Int32 MaxLength { get; set; }

        public Int32 Add(params T[] values)
        {
            foreach (var value in values) _queue.Enqueue(value);
            return values.Length;
        }

        public IEnumerable<T> Take(Int32 count = 1)
        {
            for (var i = 0; i < count && _queue.TryDequeue(out var value); i++)
            {
                yield return value;
            }
        }

        public T? TakeOne(Int32 timeout = 0)
        {
            // 与接口约定一致：超时单位为秒，0 表示一直等待
            var end = timeout > 0 ? DateTime.Now.AddSeconds(timeout) : DateTime.Now.AddSeconds(10);
            while (DateTime.Now < end)
            {
                if (_queue.TryDequeue(out var value)) return value;
                Thread.Sleep(20);
            }

            return default;
        }

        public Task<T?> TakeOneAsync(Int32 timeout) => TakeOneAsync(timeout, CancellationToken.None);

        public Task<T?> TakeOneAsync(Int32 timeout, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(TakeOne(timeout));
        }

        public Int32 Acknowledge(params String[] keys) => 0;
    }

    [Fact]
    [DisplayName("内存总线_生产消费往返")]
    public async Task MemoryBus_ProducerConsumerRoundtrip()
    {
        var bus = new MemoryMessageBus();
        var producer = bus.CreateProducer<String>("Roundtrip");

        Assert.True(await producer.ProduceAsync("13800138000", "a"));
        await producer.ProduceAsync("13800138000", "b");

        var received = new List<String>();
        var consumer = bus.CreateConsumer<String>("Roundtrip");
        await consumer.SubscribeAsync((mobile, message) =>
        {
            lock (received) received.Add(message);
            return Task.CompletedTask;
        });

        Assert.True(await WaitAsync(() => { lock (received) { return received.Count >= 2; } }));
        Assert.Equal(new[] { "a", "b" }, received);

        await consumer.UnsubscribeAsync();
    }

    [Fact]
    [DisplayName("内存总线_主题隔离")]
    public async Task MemoryBus_TopicsAreIsolated()
    {
        var bus = new MemoryMessageBus();
        var received = new List<String>();

        var consumer = bus.CreateConsumer<String>("TopicA");
        await consumer.SubscribeAsync((mobile, message) =>
        {
            lock (received) received.Add(message);
            return Task.CompletedTask;
        });

        await bus.CreateProducer<String>("TopicB").ProduceAsync("", "other");
        await Task.Delay(400);
        Assert.Empty(received);

        await bus.CreateProducer<String>("TopicA").ProduceAsync("", "mine");
        Assert.True(await WaitAsync(() => { lock (received) { return received.Count == 1; } }));
        Assert.Equal("mine", received[0]);

        await consumer.UnsubscribeAsync();
    }

    [Fact]
    [DisplayName("内存总线_上下线会话事件")]
    public async Task MemoryBus_SessionEvents()
    {
        var bus = new MemoryMessageBus();
        var online = new List<String>();
        var offline = new List<String>();

        var consumer = bus.CreateSessionConsumer();
        Assert.NotNull(consumer);
        await consumer.SubscribeAsync(
            mobile => { lock (online) online.Add(mobile); return Task.CompletedTask; },
            mobile => { lock (offline) offline.Add(mobile); return Task.CompletedTask; });

        var producer = bus.CreateSessionProducer();
        Assert.NotNull(producer);
        await producer.ProduceOnlineAsync("13800138000");
        await producer.ProduceOfflineAsync("13800138000");

        Assert.True(await WaitAsync(() =>
        {
            lock (online) lock (offline) return online.Count == 1 && offline.Count == 1;
        }));
        Assert.Equal("13800138000", online[0]);
        Assert.Equal("13800138000", offline[0]);

        await consumer.UnsubscribeAsync();
    }

    [Fact]
    [DisplayName("内存总线_不支持外部下行")]
    public void MemoryBus_HasNoDownHandler()
    {
        var bus = new MemoryMessageBus();
        Assert.Null(bus.CreateDownHandler());
        Assert.Equal(MessagingProviders.Memory, bus.Provider);
    }

    [Fact]
    [DisplayName("Redis总线_生产消费往返")]
    public async Task RedisBus_ProducerConsumerRoundtrip()
    {
        // 用内存缓存替代 Redis 服务，验证生产/消费契约
        var provider = new FakeCacheProvider();
        var bus = new RedisMessageBus(provider) { Group = "UnitTest" };

        Assert.Equal(MessagingProviders.Redis, bus.Provider);
        Assert.Null(bus.CreateDownHandler());

        var received = new List<String>();
        var consumer = bus.CreateConsumer<String>("RedisRoundtrip");
        await consumer.SubscribeAsync((mobile, message) =>
        {
            lock (received) received.Add(message);
            return Task.CompletedTask;
        });

        var producer = bus.CreateProducer<String>("RedisRoundtrip");
        var ok = await producer.ProduceAsync("13800138000", "hello");

        Assert.True(await WaitAsync(() => { lock (received) { return received.Count >= 1; } }));
        Assert.Equal("hello", received[0]);
        Assert.True(ok);

        await consumer.UnsubscribeAsync();
    }

    [Fact]
    [DisplayName("Redis总线_上下线会话事件")]
    public async Task RedisBus_SessionEvents()
    {
        var provider = new FakeCacheProvider();
        var bus = new RedisMessageBus(provider) { Group = "UnitTest", SessionTopic = "SessionUnitTest" };

        var online = new List<String>();
        var consumer = bus.CreateSessionConsumer();
        Assert.NotNull(consumer);
        await consumer.SubscribeAsync(
            mobile => { lock (online) online.Add(mobile); return Task.CompletedTask; },
            mobile => Task.CompletedTask);

        var producer = bus.CreateSessionProducer();
        Assert.NotNull(producer);
        await producer.ProduceOnlineAsync("13900139000");

        Assert.True(await WaitAsync(() => { lock (online) { return online.Count == 1; } }));
        Assert.Equal("13900139000", online[0]);

        await consumer.UnsubscribeAsync();
    }

    [Fact]
    [DisplayName("消息总线_消费取消后不再处理")]
    public async Task Consumer_StopsAfterUnsubscribe()
    {
        var bus = new MemoryMessageBus();
        var count = 0;

        var consumer = bus.CreateConsumer<String>("Cancel");
        await consumer.SubscribeAsync((mobile, message) =>
        {
            Interlocked.Increment(ref count);
            return Task.CompletedTask;
        });
        await consumer.UnsubscribeAsync();

        await bus.CreateProducer<String>("Cancel").ProduceAsync("", "ignored");
        await Task.Delay(400);

        Assert.Equal(0, count);
    }

    [Fact]
    [DisplayName("消息总线_空主题拒绝创建队列")]
    public void MemoryBus_EmptyTopic_Throws()
    {
        var bus = new MemoryMessageBus();
        Assert.Throws<ArgumentException>(() => bus.GetQueue<String>(""));
    }
}
