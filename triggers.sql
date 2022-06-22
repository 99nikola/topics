CREATE TRIGGER TG_Vote_Post_Insert
ON [Vote]
AFTER INSERT
AS
BEGIN
	DECLARE @slug VARCHAR(256), @type BIT, @upVotes INT, @downVotes INT;

	SELECT @slug = postSlug, @type = vType
	FROM Inserted

	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Post]
	WHERE slug = @slug;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes + 1;

		UPDATE [Post]
		SET upVotes = @upVotes
		WHERE slug = @slug;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes + 1;

		UPDATE [Post]
		SET downVotes = @downVotes
		WHERE slug = @slug;
	END

END

CREATE TRIGGER TG_Vote_Post_Delete
ON [Vote]
AFTER DELETE
AS
BEGIN
	DECLARE @slug VARCHAR(256), @type BIT, @upVotes INT, @downVotes INT;

	SELECT @slug = postSlug, @type = vType
	FROM Insert


	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Post]
	WHERE slug = @slug;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes - 1;

		UPDATE [Post]
		SET upVotes = @upVotes
		WHERE slug = @slug;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes - 1;

		UPDATE [Post]
		SET downVotes = @downVotes
		WHERE slug = @slug;
	END
END


CREATE TRIGGER TG_Vote_Post_Update
ON [Vote]
AFTER UPDATE
AS
BEGIN
	DECLARE @slug VARCHAR(256), @type BIT, @upVotes INT, @downVotes INT;

	SELECT @slug = postSlug, @type = vType
	FROM Inserted


	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Post]
	WHERE slug = @slug;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes + 1;
		SET @downVotes = @downVotes - 1;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes + 1;
		SET @upVotes = @upVotes - 1;
	END

	UPDATE [Post]
	SET upVotes = @upVotes,
		downVotes = @downVotes
	WHERE slug = @slug;

END


-- Vote Comment


CREATE TRIGGER TG_Vote_Comment_Insert
ON [VoteComment]
AFTER INSERT
AS
BEGIN
	DECLARE @id INT, @type BIT, @upVotes INT, @downVotes INT;

	SELECT @id = commentId, @type = vType
	FROM Inserted

	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Comment]
	WHERE id = @id;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes + 1;

		UPDATE [Comment]
		SET upVotes = @upVotes
		WHERE id = @id;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes + 1;

		UPDATE [Comment]
		SET downVotes = @downVotes
		WHERE id = @id;
	END

END

CREATE TRIGGER TG_Vote_Comment_Delete
ON [VoteComment]
AFTER DELETE
AS
BEGIN
	DECLARE @id INT, @type BIT, @upVotes INT, @downVotes INT;

	SELECT @id = commentId, @type = vType
	FROM Inserted

	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Comment]
	WHERE id = @id;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes - 1;

		UPDATE [Comment]
		SET upVotes = @upVotes
		WHERE id = @id;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes - 1;

		UPDATE [Comment]
		SET downVotes = @downVotes
		WHERE id = @id;
	END
END


CREATE TRIGGER TG_Vote_Comment_Post_Update
ON [VoteComment]
AFTER UPDATE
AS
BEGIN
	DECLARE @id INT, @type BIT, @upVotes INT, @downVotes INT;

	SELECT @id = commentId, @type = vType
	FROM Inserted


	SELECT @upVotes = upVotes, @downVotes = downVotes
	FROM [Comment]
	WHERE id = @id;

	IF (@type = 1)
	BEGIN 
		SET @upVotes = @upVotes + 1;
		SET @downVotes = @downVotes - 1;
	END
	ELSE
	BEGIN
		SET @downVotes = @downVotes + 1;
		SET @upVotes = @upVotes - 1;
	END

	UPDATE [Comment]
	SET upVotes = @upVotes,
		downVotes = @downVotes
	WHERE id = @id;

END