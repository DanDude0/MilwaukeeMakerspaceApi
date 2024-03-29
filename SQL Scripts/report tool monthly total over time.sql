CREATE TEMPORARY TABLE 
	sessions
SELECT
	CONCAT(YEAR(a.attempt_time), '-', MONTH(a.attempt_time)) AS 'month',
	r.name AS 'tool',
	TIMESTAMPDIFF(SECOND, a.attempt_time, IFNULL(
		MIN(l.attempt_time), 
		DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
	)) AS 'used'
FROM 
	attempt a 
	INNER JOIN reader r
		ON a.reader_id = r.reader_id
	LEFT JOIN attempt l
		ON a.reader_id = l.reader_id 
		AND l.access_granted = 1
		AND l.attempt_time > a.attempt_time
		AND l.attempt_time < DATE_ADD(a.attempt_time, INTERVAL r.timeout SECOND)
WHERE 
	a.reader_id IN (6,7,8,20,1004) 
	AND a.access_granted = 1
	AND a.attempt_time > '2023-01-01'
GROUP BY
	r.name,
	a.attempt_time
ORDER BY 
	r.name ASC,
	a.attempt_time ASC;

SELECT
	`month`,
	tool,
	SEC_TO_TIME(SUM(used)) AS used
FROM
	sessions
GROUP BY
	`month`, tool
ORDER BY `month` DESC;

SELECT
	`month`,
	tool,
	SEC_TO_TIME(SUM(used)) AS used
FROM
	sessions
GROUP BY
	`month`, tool
ORDER BY `tool`, `month` DESC;