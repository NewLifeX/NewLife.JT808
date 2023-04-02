using System;

namespace Newlife.JT808.UtilTool
{
    /// <summary>位置数据种类</summary>
    [Flags]
    public enum PositionKinds : byte
    {
        /// <summary>批量</summary>
        批量 = 0,

        /// <summary>盲区补报</summary>
        盲区补报 = 1,

        /// <summary>正常</summary>
        正常 = 2,
    }
}