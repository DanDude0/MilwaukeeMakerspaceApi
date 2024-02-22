SELECT 
	CONCAT(YEAR(t.month), '-', MONTH(t.month)), 
	(SELECT MIN(b.members) FROM funds b WHERE YEAR(b.month) = YEAR(t.month) AND MONTH(b.month) = MONTH(t.month) AND DAY(b.month) = 7 AND b.members > 0) AS members_before_cancel,
	(SELECT MIN(a.members) FROM funds a WHERE YEAR(a.month) = YEAR(t.month) AND MONTH(a.month) = MONTH(t.month) AND DAY(a.month) = 8 AND a.members > 0) AS members_after_cancel,
	members_before_cancel - members_after_cancel AS membership_cancel_loss,
	membership_cancel_loss / members_before_cancel * 100 AS percentage_loss
FROM 
	funds t
WHERE 
	DAY(t.month) = 8
GROUP BY 
	YEAR(t.month), MONTH(t.month) 
ORDER BY 
	t.month DESC