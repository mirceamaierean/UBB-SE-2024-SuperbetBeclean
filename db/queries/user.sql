-- Procedures for user CRUD operations

-- Create
CREATE OR ALTER PROCEDURE createUser
    @username VARCHAR(128),
    @currentFont INT,
    @currentTitle INT,
    @currentIcon INT,
    @currentTable INT,
    @chips INT,
    @stack INT,
    @streak INT,
    @handsPlayed INT,
    @level INT,
    @lastLogin DATETIME
AS
BEGIN
    INSERT INTO Users
        (user_username, user_currentFont, user_currentTitle, user_currentIcon, user_currentTable, user_chips, user_stack, user_streak, user_handsPlayed, user_level, user_lastLogin)
    VALUES
        (@username, @currentFont, @currentTitle, @currentIcon, @currentTable, @chips, @stack, @streak, @handsPlayed, @level, @lastLogin)
END
GO

-- Read
CREATE OR ALTER PROCEDURE getUser
    @username VARCHAR(128)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE user_username = @username
END
GO

CREATE OR ALTER PROCEDURE gerUserById
    @id INT
AS
BEGIN
    SELECT *
    FROM Users
    WHERE user_id = @id
END
GO

-- Read All
CREATE OR ALTER PROCEDURE getAllUsers
AS
BEGIN
    SELECT *
    FROM Users
END
GO

-- Update
CREATE OR ALTER PROCEDURE updateUser
    @id INT,
    @username VARCHAR(128),
    @currentFont INT,
    @currentTitle INT,
    @currentIcon INT,
    @currentTable INT,
    @chips INT,
    @stack INT,
    @streak INT,
    @handsPlayed INT,
    @level INT,
    @lastLogin DATETIME
AS
BEGIN
    UPDATE Users
    SET
        user_username = @username,
        user_currentFont = @currentFont,
        user_currentTitle = @currentTitle,
        user_currentIcon = @currentIcon,
        user_currentTable = @currentTable,
        user_chips = @chips,
        user_stack = @stack,
        user_streak = @streak,
        user_handsPlayed = @handsPlayed,
        user_level = @level,
        user_lastLogin = @lastLogin
    WHERE user_id = @id
END
GO

-- Delete
CREATE OR ALTER PROCEDURE deleteUser
    @username VARCHAR(128)
AS
BEGIN
    DELETE FROM Users
    WHERE user_username = @username
END
GO

CREATE OR ALTER PROCEDURE deleteUserById
    @id INT
AS
BEGIN
    DELETE FROM Users
    WHERE user_id = @id
END
GO
