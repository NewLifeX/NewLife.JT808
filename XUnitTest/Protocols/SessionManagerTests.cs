using System.ComponentModel;
using NewLife.JT808.Protocols;

namespace XUnitTest.Protocols;

/// <summary>会话管理器单元测试</summary>
public class SessionManagerTests
{
    [Fact]
    [DisplayName("会话管理_注册查找与移除")]
    public void RegisterFindRemove()
    {
        var manager = new SessionManager();
        var session = new JT808Session();

        manager.Register("13800138000", session);
        Assert.Same(session, manager.Find("13800138000"));
        Assert.True(manager.IsOnline("13800138000"));
        Assert.Single(manager.GetAll());

        manager.Remove("13800138000");
        Assert.Null(manager.Find("13800138000"));
        Assert.False(manager.IsOnline("13800138000"));
        Assert.Empty(manager.GetAll());
    }

    [Fact]
    [DisplayName("会话管理_空手机号忽略")]
    public void EmptyMobile_Ignored()
    {
        var manager = new SessionManager();

        manager.Register(null, new JT808Session());
        manager.Register("", new JT808Session());
        manager.Touch(null);
        manager.Remove(null);

        Assert.Empty(manager.GetAll());
    }

    [Fact]
    [DisplayName("会话管理_超时未续期自动失效")]
    public void Session_ExpiresWhenIdle()
    {
        var manager = new SessionManager { SessionExpire = 1 };
        manager.Register("13800138000", new JT808Session());

        Assert.NotNull(manager.Find("13800138000"));

        Thread.Sleep(1200);
        Assert.Null(manager.Find("13800138000"));
    }

    [Fact]
    [DisplayName("会话管理_续期可延长在线状态")]
    public void Touch_RefreshesExpiry()
    {
        var manager = new SessionManager { SessionExpire = 2 };
        manager.Register("13800138000", new JT808Session());

        Thread.Sleep(1100);
        manager.Touch("13800138000");
        Thread.Sleep(1100);

        // 注册后已过 2.2 秒，未续期会失效；续期后仍在有效期内
        Assert.NotNull(manager.Find("13800138000"));
    }
}
