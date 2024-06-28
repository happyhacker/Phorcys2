CREATE PROCEDURE CreateUserLink
    @UserName NVARCHAR(256),
    @AspNetUserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN
        -- Insert the new record into Users table linking it with AspNetUsers
        INSERT INTO Users (LoginId, AspNetUserId, Password)
        VALUES (@UserName, @AspNetUserId, 'password');
    END
END