CREATE VIEW vwMyCertifications
AS
SELECT dc.DiverCertificationId, certs.CertificationId, certs.Title, divers.DiverId, u.UserId, agency.Company AS Agency, dc.Certified, dc.CertificationNum, 
         Contacts.FirstName AS DiverFirstName, Contacts.LastName AS DiverLastName, instructor.FirstName AS InstructorFirstName,
         instructor.LastName AS InstructorLastName, dc.Notes, dc.Created, dc.LastModified
FROM  dbo.Certifications AS certs INNER JOIN
         dbo.DiverCertifications AS dc ON dc.CertificationId = certs.CertificationId INNER JOIN
         dbo.Divers AS divers ON divers.DiverId = dc.DiverId INNER JOIN
         dbo.Contacts AS Contacts ON Contacts.ContactId = divers.ContactId LEFT OUTER JOIN
         dbo.Users AS u ON u.UserId = Contacts.UserId INNER JOIN        
		 dbo.Instructors ON dbo.Instructors.InstructorId = dc.InstructorId LEFT OUTER JOIN
         dbo.Contacts AS instructor ON instructor.ContactId = dbo.Instructors.ContactId INNER JOIN
         dbo.DiveAgencies AS da ON da.DiveAgencyId = certs.DiveAgencyId INNER JOIN
         dbo.Contacts AS agency ON agency.ContactId = da.ContactId
