### IdentityServer4项目预览

> ​	该项目主要借鉴作者`brunobritodev`,包含用户注册，Admin管理等界面
>
> [JPProject.IdentityServer4.SSO](https://github.com/brunobritodev/JPProject.IdentityServer4.SSO)

- 项目还原过程主要通过git提交记录
- UserManagement和AdminUI主要通过Angular完成(后续可以尝试用Vue完成)

>  **以下为辅助理解部分**

#### 1、SSOContext主要DbContext类

<img src="https://raw.githubusercontent.com/BoydenYubin/PicGoHub/master/SSOContext.svg"/>

- `IConfigurationDbContext`和`IPersistedGrantDbContext`为`IdentityServer4`持久化部分的Context
- `IEventStoreContext`和`ISSOContext`是应用层持久化Context
- 继承的`IdentityDbContext`为`MiMicrosoft.AspNetCore.Identity`持久化Context
- `ISecurityKeyContext`和`IDataProtectionKeyContext`为数据保护部分持久化Context

#### 2、主要持久化对象`DbSet<XXX>`

- **Application**
  - `ISSOContext`
    - `DbSet<Template> Templates`
    - `DbSet<Email> Emails `
    - `DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings`
  - `IEventStoreContext`
    - `DbSet<StoredEvent> StoredEvent`
    - `DbSet<EventDetails> StoredEventDetails`
- **Identity**
  - `IdentityDbContext`
    - `DbSet<TUserRole> UserRoles`
    - `DbSet<TRole> Roles`
    - `DbSet<TRoleClaim> RoleClaims`
  - `IdentityUserContext`
    - `DbSet<TUser> Users`
    - `DbSet<TUserClaim> UserClaims`
    -  `DbSet<TUserLogin> UserLogins`
    -  `DbSet<TUserToken> UserTokens`
- **IdentityServer4**
  - `IPersistedGrantDbContext`
    - `DbSet<PersistedGrant> PersistedGrants`
    - `DbSet<DeviceFlowCodes> DeviceFlowCodes`
  - `IConfigurationDbContext`
    - `DbSet<Client> Clients`
    - `DbSet<IdentityResource> IdentityResources`
    - `DbSet<ApiResource> ApiResources`
- **DataProtection(TBD)**
  - `IDataProtectionKeyContext`
  - `ISecurityKeyContext`

#### 3、Identity中`User`和`Role`管理

​	Identity中主要包括User和Role的管理，用于授权和认证。

<img src="https://raw.githubusercontent.com/BoydenYubin/PicGoHub/master/Microsoft.AspNetCore.Identity.svg" width="50%" height="50%"/>

#### 4、IdentityServer4服务