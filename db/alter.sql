/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.4.0                     */
/* Target DBMS:           MS SQL Server 2022                              */
/* Project file:          Phorcys2.dez                                    */
/* Project name:          Phorcys2                                        */
/* Author:                                                                */
/* Script type:           Alter database script                           */
/* Created on:            2024-09-07 23:13                                */
/* ---------------------------------------------------------------------- */


/* ---------------------------------------------------------------------- */
/* Drop views                                                             */
/* ---------------------------------------------------------------------- */

DROP VIEW [vwMyCertifications]
GO


DROP VIEW [vwCertifications]
GO


/* ---------------------------------------------------------------------- */
/* Drop foreign key constraints                                           */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Contacts] DROP CONSTRAINT [Countries_Contacts]
GO


ALTER TABLE [Contacts] DROP CONSTRAINT [Users_Contacts]
GO


ALTER TABLE [Users] DROP CONSTRAINT [Contacts_Users]
GO


ALTER TABLE [Users] DROP CONSTRAINT [AspNetUsers_Users]
GO


ALTER TABLE [Attributes] DROP CONSTRAINT [Users_Attributes]
GO


ALTER TABLE [Certifications] DROP CONSTRAINT [Users_Certifications]
GO


ALTER TABLE [DiveAgencies] DROP CONSTRAINT [Contacts_DiveAgencies]
GO


ALTER TABLE [DiveLocations] DROP CONSTRAINT [Users_DiveLocations]
GO


ALTER TABLE [DiveLocations] DROP CONSTRAINT [Contacts_DiveLocations]
GO


ALTER TABLE [DivePlans] DROP CONSTRAINT [Users_DivePlans]
GO


ALTER TABLE [Divers] DROP CONSTRAINT [Contacts_Divers]
GO


ALTER TABLE [Dives] DROP CONSTRAINT [Users_Dives]
GO


ALTER TABLE [DiveShops] DROP CONSTRAINT [Contacts_DiveShops]
GO


ALTER TABLE [DiveShopStaff] DROP CONSTRAINT [Contacts_DiveShopStaff]
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


ALTER TABLE [Gear] DROP CONSTRAINT [Contacts_Gear]
GO


ALTER TABLE [Instructors] DROP CONSTRAINT [Contacts_Instructors]
GO


ALTER TABLE [InsurancePolicies] DROP CONSTRAINT [Contacts_InsurancePolicies]
GO


ALTER TABLE [Manufacturers] DROP CONSTRAINT [Contacts_Manufactures]
GO


ALTER TABLE [Qualifications] DROP CONSTRAINT [Users_Qualifications]
GO


ALTER TABLE [Roles] DROP CONSTRAINT [Users_Roles]
GO


ALTER TABLE [SoldGear] DROP CONSTRAINT [Contacts_SoldGear]
GO


/* ---------------------------------------------------------------------- */
/* Alter table "Contacts"                                                 */
/* ---------------------------------------------------------------------- */

ALTER TABLE [Contacts] DROP CONSTRAINT [DEF_Contacts_Created]
GO


ALTER TABLE [Contacts] DROP CONSTRAINT [DEF_Contacts_LastModified]
GO


ALTER TABLE [Contacts] ALTER COLUMN [Company] VARCHAR(40)
GO


ALTER TABLE [Contacts] ALTER COLUMN [FirstName] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [LastName] VARCHAR(30)
GO


ALTER TABLE [Contacts] ALTER COLUMN [Address1] VARCHAR(40)
GO


ALTER TABLE [Contacts] ALTER COLUMN [Address2] VARCHAR(40)
GO


ALTER TABLE [Contacts] ALTER COLUMN [City] VARCHAR(30)
GO


ALTER TABLE [Contacts] ALTER COLUMN [State] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [PostalCode] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [Email] VARCHAR(50)
GO


ALTER TABLE [Contacts] ALTER COLUMN [CellPhone] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [HomePhone] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [WorkPhone] VARCHAR(20)
GO


ALTER TABLE [Contacts] ALTER COLUMN [Gender] CHAR(1)
GO


ALTER TABLE [Contacts] ALTER COLUMN [UserId] INTEGER
GO


ALTER TABLE [Contacts] ALTER COLUMN [Created] DATETIME
GO


ALTER TABLE [Contacts] ALTER COLUMN [LastModified] DATETIME
GO


ALTER TABLE [Contacts] ADD CONSTRAINT [DEF_Contacts_Created] 
    DEFAULT (getdate()) FOR [Created]
GO


ALTER TABLE [Contacts] ADD CONSTRAINT [DEF_Contacts_LastModified] 
    DEFAULT (getdate()) FOR [LastModified]
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


ALTER TABLE [Contacts] ADD CONSTRAINT [Countries_Contacts] 
    FOREIGN KEY ([CountryCode]) REFERENCES [Countries] ([CountryCode])
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


ALTER TABLE [Instructors] ADD CONSTRAINT [Contacts_Instructors] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Divers] ADD CONSTRAINT [Contacts_Divers] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [DiveShopStaff] ADD CONSTRAINT [Contacts_DiveShopStaff] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [DiveShops] ADD CONSTRAINT [Contacts_DiveShops] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [DiveLocations] ADD CONSTRAINT [Contacts_DiveLocations] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Dives] ADD CONSTRAINT [Users_Dives] 
    FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId])
GO


ALTER TABLE [InsurancePolicies] ADD CONSTRAINT [Contacts_InsurancePolicies] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Manufacturers] ADD CONSTRAINT [Contacts_Manufactures] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [SoldGear] ADD CONSTRAINT [Contacts_SoldGear] 
    FOREIGN KEY ([SoldToContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [Gear] ADD CONSTRAINT [Contacts_Gear] 
    FOREIGN KEY ([PurchasedFromContactId]) REFERENCES [Contacts] ([ContactId])
GO


ALTER TABLE [DiveAgencies] ADD CONSTRAINT [Contacts_DiveAgencies] 
    FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
GO


/* ---------------------------------------------------------------------- */
/* Repair/add views                                                       */
/* ---------------------------------------------------------------------- */

CREATE VIEW [vwMyCertifications] AS (
SELECT dc.DiverCertificationId, certs.CertificationId, certs.Title, divers.DiverId, u.UserId, agency.Company AS Agency, dc.Certified, dc.CertificationNum,
         Contacts.FirstName AS DiverFirstName, Contacts.LastName AS DiverLastName, instructor.FirstName AS InstructorFirstName,
         instructor.LastName AS InstructorLastName, dc.Notes, dc.Created, dc.LastModified
FROM  dbo.Certifications AS certs INNER JOIN
         dbo.DiverCertifications AS dc ON dc.CertificationId = certs.CertificationId INNER JOIN
         dbo.Divers AS divers ON divers.DiverId = dc.DiverId INNER JOIN
         dbo.Contacts AS Contacts ON Contacts.ContactId = divers.ContactId LEFT OUTER JOIN
         dbo.Users AS u ON u.UserId = Contacts.UserId LEFT OUTER JOIN
		 dbo.Instructors ON dbo.Instructors.InstructorId = dc.InstructorId LEFT OUTER JOIN
         dbo.Contacts AS instructor ON instructor.ContactId = dbo.Instructors.ContactId INNER JOIN
         dbo.DiveAgencies AS da ON da.DiveAgencyId = certs.DiveAgencyId INNER JOIN
         dbo.Contacts AS agency ON agency.ContactId = da.ContactId
)
GO


/* ---------------------------------------------------------------------- */
/* Repair/add triggers                                                    */
/* ---------------------------------------------------------------------- */

CREATE TRIGGER [trgAfterInsertUsers]
ON Users
AFTER INSERT
AS
BEGIN
    -- Insert into Contacts table
    INSERT INTO Contacts (UserId, Email)
    SELECT inserted.UserId, inserted.LoginId
    FROM inserted;
END
GO

