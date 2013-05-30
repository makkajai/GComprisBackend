/* This is a temporary scratchpad for reporting queries, will be eventually used in the application */

/* things that this kid is doing - along with color (color represents good or bad - shade from green to red 
 based on at-most LAST 5 log entries FOR EACH BOARD */

WITH topnlogs as  
 (select date, duration, student_id, board_id, level, sublevel, status, comment,
         ROW_NUMBER() OVER (PARTITION BY board_id ORDER BY DATE DESC) as RowNum
         from analytics.logs
         where student_id = 2 -- replace this with @studentId - filter by student id here itself - this is essential
 )
 select date, duration, student_id, board_id, level, sublevel, status, comment
 from topnlogs
 where RowNum <= 5;


/*Activity results*/

  WITH topnlogs as  
             (select date, duration, l.student_id, board_id, level, sublevel, status, comment,
                     ROW_NUMBER() OVER (PARTITION BY board_id ORDER BY DATE DESC) as RowNum
                     from analytics.logs l inner join
                          students s on s.student_id = l.student_id
                     where login = 'test'
             )
             select sum(duration), max(level) as level, sum(status) as correct, sum(1-status) as incorrect,
             a.*, 
             s.*,
             5 as LastXNum
             from topnlogs l
             inner join analytics.activities a on board_id = activity_id
             inner join analytics.students s on s.student_id = l.student_id
             where RowNum <= 5
             group by board_id, name, title, logo, 
             a.activity_id, a.name, a.difficulty, a.logo, a.title, a.description, a.prerequisite, a.goal,
             s.student_id, s.login, s.lastname, s.firstname;