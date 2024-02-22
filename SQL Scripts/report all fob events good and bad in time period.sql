SELECT 
	r.name,
	a.attempt_time,
	a.access_granted,
	m.name,
	a.keycode
FROM
	attempt a
	LEFT JOIN reader r
		ON a.reader_id = r.reader_id
	LEFT JOIN MEMBER m
		ON a.member_id = m.member_id
WHERE
	a.login = 1
	AND a.attempt_time BETWEEN '2023-10-18 21:30:00' AND '2023-10-19 02:00:00'
	-- AND a.reader_id < 1000 -- Use this to see only Lenox Readers
	-- AND a.reader_id >= 1000 -- Use this to see only Norwich Readers
ORDER BY 
	a.attempt_time DESC