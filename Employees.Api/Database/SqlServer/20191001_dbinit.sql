DROP TABLE IF EXISTS employees;
DROP TABLE IF EXISTS tech_skills;
GO

CREATE TABLE employees (
	id UNIQUEIDENTIFIER PRIMARY KEY,
	first_name VARCHAR (255) NOT NULL,
	last_name VARCHAR (255) NOT NULL,
	job_title VARCHAR (255) NOT NULL,
	employment_type VARCHAR (255) NOT NULL,
	created_at DATETIME2 NOT NULL,
	updated_at DATETIME2 NULL,
	deleted_at DATETIME2 NULL,
);
GO

CREATE TABLE tech_skills (
	id UNIQUEIDENTIFIER  PRIMARY KEY,
	description VARCHAR (255) NOT NULL,
	
	employee_id UNIQUEIDENTIFIER NOT NULL,
	FOREIGN KEY (employee_id) REFERENCES employees (id)
);
GO