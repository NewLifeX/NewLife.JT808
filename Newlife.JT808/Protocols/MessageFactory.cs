using System.Collections.Concurrent;
using System.Reflection;
using NewLife.Data;
using NewLife.JT808.Protocols;
using NewLife.Reflection;
using NewLife.Data;
using NewLife.Serialization;

namespace NewLife.JT808.Protocols
{
    /// <summary>消息工厂</summary>
    public class MessageFactory
    {
        #region 静态
        /// <summary>静态实例</summary>
        public static MessageFactory Instance { get; set; } = new();
        #endregion

        #region 属性
        ConcurrentDictionary<MessageKinds, Type> _map1 = new();
        ConcurrentDictionary<Type, MessageKinds> _map2 = new();

        /// <summary>类型映射</summary>
        public IDictionary<MessageKinds, Type> Maps => _map1;

        /// <summary>类型映射</summary>
        public IDictionary<Type, MessageKinds> Types => _map2;
        #endregion

        #region 方法
        /// <summary>初始化，加载内部</summary>
        public void Init()
        {
            foreach (var item in GetType().Assembly.GetTypes())
            {
                //if (item.Namespace == ns)
                {
                    var att = item.GetCustomAttribute<MessageKindAttribute>();
                    if (att != null) Register(att.Kind, item);
                }
            }
        }

        /// <summary>注册新类型</summary>
        /// <param name="kind"></param>
        /// <param name="type"></param>
        public void Register(MessageKinds kind, Type type)
        {
            _map1[kind] = type;
            _map2[type] = kind;
        }

        /// <summary>注册新类型</summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>()
        {
            var type = typeof(T);
            var att = type.GetCustomAttribute<MessageKindAttribute>();
            if (att != null) Register(att.Kind, type);
        }

        /// <summary>从指定程序集扫描并注册所有带有[MessageKind]特性的类型</summary>
        /// <param name="assembly">要扫描的程序集</param>
        public void Register(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var att = type.GetCustomAttribute<MessageKindAttribute>();
                if (att != null) Register(att.Kind, type);
            }
        }

        /// <summary>注册自定义消息类型映射（可覆盖已有映射）</summary>
        /// <typeparam name="T">消息体类型</typeparam>
        public void SetMap<T>()
        {
            var type = typeof(T);
            var att = type.GetCustomAttribute<MessageKindAttribute>();
            if (att != null) _map1[att.Kind] = type;
        }

        /// <summary>注册自定义消息类型映射（可覆盖已有映射）</summary>
        /// <param name="kind">消息类型</param>
        /// <param name="type">消息体类型</param>
        public void SetMap(MessageKinds kind, Type type)
        {
            _map1[kind] = type;
            _map2[type] = kind;
        }

        /// <summary>创建指定类型的模型对象</summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public object Create(MessageKinds kind)
        {
            if (!_map1.TryGetValue(kind, out var type)) return null;

            return type.CreateInstance();
        }

        /// <summary>解析消息包，得到业务模型</summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public object Parse(JTMessage message)
        {
            var msg = Create(message.Kind);
            if (msg == null) return null;

            var pk = message.Payload as IPacket;
            if (pk != null && pk.Total > 0)
            {
                var ms = pk.GetStream();
                if (msg is IAccessor accessor)
                    accessor.Read(ms, message);
                else
                    Helper.CreateReader(ms, message).TryRead(null, ref msg);

#if DEBUG
                if (ms.Position < ms.Length) throw new Exception($"解析未完成，可用[{ms.Length}]，实际[{ms.Position}]");
#endif
            }

            return msg;
        }
        #endregion

        #region 辅助
        static Array _vs;
        /// <summary>获取类型名</summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static string GetKindName(MessageKinds kind)
        {
            if (_vs == null) _vs = Enum.GetValues(typeof(MessageKinds));

            return Array.IndexOf(_vs, kind) >= 0 ? kind + "" : ((int)kind).ToString("X4");
        }

        /// <summary>是否响应消息</summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsReply(MessageKinds kind)
        {
            return kind switch
            {
                MessageKinds.终端通用应答 or
                MessageKinds.平台通用应答 or
                MessageKinds.终端注册应答 or
                MessageKinds.查询服务器时间应答 => true,

                MessageKinds.补传分包请求 => false,

                MessageKinds.设置终端参数 or
                MessageKinds.查询终端参数 or
                MessageKinds.终端控制 or
                MessageKinds.查询指定终端参数 => false,
                MessageKinds.查询终端参数应答 => true,

                MessageKinds.查询终端属性 => false,
                MessageKinds.查询终端属性应答 => true,

                MessageKinds.位置信息汇报 => false,
                MessageKinds.位置信息查询 => false,
                MessageKinds.位置信息查询应答 => true,

                MessageKinds.临时位置跟踪控制 => false,
                MessageKinds.人工确认报警消息 => false,
                MessageKinds.链路检测 => false,

                MessageKinds.文本信息下发 => false,
                MessageKinds.文本信息下发响应 => true,

                MessageKinds.电话回拨 => false,

                MessageKinds.车辆控制 => false,
                MessageKinds.车辆控制应答 => true,

                MessageKinds.行驶记录数据采集命令 => false,
                MessageKinds.行驶记录数据上传 => false,
                MessageKinds.行驶记录参数下传命令 => false,
                MessageKinds.电子运单上报 => false,
                MessageKinds.驾驶员上报 => false,
                MessageKinds.上报驾驶员请求 => false,
                MessageKinds.定位数据批量上传 => false,
                MessageKinds.CAN总线数据上传 => false,

                MessageKinds.多媒体事件信息上传 => false,
                MessageKinds.多媒体数据上传 => false,
                MessageKinds.多媒体数据上传应答 => true,

                MessageKinds.存储多媒体数据检索 => false,
                MessageKinds.存储多媒体数据检索应答 => true,
                MessageKinds.存储多媒体数据上传 => false,

                MessageKinds.报警附件上传 => false,
                MessageKinds.报警附件信息 => false,
                MessageKinds.文件信息上传 => false,
                MessageKinds.文件上传完成 => false,
                MessageKinds.文件上传完成应答 => true,

                MessageKinds.音视频传输请求 => false,
                MessageKinds.音视频传输控制 => false,

                _ => ((int)kind & 0xF000) == 0x8000,
            };
        }
        #endregion
    }
}