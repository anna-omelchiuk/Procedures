ALTER PROCEDURE Sp_GetUserById
    @UserId INT
AS
BEGIN
    -- Зверніть увагу: тут має бути саме Usrs, як у вашій БД ліворуч!
    SELECT * FROM Users WHERE Id = @UserId; 
END;

--CREATE PROCEDURE Sp_GetActiveUsers
--AS
--BEGIN
--    SELECT * FROM Users WHERE IsActive = 1;
--END;

