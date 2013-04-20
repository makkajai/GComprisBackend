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
	date timestamp,
	duration INTEGER,
	student_id INTEGER,
	board_id INTEGER,
	level INTEGER,
	sublevel INTEGER,
	status INTEGER,
	comment TEXT
)
;

CREATE TABLE students
(
	student_id SERIAL,
	login TEXT,
	lastname TEXT,
	firstname TEXT
)
;