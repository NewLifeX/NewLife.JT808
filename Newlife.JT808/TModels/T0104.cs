using Newlife.JT808.Protocols;
using Newlife.JT808.UtilTool;
using NewLife;

namespace Newlife.JT808.TModels;

/// <summary>查询终端参数应答</summary>
[MessageKind(MessageKinds.查询终端参数应答)]
public class T0104
{
    /// <summary>
    /// 应答流水号
    /// 查询指定终端参数的流水号
    /// </summary>
    public UInt16 Sequence { get; set; }

    /// <summary>
    /// 参数列表
    /// </summary>
    public List<TLV> Items { get; set; }

    public IDictionary<ParameterKinds, Object> GetParameters()
    {
        var dic = TLV.Convert<ParameterKinds>(Items);

        foreach (var item in dic)
        {
            if (item.Value is Byte[] buf)
            {
                var encoding = Helper.Encoding;
                dic[item.Key] = item.Key switch
                {
                    ParameterKinds.主APN or ParameterKinds.主用户名 or ParameterKinds.主密码 or ParameterKinds.主地址 or ParameterKinds.备APN or ParameterKinds.备用户名 or ParameterKinds.备密码 or ParameterKinds.备地址 or ParameterKinds.IC卡主服务器 or ParameterKinds.IC卡备服务器 => buf.ToStr(encoding).Trim('\0'),
                    ParameterKinds.从APN or ParameterKinds.从用户名 or ParameterKinds.从密码 or ParameterKinds.从地址 => buf.ToStr(encoding).Trim('\0'),
                    ParameterKinds.监控平台电话 or ParameterKinds.终端复位电话 or ParameterKinds.恢复出厂电话 or ParameterKinds.监控平台SMS号码 or ParameterKinds.文本报警SMS号码 => buf.ToStr(encoding).Trim('\0'),
                    ParameterKinds.监听电话号码 or ParameterKinds.监管平台特权短信号码 => buf.ToStr(encoding).Trim('\0'),
                    ParameterKinds.号牌 => buf.ToStr(encoding).Trim('\0'),
                    ParameterKinds.CAN总线ID => buf.ToHex(),
                    _ => buf.ToHex(),
                };
            }
            else if (item.Value is UInt32 num)
            {
                switch (item.Key)
                {
                    case ParameterKinds.报警屏蔽字:
                    case ParameterKinds.视频报警屏蔽字:
                        dic[item.Key] = num.ToString("X8");
                        break;
                }
            }
        }

        return dic;
    }
}