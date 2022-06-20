CREATE DATABASE topics;
USE topics;


CREATE TABLE [Role] (
	name VARCHAR(16) PRIMARY KEY
);

INSERT INTO 
[Role] (name)
VALUES ('basic'), ('admin');



CREATE TABLE [User] (
	id INT NOT NULL IDENTITY(1,1),
	username VARCHAR(64) PRIMARY KEY,
	email VARCHAR(128) UNIQUE NOT NULL,
	password VARCHAR(256) NOT NULL,
	role VARCHAR(16) FOREIGN KEY REFERENCES [Role](name),
	firstName VARCHAR(32),
	lastName VARCHAR(32),
	avatar VARCHAR(256),
	about TEXT
);



CREATE TABLE [Follow] (
	follower VARCHAR(64),
	following VARCHAR(64),
	PRIMARY KEY(follower, following),

	CONSTRAINT FK_Follow_follower 
	FOREIGN KEY (follower)
	REFERENCES [User](username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,

	CONSTRAINT FK_Follow_following
	FOREIGN KEY (following)
	REFERENCES [User](username)
);


CREATE TABLE [Topic] (
	id INT NOT NULL IDENTITY(1,1),
	name VARCHAR(64) PRIMARY KEY,
	title VARCHAR(256),
	description TEXT,
	cover VARBINARY(MAX),
	avatar VARBINARY(MAX),
	owner VARCHAR(64) NOT NULL,

	CONSTRAINT FK_Topic_owner
	FOREIGN KEY (owner)
	REFERENCES [User](username) 
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);


CREATE TABLE [Topic_Moderator] (
	topicName VARCHAR(64),
	moderatorName VARCHAR(64),
	PRIMARY KEY (topicName, moderatorName),

	CONSTRAINT FK_TopicModerator_moderatorName
	FOREIGN KEY (moderatorName)
	REFERENCES [User](username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,

	CONSTRAINT FK_TopicModerator_topicName
	FOREIGN KEY (topicName)
	REFERENCES [Topic](name)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);


CREATE TABLE [Topic_Member] (
	topicName VARCHAR(64),
	memberName VARCHAR(64),
	PRIMARY KEY (topicName, memberName),

	CONSTRAINT FK_TopicMember_memberName
	FOREIGN KEY (memberName)
	REFERENCES [User](username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	
	CONSTRAINT FK_TopicMember_topicName
	FOREIGN KEY (topicName)
	REFERENCES [Topic](name)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);


CREATE TABLE [Post] (
	id INT NOT NULL IDENTITY(1,1),
	slug VARCHAR(256) PRIMARY KEY,
	username VARCHAR(64),
	topicName VARCHAR(64),
	title VARCHAR(256),
	content VARBINARY(MAX),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	dateCreated DATETIME DEFAULT GETDATE(),

	CONSTRAINT FK_Post_ownerUsername
	FOREIGN KEY (username)
	REFERENCES [User](username)
	ON DELETE CASCADE
	ON UPDATE CASCADE,

	CONSTRAINT FK_Post_topicName
	FOREIGN KEY (topicName)
	REFERENCES [Topic]  (name)
	ON DELETE CASCADE
	ON UPDATE CASCADE	
);



CREATE TABLE [Vote] (
	username VARCHAR(64),
	postSlug VARCHAR(256),
	vType BIT,

	PRIMARY KEY(username, postSlug),

	CONSTRAINT FK_Vote_username
	FOREIGN KEY (username)
	REFERENCES [User](username),

	CONSTRAINT FK_Vote_postSlug
	FOREIGN KEY (postSlug)
	REFERENCES [Post](slug)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
);

CREATE TABLE [Comment] (
	id INT PRIMARY KEY IDENTITY(1,1),
	upVotes INT DEFAULT 0,
	downVotes INT DEFAULT 0,
	parent INT,
	ownerUsername VARCHAR(64) NOT NULL,
	postSlug VARCHAR(256),
	topicName VARCHAR(64),

	CONSTRAINT FK_Comment_parent
	FOREIGN KEY (parent)
	REFERENCES [Comment](id)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	CONSTRAINT FK_Comment_ownerUsername
	FOREIGN KEY (ownerUsername)
	REFERENCES [User](username)
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION,

	CONSTRAINT FK_Comment_postSlug
	FOREIGN KEY (postSlug)
	REFERENCES [Post](slug) 
	ON DELETE CASCADE
	ON UPDATE CASCADE
);
