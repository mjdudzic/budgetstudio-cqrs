DROP TABLE IF EXISTS employees;
DROP TABLE IF EXISTS tech_skills;

CREATE TABLE employees (
	id UUID PRIMARY KEY,
	first_name VARCHAR (255) NOT NULL,
	last_name VARCHAR (255) NOT NULL,
	job_title VARCHAR (255) NOT NULL,
	employment_type VARCHAR (255) NOT NULL,
	created_at TIMESTAMP NOT NULL,
	updated_at TIMESTAMP NULL,
	deleted_at TIMESTAMP NULL
);

CREATE TABLE tech_skills (
	id UUID  PRIMARY KEY,
	description VARCHAR (255) NOT NULL,
	
	employee_id UUID NOT NULL,
	FOREIGN KEY (employee_id) REFERENCES employees (id)
);

