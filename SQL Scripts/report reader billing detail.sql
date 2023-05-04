SELECT 
	m.name, 
	r.name, 
	c.charge_time, 
	c.amount, 
	c.description 
FROM 
	charge c 
	INNER JOIN `member` m 
		ON c.member_id = m.member_id 
	INNER JOIN reader r 
		ON c.reader_id = r.reader_id 
WHERE
	r.name = 'Long Arm Quilting Machine'
	AND c.charge_time > '2023-01-01'
ORDER BY 
	m.name ASC, 
	r.name ASC, 
	c.charge_time DESC