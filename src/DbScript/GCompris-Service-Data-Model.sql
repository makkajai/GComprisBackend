/*Data model for GCompris backend service*/

CREATE TABLE boards
(
	board_id INTEGER,
	name TEXT,
	section_id INTEGER,
	section TEXT,
	author TEXT,
	type TEXT,
	mode TEXT,
	difficulty INTEGER,
	icon TEXT,
	boarddir TEXT,
	mandatory_sound_file TEXT,
	mandatory_sound_dataset TEXT,
	filename TEXT,
	title TEXT,
	description TEXT,
	prerequisite TEXT,
	goal TEXT,
	manual TEXT,
	credit TEXT,
	demo INTEGER
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