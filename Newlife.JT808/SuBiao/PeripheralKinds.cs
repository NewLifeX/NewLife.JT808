namespace NewLife.JT808.SuBiao
{
    /// <summary>外设类型</summary>
    public enum PeripheralKinds : Byte
    {
        /// <summary>视频相关报警。32位</summary>
        VideoAlarm = 0x14,

        /// <summary>视频信号丢失。32位，表示不同视频通道</summary>
        VideoLost = 0x15,

        /// <summary>视频信号阻挡。32位，表示不同视频通道</summary>
        VideoBlock = 0x16,

        /// <summary>存储器故障。16位，12位表示不同存储，4位表示灾备</summary>
        StorageError = 0x17,

        /// <summary>异常驾驶行为</summary>
        AbnormalDrive = 0x18,

        /// <summary>主动安全</summary>
        ADAS = 0x64,

        /// <summary>驾驶状态</summary>
        DSM = 0x65,

        /// <summary>胎压监测</summary>
        TPMS = 0x66,

        /// <summary>盲区监测</summary>
        BSD = 0x67,

        /// <summary>安装异常</summary>
        Install = 0xF1,

        /// <summary>算法异常</summary>
        Algorithm = 0xF2,
    }
}