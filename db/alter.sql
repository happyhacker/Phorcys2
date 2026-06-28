/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.9.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2026-06-28 15:21                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [Users_DivePlans]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [DiveSites_Dives]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [Users_Dives]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [Dives_DiveDetails]
GO


ALTER TABLE [DiveUrls] DROP CONSTRAINT [DiveDetails_ContentLinks]
GO


ALTER TABLE [TanksOnDives] DROP CONSTRAINT [DivePlans_TanksOnDive]
GO


ALTER TABLE [DivePlanGear] DROP CONSTRAINT [DivePlans_DivePlanGear]
GO


ALTER TABLE [DiveTeams] DROP CONSTRAINT [Dives_DiveTeam]
GO


ALTER TABLE [DivePlansDiveTypes] DROP CONSTRAINT [Dives_DiveTypesXref]
GO


ALTER TABLE [DiveComputerLogs] DROP CONSTRAINT [Dives_DiveComputerLogs]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "DivePlans"                                    */
/* ---------------------------------------------------------------------- */

GO


/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [DivePlans] DROP CONSTRAINT [DEF_DivePlans_MaxDepth]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [DEF_DivePlans_Created]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [DEF_DivePlans_LastModified]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [PK_DivePlans]
GO


CREATE TABLE [DivePlans_TMP] (
    [DivePlanId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveSiteId] INTEGER,
    [Title] VARCHAR(60) NOT NULL,
    [Minutes] INTEGER,
    [ScheduledTime] DATETIME NOT NULL,
    [MaxDepth] INTEGER CONSTRAINT [DEF_DivePlans_MaxDepth] DEFAULT 0,
    [AveragePO2] DECIMAL(3,2),
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DivePlans_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DivePlans_LastModified] DEFAULT getdate() NOT NULL)
GO



SET IDENTITY_INSERT [DivePlans_TMP] ON
GO



INSERT INTO [DivePlans_TMP]
    ([DivePlanId],[DiveSiteId],[Title],[Minutes],[ScheduledTime],[MaxDepth],[Notes],[UserId],[Created],[LastModified])
SELECT
    [DivePlanId],[DiveSiteId],[Title],[Minutes],[ScheduledTime],[MaxDepth],[Notes],[UserId],[Created],[LastModified]
FROM [DivePlans]
GO



SET IDENTITY_INSERT [DivePlans_TMP] OFF
GO



DROP INDEX [DivePlans].[IDX_DivePlans_DiveId]
GO


DROP TABLE [DivePlans]
GO


EXEC sp_rename '[DivePlans_TMP]', 'DivePlans', 'OBJECT'
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [PK_DivePlans] 
    PRIMARY KEY CLUSTERED ([DivePlanId])
GO


CREATE NONCLUSTERED INDEX [IDX_DivePlans_DiveId] ON [DivePlans] ([DivePlanId] ASC)
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Dives"                                        */
/* ---------------------------------------------------------------------- */

GO


/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [Dives] DROP CONSTRAINT [DF__DiveDetai__Minut__31B762FC]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DF__DiveDetai__AvgDe__32AB8735]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DF__DiveDetai__MaxDe__339FAB6E]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DF__DiveDetai__Tempe__3493CFA7]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DF__DiveDetai__Addit__3587F3E0]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DEF_Dives_Created]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [DEF_Dives_LastModified]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [PK_Dives]
GO


CREATE TABLE [Dives_TMP] (
    [DiveId] INTEGER IDENTITY(1,1) NOT NULL,
    [DivePlanId] INTEGER,
    [Title] VARCHAR(60),
    [Minutes] INTEGER CONSTRAINT [DF__DiveDetai__Minut__31B762FC] DEFAULT 0,
    [DescentTime] DATETIME,
    [AvgDepth] INTEGER CONSTRAINT [DF__DiveDetai__AvgDe__32AB8735] DEFAULT 0,
    [MaxDepth] INTEGER CONSTRAINT [DF__DiveDetai__MaxDe__339FAB6E] DEFAULT 0,
    [AveragePO2] DECIMAL(3,2),
    [Temperature] INTEGER CONSTRAINT [DF__DiveDetai__Tempe__3493CFA7] DEFAULT 0,
    [AdditionalWeight] INTEGER CONSTRAINT [DF__DiveDetai__Addit__3587F3E0] DEFAULT 0,
    [Notes] VARCHAR(max) NOT NULL,
    [DiveNumber] INTEGER,
    [UserId] INTEGER,
    [Created] DATETIME CONSTRAINT [DEF_Dives_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Dives_LastModified] DEFAULT getdate() NOT NULL)
GO



SET IDENTITY_INSERT [Dives_TMP] ON
GO



INSERT INTO [Dives_TMP]
    ([DiveId],[DivePlanId],[Title],[Minutes],[DescentTime],[AvgDepth],[MaxDepth],[Temperature],[AdditionalWeight],[Notes],[DiveNumber],[UserId],[Created],[LastModified])
SELECT
    [DiveId],[DivePlanId],[Title],[Minutes],[DescentTime],[AvgDepth],[MaxDepth],[Temperature],[AdditionalWeight],[Notes],[DiveNumber],[UserId],[Created],[LastModified]
FROM [Dives]
GO



SET IDENTITY_INSERT [Dives_TMP] OFF
GO



DROP TABLE [Dives]
GO


EXEC sp_rename '[Dives_TMP]', 'Dives', 'OBJECT'
GO


ALTER TABLE [Dives] ADD CONSTRAINT [PK_Dives] 
    PRIMARY KEY CLUSTERED ([DiveId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [Users_DivePlans] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [DiveSites_Dives] 
    FOREIGN KEY ([DiveSiteId]) REFERENCES [DiveSites] ([DiveSiteId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Users_Dives] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Dives_DiveDetails] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DiveUrls] ADD CONSTRAINT [DiveDetails_ContentLinks] 
    FOREIGN KEY ([DiveId]) REFERENCES [Dives] ([DiveId])
GO


ALTER TABLE [TanksOnDives] ADD CONSTRAINT [DivePlans_TanksOnDive] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DivePlanGear] ADD CONSTRAINT [DivePlans_DivePlanGear] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DiveTeams] ADD CONSTRAINT [Dives_DiveTeam] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DivePlansDiveTypes] ADD CONSTRAINT [Dives_DiveTypesXref] 
    FOREIGN KEY ([DivePlanId]) REFERENCES [DivePlans] ([DivePlanId])
GO


ALTER TABLE [DiveComputerLogs] ADD CONSTRAINT [Dives_DiveComputerLogs] 
    FOREIGN KEY ([DiveId]) REFERENCES [Dives] ([DiveId]) ON DELETE CASCADE
GO

