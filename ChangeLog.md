# NewLife.JT808 版本更新记录

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
