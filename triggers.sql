-- User triggers

use topics;

CREATE TRIGGER Trigger_User_Update
ON tUser
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON
	IF UPDATE(username)
	BEGIN

		DECLARE @update VARCHAR(64), @current VARCHAR(64);

		SELECT @update = username
		FROM inserted;

		SELECT @current = u.username
		FROM tUser u
		INNER JOIN Inserted i
			ON i.id = u.id;

		INSERT INTO 
		tUser (username, password, email, firstName, lastName, avatar, about)
		SELECT @update, password, email, firstName, lastName, avatar, about
		FROM tUser
		WHERE username = @current;

		UPDATE Follow
		SET following  = @update
		WHERE following = @current;

		UPDATE Follow
		SET follower = @update
		WHERE follower = @current;

		UPDATE Comment
		SET ownerUsername = @update
		WHERE ownerUsername = @current;

		UPDATE Topic
		SET owner = @update
		WHERE owner = @current;

		UPDATE Post
		SET ownerUsername = @update
		WHERE ownerUsername = @current;

		UPDATE Topic_Member
		SET memberName = @update
		WHERE memberName = @current;

		UPDATE Topic_Moderator
		SET moderatorName = @update
		WHERE moderatorName = @current;

		UPDATE User_Role
		SET username = @update
		WHERE username = @current;

		DELETE FROM tUser
		WHERE username = @current;
	END

	UPDATE tUser 
	SET password = i.password, 
		email = i.email, 
		firstName = i.firstName, 
		lastName = i.lastName, 
		avatar = i.avatar, 
		about = i.about
	FROM tUser u
		INNER JOIN inserted i
		ON i.username = u.username;
;

CREATE TRIGGER Trigger_User_Delete
ON tUser
INSTEAD OF DELETE
AS
	SET NOCOUNT ON
	DECLARE @username VARCHAR(64)

	SELECT @username = username
	FROM deleted;

	DELETE FROM Follow
	WHERE follower = @username OR following = @username;

	DELETE FROM Comment
	WHERE ownerUsername = @username;

	DELETE FROM Topic
	WHERE owner = @username;

	DELETE FROM Post
	WHERE ownerUsername = @username;

	DELETE FROM Topic_Member
	WHERE memberName = @username;

	DELETE FROM Topic_Moderator
	WHERE moderatorName = @username;

	DELETE FROM User_Role
	WHERE username = @username;

    DELETE FROM tUser 
	WHERE username = @username;
;