SELECT 
DATE(MIN(attempt_time)), COUNT(IF(reader_id < 1000, 1, NULL)) AS 'Lenox', COUNT(IF(reader_id > 1000, 1, NULL)) AS 'Norwich'
FROM attempt
WHERE 
access_granted = 1 
AND logout = 0 
AND attempt_time BETWEEN '2021-03-01' AND '2026-03-01'
AND reader_id IN (1,2,3,4,12,16,18,19,1001,1002,1003,1005)
GROUP BY YEAR(attempt_time), MONTH(attempt_time)
ORDER BY YEAR(attempt_time)DESC , MONTH(attempt_time) DESC, reader_id DESC;