SELECT  
	m.name,
	r.name,
	a.attempt_time AS 'login',
	IFNULL(
		MIN(l.attempt_time), 
		DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
	) AS 'logout',
	SEC_TO_TIME(TIMESTAMPDIFF(SECOND, a.attempt_time, IFNULL(
		MIN(l.attempt_time), 
		DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
	))) AS usage_time
FROM 
	attempt a 
	INNER JOIN MEMBER m 
		ON a.member_id = m.member_id 
	INNER JOIN reader r
		ON a.reader_id = r.reader_id
	LEFT JOIN attempt l
		ON a.reader_id = l.reader_id 
		AND l.access_granted = 1
		AND l.attempt_time > a.attempt_time
		AND l.attempt_time < DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
WHERE 
	a.reader_id IN (6,7,8,20) 
	AND a.access_granted = 1
	AND a.attempt_time > '2023-11-23'
GROUP BY
	m.name,
	r.name,
	a.attempt_time
ORDER BY 
	a.attempt_time ASC