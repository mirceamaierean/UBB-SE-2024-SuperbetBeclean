-- Create 
CREATE OR ALTER PROCEDURE createIcon
    @icon_name VARCHAR(255),
    @icon_price INT,
    @icon_image IMAGE
AS
BEGIN
    INSERT INTO Icon
        (icon_name, icon_price, icon_image)
    VALUES
        (@icon_name, @icon_price, @icon_image)
END
GO

-- Read
CREATE OR ALTER PROCEDURE getIcon
    @icon_name VARCHAR(255)
AS
BEGIN
    SELECT *
    FROM Icon
    WHERE icon_name = @icon_name
END
GO

CREATE OR ALTER PROCEDURE getIconByID
    @icon_id INT
AS
BEGIN
    SELECT *
    FROM Icon
    WHERE icon_id = @icon_id
END
GO

-- Read All
CREATE OR ALTER PROCEDURE getAllIcons
AS
BEGIN
    SELECT *
    FROM Icon
END
GO

-- Update
CREATE OR ALTER PROCEDURE updateIcon
    @icon_id INT,
    @icon_name VARCHAR(255),
    @icon_price INT,
    @icon_image IMAGE
AS
BEGIN
    UPDATE Icon
    SET 
        icon_name = @icon_name,
        icon_price = @icon_price,
        icon_image = @icon_image
    WHERE 
        icon_id = @icon_id
END
GO

-- Delete
CREATE OR ALTER PROCEDURE deleteIcon
    @icon_name VARCHAR(255)
AS
BEGIN
    DELETE FROM Icon WHERE icon_name = @icon_name
END
GO

CREATE OR ALTER PROCEDURE deleteIconById
    @icon_id INT
AS
BEGIN
    DELETE FROM Icon WHERE icon_id = @icon_id
END
GO
