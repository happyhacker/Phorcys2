/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.4.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2024-07-04 17:24                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [DiveSites] DROP CONSTRAINT [Users_DiveSites]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [DiveLocations_DiveSites]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [DiveSites_Dives]
GO


ALTER TABLE [DiveSiteUrls] DROP CONSTRAINT [DiveSites_DiveSiteUrls]
GO


/* ---------------------------------------------------------------------- */
/* Alter table "DiveSites"                                                */
/* ---------------------------------------------------------------------- */

ALTER TABLE [DiveSites] ALTER COLUMN [GeoCode] VARCHAR(30)
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

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

