using System.ComponentModel;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>SIM 黑名单管理器单元测试</summary>
public class BlacklistManagerTests
{
    [Fact]
    [DisplayName("黑名单_增删查清空")]
    public void AddRemoveQueryClear()
    {
        var manager = new BlacklistManager();

        Assert.True(manager.Add("13800138000"));
        Assert.False(manager.Add("13800138000"));
        Assert.True(manager.IsBlacklisted("13800138000"));
        Assert.False(manager.IsBlacklisted("13900139000"));
        Assert.Equal(1, manager.Count);

        Assert.True(manager.Remove("13800138000"));
        Assert.False(manager.Remove("13800138000"));
        Assert.False(manager.IsBlacklisted("13800138000"));

        manager.Add("1");
        manager.Add("2");
        manager.Clear();
        Assert.Equal(0, manager.Count);
        Assert.Empty(manager.GetAll());
    }

    [Fact]
    [DisplayName("黑名单_文件持久化往返")]
    public void FilePersistence()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"jt808_bl_{Guid.NewGuid():N}");
        Directory.CreateDirectory(dir);
        try
        {
            var file = Path.Combine(dir, "blacklist.txt");

            var manager = new BlacklistManager { FilePath = file };
            Assert.True(manager.Add("13800138000"));
            Assert.True(File.Exists(file));

            var manager2 = new BlacklistManager { FilePath = file };
            manager2.LoadFile();
            Assert.True(manager2.IsBlacklisted("13800138000"));
            Assert.Equal(1, manager2.Count);

            manager2.Remove("13800138000");
            var manager3 = new BlacklistManager { FilePath = file };
            manager3.LoadFile();
            Assert.Equal(0, manager3.Count);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    [DisplayName("黑名单_空手机号忽略")]
    public void EmptyMobile_Ignored()
    {
        var manager = new BlacklistManager();
        Assert.False(manager.Add(null));
        Assert.False(manager.Add(""));
        Assert.False(manager.Remove(null));
        Assert.False(manager.IsBlacklisted(null));
        Assert.Equal(0, manager.Count);
    }
}
