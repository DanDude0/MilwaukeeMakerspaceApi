SELECT 
	m.name, 
	a.keycode,
	a.attempt_time, 
	r.name, 
	a.access_granted 
FROM 
	attempt a 
	LEFT JOIN reader r 
		ON a.reader_id = r.reader_id 
	LEFT JOIN keycode k 
		ON a.keycode = k.keycode_id OR CONCAT(a.keycode, '#') = k.keycode_id
	LEFT JOIN member m 
		ON m.member_id = k.member_id OR m.member_id = a.member_id
WHERE 
	login = 1
ORDER BY 
	a.attempt_time DESC