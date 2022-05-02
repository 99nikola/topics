CREATE DATABASE topics;

USE topics;

CREATE TABLE Role (
	name VARCHAR(32) PRIMARY KEY
);

INSERT INTO 
Role (name)
VALUES ('basic');


CREATE TABLE tUser (
	id INT NOT NULL IDENTITY(1,1),
	username VARCHAR(64) PRIMARY KEY,
	email VARCHAR(128) NOT NULL,
	password VARCHAR(256) NOT NULL,
	firstName VARCHAR(32),
	lastName VARCHAR(32),
	avatar VARCHAR(256),
	about TEXT
);

CREATE TABLE User_Role (
	username VARCHAR(64),
	roleName VARCHAR(32) DEFAULT 'basic',
	PRIMARY KEY(username, roleName),

	CONSTRAINT fk_username
		FOREIGN KEY (username)
		REFERENCES tUser(username)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT fk_rolename
		FOREIGN KEY (roleName)
		REFERENCES Role(name)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE Follow (
	follower VARCHAR(64),
	following VARCHAR(64),
	PRIMARY KEY (follower, following),

	CONSTRAINT FK_F_follower 
	FOREIGN KEY (follower)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	CONSTRAINT FK_F_following
	FOREIGN KEY (following)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
	-- Manualy CASCADE (ON DELETE and ON UPDATE)
);


CREATE TRIGGER Trigger_User_Update
ON tUser
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON
	IF UPDATE(username)
	BEGIN

		DECLARE @update VARCHAR(64), @current VARCHAR(64)

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
		WHERE username = @current

		UPDATE Follow
		SET following  = @update
		WHERE following = @current

		UPDATE Follow
		SET follower = @update
		WHERE follower = @current

		UPDATE Comment
		SET ownerUsername = @update
		WHERE ownerUsername = @current

		DELETE FROM tUser
		WHERE username = @current
	END
	ELSE
	BEGIN
		UPDATE tUser 
		SET password = i.password, 
			email = i.email, 
			firstName = i.firstName, 
			lastName = i.lastName, 
			avatar = i.avatar, 
			about = i.about
		FROM tUser u
			INNER JOIN inserted i
			ON i.username = u.username
	END
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
	WHERE ownerUsername = @username

    DELETE FROM tUser WHERE username IN (SELECT username FROM deleted)
;


CREATE TABLE Topic (
	name VARCHAR(64) PRIMARY KEY,
	title VARCHAR(256),
	description TEXT,
	cover VARCHAR(256),
	owner VARCHAR(64) NOT NULL,

	CONSTRAINT fk_owner
		FOREIGN KEY (owner)
		REFERENCES tUser(username) 
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE Topic_Moderator (
	topicName VARCHAR(64),
	moderatorName VARCHAR(64),
	PRIMARY KEY (topicName, moderatorName),

	CONSTRAINT fk_moderatorName
	FOREIGN KEY (moderatorName)
	REFERENCES tUser(username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,

	CONSTRAINT fk_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
	-- Manulay CASCADE
);


CREATE TABLE Topic_Member (
	topicName VARCHAR(64) FOREIGN KEY REFERENCES Topic(name),
	memberName VARCHAR(64) FOREIGN KEY REFERENCES tUser(username),
	PRIMARY KEY (topicName, memberName),

	CONSTRAINT fk_memberName
	FOREIGN KEY (memberName)
	REFERENCES tUser(username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,

	CONSTRAINT fk_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	--ON DELETE CASCADE
	--ON UPDATE CASCADE
);

CREATE TABLE Post (
	slug VARCHAR(256) PRIMARY KEY,
	title VARCHAR(256),
	content VARBINARY(MAX),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	dateCreated DATETIME DEFAULT GETDATE()
);

CREATE TABLE Comment (
	id INT PRIMARY KEY IDENTITY(1,1),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	parent INT,
	ownerUsername VARCHAR(64) NOT NULL,
	postSlug VARCHAR(256),

	CONSTRAINT fk_parent
	FOREIGN KEY (parent)
	REFERENCES Comment(id)
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION,
	-- Manualy CASCADE

	CONSTRAINT fk_ownerUsername
	FOREIGN KEY (ownerUsername)
	REFERENCES tUser(username)
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION,
	-- Manulay CASCADE

	CONSTRAINT fk_postSlug
	FOREIGN KEY (postSlug)
	REFERENCES Post(slug) 
	ON DELETE CASCADE 
	ON UPDATE CASCADE
);

drop table Topic_Post_User


CREATE TABLE Topic_Post_User (
	topicName VARCHAR(64),
	postSlug VARCHAR(256),
	ownerUsername VARCHAR(64),
	PRIMARY KEY (topicName, postSlug, ownerUsername),

	CONSTRAINT FK_TPU_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	-- Manualy CASCADE

	CONSTRAINT FK_TPU_postSlug
	FOREIGN KEY (postSlug)
	REFERENCES Post(slug) 
	ON DELETE CASCADE 
	ON UPDATE CASCADE,

	CONSTRAINT FK_TPU_ownerUsername
	FOREIGN KEY (ownerUsername)
	REFERENCES tUser(username)
	ON DELETE CASCADE 
	ON UPDATE CASCADE
);
