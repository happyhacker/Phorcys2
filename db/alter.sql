/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.4.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2024-05-01 20:25                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Users] DROP CONSTRAINT [Contacts_Users]
GO


ALTER TABLE [Attributes] DROP CONSTRAINT [Users_Attributes]
GO


ALTER TABLE [Certifications] DROP CONSTRAINT [Users_Certifications]
GO


ALTER TABLE [Contacts] DROP CONSTRAINT [Users_Contacts]
GO


ALTER TABLE [DiveLocations] DROP CONSTRAINT [Users_DiveLocations]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [Users_DivePlans]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [Users_Dives]
GO


ALTER TABLE [DiveSites] DROP CONSTRAINT [Users_DiveSites]
GO


ALTER TABLE [DiveTypes] DROP CONSTRAINT [Users_DiveTypes]
GO


ALTER TABLE [Friends] DROP CONSTRAINT [Users_Friends1]
GO


ALTER TABLE [Friends] DROP CONSTRAINT [Users_Friends2]
GO


ALTER TABLE [Gear] DROP CONSTRAINT [Users_Gear]
GO


ALTER TABLE [Qualifications] DROP CONSTRAINT [Users_Qualifications]
GO


ALTER TABLE [Roles] DROP CONSTRAINT [Users_Roles]
GO


/* ---------------------------------------------------------------------- */
/* Drop and recreate table "Users"                                        */
/* ---------------------------------------------------------------------- */

/* Table must be recreated because some of the changes can't be done with the regular commands available. */

ALTER TABLE [Users] DROP CONSTRAINT [DEF_Users_LoginCount]
GO


ALTER TABLE [Users] DROP CONSTRAINT [DEF_Users_Created]
GO


ALTER TABLE [Users] DROP CONSTRAINT [DEF_Users_LastModified]
GO


ALTER TABLE [Users] DROP CONSTRAINT [PK_Users]
GO


ALTER TABLE [Users] DROP CONSTRAINT [TUC_Users_1]
GO


CREATE TABLE [Users_TMP] (
    [UserId] INTEGER IDENTITY(1,1) NOT NULL,
    [LoginId] VARCHAR(30) NOT NULL,
    [Password] VARCHAR(20) NOT NULL,
    [LoginCount] INTEGER CONSTRAINT [DEF_Users_LoginCount] DEFAULT 0,
    [ContactId] INTEGER,
    [AspNetUserId] NVARCHAR(450),
    [Created] DATETIME CONSTRAINT [DEF_Users_Created] DEFAULT getdate() NOT NULL,
    [LastModified] DATETIME CONSTRAINT [DEF_Users_LastModified] DEFAULT getdate() NOT NULL)
GO



SET IDENTITY_INSERT [Users_TMP] ON
GO



INSERT INTO [Users_TMP]
    ([UserId],[LoginId],[Password],[LoginCount],[ContactId],[Created],[LastModified])
SELECT
    [UserId],[LoginId],[Password],[LoginCount],[ContactId],[Created],[LastModified]
FROM [Users]
GO



SET IDENTITY_INSERT [Users_TMP] OFF
GO



DROP TABLE [Users]
GO


EXEC sp_rename '[Users_TMP]', 'Users', 'OBJECT'
GO


ALTER TABLE [Users] ADD CONSTRAINT [PK_Users] 
    PRIMARY KEY CLUSTERED ([UserId])
GO


ALTER TABLE [Users] ADD CONSTRAINT [TUC_Users_1] 
    UNIQUE ([LoginId])
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


ALTER TABLE [Contacts] ADD CONSTRAINT [Users_Contacts] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DiveLocations] ADD CONSTRAINT [Users_DiveLocations] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DivePlans] ADD CONSTRAINT [Users_DivePlans] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DiveTypes] ADD CONSTRAINT [Users_DiveTypes] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Roles] ADD CONSTRAINT [Users_Roles] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Users_Gear] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Certifications] ADD CONSTRAINT [Users_Certifications] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Qualifications] ADD CONSTRAINT [Users_Qualifications] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Friends] ADD CONSTRAINT [Users_Friends1] 
    FOREIGN KEY ([RequestorUserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Friends] ADD CONSTRAINT [Users_Friends2] 
    FOREIGN KEY ([RecipientUserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Attributes] ADD CONSTRAINT [Users_Attributes] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [DiveSites] ADD CONSTRAINT [Users_DiveSites] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Users_Dives] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO

