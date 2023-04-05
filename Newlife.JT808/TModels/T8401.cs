using Newlife.JT808.Protocols;
using Newlife.JT808.UtilTool;

namespace NewLife.JT808.Models;

/// <summary>
/// 设置电话本 设置类型
/// </summary>
public enum SetTelephoneBook : byte
{
    /// <summary>
    /// 删除终端上所有存储的联系人
    /// </summary>
    删除终端上所有存储的联系人 = 0,
    /// <summary>
    /// 更新电话本_删除终端中已有全部联系人并追加消息中的联系人
    /// </summary>
    更新电话本_删除终端中已有全部联系人并追加消息中的联系人 = 1,
    /// <summary>
    /// 追加电话本
    /// </summary>
    追加电话本 = 2,
    /// <summary>
    /// 修改电话本_以联系人为索引
    /// </summary>
    修改电话本_以联系人为索引 = 3
}

/// <summary>
/// 设置电话本
/// </summary>
[MessageKind(MessageKinds.设置电话本)]
public class T8401
{
    /// <summary>
    /// 设置类型
    /// </summary>
    public SetTelephoneBook SettingTelephoneBook { get; set; }

    /// <summary>
    /// 联系人总数
    /// </summary>
    public Byte ContactCount { get; set; }

    /// <summary>
    /// 联系人项
    /// </summary>
    public IList<ContactProperty> JT808ContactProperties { get; set; }
}