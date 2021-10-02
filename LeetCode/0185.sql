WITH EmployeeWithSalaryRank AS (
	SELECT
		*,
		DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS SalaryRank
	FROM Employee
)
SELECT
	d.Name AS Department,
	e.Name AS Employee,
	e.Salary
FROM EmployeeWithSalaryRank e
INNER JOIN Department d ON d.Id = e.DepartmentId
WHERE e.SalaryRank <= 3
