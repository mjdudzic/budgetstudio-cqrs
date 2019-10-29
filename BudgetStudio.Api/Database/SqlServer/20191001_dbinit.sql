--IF OBJECT_ID('dbo.budgets_view', 'U') IS NOT NULL
--	DROP TABLE dbo.budgets_view;

DROP VIEW IF EXISTS budgets_view;
DROP TABLE IF EXISTS employee_costs;
DROP TABLE IF EXISTS extra_costs;
DROP TABLE IF EXISTS budgets;
GO

CREATE TABLE budgets (
	id UNIQUEIDENTIFIER PRIMARY KEY,
	project_id UNIQUEIDENTIFIER  NOT NULL,
	created_at DATETIME2 NOT NULL,
	confirmed_at DATETIME2 NULL,
	rejected_at DATETIME2 NULL,
	rejection_reason VARCHAR (500) NULL,
	total_cost_amount DECIMAL(10, 2) NOT NULL,
	total_cost_currency VARCHAR (3) NULL
);

CREATE TABLE employee_costs (
	id UNIQUEIDENTIFIER  PRIMARY KEY,
	employee_code VARCHAR (50) NOT NULL,
	participation_started_at DATETIME2 NOT NULL,
	participation_ended_at DATETIME2 NULL,
	cost_amount DECIMAL(10, 2) NOT NULL,
	cost_currency VARCHAR (3) NULL,

	budget_id UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (budget_id) REFERENCES budgets (id)
);

CREATE TABLE extra_costs (
	id UNIQUEIDENTIFIER  PRIMARY KEY,
	description VARCHAR (500) NOT NULL,
	cost_amount DECIMAL(10, 2) NOT NULL,
	cost_currency VARCHAR (3) NULL,

	budget_id UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (budget_id) REFERENCES budgets (id)
);
GO

CREATE VIEW budgets_view AS
	SELECT 
		 b.id AS budget_id
		,b.project_id
		,b.created_at
		,b.confirmed_at
		,b.rejected_at
		,b.rejection_reason
		,b.total_cost_amount
		,b.total_cost_currency
		,COALESCE(employee_costs.items_count, 0) AS employees_count
		,COALESCE(employee_costs.total_cost, 0) AS employees_total_cost
		,COALESCE(extra_costs.items_count, 0) AS extra_costs_count
		,COALESCE(extra_costs.total_cost, 0) AS extra_costs_total_cost
	FROM budgets AS b
	OUTER APPLY (
		SELECT 
			 COUNT(1) AS items_count
			,SUM(emp.cost_amount) AS total_cost
		FROM employee_costs emp
		WHERE emp.budget_id = b.id
	) employee_costs
	OUTER APPLY (
		SELECT 
			 COUNT(1) AS items_count
			,SUM(ext.cost_amount) AS total_cost
		FROM extra_costs ext
		WHERE ext.budget_id = b.id
	) extra_costs;
GO
