/* This is a temporary scratchpad for reporting queries, will be eventually used in the application */

/* things that this kid is doing - along with color (color represents good or bad - shade from green to red 
 based on at-most LAST 5 log entries FOR EACH BOARD */

WITH top5logs as  
 (select date, duration, student_id, board_id, level, sublevel, status, comment,
         ROW_NUMBER() OVER (PARTITION BY board_id ORDER BY DATE DESC) as RowNum
         from analytics.logs
         where student_id = 2 -- replace this with @studentId - filter by student id here itself - this is essential
 )
 select date, duration, student_id, board_id, level, sublevel, status, comment
 from top5logs
 where RowNum <= 5;