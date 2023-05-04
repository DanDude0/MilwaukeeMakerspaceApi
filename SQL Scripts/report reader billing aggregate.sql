SELECT 
	m.name, 
	r.name, 
	SUM(c.amount)
FROM 
	charge c 
	INNER JOIN MEMBER m 
		ON c.member_id = m.member_id 
	INNER JOIN reader r 
		ON c.reader_id = r.reader_id 
WHERE
	r.name = 'Long Arm Quilting Machine'
	AND c.charge_time > '2023-01-01'
GROUP BY 
	m.name, 
	r.name
ORDER BY 
	m.name ASC, 
	r.name ASC