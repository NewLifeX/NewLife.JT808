using NewLife.JT808.Protocols;

namespace NewLife.JT808.Models
{
    /// <summary>终端升级结果通知</summary>
    [MessageKind(MessageKinds.终端升级结果通知)]
    public class T0108
    {
        /// <summary>
        /// 升级类型
        /// 0：终端，12：道路运输证 IC 卡读卡器，52：北斗卫星定位模块
        /// </summary>
        public Byte Type { get; set; }

        /// <summary>
        /// 升级结果
        /// 0：成功，1：失败，2：取消
        /// </summary>
        public Byte Result { get; set; }
    }
}