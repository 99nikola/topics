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

	CONSTRAINT FK_UserRole_username
		FOREIGN KEY (username)
		REFERENCES tUser(username)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION,
	CONSTRAINT FK_UserRole_roleName
		FOREIGN KEY (roleName)
		REFERENCES Role(name)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);

CREATE TABLE Follow (
	follower VARCHAR(64),
	following VARCHAR(64),
	PRIMARY KEY (follower, following),

	CONSTRAINT FK_Follow_follower 
	FOREIGN KEY (follower)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	CONSTRAINT FK_Follow_following
	FOREIGN KEY (following)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION

	-- Manualy CASCADE (ON DELETE and ON UPDATE)
);

CREATE TABLE Topic (
	id INT NOT NULL IDENTITY(1,1),
	name VARCHAR(64) PRIMARY KEY,
	title VARCHAR(256),
	description TEXT,
	cover VARCHAR(256),
	owner VARCHAR(64) NOT NULL,

	CONSTRAINT FK_Topic_owner
		FOREIGN KEY (owner)
		REFERENCES tUser(username) 
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
);

CREATE TABLE Topic_Moderator (
	topicName VARCHAR(64),
	moderatorName VARCHAR(64),
	PRIMARY KEY (topicName, moderatorName),

	CONSTRAINT FK_TopicModerator_moderatorName
	FOREIGN KEY (moderatorName)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	-- Manulay CASCADE

	CONSTRAINT FK_TopicModerator_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	ON DELETE CASCADE
	ON UPDATE CASCADE
)

CREATE TABLE Topic_Member (
	topicName VARCHAR(64),
	memberName VARCHAR(64),
	PRIMARY KEY (topicName, memberName),

	CONSTRAINT FK_TopicMember_memberName
	FOREIGN KEY (memberName)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	--Manualy CASCADE
	
	CONSTRAINT FK_TopicMember_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);


CREATE TABLE Post (
	id INT NOT NULL IDENTITY(1,1),
	slug VARCHAR(256) PRIMARY KEY,
	ownerUsername VARCHAR(64),
	topicName VARCHAR(64),
	title VARCHAR(256),
	content VARBINARY(MAX),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	dateCreated DATETIME DEFAULT GETDATE(),

	CONSTRAINT FK_Post_ownerUsername
	FOREIGN KEY (ownerUsername)
	REFERENCES tUser(username)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,
	-- Manualy CASCADE

	CONSTRAINT FK_Post_topicName
	FOREIGN KEY (topicName)
	REFERENCES Topic(name)
	ON DELETE CASCADE
	ON UPDATE CASCADE	
	-- Manualy CASCADE
);

CREATE TABLE Comment (
	id INT PRIMARY KEY IDENTITY(1,1),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	parent INT,
	ownerUsername VARCHAR(64) NOT NULL,
	postSlug VARCHAR(256),
	topicName VARCHAR(64),

	CONSTRAINT FK_Comment_parent
	FOREIGN KEY (parent)
	REFERENCES Comment(id)
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION,
	-- Manualy CASCADE

	CONSTRAINT FK_Comment_ownerUsername
	FOREIGN KEY (ownerUsername)
	REFERENCES tUser(username)
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION,

	CONSTRAINT FK_Comment_postSlug
	FOREIGN KEY (postSlug)
	REFERENCES Post(slug) 
	ON DELETE CASCADE
	ON UPDATE CASCADE
);
