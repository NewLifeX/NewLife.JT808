using Newlife.JT808.Protocols;

namespace Newlife.JT808.Models
{
    /// <summary>
    /// 提问下发
    /// 0x8302
    /// 2019版本已作删除
    /// </summary>
    [MessageKind(MessageKinds.提问下发)]
    public class T8302
    {
        /// <summary>
        /// 标志
        /// 提问下发标志位定义
        /// </summary>
        public Byte Flag { get; set; }

        /// <summary>问题内容长度</summary>
        public Byte IssueContentLength { get; set; }

        /// <summary>
        /// 问题
        /// 问题文本，经 GBK 编码，长度为 N
        /// </summary>
        public String Issue { get; set; }

        /// <summary>候选答案列表</summary>
        public List<Answer> Answers { get; set; }

        /// <summary>候选答案信息</summary>
        public class Answer
        {
            /// <summary>答案 ID</summary>
            public Byte Id { get; set; }

            /// <summary>
            /// 答案内容长度
            /// 答案内容字段字节长度
            /// </summary>
            public UInt16 ContentLength { get; set; }

            /// <summary>
            /// 答案内容 
            /// 答案内容，经 GBK 编码
            /// </summary>
            public String Content { get; set; }
        }
    }
}