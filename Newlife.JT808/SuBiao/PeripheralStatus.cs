namespace Newlife.JT808.SuBiao
{
    /// <summary>外设状态</summary>
    public class PeripheralStatus
    {
        /// <summary>工作状态</summary>
        public PeripheralWorkStatus Status { get; set; }

        /// <summary>告警状态</summary>
        public PeripheralAlarmStatus Alarm { get; set; }
    }

    /// <summary>外设工作状态</summary>
    public enum PeripheralWorkStatus : Byte
    {
        /// <summary>正常工作</summary>
        正常工作 = 0x01,

        /// <summary>待机状态</summary>
        待机状态 = 0x02,

        /// <summary>升级维护</summary>
        升级维护 = 0x03,

        /// <summary>设备异常</summary>
        设备异常 = 0x04,

        /// <summary>断开连接</summary>
        断开连接 = 0x10,
    }

    /// <summary>外设报警状态</summary>
    [Flags]
    public enum PeripheralAlarmStatus : UInt32
    {
        /// <summary>摄像头异常</summary>
        摄像头 = 1 << 0,

        /// <summary>主存储器异常</summary>
        主存储器 = 1 << 1,

        /// <summary>辅存储器异常</summary>
        辅存储器 = 1 << 2,

        /// <summary>红外补光异常</summary>
        红外补光 = 1 << 3,

        /// <summary>扬声器异常</summary>
        扬声器 = 1 << 4,

        /// <summary>电池异常</summary>
        电池 = 1 << 5,

        /// <summary>Keep6异常</summary>
        Keep6 = 1 << 6,

        /// <summary>Keep7异常</summary>
        Keep7 = 1 << 7,

        /// <summary>Keep8异常</summary>
        Keep8 = 1 << 8,

        /// <summary>Keep9异常</summary>
        Keep9 = 1 << 9,

        /// <summary>通信模块异常</summary>
        通信模块 = 1 << 10,

        /// <summary>定位模块异常</summary>
        定位模块 = 1 << 11,

        /// <summary>保留</summary>
        Keep12 = 1 << 12,

        /// <summary>保留</summary>
        Keep13 = 1 << 13,

        /// <summary>保留</summary>
        Keep14 = 1 << 14,

        /// <summary>保留</summary>
        Keep15 = 1 << 15,
    }
}
