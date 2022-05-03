USE topics;

SELECT * FROM tUser;
SELECT * FROM Follow;
SELECT * FROM Comment;
SELECT * FROM Post;
SELECT * FROM Topic;
SELECT * FROM Topic_Member;
SELECT * FROM Topic_Moderator;
SELECT * FROM User_Role;
SELECT * FROM Role;

INSERT INTO tUser 
(username, password, email)
VALUES 
	('test', 'pw', '@'),
	('hello', 'pw', '@'),
	('nikola', 'pw', '@');

INSERT INTO 
Follow (follower, following)
VALUES 
	('nikola', 'hello'), 
	('test', 'nikola'), 
	('nikola', 'test'), 
	('hello', 'nikola');

INSERT INTO 
Topic (name, owner)
VALUES 
	('topic1', 'nikola'),
	('topic2', 'test'),
	('topic3', 'hello');

INSERT INTO 
Topic_Member (topicName, memberName)
VALUES 
	( 'topic1' , 'test' ),
	( 'topic1', 'hello' ),
	( 'topic2', 'hello' ),
	( 'topic3', 'test' ),
	( 'topic2', 'nikola' ),
	( 'topic3', 'nikola' );

INSERT INTO
Topic_Moderator (topicName, moderatorName)
VALUES
	( 'topic2', 'nikola' ),
	( 'topic3', 'nikola' )
	( 'topic1', 'test' );
	

INSERT INTO 
Post (slug, ownerUsername, topicName)
VALUES 
	('post2', 'test', 'topic1')
	('post1', 'nikola', 'topic1')
	('post3', 'hello', 'topic2');


INSERT INTO 
Comment (ownerUsername, postSlug, topicName)
VALUES 
	('hello', 'post2', 'topic1')
	('nikola', 'post3', 'topic2')	
	('nikola', 'post2', 'topic1')	
	('nikola', 'post1', 'topic1');

