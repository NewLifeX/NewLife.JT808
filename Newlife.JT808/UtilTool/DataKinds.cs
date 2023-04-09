namespace NewLife.JT808.UtilTool
{
    /// <summary>透传消息数据类型</summary>
    public enum DataKinds : byte
    {
        /// <summary>GNSS模块详细定位数据</summary>
        GNSS = 0,

        /// <summary>道路运输证IC卡信息</summary>
        ICCard = 0x0B,

        /// <summary>串口1透传</summary>
        COM1 = 0x41,

        /// <summary>串口2透传</summary>
        COM2 = 0x42,

        /// <summary>行车记录仪</summary>
        Recorder = 0x61,

        /// <summary>文本信息</summary>
        Text = 0x62,

        /// <summary>CAN1透传</summary>
        CAN1 = 0xF1,

        /// <summary>CAN2透传</summary>
        CAN2 = 0xF2,

        /// <summary>状态查询 247 = 0xF7。外设状态信息：外设工作状态、设备报警信息</summary>
        Status = 0xF7,

        /// <summary>信息查询。外设传感器的基本信息：公司信息、 产品代码、 版本号、 外设 ID、 客户代码。</summary>
        Info = 0xF8,
    }
}