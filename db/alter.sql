/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2026-04-05 20:51                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Add table "DiveComputerLogs"                                           */
/* ---------------------------------------------------------------------- */

GO


CREATE TABLE [DiveComputerLogs] (
    [DiveComputerLogId] INTEGER IDENTITY(1,1) NOT NULL,
    [DiveId] INTEGER,
    [Vendor] VARCHAR(40),
    [Product] VARCHAR(40),
    [Model] VARCHAR(40),
    [SerialNumber] VARCHAR(40),
    [FirmareVersion] VARCHAR(40),
    [DiveMode] VARCHAR(40),
    [BatteryVoltageEnd] INTEGER,
    [StartCNS] INTEGER,
    [EndCNS] INTEGER,
    [DiveNumber] INTEGER,
    [IsEmperial] BIT,
    [Descended] DATETIME2,
    [Surfaced] DATETIME2,
    [MaxDepth] INTEGER,
    [Minutes] INTEGER,
    [ImportedDateTime] DATETIME2 CONSTRAINT [DEF_DiveComputerLogs_ImportedDateTime] DEFAULT getdate(),
    CONSTRAINT [PK_DiveComputerLogs] PRIMARY KEY ([DiveComputerLogId])
)
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [DiveComputerLogs] ADD CONSTRAINT [Dives_DiveComputerLogs] 
    FOREIGN KEY ([DiveId]) REFERENCES [Dives] ([DiveId])
GO

