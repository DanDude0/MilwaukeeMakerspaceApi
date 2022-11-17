SELECT 
	m.name, 
	a.attempt_time, 
	r.name
FROM 
	attempt a 
	INNER JOIN `member` m 
		ON a.member_id = m.member_id 
	INNER JOIN reader r 
		ON a.reader_id = r.reader_id 
WHERE 
	m.name LIKE 'Dan%' 
	AND a.attempt_time > '2022-06-01' 
ORDER BY 
	a.attempt_time DESC