SELECT bank_statement_id, account_name, `time`, starting_balance + income + transfers - spending - fees - ending_balance AS 'error' FROM bank_statement ORDER BY TIME DESC