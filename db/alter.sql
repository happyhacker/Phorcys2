/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.6.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2026-02-28 22:41                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [Gear] DROP CONSTRAINT [Contacts_Gear]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [Users_Gear]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [Manufacturers_Gear]
GO


ALTER TABLE [DiverGear] DROP CONSTRAINT [Gear_DiverGear]
GO


ALTER TABLE [InsuredGear] DROP CONSTRAINT [Gear_InsuredGear]
GO


ALTER TABLE [ServiceSchedules] DROP CONSTRAINT [Gear_ServiceSchedules]
GO


ALTER TABLE [Tanks] DROP CONSTRAINT [Gear_Tanks]
GO


ALTER TABLE [DivePlanGear] DROP CONSTRAINT [Gear_GearOnDive]
GO


ALTER TABLE [GearServiceEvents] DROP CONSTRAINT [Gear_GearServiceEvents]
GO


ALTER TABLE [SoldGear] DROP CONSTRAINT [Gear_SoldGear]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Gear"                                         */
/* ---------------------------------------------------------------------- */

GO


/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [Gear] DROP CONSTRAINT [DF__Gear_TMP__Retail__160F4887]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [DF__Gear_TMP__Paid__17036CC0]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [DF__Gear_TMP__Weight__17F790F9]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [GearCreated]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [GearLastModified]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [PK_Gear]
GO


CREATE TABLE [Gear_TMP] (
    [GearId] INTEGER IDENTITY(1,1) NOT NULL,
    [ManufacturerId] INTEGER,
    [PurchasedFromContactId] INTEGER,
    [Title] VARCHAR(100) NOT NULL,
    [RetailPrice] MONEY CONSTRAINT [DF__Gear_TMP__Retail__160F4887] DEFAULT 0,
    [Paid] MONEY CONSTRAINT [DF__Gear_TMP__Paid__17036CC0] DEFAULT 0,
    [SN] VARCHAR(30),
    [Acquired] DATE,
    [NoLongerUse] DATE,
    [IsSelectable] BIT CONSTRAINT [DEF_Gear_IsSelectable] DEFAULT 1 NOT NULL,
    [Weight] FLOAT(53) CONSTRAINT [DF__Gear_TMP__Weight__17F790F9] DEFAULT 0,
    [Notes] VARCHAR(max),
    [UserId] INTEGER NOT NULL,
    [Created] DATETIME CONSTRAINT [GearCreated] DEFAULT getdate(),
    [LastModified] DATETIME CONSTRAINT [GearLastModified] DEFAULT getdate())
GO



SET IDENTITY_INSERT [Gear_TMP] ON
GO



INSERT INTO [Gear_TMP]
    ([GearId],[ManufacturerId],[PurchasedFromContactId],[Title],[RetailPrice],[Paid],[SN],[Acquired],[NoLongerUse],[Weight],[Notes],[UserId],[Created],[LastModified])
SELECT
    [GearId],[ManufacturerId],[PurchasedFromContactId],[Title],[RetailPrice],[Paid],[SN],[Acquired],[NoLongerUse],[Weight],[Notes],[UserId],[Created],[LastModified]
FROM [Gear]
GO



SET IDENTITY_INSERT [Gear_TMP] OFF
GO



DROP TABLE [Gear]
GO


EXEC sp_rename '[Gear_TMP]', 'Gear', 'OBJECT'
GO


ALTER TABLE [Gear] ADD CONSTRAINT [PK_Gear] 
    PRIMARY KEY CLUSTERED ([GearId])
GO


EXECUTE sp_addextendedproperty N'MS_Description', N'Mark as 0, false, if it should not be displayed on the DivePlan Gear selection.', 'SCHEMA', N'dbo', 'TABLE', N'Gear', NULL, NULL
GO


/* ---------------------------------------------------------------------- */
/* Add foreign key constraints                                            */
/* ---------------------------------------------------------------------- */

GO


ALTER TABLE [Gear] ADD CONSTRAINT [Users_Gear] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Manufacturers_Gear] 
    FOREIGN KEY ([ManufacturerId]) REFERENCES [Manufacturers] ([ManufacturerId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Contacts_Gear] 
    FOREIGN KEY ([PurchasedFromContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [GearServiceEvents] ADD CONSTRAINT [Gear_GearServiceEvents] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [InsuredGear] ADD CONSTRAINT [Gear_InsuredGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [ServiceSchedules] ADD CONSTRAINT [Gear_ServiceSchedules] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [DiverGear] ADD CONSTRAINT [Gear_DiverGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [DivePlanGear] ADD CONSTRAINT [Gear_GearOnDive] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO


ALTER TABLE [Tanks] ADD CONSTRAINT [Gear_Tanks] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId]) ON DELETE CASCADE ON UPDATE CASCADE
GO


ALTER TABLE [SoldGear] ADD CONSTRAINT [Gear_SoldGear] 
    FOREIGN KEY ([GearId]) REFERENCES [Gear] ([GearId])
GO

