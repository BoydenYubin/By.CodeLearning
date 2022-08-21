### IdentityServer4项目预览

> ​	该项目主要借鉴作者`brunobritodev`,包含用户注册，Admin管理等界面
>
> [JPProject.IdentityServer4.SSO](https://github.com/brunobritodev/JPProject.IdentityServer4.SSO)

- 项目还原过程主要通过git提交记录
- UserManagement和AdminUI主要通过Angular完成(后续可以尝试用Vue完成)

>  **以下为辅助理解部分**

#### 1、SSOContext主要DbContext类

![](.\SSOContext.svg)

- `IConfigurationDbContext`和`IPersistedGrantDbContext`为`IdentityServer4`持久化部分的Context
- `IEventStoreContext`和`ISSOContext`是应用层持久化Context
- 继承的`IdentityDbContext`为`MiMicrosoft.AspNetCore.Identity`持久化Context
- `ISecurityKeyContext`和`IDataProtectionKeyContext`为数据保护部分持久化Context

#### 2、主要持久化对象`DbSet<XXX>`