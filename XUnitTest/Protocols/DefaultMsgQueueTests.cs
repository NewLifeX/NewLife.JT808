using System.ComponentModel;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>默认内存消息队列测试</summary>
public class DefaultMsgQueueTests
{
    [Fact]
    [DisplayName("DefaultMsgQueue_生产消费_按顺序回调")]
    public async Task ProduceAndConsume()
    {
        using var queue = new DefaultMsgQueue<String>("test-topic");
        var received = new List<(String Mobile, String Message)>();
        var done = new TaskCompletionSource<Boolean>(TaskCreationOptions.RunContinuationsAsynchronously);

        await queue.SubscribeAsync((mobile, message) =>
        {
            lock (received)
            {
                received.Add((mobile, message));
                if (received.Count >= 2) done.TrySetResult(true);
            }

            return Task.CompletedTask;
        });

        Assert.True(await queue.ProduceAsync("13880001111", "A"));
        Assert.True(await queue.ProduceAsync("13880002222", "B"));

        var completed = await Task.WhenAny(done.Task, Task.Delay(3000));
        Assert.Same(done.Task, completed);

        await queue.UnsubscribeAsync();

        Assert.Equal(2, received.Count);
        Assert.Equal("13880001111", received[0].Mobile);
        Assert.Equal("A", received[0].Message);
        Assert.Equal("13880002222", received[1].Mobile);
        Assert.Equal("B", received[1].Message);
    }

    [Fact]
    [DisplayName("DefaultMsgQueue_队列满_丢弃并返回失败")]
    public async Task Produce_DropWhenFull()
    {
        using var queue = new DefaultMsgQueue<String>("full-topic") { MaxQueueLength = 1 };

        // 未订阅时消息停留在队列中，第二条应被丢弃
        Assert.True(await queue.ProduceAsync("13880001111", "A"));
        Assert.False(await queue.ProduceAsync("13880002222", "B"));
    }

    [Fact]
    [DisplayName("DefaultMsgQueue_处理器异常_不中断后续消息")]
    public async Task HandlerError_ContinuesProcessing()
    {
        using var queue = new DefaultMsgQueue<String>("error-topic");
        var done = new TaskCompletionSource<Boolean>(TaskCreationOptions.RunContinuationsAsynchronously);
        var count = 0;

        await queue.SubscribeAsync((mobile, message) =>
        {
            if (Interlocked.Increment(ref count) == 1) throw new InvalidOperationException("boom");

            done.TrySetResult(true);
            return Task.CompletedTask;
        });

        await queue.ProduceAsync("13880001111", "A");
        await queue.ProduceAsync("13880002222", "B");

        var completed = await Task.WhenAny(done.Task, Task.Delay(3000));
        Assert.Same(done.Task, completed);

        await queue.UnsubscribeAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    [DisplayName("DefaultMsgQueue_销毁后清空队列")]
    public async Task Dispose_ClearsQueue()
    {
        var queue = new DefaultMsgQueue<String>("dispose-topic");
        Assert.True(await queue.ProduceAsync("13880001111", "A"));

        queue.Dispose();

        // 队列已清空，可继续写入但不会回调
        Assert.True(await queue.ProduceAsync("13880002222", "B"));
    }
}
