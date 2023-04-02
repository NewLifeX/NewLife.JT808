namespace Newlife.JT808.UtilTool
{
    /// <summary>多媒体属性</summary>
    public struct MediaSearchProperty
    {
        /// <summary>
        /// 标志 1：呼入；2：呼出；3：呼入/呼出
        /// </summary>
        public ContactType Type { get; set; }

        /// <summary>
        /// 号码长度
        /// </summary>
        public byte PhoneNumberLength { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 联系人长度
        /// </summary>
        public byte ContactLength { get; set; }

        /// <summary>
        /// 联系人 经 GBK 编码
        /// </summary>
        public string Contact { get; set; }
    }
}