# NewLife.JT808 版本更新记录

## v1.1.2026.1024 (2026-10-24)

### 协议编解码
- **位置查询应答**：新增 0x0201 位置信息查询应答消息体
- **围栏与路线**：补齐 0x8600/0x8602/0x8604/0x8606 设置消息的行标字节编解码
- **消息体建模修正**：多媒体检索、拍摄应答等消息体字段对齐标准
- **边界缺陷修复**：KLV 截断、TLV 越界、分包字段顺序与 1 基序号、T0801/T8401/T1FC4 编解码、T0705 时间格式、AlarmDevice 空值
- **帧读写加固**：转义帧长度计算、帧边界校验、消息工厂并发初始化
- **苏标/记录仪**：附件帧与行车记录仪数据块定位对齐

### 集群与消息总线
- **集群共享状态**：管理 API 与网关改为基于 Redis 共享在线会话、节点心跳与黑名单，指令路由到终端所在节点
- **消息总线**：消息队列抽象接入 `IMessageBus`，外部 RocketMQ/Kafka 通过 `JT808:Messaging` 配置开关启用
- **适配器分层**：RocketMQ/Kafka 适配器迁至 `JT808.Server`，不随 NuGet 包发布
- **黑名单**：设备强制断开并限制自动重连次数

### NativeAOT 与多语言
- **NativeAOT**：新增 `net8.0` 目标与 `IsAotCompatible`，`AotSmoke` 16 场景覆盖帧编解码、消息工厂、TLV/KLV、多语言与 JSON
- **运行时本地化**：11 种语言、44 条词条；程序集标题/描述与 NuGet 描述本地化
- **日志与异常翻译**：经 `L.T`/`Logger.FormatMessage` 按 `CurrentUICulture` 翻译，英文源文兜底
- **AOT CI**：win-x64/linux-x64/linux-arm64 原生冒烟与 IL 告警门禁

### 测试
- **协议全覆盖**：新增 `ProtocolCoverageTests` 遍历全部消息体往返，全套 172 个用例
- **多运行时**：net8.0/net9.0/net10.0 三运行时全部通过
- **补充用例**：内存队列、粤标透传、消息工厂、终端注册与鉴权版本字段

### 工程与发布
- **解决方案**：迁移 `slnx` 格式，测试 CI 覆盖 Windows/Linux 双平台
- **发布链路**：改为 Ubuntu runner，同时推送 NuGet.org 与 GitHub Packages，tag 发布时创建 GitHub Release 并附加 `nupkg/snupkg`
- **质量门禁**：新增 `Tools/Scripts/check-package.ps1` 包内容校验与 README/词表校验脚本
- **文档**：README 徽章与语言切换行按 Core 规范生成，11 语种全文翻译，生态版图与团队介绍对齐 NewLife.Core

---

## v1.1.2026.0713 (2026-07-13)

### Web API 管理接口
- **统一下发指令**：POST /api/jt808/command 向终端下发任意协议消息体
- **会话管理**：GET /api/jt808/sessions 在线终端列表及详情，DELETE 强制移除会话
- **黑名单管理**：BlacklistManager 文件持久化 + 运行时热更新，HTTP API 增删查
- **Token 鉴权**：JT808Auth Bearer Token 鉴权，环境变量配置，可选启用

### 消息队列抽象
- **消息生产/消费接口**：IMsgProducer/IMsgConsumer 解耦消息处理与业务逻辑
- **会话通知接口**：ISessionProducer/ISessionConsumer 在线/离线状态通知
- **下行消息处理接口**：IDownMessageHandler 外部指令统一处理
- **默认内存实现**：DefaultMsgQueue&lt;T&gt; 基于 Channel 的内存队列

### 跨服务器指令下发
- **ICommandBus 接口**：统一本地/远程指令下发抽象，支持 Stardust 事件总线广播
- **SessionManager 增强**：实现 ICommandBus，支持 CommandReceived 事件
- **Server 集成**：StarCommandBus 本地优先执行 + 远程集群广播

### 客户端增强
- **自动重连**：AutoReconnect 可配置重连次数和间隔，断线自动恢复
- **心跳管理**：HeartbeatInterval 内置心跳线程，可独立配置
- **统计计数器**：SentCount/ReceivedCount 发送接收实时统计

### 重构与优化
- **协议类结构重组**：提取 Interfaces/Attributes/Enums 子目录，统一命名空间风格

### 测试与质量
- **39/39 测试通过**：全部单元测试通过，覆盖核心协议、序列化、消息体

---

## v1.0.2026.0712 (2026-07-12)

### 初始版本
- **核心协议**：完整实现 JT/T 808-2011/2013/2019 消息帧解析，支持 0x7E 帧边界、转义处理、异或校验
- **消息体模型**：覆盖 70+ 标准消息体（T0001~T8A00），支持双模式序列化（反射自动 + IAccessor 手动）
- **三版本兼容**：通过 Version 字段 + [JT2011]/[JT2019] 特性实现三版本字段自动适配
- **TLV/KLV**：通用 TLV（Type-Length-Value）和 KLV（Kind-Length-Value）编解码
- **苏标扩展**：支持 ADAS/DSM/TPMS/BSD 主动安全报警信息，外设状态/信息解析、报警附件协议
- **BCD/GBK 编码**：BCD 字符串/时间自定义序列化特性，GBK 中文字符串编解码

### 网络通信层
- **JTCodec**：TCP 粘包编解码器，手动缓冲模式，0x7E 帧边界分割
- **JT808Client**：TCP 客户端，Send/SendAsync，[MessageKind] 自动处理器路由
- **JT808Server**：轻量级 TCP 服务端，NetServer+SessionManager+ICommand，不内置 DB
- **FileCodec**：告警文件粘包编解码器
- **分包合并**：MemoryCache 缓存，60s 超时自动清理

### 消息体增强
- **[JT2011]**：标记仅 2011 版使用的字段
- **KindSizeAttribute**：TLV/KLV 枚举值字节大小标注
- **MediaTypes**：多媒体类型枚举
- **T0A00**：终端 RSA 公钥消息体
- **IJT808Session**：会话抽象接口
- **IJT808Handler**：消息处理器标记接口

### 苏标/音视频/行车记录仪
- **JT/T 1078 音视频扩展**：T1003/T9003/T9101/T9102/T9201/T9202/T9205 + 12 个枚举
- **JT/T 19056 行车记录仪**：CarDVR/UpPackage/DownPackage/UpBody/DownBody

### 项目工程
- **JT808.Server**：控制台服务端示例（NormalHandler+PositionHandler）
- **Client**：WinForms 报文解调试器
- **JT808.Data**：XCode 数据层（5 张核心表）
- **JT808.Web**：Cube 魔方管理后台
- **XUnitTest**：xUnit 单元测试骨架
- **CI/CD**：GitHub Actions（test/publish/publish-beta）

### 架构更新
- **Packet → IPacket 消解**：全面替换为 IPacket/ArrayPacket，消除废弃类型依赖
- **多目标框架**：netstandard2.1/2.0/net461/net45
- **MIT 开源协议**：LICENSE、NuGet 发布配置
- **BCD 编码**：BCD 字符串和 BCD 时间自定义序列化特性
- **GBK 编码**：内置 GBK 中文字符串编解码支持

### 依赖
- 基于 NewLife.Core v10.x 序列化、网络、日志基础设施
