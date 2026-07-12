# NewLife.JT808 版本更新记录

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
- **Server**：控制台服务端示例（NormalHandler+PositionHandler）
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
