CREATE PROCEDURE CreateChallenge --CREATE
    @challenge_description VARCHAR(MAX),
    @challenge_rule VARCHAR(MAX),
    @challenge_amount INT,
    @challenge_reward INT
AS
BEGIN
    INSERT INTO Challenge (challenge_description, challenge_rule, challenge_amount, challenge_reward)
    VALUES (@challenge_description, @challenge_rule, @challenge_amount, @challenge_reward)
END
GO

CREATE PROCEDURE GetChallenge -- READ & READ ALL
    @challenge_id INT
AS
BEGIN
    SELECT * FROM Challenge WHERE challenge_id = @challenge_id
END
GO

CREATE PROCEDURE GetAllChallenges
AS
BEGIN
    SELECT * FROM Challenge
END
GO

CREATE PROCEDURE UpdateChallenge --UPDATE
    @challenge_id INT,
    @challenge_description VARCHAR(MAX),
    @challenge_rule VARCHAR(MAX),
    @challenge_amount INT,
    @challenge_reward INT
AS
BEGIN
    UPDATE Challenge
    SET 
        challenge_description = @challenge_description,
        challenge_rule = @challenge_rule,
        challenge_amount = @challenge_amount,
        challenge_reward = @challenge_reward
    WHERE 
        challenge_id = @challenge_id
END
GO

CREATE PROCEDURE DeleteChallenge --DELETE 
    @challenge_id INT
AS
BEGIN
    DELETE FROM Challenge WHERE challenge_id = @challenge_id
END