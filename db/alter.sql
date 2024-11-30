/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2024-11-29 18:39                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [Dives] DROP CONSTRAINT [Users_Dives]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [Dives_DiveDetails]
GO


ALTER TABLE [DiveUrls] DROP CONSTRAINT [DiveDetails_ContentLinks]
GO


/* ---------------------------------------------------------------------- */
/* Alter table "Dives"                                                    */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [Dives] ALTER COLUMN [DiveNumber] INTEGER
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

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

