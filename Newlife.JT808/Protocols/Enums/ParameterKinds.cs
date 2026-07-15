namespace NewLife.JT808.Protocols;
    /// <summary>终端参数标识枚举。JT/T 808 协议定义的终端参数 ID</summary>
    public enum ParameterKinds : uint
    {
        /// <summary>心跳发送间隔（秒）</summary>
        心跳间隔 = 0x0001,
        /// <summary>TCP 连接超时（秒）</summary>
        Tcp超时 = 0x0002,
        /// <summary>TCP 重连间隔（秒）</summary>
        Tcp重传 = 0x0003,
        /// <summary>UDP 应答超时（秒）</summary>
        Udp超时 = 0x0004,
        /// <summary>UDP 重传间隔（秒）</summary>
        Udp重传 = 0x0005,
        /// <summary>SMS 应答超时（秒）</summary>
        Sms超时 = 0x0006,
        /// <summary>SMS 重传间隔（秒）</summary>
        Sms重传 = 0x0007,

        /// <summary>主服务器 APN 接入点</summary>
        主APN = 0x0010,
        /// <summary>主服务器无线通信拨号用户名</summary>
        主用户名 = 0x0011,
        /// <summary>主服务器无线通信拨号密码</summary>
        主密码 = 0x0012,
        /// <summary>主服务器地址 IP 或域名</summary>
        主地址 = 0x0013,

        /// <summary>备用服务器 APN 接入点</summary>
        备APN = 0x0014,
        /// <summary>备用服务器无线通信拨号用户名</summary>
        备用户名 = 0x0015,
        /// <summary>备用服务器无线通信拨号密码</summary>
        备密码 = 0x0016,
        /// <summary>备用服务器地址 IP 或域名</summary>
        备地址 = 0x0017,
        /// <summary>服务器 TCP 端口</summary>
        Tcp端口 = 0x0018,
        /// <summary>服务器 UDP 端口</summary>
        Udp端口 = 0x0019,

        /// <summary>IC 卡主服务器地址</summary>
        IC卡主服务器 = 0x001A,
        /// <summary>IC 卡主服务器 TCP 端口</summary>
        IC卡Tcp = 0x001B,
        /// <summary>IC 卡主服务器 UDP 端口</summary>
        IC卡Udp = 0x001C,
        /// <summary>IC 卡备用服务器地址</summary>
        IC卡备服务器 = 0x001D,

        /// <summary>0定时汇报，1定距汇报，2定时和定距汇报</summary>
        位置汇报策略 = 0x0020,
        /// <summary>0根据ACC状态，1根据登录状态和ACC状态</summary>
        位置汇报方案 = 0x0021,

        /// <summary>驾驶员未登录时位置汇报间隔（秒）</summary>
        驾驶员未登录汇报间隔 = 0x0022,

        /// <summary>从服务器 APN 接入点</summary>
        从APN = 0x0023,
        /// <summary>从服务器无线通信拨号用户名</summary>
        从用户名 = 0x0024,
        /// <summary>从服务器无线通信拨号密码</summary>
        从密码 = 0x0025,
        /// <summary>从服务器地址 IP 或域名</summary>
        从地址 = 0x0026,

        /// <summary>休眠时位置汇报间隔（秒）</summary>
        休眠汇报间隔 = 0x0027,
        /// <summary>紧急报警时位置汇报间隔（秒）</summary>
        报警汇报间隔 = 0x0028,
        /// <summary>缺省位置汇报间隔（秒）</summary>
        缺省汇报间隔 = 0x0029,

        /// <summary>缺省位置汇报距离（米）</summary>
        缺省汇报距离 = 0x002C,
        /// <summary>驾驶员未登录时汇报距离（米）</summary>
        驾驶员未登录汇报距离 = 0x002D,
        /// <summary>休眠时汇报距离（米）</summary>
        休眠汇报距离 = 0x002E,
        /// <summary>紧急报警时汇报距离（米）</summary>
        报警汇报距离 = 0x002F,

        /// <summary>拐点补传角度（度）</summary>
        拐点补传角度 = 0x0030,
        /// <summary>电子围栏半径（米）</summary>
        电子围栏半径 = 0x0031,
        /// <summary>违规行驶时段 HHMM-HHMM</summary>
        违规行驶时段 = 0x0032,

        /// <summary>监控平台电话号码</summary>
        监控平台电话 = 0x0040,
        /// <summary>终端复位电话号码</summary>
        终端复位电话 = 0x0041,
        /// <summary>恢复出厂设置电话号码</summary>
        恢复出厂电话 = 0x0042,
        /// <summary>监控平台 SMS 号码</summary>
        监控平台SMS号码 = 0x0043,
        /// <summary>文本报警 SMS 号码</summary>
        文本报警SMS号码 = 0x0044,
        /// <summary>终端电话接听策略 0-自动接听 1-手动接听</summary>
        终端接听策略 = 0x0045,
        /// <summary>每次最长通话时间（秒）</summary>
        每次最长通话 = 0x0046,
        /// <summary>当月最长通话时间（秒）</summary>
        当月最长通话 = 0x0047,
        /// <summary>监听电话号码</summary>
        监听电话号码 = 0x0048,
        /// <summary>监管平台特权短信号码</summary>
        监管平台特权短信号码 = 0x0049,

        /// <summary>报警屏蔽字。按位屏蔽报警类型</summary>
        报警屏蔽字 = 0x0050,
        /// <summary>报警 SMS 发送开关 0-关 1-开</summary>
        报警SMS开关 = 0x0051,
        /// <summary>报警拍照开关 0-关 1-开</summary>
        报警拍照开关 = 0x0052,
        /// <summary>报警拍照存储间隔（秒）</summary>
        报警拍照存储 = 0x0053,
        /// <summary>关键标志位</summary>
        关键标志 = 0x0054,

        /// <summary>最高速度（km/h）</summary>
        最高速度 = 0x0055,
        /// <summary>超速持续时间（秒）</summary>
        超速持续时间 = 0x0056,
        /// <summary>连续驾驶时间（秒）</summary>
        连续驾驶时间 = 0x0057,
        /// <summary>当天累计驾驶时间（秒）</summary>
        当天累计驾驶时间 = 0x0058,
        /// <summary>最小休息时间（秒）</summary>
        最小休息时间 = 0x0059,
        /// <summary>最长停车时间（秒）</summary>
        最长停车时间 = 0x005A,
        /// <summary>超速预警差值（km/h）</summary>
        超速预警差值 = 0x005B,
        /// <summary>疲劳驾驶预警差值（秒）</summary>
        疲劳预警差值 = 0x005C,
        /// <summary>碰撞报警参数</summary>
        碰撞报警参数 = 0x005D,
        侧翻报警参数 = 0x005E,

        定时拍照控制 = 0x0064,
        定距拍照控制 = 0x0065,

        图像质量 = 0x0070,
        亮度 = 0x0071,
        对比度 = 0x0072,
        饱和度 = 0x0073,
        色度 = 0x0074,

        #region JT1078
        音视频参数 = 0x0075,
        音视频通道列表 = 0x0076,
        单独视频通道 = 0x0077,
        特殊报警录像 = 0x0079,
        视频报警屏蔽字 = 0x007A,
        图像分析报警 = 0x007B,
        终端休眠唤醒模式 = 0x007C,
        #endregion

        里程 = 0x0080,
        省份 = 0x0081,
        城市 = 0x0082,
        号牌 = 0x0083,
        颜色 = 0x0084,

        GNSS模式 = 0x0090,
        GNSS波特率 = 0x0091,
        GNSS输出频率 = 0x0092,
        GNSS采集频率 = 0x0093,
        GNSS上传方式 = 0x0094,
        GNSS上传设置 = 0x0095,

        CAN1采集间隔 = 0x0100,
        CAN1上传间隔 = 0x0101,
        CAN2采集间隔 = 0x0102,
        CAN2上传间隔 = 0x0103,
        CAN总线ID = 0x0110,
    }