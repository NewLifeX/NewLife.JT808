namespace Newlife.JT808.UtilTool
{
    /// <summary>电话本联系人标志</summary>
    public enum ContactType : byte
    {
        /// <summary>呼入</summary>
        呼入 = 1,

        /// <summary>呼出</summary>
        呼出 = 2,

        /// <summary>呼入_呼出</summary>
        呼入_呼出 = 3
    }

    /// <summary>电话本联系人项数据</summary>
    public struct ContactProperty
    {
        /// <summary>标志 1：呼入；2：呼出；3：呼入/呼出</summary>
        public ContactType Type { get; set; }

        /// <summary>号码长度</summary>
        public byte PhoneNumberLength { get; set; }

        /// <summary>电话号码</summary>
        public string PhoneNumber { get; set; }

        /// <summary>联系人长度</summary>
        public byte ContactLength { get; set; }

        /// <summary>联系人 经 GBK 编码</summary>
        public string Contact { get; set; }
    }
}