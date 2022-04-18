CREATE DATABASE topics;

USE topics;

CREATE TABLE Gender (
	name VARCHAR(16) PRIMARY KEY
);

INSERT INTO Gender (name)
VALUES ('male'),
		('female');

CREATE TABLE AppUser (
	username VARCHAR(64) PRIMARY KEY,
	email VARCHAR(128) NOT NULL,
	password VARCHAR(256) NOT NULL,
	displayName VARCHAR(64),
	avatar VARCHAR(256),
	gender VARCHAR(16) FOREIGN KEY REFERENCES Gender(name),
	about TEXT
);

CREATE TABLE Follow (
	follower VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username),
	following VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username),
	PRIMARY KEY (follower, following)
);

CREATE TABLE Topic (
	name VARCHAR(64) PRIMARY KEY,
	title VARCHAR(256),
	description TEXT,
	cover VARCHAR(256),
	owner VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username) NOT NULL
);

CREATE TABLE Topic_Moderator (
	topicName VARCHAR(64) FOREIGN KEY REFERENCES Topic(name),
	moderatorName VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username),
	PRIMARY KEY (topicName, moderatorName)
);

CREATE TABLE Topic_Member (
	topicName VARCHAR(64) FOREIGN KEY REFERENCES Topic(name),
	memberName VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username),
	PRIMARY KEY (topicName, memberName)
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
	parent INT FOREIGN KEY REFERENCES Comment(id) NOT NULL,
	ownerUsername VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username) NOT NULL,
	postSlug VARCHAR(256) FOREIGN KEY REFERENCES Post(slug) NOT NULL
);



CREATE TABLE Topic_Post_User (
	topicName VARCHAR(64) FOREIGN KEY REFERENCES Topic(name),
	postSlug VARCHAR(256) FOREIGN KEY REFERENCES Post(slug),
	ownerUsername VARCHAR(64) FOREIGN KEY REFERENCES AppUser(username),
	PRIMARY KEY (topicName, postSlug, ownerUsername)
);
