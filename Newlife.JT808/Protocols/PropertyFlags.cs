using System;

namespace Newlife.JT808.Protocols
{
    /// <summary>属性标记</summary>
    [Flags]
    public enum PropertyFlags : ushort
    {
        /// <summary>加密</summary>
        Encrypt = 1 << 10,

        /// <summary>分包</summary>
        Package = 1 << 13,

        /// <summary>版本</summary>
        Version = 1 << 14,
    }
}