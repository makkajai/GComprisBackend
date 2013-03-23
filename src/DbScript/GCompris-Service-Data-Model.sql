/*Data model for GCompris backend service*/

CREATE TABLE boards
(
	board_id INTEGER,
	name TEXT,
	difficulty INTEGER,
	title TEXT,
	description TEXT,
	prerequisite TEXT,
	goal TEXT
)
;

CREATE TABLE logs
(
	date TEXT,
	duration INTEGER,
	user_id INTEGER,
	board_id INTEGER,
	level INTEGER,
	sublevel INTEGER,
	status INTEGER,
	comment TEXT
)
;

CREATE TABLE users
(
	user_id INTEGER,
	login TEXT,
	lastname TEXT,
	firstname TEXT
)
;