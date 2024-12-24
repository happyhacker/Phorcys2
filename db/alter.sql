/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2024-12-24 13:30                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [Users_DiveSites]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [DiveLocations_DiveSites]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [DiveSites_Dives]
GO


ALTER TABLE [DiveSiteUrls] DROP CONSTRAINT [DiveSites_DiveSiteUrls]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "DiveSites"                                    */
/* ---------------------------------------------------------------------- */

GO


/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [DiveSites] DROP CONSTRAINT [DEF_DiveSites_IsFreshWater]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [DEF_DiveSites_Created]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [DEF_DiveSites_LastModified]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [PK_DiveSites]
GO


CREATE TABLE [DiveSites_TMP] (
    [DiveSiteId] INTEGER IDENTITY(0,1) NOT NULL,
    [DiveLocationId] INTEGER,
    [Title] VARCHAR(40) NOT NULL,
    [IsFreshWater] BIT CONSTRAINT [DEF_DiveSites_IsFreshWater] DEFAULT 0 NOT NULL,
    [MaxDepth] INTEGER,
    [GeoCode] VARCHAR(30),
    [Latitude] DECIMAL(9,6),
    [Longitude] DECIMAL(9,6),
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [DEF_DiveSites_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_DiveSites_LastModified] DEFAULT getdate() NOT NULL)
GO



SET IDENTITY_INSERT [DiveSites_TMP] ON
GO



INSERT INTO [DiveSites_TMP]
    ([DiveSiteId],[DiveLocationId],[Title],[IsFreshWater],[MaxDepth],[GeoCode],[Notes],[UserId],[Created],[LastModified])
SELECT
    [DiveSiteId],[DiveLocationId],[Title],[IsFreshWater],[MaxDepth],[GeoCode],[Notes],[UserId],[Created],[LastModified]
FROM [DiveSites]
GO



SET IDENTITY_INSERT [DiveSites_TMP] OFF
GO



DROP TABLE [DiveSites]
GO


EXEC sp_rename '[DiveSites_TMP]', 'DiveSites', 'OBJECT'
GO


ALTER TABLE [DiveSites] ADD CONSTRAINT [PK_DiveSites] 
    PRIMARY KEY CLUSTERED ([DiveSiteId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveSites] ADD CONSTRAINT [Users_DiveSites] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DiveSites] ADD CONSTRAINT [DiveLocations_DiveSites] 
    FOREIGN KEY ([DiveLocationId]) REFERENCES [DiveLocations] ([DiveLocationId])
GO


ALTER TABLE [DiveSiteUrls] ADD CONSTRAINT [DiveSites_DiveSiteUrls] 
    FOREIGN KEY ([DiveSiteId]) REFERENCES [DiveSites] ([DiveSiteId])
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [DiveSites_Dives] 
    FOREIGN KEY ([DiveSiteId]) REFERENCES [DiveSites] ([DiveSiteId])
GO

