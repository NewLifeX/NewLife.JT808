using System;

namespace NewLife.JT808.UtilTool
{
    /// <summary>车辆信号状态</summary>
    [Flags]
    public enum SignalStatus : uint
    {
        /// <summary>近光灯</summary>
        近光灯 = 1 << 0,

        /// <summary>远光灯</summary>
        远光灯 = 1 << 1,

        /// <summary>右转向灯</summary>
        右转向灯 = 1 << 2,

        /// <summary>左转向灯</summary>
        左转向灯 = 1 << 3,

        /// <summary>制动</summary>
        制动 = 1 << 4,

        /// <summary>倒挡</summary>
        倒挡 = 1 << 5,

        /// <summary>雾灯</summary>
        雾灯 = 1 << 6,

        /// <summary>示廓灯</summary>
        示廓灯 = 1 << 7,

        /// <summary>喇叭</summary>
        喇叭 = 1 << 8,

        /// <summary>空调</summary>
        空调 = 1 << 9,

        /// <summary>空档</summary>
        空档 = 1 << 10,

        /// <summary>缓速器</summary>
        缓速器 = 1 << 11,

        /// <summary>ABS</summary>
        ABS = 1 << 12,

        /// <summary>加热器</summary>
        加热器 = 1 << 13,

        /// <summary>离合器</summary>
        离合器 = 1 << 14,
    }
}