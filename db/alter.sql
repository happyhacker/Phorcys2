/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2026-04-19 23:31                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [LogSamples] DROP CONSTRAINT [FK_LogSamples_DiveComputerLogs]
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [LogSamples] ADD CONSTRAINT [FK_LogSamples_DiveComputerLogs] 
    FOREIGN KEY ([DiveComputerLogId]) REFERENCES [DiveComputerLogs] ([DiveComputerLogId]) ON DELETE CASCADE
GO

