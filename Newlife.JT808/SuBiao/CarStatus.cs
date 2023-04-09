namespace NewLife.JT808.SuBiao
{
    /// <summary>车辆状态</summary>
    [Flags]
    public enum CarStatus : UInt16
    {
        /// <summary>ACC</summary>
        ACC = 1 << 0,

        /// <summary>左转向</summary>
        左转向 = 1 << 1,

        /// <summary>右转向</summary>
        右转向 = 1 << 2,

        /// <summary>雨刷器</summary>
        雨刷器 = 1 << 3,

        /// <summary>制动</summary>
        制动 = 1 << 4,

        /// <summary>插卡</summary>
        插卡 = 1 << 5,

        /// <summary>定位</summary>
        定位 = 1 << 10,
    }
}