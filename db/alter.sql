CREATE TABLE [AspNetRoleClaims] (
    [Id] INTEGER IDENTITY(1,1) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    [ClaimType] NVARCHAR(max),
    [ClaimValue] NVARCHAR(max),
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId] ASC)
GO

CREATE TABLE [AspNetRoles] (
    [Id] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(256),
    [NormalizedName] NVARCHAR(256),
    [ConcurrencyStamp] NVARCHAR(max),
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL)
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] INTEGER IDENTITY(1,1) NOT NULL,
    [UserId] NVARCHAR(450) NOT NULL,
    [ClaimType] NVARCHAR(max),
    [ClaimValue] NVARCHAR(max),
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId] ASC)
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [ProviderKey] NVARCHAR(128) NOT NULL,
    [ProviderDisplayName] NVARCHAR(max),
    [UserId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId] ASC)
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId] ASC)
GO

CREATE TABLE [AspNetUsers] (
    [Id] NVARCHAR(450) NOT NULL,
    [UserName] NVARCHAR(256),
    [NormalizedUserName] NVARCHAR(256),
    [Email] NVARCHAR(256),
    [NormalizedEmail] NVARCHAR(256),
    [EmailConfirmed] BIT NOT NULL,
    [PasswordHash] NVARCHAR(max),
    [SecurityStamp] NVARCHAR(max),
    [ConcurrencyStamp] NVARCHAR(max),
    [PhoneNumber] NVARCHAR(max),
    [PhoneNumberConfirmed] BIT NOT NULL,
    [TwoFactorEnabled] BIT NOT NULL,
    [LockoutEnd] DATETIMEOFFSET,
    [LockoutEnabled] BIT NOT NULL,
    [AccessFailedCount] INTEGER NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id])
)
GO

CREATE NONCLUSTERED INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL)
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] NVARCHAR(450) NOT NULL,
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [Value] NVARCHAR(max),
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [Name])
)
GO

ALTER TABLE [AspNetRoleClaims] ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] 
    FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] 
    FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] 
    FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
GO


