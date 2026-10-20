using NewLife.Log;

namespace NewLife.JT808.Protocols;

/// <summary>SIM 黑名单管理器</summary>
/// <remarks>
/// 管理被禁止接入的设备 SIM 卡号列表。
/// 支持运行时动态增删，变更实时生效。
/// 黑名单存储在内存中，可通过 <see cref="LoadFile"/> 从文件加载持久化。
/// 
/// 使用示例：
/// <code>
/// var blacklist = new BlacklistManager();
/// blacklist.Add("13800138000");
/// 
/// if (blacklist.IsBlacklisted("13800138000"))
///     session.Close("黑名单设备");
/// </code>
/// </remarks>
public class BlacklistManager
{
    #region 属性
    private readonly HashSet<String> _blacklist = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>日志</summary>
    public ILog Log { get; set; } = Logger.Null;

    /// <summary>黑名单文件路径。为空时不持久化</summary>
    public String? FilePath { get; set; }

    /// <summary>当前黑名单数量</summary>
    public Int32 Count
    {
        get { lock (_blacklist) return _blacklist.Count; }
    }
    #endregion

    #region 核心操作
    /// <summary>添加手机号到黑名单</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否新增（已存在的返回 false）</returns>
    public Boolean Add(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return false;

        lock (_blacklist)
        {
            if (_blacklist.Add(mobile))
            {
                Log?.Info("Blacklist add: {0}", mobile);
                SaveFile();
                return true;
            }
        }
        return false;
    }

    /// <summary>从黑名单移除手机号</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否存在并移除</returns>
    public Boolean Remove(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return false;

        lock (_blacklist)
        {
            if (_blacklist.Remove(mobile))
            {
                Log?.Info("Blacklist remove: {0}", mobile);
                SaveFile();
                return true;
            }
        }
        return false;
    }

    /// <summary>判断手机号是否在黑名单中</summary>
    /// <param name="mobile">终端手机号</param>
    /// <returns>是否在黑名单中</returns>
    public Boolean IsBlacklisted(String mobile)
    {
        if (mobile.IsNullOrEmpty()) return false;

        lock (_blacklist)
        {
            return _blacklist.Contains(mobile);
        }
    }

    /// <summary>获取所有黑名单列表</summary>
    /// <returns>黑名单副本</returns>
    public String[] GetAll()
    {
        lock (_blacklist)
        {
            return [.. _blacklist];
        }
    }

    /// <summary>清空黑名单</summary>
    public void Clear()
    {
        lock (_blacklist)
        {
            _blacklist.Clear();
        }
        Log?.Info("Blacklist cleared.");
        SaveFile();
    }
    #endregion

    #region 文件持久化
    /// <summary>从文件加载黑名单</summary>
    /// <param name="filePath">文件路径，为空时使用 <see cref="FilePath"/></param>
    public void LoadFile(String? filePath = null)
    {
        filePath ??= FilePath;
        if (filePath.IsNullOrEmpty() || !File.Exists(filePath)) return;

        lock (_blacklist)
        {
            _blacklist.Clear();
            foreach (var line in File.ReadAllLines(filePath))
            {
                var mobile = line.Trim();
                if (!mobile.IsNullOrEmpty())
                    _blacklist.Add(mobile);
            }
        }
        Log?.Info("Blacklist loaded: {0} entries.", Count);
    }

    /// <summary>保存黑名单到文件</summary>
    /// <param name="filePath">文件路径，为空时使用 <see cref="FilePath"/></param>
    public void SaveFile(String? filePath = null)
    {
        filePath ??= FilePath;
        if (filePath.IsNullOrEmpty()) return;

        var dir = Path.GetDirectoryName(filePath);
        if (!dir.IsNullOrEmpty()) Directory.CreateDirectory(dir);

        lock (_blacklist)
        {
            File.WriteAllLines(filePath, _blacklist);
        }
    }
    #endregion
}
