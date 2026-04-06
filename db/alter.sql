/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2026-04-05 23:57                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveComputerLogs] DROP CONSTRAINT [Dives_DiveComputerLogs]
GO


/* ---------------------------------------------------------------------- */
/* Alter table "DiveComputerLogs"                                         */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveComputerLogs] DROP CONSTRAINT [PK_DiveComputerLogs]
GO


ALTER TABLE [DiveComputerLogs] ADD CONSTRAINT [PK_DiveComputerLogs] 
    PRIMARY KEY ([DiveComputerLogId])
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveComputerLogs] ADD CONSTRAINT [Dives_DiveComputerLogs] 
    FOREIGN KEY ([DiveId]) REFERENCES [Dives] ([DiveId]) ON DELETE CASCADE
GO

