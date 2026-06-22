--CREATE PROCEDURE Sp_GetUserById
--@UserId INT

--AS
--BEGIN

--SELECT * FROM Users WHERE Id = @UserId; 

--END;




--CREATE PROCEDURE Sp_GetActiveUsers

--AS
--BEGIN

--SELECT * FROM Users WHERE IsActive = 1;

--END;





--CREATE PROCEDURE Sp_CreateUser
--@Name NVARCHAR(MAX),
--@Email NVARCHAR(MAX),
--@IsActive BIT,
--@RegistrationDate DATETIME,
--@BirthDate DATETIME

--AS
--BEGIN

--INSERT INTO Users (Name, Email, IsActive, RegistrationDate, BirthDate) VALUES (@Name, @Email, @IsActive, @RegistrationDate, @BirthDate);

--END;



--CREATE PROCEDURE Sp_UpdateUserEmail
--@UserId INT,
--@NewEmail NVARCHAR(MAX)

--AS
--BEGIN

--UPDATE Users SET Email = @NewEmail WHERE Id = @UserId;

--END;



--CREATE PROCEDURE Sp_DeleteUser
--@UserId INT

--AS
--BEGIN

--DELETE FROM Users WHERE Id = @UserId;

--END;




--CREATE PROCEDURE Sp_GetActiveUsersCount

--AS
--BEGIN
    
--SELECT COUNT(*) FROM Users WHERE IsActive = 1;

--END;





--CREATE PROCEDURE Sp_GetUsersByRegistrationDateRange
--@StartDate DATETIME,
--@EndDate DATETIME

--AS
--BEGIN

--SELECT * FROM Users WHERE RegistrationDate BETWEEN @StartDate AND @EndDate;

--END;




--CREATE PROCEDURE Sp_GetAverageUserAge

--AS
--BEGIN

--SELECT AVG(DATEDIFF(YEAR, BirthDate, GETDATE())) FROM Users;

--END;




--CREATE PROCEDURE Sp_GetOrdersByUserId
--@UserId INT

--AS
--BEGIN

--SELECT * FROM Orders WHERE UserId = @UserId;

--END;





--CREATE PROCEDURE Sp_GetProductsByPrice
--@MinPrice DECIMAL(18,2) = NULL,
--@MaxPrice DECIMAL(18,2) = NULL

--AS
--BEGIN

--SELECT * FROM Products WHERE (@MinPrice IS NULL OR Price >= @MinPrice) AND (@MaxPrice IS NULL OR Price <= @MaxPrice);

--END;






--CREATE PROCEDURE Sp_GetUserOrdersSummary
--@UserId INT

--AS
--BEGIN

--SELECT Id, OrderDate, TotalAmount FROM Orders WHERE UserId = @UserId;

--END;






--CREATE PROCEDURE Sp_GetMostExpensiveOrder

--AS
--BEGIN

--SELECT TOP 1 * FROM Orders ORDER BY TotalAmount DESC;

--END;