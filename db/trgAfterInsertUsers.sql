CREATE TRIGGER trgAfterInsertUsers
ON Users
AFTER INSERT
AS
BEGIN
    -- Insert into Contacts table
    INSERT INTO Contacts (UserId, Email)
    SELECT inserted.UserId, inserted.LoginId
    FROM inserted;
END