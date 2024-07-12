/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.4.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2024-07-09 22:41                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Add tables                                                             */
/* ---------------------------------------------------------------------- */

/* ---------------------------------------------------------------------- */
/* Add table "dbo.Countries"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Countries] (
    [CountryCode] VARCHAR(2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Name] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryCode])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Gases"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Gases] (
    [GasId] INTEGER IDENTITY(1,1) NOT NULL,
    [Name] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT [PK_Gases] PRIMARY KEY CLUSTERED ([GasId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Services"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Services] (
    [ServiceId] INTEGER IDENTITY(1,1) NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [Created] DATETIME CONSTRAINT [DEF_Services_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Services_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([ServiceId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AspNetRoles"                                                */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Add table "AspNetUsers"                                                */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Add table "AspNetUserTokens"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AspNetUserTokens] (
    [UserId] NVARCHAR(450) NOT NULL,
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [Value] NVARCHAR(max),
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [Name])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "AspNetRoleClaims"                                           */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Add table "AspNetUserClaims"                                           */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Add table "AspNetUserLogins"                                           */
/* ---------------------------------------------------------------------- */

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


/* ---------------------------------------------------------------------- */
/* Add table "AspNetUserRoles"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [AspNetUserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId], [RoleId])
)
GO


CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId] ASC)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Users"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Users] (
    [UserId] INTEGER IDENTITY(1,1) NOT NULL,
    [LoginId] VARCHAR(30) NOT NULL,
    [Password] VARCHAR(20) NOT NULL,
    [LoginCount] INTEGER CONSTRAINT [DEF_Users_LoginCount] DEFAULT 0,
    [ContactId] INTEGER,
    [AspNetUserId] NVARCHAR(450),
    [Created] DATETIME CONSTRAINT [DEF_Users_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Users_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId]),
    CONSTRAINT [TUC_Users_1] UNIQUE ([LoginId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveLocations"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveLocations] (
    [DiveLocationId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DiveLocations_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiveLocations_LastModified] DEFAULT getdate() NOT NULL,
    [Notes] VARCHAR(max),
    CONSTRAINT [PK_DiveLocations] PRIMARY KEY CLUSTERED ([DiveLocationId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Friends"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Friends] (
    [RequestorUserId] INTEGER NOT NULL,
    [RecipientUserId] INTEGER NOT NULL,
    [DateRequested] DATE NOT NULL,
    [Status] VARCHAR(20),
    [LastUpdated] DATE NOT NULL,
    CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED ([RequestorUserId], [RecipientUserId], [DateRequested])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveSites"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveSites] (
    [DiveSiteId] INTEGER IDENTITY(0,1) NOT NULL,
    [DiveLocationId] INTEGER,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [IsFreshWater] BIT CONSTRAINT [DEF_DiveSites_IsFreshWater] DEFAULT 0 NOT NULL,
    [GeoCode] VARCHAR(30) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DiveSites_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiveSites_LastModified] DEFAULT getdate() NOT NULL,
    [MaxDepth] INTEGER,
    CONSTRAINT [PK_DiveSites] PRIMARY KEY CLUSTERED ([DiveSiteId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DiveUrls"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [DiveUrls] (
    [DiveUrlId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveId] INTEGER NOT NULL,
    [URL] VARCHAR(512) NOT NULL,
    [IsImage] BIT NOT NULL,
    [Title] VARCHAR(40),
    CONSTRAINT [PK_ContentLinks] PRIMARY KEY CLUSTERED ([DiveUrlId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Certifications"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Certifications] (
    [CertificationId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveAgencyId] INTEGER,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_Certifications_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Certifications_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Certifications] PRIMARY KEY CLUSTERED ([CertificationId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DiverCertifications"                                        */
/* ---------------------------------------------------------------------- */

CREATE TABLE [DiverCertifications] (
    [DiverCertificationId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiverId] INTEGER NOT NULL,
    [CertificationId] INTEGER NOT NULL,
    [Certified] DATE,
    [CertificationNum] VARCHAR(20) NOT NULL,
    [Notes] VARCHAR(max),
    [Created] DATETIME CONSTRAINT [DEF_DiverCertifications_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiverCertifications_LastModified] DEFAULT getdate() NOT NULL,
    [InstructorId] INTEGER,
    CONSTRAINT [PK_DiverCertifications] PRIMARY KEY CLUSTERED ([DiverCertificationId])
)
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', NULL, NULL
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'DiverCertificationId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'DiverId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'CertificationId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'Certified'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'CertificationNum'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'Notes'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'Created'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'LastModified'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'DiverCertifications', 'COLUMN', N'InstructorId'
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveSiteUrls"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveSiteUrls] (
    [DiveSiteUrlId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveSiteId] INTEGER NOT NULL,
    [URL] VARCHAR(1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [IsImage] BIT NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_DiveSiteUrls] PRIMARY KEY CLUSTERED ([DiveSiteUrlId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.GasMixes"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[GasMixes] (
    [DivePlanId] INTEGER NOT NULL,
    [GearId] INTEGER NOT NULL,
    [GasId] INTEGER NOT NULL,
    [VolumeAdded] INTEGER,
    [Percentage] FLOAT(53),
    [CostPerVolumeOfMeasure] MONEY,
    CONSTRAINT [PK__GasMixes__56FD4E221D9B5BB6] PRIMARY KEY CLUSTERED ([DivePlanId], [GearId], [GasId])
)
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', NULL, NULL
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'DivePlanId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'GearId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'GasId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'VolumeAdded'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'Percentage'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'GasMixes', 'COLUMN', N'CostPerVolumeOfMeasure'
GO


/* ---------------------------------------------------------------------- */
/* Add table "Instructors"                                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Instructors] (
    [InstructorId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    [Notes] VARCHAR(max),
    CONSTRAINT [PK_Instructors] PRIMARY KEY CLUSTERED ([InstructorId])
)
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'Instructors', NULL, NULL
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'Instructors', 'COLUMN', N'InstructorId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'Instructors', 'COLUMN', N'ContactId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'Instructors', 'COLUMN', N'Notes'
GO


/* ---------------------------------------------------------------------- */
/* Add table "Tanks"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Tanks] (
    [GearId] INTEGER NOT NULL,
    [Volume] INTEGER CONSTRAINT [DF__Tanks_TMP__Volum__282DF8C2] DEFAULT 0,
    [WorkingPressure] INTEGER CONSTRAINT [DF__Tanks_TMP__Worki__29221CFB] DEFAULT 0,
    [ManufacturedMonth] TINYINT,
    [ManufacturedYear] TINYINT,
    CONSTRAINT [PK_Tanks] PRIMARY KEY CLUSTERED ([GearId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DivePlans"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [DivePlans] (
    [DivePlanId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveSiteId] INTEGER,
    [Title] VARCHAR(40) NOT NULL,
    [Minutes] INTEGER,
    [ScheduledTime] DATETIME NOT NULL,
    [MaxDepth] INTEGER CONSTRAINT [DEF_DivePlans_MaxDepth] DEFAULT 0,
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DivePlans_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DivePlans_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_DivePlans] PRIMARY KEY CLUSTERED ([DivePlanId])
)
GO


CREATE NONCLUSTERED INDEX [IDX_DivePlans_DiveId] ON [DivePlans] ([DivePlanId] ASC)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.InsurancePolicies"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[InsurancePolicies] (
    [InsurancePolicyId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    [StartOfTerm] DATE,
    [EndOfTerm] DATE,
    [Deductible] MONEY CONSTRAINT [DF__Insurance__Deduc__2BFE89A6] DEFAULT 0,
    [ValueAmount] MONEY CONSTRAINT [DF__Insurance__Value__2CF2ADDF] DEFAULT 0,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_InsurancePolicies] PRIMARY KEY CLUSTERED ([InsurancePolicyId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.InsuredGear"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[InsuredGear] (
    [GearId] INTEGER NOT NULL,
    [InsurancePolicyId] INTEGER NOT NULL,
    CONSTRAINT [PK_InsuredGear] PRIMARY KEY CLUSTERED ([GearId], [InsurancePolicyId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Dives"                                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Dives] (
    [DiveId] INTEGER IDENTITY(1,1) NOT NULL,
    [DivePlanId] INTEGER,
    [Title] VARCHAR(40),
    [Minutes] INTEGER CONSTRAINT [DF__DiveDetai__Minut__31B762FC] DEFAULT 0,
    [DescentTime] DATETIME,
    [AvgDepth] INTEGER CONSTRAINT [DF__DiveDetai__AvgDe__32AB8735] DEFAULT 0,
    [MaxDepth] INTEGER CONSTRAINT [DF__DiveDetai__MaxDe__339FAB6E] DEFAULT 0,
    [Temperature] INTEGER CONSTRAINT [DF__DiveDetai__Tempe__3493CFA7] DEFAULT 0,
    [AdditionalWeight] INTEGER CONSTRAINT [DF__DiveDetai__Addit__3587F3E0] DEFAULT 0,
    [Notes] VARCHAR(max) NOT NULL,
    [DiveNumber] INTEGER NOT NULL,
    [UserId] INTEGER,
    [Created] DATETIME CONSTRAINT [DEF_Dives_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Dives_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Dives] PRIMARY KEY CLUSTERED ([DiveId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Divers"                                                 */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Divers] (
    [DiverId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    [WorkingSacRate] FLOAT(53) CONSTRAINT [DF__Divers_TM__SacRa__114A936A] DEFAULT 0,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [RestingSacRate] REAL CONSTRAINT [DF__Divers__RestingS__3F3159AB] DEFAULT 0,
    CONSTRAINT [PK_Divers] PRIMARY KEY CLUSTERED ([DiverId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Qualifications"                                         */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Qualifications] (
    [QualificationId] INTEGER IDENTITY(1,1) NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_Qualifications_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Qualifications_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Qualifications] PRIMARY KEY CLUSTERED ([QualificationId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiverQualifications"                                    */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiverQualifications] (
    [DiverId] INTEGER NOT NULL,
    [QualificationId] INTEGER NOT NULL,
    [Qualified] DATE,
    [Notes] VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DiverQualifications_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiverQualifications_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_DiverQualifications] PRIMARY KEY CLUSTERED ([DiverId], [QualificationId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.TanksOnDive"                                            */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[TanksOnDive] (
    [DivePlanId] INTEGER NOT NULL,
    [GearId] INTEGER NOT NULL,
    [GasContentTitle] VARCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [StartingPressure] INTEGER CONSTRAINT [DF__TanksOnDi__Start__3C34F16F] DEFAULT 0,
    [EndingPressure] INTEGER CONSTRAINT [DF__TanksOnDi__Endin__3D2915A8] DEFAULT 0,
    [FillCost] MONEY,
    [FillDate] DATETIME,
    CONSTRAINT [PK_TanksOnDive] PRIMARY KEY CLUSTERED ([DivePlanId], [GearId])
)
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', NULL, NULL
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'DivePlanId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'GearId'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'GasContentTitle'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'StartingPressure'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'EndingPressure'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'FillCost'
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'N', 'SCHEMA', N'dbo', 'TABLE', N'TanksOnDive', 'COLUMN', N'FillDate'
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveShops"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveShops] (
    [DiveShopId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_DiveShops] PRIMARY KEY CLUSTERED ([DiveShopId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "DiveTeams"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [DiveTeams] (
    [DivePlanId] INTEGER NOT NULL,
    [DiverId] INTEGER NOT NULL,
    [RoleId] INTEGER,
    CONSTRAINT [PK_DiveTeam] PRIMARY KEY CLUSTERED ([DivePlanId], [DiverId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveShopServices"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveShopServices] (
    [DiveShopId] INTEGER NOT NULL,
    [ServiceId] INTEGER NOT NULL,
    CONSTRAINT [PK_DiveShopServices] PRIMARY KEY CLUSTERED ([DiveShopId], [ServiceId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiverGear"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiverGear] (
    [DiverId] INTEGER NOT NULL,
    [GearId] INTEGER NOT NULL,
    CONSTRAINT [PK_DiverGear] PRIMARY KEY CLUSTERED ([DiverId], [GearId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveTypes"                                              */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveTypes] (
    [DiveTypeId] INTEGER IDENTITY(1,1) NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DiveTypes_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiveTypes_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_DiveTypes] PRIMARY KEY CLUSTERED ([DiveTypeId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Contacts"                                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Contacts] (
    [ContactId] INTEGER IDENTITY(1,1) NOT NULL,
    [Company] VARCHAR(40) NOT NULL,
    [FirstName] VARCHAR(20) NOT NULL,
    [LastName] VARCHAR(30) NOT NULL,
    [Address1] VARCHAR(40) NOT NULL,
    [Address2] VARCHAR(40) NOT NULL,
    [City] VARCHAR(30) NOT NULL,
    [State] VARCHAR(20) NOT NULL,
    [PostalCode] VARCHAR(20) NOT NULL,
    [CountryCode] VARCHAR(2),
    [Email] VARCHAR(50) NOT NULL,
    [CellPhone] VARCHAR(20) NOT NULL,
    [HomePhone] VARCHAR(20) NOT NULL,
    [WorkPhone] VARCHAR(20) NOT NULL,
    [Birthday] DATE,
    [Gender] CHAR(1) NOT NULL,
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_Contacts_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Contacts_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([ContactId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveShopStaff"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveShopStaff] (
    [ContactId] INTEGER NOT NULL,
    [DiveShopId] INTEGER NOT NULL,
    CONSTRAINT [PK_DiveShopStaff] PRIMARY KEY CLUSTERED ([ContactId], [DiveShopId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.GearOnDive"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[GearOnDive] (
    [GearId] INTEGER NOT NULL,
    [DivePlanId] INTEGER NOT NULL,
    CONSTRAINT [PK_GearOnDive] PRIMARY KEY CLUSTERED ([GearId], [DivePlanId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "Gear"                                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [Gear] (
    [GearId] INTEGER IDENTITY(1,1) NOT NULL,
    [ManufacturerId] INTEGER,
    [PurchasedFromContactId] INTEGER,
    [Title] VARCHAR(100) NOT NULL,
    [RetailPrice] MONEY CONSTRAINT [DF__Gear_TMP__Retail__160F4887] DEFAULT 0,
    [Paid] MONEY CONSTRAINT [DF__Gear_TMP__Paid__17036CC0] DEFAULT 0,
    [SN] VARCHAR(30),
    [Acquired] DATE,
    [NoLongerUse] DATE,
    [Weight] FLOAT(53) CONSTRAINT [DF__Gear_TMP__Weight__17F790F9] DEFAULT 0,
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [GearCreated] DEFAULT getdate(),
    [LastModified] DATETIME CONSTRAINT [GearLastModified] DEFAULT getdate(),
    CONSTRAINT [PK_Gear] PRIMARY KEY CLUSTERED ([GearId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DivePlansDiveTypes"                                     */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DivePlansDiveTypes] (
    [DiveTypeId] INTEGER NOT NULL,
    [DivePlanId] INTEGER NOT NULL,
    CONSTRAINT [PK_DivePlansDiveTypes] PRIMARY KEY CLUSTERED ([DiveTypeId], [DivePlanId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "CertificationInstructors"                                   */
/* ---------------------------------------------------------------------- */

CREATE TABLE [CertificationInstructors] (
    [InstructorId] INTEGER NOT NULL,
    [CertificationId] INTEGER NOT NULL,
    CONSTRAINT [PK_CertificationInstructors] PRIMARY KEY CLUSTERED ([InstructorId], [CertificationId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.ServiceSchedules"                                       */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[ServiceSchedules] (
    [ServiceScheduleId] INTEGER IDENTITY(1,1) NOT NULL,
    [GearId] INTEGER NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [NextServiceDate] DATE,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_ServiceSchedules] PRIMARY KEY CLUSTERED ([ServiceScheduleId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.DiveAgencies"                                           */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[DiveAgencies] (
    [DiveAgencyId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_DiveAgencies] PRIMARY KEY CLUSTERED ([DiveAgencyId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Manufacturers"                                          */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Manufacturers] (
    [ManufacturerId] INTEGER IDENTITY(1,1) NOT NULL,
    [ContactId] INTEGER NOT NULL,
    CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED ([ManufacturerId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Roles"                                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Roles] (
    [RoleId] INTEGER IDENTITY(1,1) NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.GearServiceEvents"                                      */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[GearServiceEvents] (
    [GearServiceEventsId] INTEGER NOT NULL,
    [GearId] INTEGER NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [ServicedDate] DATE NOT NULL,
    [Cost] MONEY CONSTRAINT [DEF_GearServiceEvents_Cost] DEFAULT 0,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_GearServiceEvents] PRIMARY KEY CLUSTERED ([GearServiceEventsId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.SoldGear"                                               */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[SoldGear] (
    [GearId] INTEGER NOT NULL,
    [SoldToContactId] INTEGER,
    [SoldOn] DATE NOT NULL,
    [Amount] MONEY CONSTRAINT [DF__SoldGear___Amoun__5BAD9CC8] DEFAULT 0,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_SoldGear] PRIMARY KEY CLUSTERED ([GearId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.Attributes"                                             */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[Attributes] (
    [AttributeId] INTEGER IDENTITY(1,1) NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [TableToAssociate] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_Attributes_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Attributes_LastModified] DEFAULT getdate() NOT NULL,
    CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED ([AttributeId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add table "dbo.AttributeAssociations"                                  */
/* ---------------------------------------------------------------------- */

CREATE TABLE [dbo].[AttributeAssociations] (
    [AttributeId] INTEGER NOT NULL,
    [TableRowId] INTEGER NOT NULL,
    [Title] VARCHAR(40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Notes] VARCHAR(max) COLLATE SQL_Latin1_General_CP1_CI_AS,
    CONSTRAINT [PK_AttributeAssociations] PRIMARY KEY CLUSTERED ([AttributeId], [TableRowId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Users] ADD CONSTRAINT [Contacts_Users] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Users] ADD CONSTRAINT [AspNetUsers_Users] 
    FOREIGN KEY ([AspNetUserId]) REFERENCES [AspNetUsers] ([Id])
GO


ALTER TABLE [dbo].[DiveLocations] ADD CONSTRAINT [Users_DiveLocations] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[DiveLocations] ADD CONSTRAINT [Contacts_DiveLocations] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[Friends] ADD CONSTRAINT [Users_Friends1] 
    FOREIGN KEY ([RequestorUserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[Friends] ADD CONSTRAINT [Users_Friends2] 
    FOREIGN KEY ([RecipientUserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[DiveSites] ADD CONSTRAINT [Users_DiveSites] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[DiveSites] ADD CONSTRAINT [DiveLocations_DiveSites] 
    FOREIGN KEY ([DiveLocationId]) REFERENCES [dbo].[DiveLocations] ([DiveLocationId])
GO


ALTER TABLE [DiveUrls] ADD CONSTRAINT [DiveDetails_ContentLinks] 
    FOREIGN KEY ([DiveId]) REFERENCES [Dives] ([DiveId])
GO


ALTER TABLE [dbo].[Certifications] ADD CONSTRAINT [Users_Certifications] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[Certifications] ADD CONSTRAINT [DiveAgencies_Certifications] 
    FOREIGN KEY ([DiveAgencyId]) REFERENCES [dbo].[DiveAgencies] ([DiveAgencyId])
GO


ALTER TABLE [DiverCertifications] ADD CONSTRAINT [Divers_DiverCertifications] 
    FOREIGN KEY ([DiverId]) REFERENCES [dbo].[Divers] ([DiverId])
GO


ALTER TABLE [DiverCertifications] ADD CONSTRAINT [Certifications_DiverCertifications] 
    FOREIGN KEY ([CertificationId]) REFERENCES [dbo].[Certifications] ([CertificationId])
GO


ALTER TABLE [DiverCertifications] ADD CONSTRAINT [Instructors_DiverCertifications] 
    FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId])
GO


ALTER TABLE [dbo].[DiveSiteUrls] ADD CONSTRAINT [DiveSites_DiveSiteUrls] 
    FOREIGN KEY ([DiveSiteId]) REFERENCES [dbo].[DiveSites] ([DiveSiteId])
GO


ALTER TABLE [dbo].[GasMixes] ADD CONSTRAINT [TanksOnDive_GasMixes] 
    FOREIGN KEY ([DivePlanId], [GearId]) REFERENCES [dbo].[TanksOnDive] ([DivePlanId],[GearId])
GO


ALTER TABLE [dbo].[GasMixes] ADD CONSTRAINT [Gases_GasMixes] 
    FOREIGN KEY ([GasId]) REFERENCES [dbo].[Gases] ([GasId])
GO


ALTER TABLE [Instructors] ADD CONSTRAINT [Contacts_Instructors] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Tanks] ADD CONSTRAINT [Gear_Tanks] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId]) ON DELETE CASCADE ON UPDATE CASCADE
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [Users_DivePlans] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [DiveSites_Dives] 
    FOREIGN KEY ([DiveSiteId]) REFERENCES [dbo].[DiveSites] ([DiveSiteId])
GO


ALTER TABLE [dbo].[InsurancePolicies] ADD CONSTRAINT [Contacts_InsurancePolicies] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[InsuredGear] ADD CONSTRAINT [Gear_InsuredGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [dbo].[InsuredGear] ADD CONSTRAINT [InsurancePolicies_InsuredGear] 
    FOREIGN KEY ([InsurancePolicyId]) REFERENCES [dbo].[InsurancePolicies] ([InsurancePolicyId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Users_Dives] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Dives_DiveDetails] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [dbo].[Divers] ADD CONSTRAINT [Contacts_Divers] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[Qualifications] ADD CONSTRAINT [Users_Qualifications] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[DiverQualifications] ADD CONSTRAINT [Qualifications_DiverQualifications] 
    FOREIGN KEY ([QualificationId]) REFERENCES [dbo].[Qualifications] ([QualificationId])
GO


ALTER TABLE [dbo].[DiverQualifications] ADD CONSTRAINT [Divers_DiverQualifications] 
    FOREIGN KEY ([DiverId]) REFERENCES [dbo].[Divers] ([DiverId])
GO


ALTER TABLE [dbo].[TanksOnDive] ADD CONSTRAINT [DivePlans_TanksOnDive] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [dbo].[TanksOnDive] ADD CONSTRAINT [Tanks_TanksOnDive] 
    FOREIGN KEY ([GearId]) REFERENCES [Tanks] ([GearId])
GO


ALTER TABLE [dbo].[DiveShops] ADD CONSTRAINT [Contacts_DiveShops] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [DiveTeams] ADD CONSTRAINT [Dives_DiveTeam] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DiveTeams] ADD CONSTRAINT [Divers_DiveTeam] 
    FOREIGN KEY ([DiverId]) REFERENCES [dbo].[Divers] ([DiverId])
GO


ALTER TABLE [DiveTeams] ADD CONSTRAINT [Roles_DiveTeams] 
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
GO


ALTER TABLE [dbo].[DiveShopServices] ADD CONSTRAINT [DiveShops_DiveShopServices] 
    FOREIGN KEY ([DiveShopId]) REFERENCES [dbo].[DiveShops] ([DiveShopId])
GO


ALTER TABLE [dbo].[DiveShopServices] ADD CONSTRAINT [Services_DiveShopServices] 
    FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([ServiceId])
GO


ALTER TABLE [dbo].[DiverGear] ADD CONSTRAINT [Divers_DiverGear] 
    FOREIGN KEY ([DiverId]) REFERENCES [dbo].[Divers] ([DiverId])
GO


ALTER TABLE [dbo].[DiverGear] ADD CONSTRAINT [Gear_DiverGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [dbo].[DiveTypes] ADD CONSTRAINT [Users_DiveTypes] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Contacts] ADD CONSTRAINT [Users_Contacts] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Contacts] ADD CONSTRAINT [Countries_Contacts] 
    FOREIGN KEY ([CountryCode]) REFERENCES [dbo].[Countries] ([CountryCode])
GO


ALTER TABLE [dbo].[DiveShopStaff] ADD CONSTRAINT [Contacts_DiveShopStaff] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[DiveShopStaff] ADD CONSTRAINT [DiveShops_DiveShopStaff] 
    FOREIGN KEY ([DiveShopId]) REFERENCES [dbo].[DiveShops] ([DiveShopId])
GO


ALTER TABLE [dbo].[GearOnDive] ADD CONSTRAINT [DivePlans_GearOnDive] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [dbo].[GearOnDive] ADD CONSTRAINT [Gear_GearOnDive] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Users_Gear] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Manufacturers_Gear] 
    FOREIGN KEY ([ManufacturerId]) REFERENCES [dbo].[Manufacturers] ([ManufacturerId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Contacts_Gear] 
    FOREIGN KEY ([PurchasedFromContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[DivePlansDiveTypes] ADD CONSTRAINT [DiveTypes_DiveTypesXref] 
    FOREIGN KEY ([DiveTypeId]) REFERENCES [dbo].[DiveTypes] ([DiveTypeId])
GO


ALTER TABLE [dbo].[DivePlansDiveTypes] ADD CONSTRAINT [Dives_DiveTypesXref] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [CertificationInstructors] ADD CONSTRAINT [Instructors_CertificationInstructors] 
    FOREIGN KEY ([InstructorId]) REFERENCES [Instructors] ([InstructorId])
GO


ALTER TABLE [CertificationInstructors] ADD CONSTRAINT [Certifications_CertificationInstructors] 
    FOREIGN KEY ([CertificationId]) REFERENCES [dbo].[Certifications] ([CertificationId])
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


ALTER TABLE [dbo].[ServiceSchedules] ADD CONSTRAINT [Gear_ServiceSchedules] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [dbo].[DiveAgencies] ADD CONSTRAINT [Contacts_DiveAgencies] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[Manufacturers] ADD CONSTRAINT [Contacts_Manufactures] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [Users_Roles] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[GearServiceEvents] ADD CONSTRAINT [Gear_GearServiceEvents] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [dbo].[SoldGear] ADD CONSTRAINT [Contacts_SoldGear] 
    FOREIGN KEY ([SoldToContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [dbo].[SoldGear] ADD CONSTRAINT [Gear_SoldGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [dbo].[Attributes] ADD CONSTRAINT [Users_Attributes] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [dbo].[AttributeAssociations] ADD CONSTRAINT [Attributes_AttributeAssociations] 
    FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([AttributeId])
GO


/* ---------------------------------------------------------------------- */
/* Add views                                                              */
/* ---------------------------------------------------------------------- */

CREATE VIEW [vwCertifications]
AS
SELECT certs.CertificationId, certs.Title, divers.DiverId, agency.Company AS Agency, dc.Certified, dc.CertificationNum, diverContact.FirstName AS DiverFirstName, diverContact.LastName AS DiverLastName, instructor.FirstName AS InstructorFirstName,
         instructor.LastName AS InstructorLastName
FROM  dbo.Certifications AS certs INNER JOIN
         dbo.DiverCertifications AS dc ON dc.CertificationId = certs.CertificationId INNER JOIN
         dbo.Users AS u ON u.UserId = dc.DiverId INNER JOIN
         dbo.Divers AS divers ON divers.DiverId = dc.DiverId INNER JOIN
         dbo.Contacts AS diverContact ON diverContact.ContactId = divers.ContactId LEFT OUTER JOIN
         dbo.Instructors ON dbo.Instructors.InstructorId = dc.InstructorId LEFT OUTER JOIN
         dbo.Contacts AS instructor ON instructor.ContactId = dbo.Instructors.ContactId INNER JOIN
         dbo.DiveAgencies AS da ON da.DiveAgencyId = certs.DiveAgencyId INNER JOIN
         dbo.Contacts AS agency ON agency.ContactId = da.ContactId
GO


CREATE VIEW [vwTanksOnDive] AS (
SELECT dp.DivePlanId, dp.Title AS DiveTitle, g.Title AS Tank, t.GearId, t.Volume, t.WorkingPressure, tod.StartingPressure, tod.EndingPressure,
       dbo.CalcVolume(t.Volume, t.WorkingPressure, tod.StartingPressure) AS FillVolume, ROUND(dbo.CalcVolume(t.Volume,
       t.WorkingPressure, tod.StartingPressure) / 3, 0) AS Thirds,
	   dbo.CalcVolume(t.Volume, t.WorkingPressure, tod.EndingPressure) AS TurnVolume,
	   dbo.CalcVolume(t.Volume, t.WorkingPressure, tod.StartingPressure)
                         - dbo.CalcVolume(t.Volume, t.WorkingPressure, tod.EndingPressure) AS GasUsed
FROM dbo.Tanks AS t
INNER JOIN TanksOnDive AS tod ON tod.GearId = t.GearId
INNER JOIN DivePlans AS dp ON tod.DivePlanId = dp.DivePlanId
INNER JOIN Gear AS g ON tod.GearId = g.GearId
)
GO


CREATE VIEW [vwInstructors] AS (
SELECT i.InstructorId, c.FirstName, c.LastName, c.CountryCode, c.Email, dac.Company AS 'Agency', ai.InstructorAgencyId
FROM Instructors AS i LEFT JOIN
	 Contacts AS c ON i.ContactId = c.ContactId LEFT JOIN
	 AgencyInstructors ai ON ai.InstructorId = i.InstructorId LEFT JOIN
     DiveAgencies AS da ON da.DiveAgencyId = ai.DiveAgencyId LEFT JOIN
     Contacts AS dac ON da.ContactId = dac.ContactId
	 )
GO


CREATE VIEW [vwCertificationInstructors]
AS
SELECT        c.ContactId, c.Company, c.FirstName, c.LastName, i.InstructorId, da.DiveAgencyId, c2.Company AS Agency, cer.Title AS Certification, cer.CertificationId
FROM            dbo.Contacts AS c INNER JOIN
                         dbo.Instructors AS i ON i.ContactId = c.ContactId INNER JOIN
                         dbo.CertificationInstructors AS ci ON ci.InstructorId = i.InstructorId INNER JOIN
                         dbo.Certifications AS cer ON cer.CertificationId = ci.CertificationId INNER JOIN
                         dbo.DiveAgencies AS da ON da.DiveAgencyId = cer.DiveAgencyId INNER JOIN
                         dbo.Contacts AS c2 ON c2.ContactId = da.ContactId
GO


CREATE VIEW [vwGear] AS (
SELECT g.GearId, ManufacturerId, PurchasedFromContactId, Title, RetailPrice, Paid, SN, Acquired,NoLongerUse,Weight, Volume AS 'TankVolume', WorkingPressure AS 'TankWorkingPressure', ManufacturedMonth AS 'TankManufacturedMonth', ManufacturedYear AS 'TankManufacturedYear', Notes, UserId, Created, LastModified
FROM Gear g
LEFT JOIN Tanks t on t.GearId = g.GearId
)
GO


CREATE VIEW [dbo].[vwDiveShops] AS (
  SELECT c.*, ds.Notes AS ShopNotes
  FROM DiveShops ds JOIN Contacts c ON c.ContactId = ds.ContactId
)
GO


CREATE VIEW [dbo].[vwDivers] AS (
  SELECT d.DiverId, d.WorkingSacRate, d.RestingSacRate, d.Notes AS DiverNotes, c.*
  FROM Divers d JOIN Contacts c ON d.ContactId = c.ContactId
)
GO


/* ---------------------------------------------------------------------- */
/* Add procedures                                                         */
/* ---------------------------------------------------------------------- */

CREATE PROCEDURE [CreateUserLink]
    @UserName NVARCHAR(256),
    @AspNetUserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN
        -- Insert the new record into Users table linking it with AspNetUsers
        INSERT INTO Users (LoginId, AspNetUserId, Password)
        VALUES (@UserName, @AspNetUserId, 'password');
    END
END
GO


CREATE FUNCTION [dbo].[CalcVolume](@volume float, @workingPressure float, @startingPressure float)
RETURNS int
AS
BEGIN
DECLARE @retVal int;
SET @retVal = ROUND(@volume / @workingPressure * @startingPressure , 0);
RETURN(@retVal);
END;
GO


/* ---------------------------------------------------------------------- */
/* Add triggers                                                           */
/* ---------------------------------------------------------------------- */

CREATE TRIGGER [trg_AfterInsert_AspNetUsers]
ON AspNetUsers
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into Users table using the inserted row data
    INSERT INTO Users (LoginId, AspNetUserId, Password)
    SELECT UserName, Id, 'password'
    FROM inserted;
END
GO

