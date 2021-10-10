SELECT
	(
		WITH RankedSalaries AS (
			SELECT
				Salary,
				DENSE_RANK() OVER (ORDER BY Salary DESC) AS Ranked
			FROM Employee
		)
		SELECT Salary
		FROM RankedSalaries
		WHERE Ranked = 2
		LIMIT 1
	) AS SecondHighestSalary
