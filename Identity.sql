IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Contacts] (
    [ContactId] int NOT NULL IDENTITY,
    [Company] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Address1] nvarchar(max) NOT NULL,
    [Address2] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [CountryCode] nvarchar(max) NULL,
    [Email] nvarchar(max) NOT NULL,
    [CellPhone] nvarchar(max) NOT NULL,
    [HomePhone] nvarchar(max) NOT NULL,
    [WorkPhone] nvarchar(max) NOT NULL,
    [Birthday] datetime2 NULL,
    [Gender] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY ([ContactId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [LoginId] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [LoginCount] int NULL,
    [ContactId] int NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Locations] (
    [DiveLocationId] int NOT NULL IDENTITY,
    [ContactId] int NULL,
    [Title] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    [Notes] nvarchar(max) NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([DiveLocationId]),
    CONSTRAINT [FK_Locations_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId]),
    CONSTRAINT [FK_Locations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [DiveSites] (
    [DiveSiteId] int NOT NULL IDENTITY,
    [DiveLocationId] int NULL,
    [Title] nvarchar(max) NOT NULL,
    [IsFreshWater] bit NOT NULL,
    [GeoCode] nvarchar(max) NULL,
    [Notes] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    [MaxDepth] int NULL,
    CONSTRAINT [PK_DiveSites] PRIMARY KEY ([DiveSiteId]),
    CONSTRAINT [FK_DiveSites_Locations_DiveLocationId] FOREIGN KEY ([DiveLocationId]) REFERENCES [Locations] ([DiveLocationId]),
    CONSTRAINT [FK_DiveSites_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [DivePlans] (
    [DivePlanId] int NOT NULL IDENTITY,
    [DiveSiteId] int NULL,
    [Title] nvarchar(max) NOT NULL,
    [Minutes] int NULL,
    [ScheduledTime] datetime2 NOT NULL,
    [MaxDepth] int NULL,
    [Notes] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_DivePlans] PRIMARY KEY ([DivePlanId]),
    CONSTRAINT [FK_DivePlans_DiveSites_DiveSiteId] FOREIGN KEY ([DiveSiteId]) REFERENCES [DiveSites] ([DiveSiteId]),
    CONSTRAINT [FK_DivePlans_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [DiveSiteUrl] (
    [DiveSiteUrlId] int NOT NULL IDENTITY,
    [DiveSiteId] int NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [IsImage] bit NOT NULL,
    [Title] nvarchar(max) NULL,
    CONSTRAINT [PK_DiveSiteUrl] PRIMARY KEY ([DiveSiteUrlId]),
    CONSTRAINT [FK_DiveSiteUrl_DiveSites_DiveSiteId] FOREIGN KEY ([DiveSiteId]) REFERENCES [DiveSites] ([DiveSiteId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Dives] (
    [DiveId] int NOT NULL IDENTITY,
    [DivePlanId] int NULL,
    [Title] nvarchar(max) NULL,
    [Minutes] int NULL,
    [DescentTime] datetime2 NULL,
    [AvgDepth] int NULL,
    [MaxDepth] int NULL,
    [Temperature] int NULL,
    [AdditionalWeight] int NULL,
    [Notes] nvarchar(max) NOT NULL,
    [DiveNumber] int NOT NULL,
    [UserId] int NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_Dives] PRIMARY KEY ([DiveId]),
    CONSTRAINT [FK_Dives_DivePlans_DivePlanId] FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId]),
    CONSTRAINT [FK_Dives_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_DivePlans_DiveSiteId] ON [DivePlans] ([DiveSiteId]);
GO

CREATE INDEX [IX_DivePlans_UserId] ON [DivePlans] ([UserId]);
GO

CREATE INDEX [IX_Dives_DivePlanId] ON [Dives] ([DivePlanId]);
GO

CREATE INDEX [IX_Dives_UserId] ON [Dives] ([UserId]);
GO

CREATE INDEX [IX_DiveSites_DiveLocationId] ON [DiveSites] ([DiveLocationId]);
GO

CREATE INDEX [IX_DiveSites_UserId] ON [DiveSites] ([UserId]);
GO

CREATE INDEX [IX_DiveSiteUrl_DiveSiteId] ON [DiveSiteUrl] ([DiveSiteId]);
GO

CREATE INDEX [IX_Locations_ContactId] ON [Locations] ([ContactId]);
GO

CREATE INDEX [IX_Locations_UserId] ON [Locations] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240415032530_AddIdentity', N'8.0.4');
GO

COMMIT;
GO

