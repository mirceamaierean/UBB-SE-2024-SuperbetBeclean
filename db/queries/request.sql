CREATE PROCEDURE CreateRequest
    @fromUser INT,
    @toUser INT,
    @date DATE
AS
BEGIN
    INSERT INTO Request (request_fromUser, request_toUser, request_date)
    VALUES (@fromUser, @toUser, @date)
END
GO

CREATE PROCEDURE GetRequestByID
    @request_id INT
AS
BEGIN
    SELECT * FROM Request WHERE request_id = @request_id
END
GO

CREATE PROCEDURE GetAllRequests
AS
BEGIN
    SELECT * FROM Request
END
GO

CREATE PROCEDURE GetAllRequestsByFromUserID
    @fromUser INT
AS
BEGIN
    SELECT * FROM Request WHERE request_fromUser = @fromUser
END
GO

CREATE PROCEDURE GetAllRequestsByToUserID
    @toUser INT
AS
BEGIN
    SELECT * FROM Request WHERE request_toUser = @toUser
END
GO

CREATE PROCEDURE DeleteOldRequests
AS
BEGIN
    DELETE FROM Request WHERE request_date < DATEADD(YEAR, -1, GETDATE())
END
