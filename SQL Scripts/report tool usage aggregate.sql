SELECT
	member,
	SEC_TO_TIME(SUM(used)) AS used
FROM(
SELECT  
	m.name AS 'member',
	r.name AS 'tool',
	TIMESTAMPDIFF(SECOND, a.attempt_time, IFNULL(
		MIN(l.attempt_time), 
		DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
	)) AS 'used'
FROM 
	attempt a 
	INNER JOIN member m 
		ON a.member_id = m.member_id 
	INNER JOIN reader r
		ON a.reader_id = r.reader_id
	LEFT JOIN attempt l
		ON a.reader_id = l.reader_id 
		AND l.access_granted = 1
		AND l.attempt_time > a.attempt_time
		AND l.attempt_time < DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
WHERE 
	a.reader_id IN (6,7,8) 
	AND a.access_granted = 1
	AND a.attempt_time > '2019-05-01'
GROUP BY
	m.name,
	r.name,
	a.attempt_time
ORDER BY 
	m.name ASC,
	r.name ASC,
	a.attempt_time ASC
) used
GROUP BY
	member
ORDER BY used DESC