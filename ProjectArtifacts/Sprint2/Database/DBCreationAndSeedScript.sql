USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'MJRecords')
BEGIN
	ALTER DATABASE MJRecords SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MJRecords;  
END;
CREATE DATABASE MJRecords;
GO

USE MJRecords;
GO

-- DROP TABLE Department;
CREATE TABLE Department(

	Id				INT				NOT NULL	IDENTITY(1,1),
	Name			VARCHAR(128)	NOT NULL,
	Description		VARCHAR(512)	NOT NULL,
	InvocationDate	datetime		NOT NULL,
	RecordVersion	ROWVERSION,

	CONSTRAINT PK_Department		PRIMARY KEY (Id),
	CONSTRAINT UQ_Department_Name	UNIQUE	(Name)
);

-- DROP TABLE Job;
GO
CREATE TABLE Job(
	Id		INT				NOT NULL	IDENTITY(1,1),
	Title	VARCHAR(255)	NOT NULL,

	CONSTRAINT	PK_Job		PRIMARY KEY (Id)
);


GO
CREATE TABLE EmploymentStatus (
	Id		INT		NOT NULL		IDENTITY(1,1),
	Status	VARCHAR(255)	NOT NULL,

	CONSTRAINT PK_EmploymentStatus			PRIMARY KEY (Id),
	CONSTRAINT UQ_EmploymentStatus_Status	UNIQUE	(Status)
);

GO
CREATE TABLE RatingOptions (
	Id		INT				NOT NULL		IDENTITY(1,1),
	Rating	VARCHAR(255)	NOT NULL,

	CONSTRAINT	PK_RatingOptions	PRIMARY KEY (Id)
);



GO
-- DROP TABLE Employee;
CREATE TABLE Employee(
	Id					CHAR(8)			NOT NULL,
	EmploymentStatusId	INT				NOT NULL,
	SupervisorId		CHAR(8)			NULL,
	DepartmentId		INT				NULL,
	JobAssignmentId		INT				NOT NULL,
	Password			VARCHAR(255)	NOT NULL,
	PasswordSalt		BINARY(16)		NOT NULL,
	FirstName			VARCHAR(50)		NOT NULL,
	LastName			VARCHAR(50)		NOT NULL,
	MiddleInitial		CHAR(1)			NULL,
	StreetAddress		VARCHAR(255)	NOT NULL,
	City				VARCHAR(255)	NOT NULL,
	Province			VARCHAR(255)	NOT NULL,
	PostalCode			VARCHAR(7)		NOT NULL,
	DOB					DATETIME		NOT NULL,
	SIN					VARCHAR(11)		NOT NULL,
	SeniorityDate		DATETIME		NOT NULL,
	JobStartDate		DATETIME		NOT NULL,
	WorkPhone			VARCHAR(14)		NOT NULL,
	CellPhone			VARCHAR(14)		NOT NULL,
	EmailAddress		VARCHAR(255)	NOT NULL,
	OfficeLocation		VARCHAR(255)	NOT NULL,
	RetirementDate		DATETIME		NULL,
	TerminationDate		DATETIME		NULL,
	RecordVersion		ROWVERSION,

	CONSTRAINT PK_Employee						PRIMARY KEY	(Id),
	CONSTRAINT FK_Employee_EmploymentStatusId	FOREIGN KEY (EmploymentStatusId) REFERENCES	EmploymentStatus(Id),
	CONSTRAINT FK_Employee_SupervisorId			FOREIGN KEY (SupervisorId)		REFERENCES Employee(Id),
	CONSTRAINT FK_Employee_DepartmentId			FOREIGN KEY (DepartmentId)		REFERENCES Department(Id),
	CONSTRAINT FK_Employee_JobAssignmentId		FOREIGN KEY (JobAssignmentId)	REFERENCES	Job(Id),	
	CONSTRAINT UQ_Employee_SIN					UNIQUE	(SIN),
	CONSTRAINT UQ_Employee_Id					UNIQUE	(Id)
);


GO
CREATE TABLE EmployeeReview(
	Id					INT					NOT NULL	IDENTITY(1,1),
	EmployeeId			CHAR(8)				NOT NULL,
	RatingOptionsId		INT					NOT NULL,
	Comment				VARCHAR(600)		NOT NULL,
	ReviewDate			DATETIME			NOT NULL,

	CONSTRAINT	PK_EmployeeReview					PRIMARY KEY	(Id),
	CONSTRAINT	FK_EmployeeReview_EmployeeId		FOREIGN KEY (EmployeeId)		REFERENCES	Employee(Id),
	CONSTRAINT	FK_EmployeeReview_RatingOptionsId	FOREIGN KEY	(RatingOptionsId)	REFERENCES	RatingOptions(Id)
);

GO

--DROP TABLE PurchaseOrder
CREATE TABLE PurchaseOrder (
    PurchaseOrderId CHAR(8)      NOT NULL,
    EmployeeId      CHAR(8)      NOT NULL,
    Status          INT          NOT NULL,
    DateCreated     DATETIME     NOT NULL DEFAULT GETDATE(),
    RecordVersion   ROWVERSION,

	CONSTRAINT PK_PurchaseOrder          PRIMARY KEY (PurchaseOrderId),
    CONSTRAINT FK_PurchaseOrder_Employee FOREIGN KEY (EmployeeId) REFERENCES dbo.Employee(Id),
	CONSTRAINT UQ_PurchaseOrderId		 UNIQUE (PurchaseOrderId)
);
GO

--DROP TABLE PurchaseOrderItem
CREATE TABLE PurchaseOrderItem (
    Id               INT          NOT NULL	IDENTITY(1,1),
    PurchaseOrderId  CHAR(8)      NOT NULL,
    Name             VARCHAR(255) NOT NULL,
    Description      VARCHAR(255) NOT NULL,
    Quantity         INT          NOT NULL,
    Justification    VARCHAR(255) NOT NULL,
    PurchaseLocation VARCHAR(255) NOT NULL,
    ItemStatus       VARCHAR(10)  NOT NULL,
    Price            MONEY        NOT NULL,
    RecordVersion    ROWVERSION,
	  DenialReason VARCHAR(500) NULL,


	CONSTRAINT PK_PurchaseOrderItem					PRIMARY KEY (Id),
    CONSTRAINT FK_PurchaseOrderItem_PurchaseOrderId FOREIGN KEY (PurchaseOrderId) REFERENCES dbo.PurchaseOrder(PurchaseOrderId)
	
);
GO




GO
--SEED DATA MR--
INSERT INTO Job (Title) VALUES 
('CEO'),
('HR Manager'),
('Project Manager'),
('Product Manager'),
('Developer Manager'),
('Infrastructure Manager'),
('HR Officer'),
('Systems Analyst'),
('Solution Architect'),
('Software Engineer'),
('Network Engineer'),
('Database Administrator'),
('Cybersecurity Specialist'),
('QA Engineer'),
('Technical Support Engineer'),
('Business Analyst'),
('DevOps Engineer'),
('UI/UX Designer');

GO
INSERT INTO Department (Name, Description, InvocationDate) VALUES
('Human Resources', 'Manages hiring, training, and employee welfare.','2025-02-20'),
('Project Management', 'Oversees planning and execution of company projects.', '2025-05-03'),
('Software Development', 'Develops and maintains software products and platforms.', '2025-05-01'),
('IT Operations', 'Manages infrastructure, deployment, and system availability.', '2025-05-02'),
('Business Solutions', 'Analyzes systems and business needs for optimization.', '2025-05-06'),
('Architecture', 'Designs system architecture and technology roadmap.', '2025-05-25'),
('Networking', 'Manages network infrastructure and connectivity.', '2025-06-15'),
('Data Services', 'Maintains and optimizes company databases and data flow.', '2025-06-01'),
('Information Security', 'Ensures systems and data are protected from threats.', '2025-08-07'),
('Quality Assurance', 'Tests and validates software to meet standards.', '2025-09-23'),
('Technical Support', 'Provides assistance and support for technical issues.', '2025-06-15'),
('Design', 'Designs user interfaces and improves user experience.', '2025-06-15');


GO
INSERT INTO EmploymentStatus(Status)
VALUES
('Active'),
('Retired'),
('Terminated');

GO
INSERT INTO RatingOptions(Rating)
VALUES
('Exceeds Expectations'),
('Meets Expectations'),
('Below Expecttations');


GO
INSERT INTO Employee (
    Id, EmploymentStatusId, SupervisorId, DepartmentId, JobAssignmentId, Password, PasswordSalt,
    FirstName, LastName, MiddleInitial,
    StreetAddress, City, Province, PostalCode,
    DOB, SIN, SeniorityDate, JobStartDate,
    WorkPhone, CellPhone, EmailAddress, OfficeLocation, RetirementDate, TerminationDate
)
VALUES
-- CEO (no supervisor)
('00000001', 1, NULL, NULL, 1, '24388386653dd1434991cb75d3669a8b7b8361c87986110575b4e88e35c66504', 0xE0187C6A4AD19746F368CFAA1FB93FCF, 'Alice', 
'Brown', 'J', '100 King St', 'Moncton', 'NB', 'E1A1A1', '1980-01-01', '111-111-111', '2005-01-01', '2005-01-01', 
'(506) 111-1111', '(506) 911-1111', 'alice.brown@example.com', 'HQ', NULL, NULL),

-- HUMAN RESOURCES DEPARTMENT (ID: 1)
-- HR Manager (Supervisor 1 for HR dept)
('00000002', 1, '00000001', 1, 2, 'a5b6d7c1bfbc023930e33f8b0f2658b782a80a19251e9885c6468cb823037930', 0x7D1384BECD09A7091F3CC453EE18A5FE, 'Bob', 
'Smith', NULL, '101 King St', 'Moncton', 'NB', 'E1A1A2', '1985-02-01', '111-111-112', '2006-02-01', '2006-02-01', 
'(506) 222-1111', '(506) 922-1111', 'bob.smith@example.com', 'HQ', NULL, NULL),

-- HR Manager (Supervisor 2 for HR dept)
('00000003', 1, '00000001', 1, 2, 'b12db4f06d40c58197aee70dedda8ddfe23663d043306af1469f760af9da55e3', 0x5A6885A5C69A9D8FB337B21AC80D8016, 'Cathy', 
'Mills', 'A', '102 King St', 'Moncton', 'NB', 'E1A1A3', '1986-03-01', '111-111-113', '2007-03-01', '2007-03-01', 
'(506) 333-1111', '(506) 933-1111', 'cathy.mills@example.com', 'East Wing', NULL, NULL),

-- HR Employees under Bob (Supervisor 1)
('00000004', 1, '00000002', 1, 7, 'd98be886ae2f7d60f5c2fd7118d287db1d49e5503a6c8479ec125b7826e0553a', 0xB50B8BA20BDEDD2EDF7B1CFBA649AC71, 'David', 
'Nguyen', 'B', '103 King St', 'Moncton', 'NB', 'E1A1A4', '1987-04-01', '111-111-114', '2008-04-01', '2008-04-01', 
'(506) 444-1111', '(506) 944-1111', 'david.nguyen@example.com', 'East Wing', NULL, NULL),

-- HR Employees under Cathy (Supervisor 2)
('00000008', 1, '00000003', 1, 7, '992745aae07edbc2ca167ab2d4c5166dd86b0b46adecd31f7e9f8219090a9dce', 0xF61E8EBD4C9762ACAF62134F73317B27, 'Henry', 
'Lee', 'E', '107 King St', 'Moncton', 'NB', 'E1A1A8', '1991-08-01', '111-111-118', '2012-08-01', '2012-08-01', 
'(506) 888-1111', '(506) 988-1111', 'henry.lee@example.com', 'North Office', NULL, NULL),

-- PROJECT MANAGEMENT DEPARTMENT (ID: 2)
-- Project Manager (Supervisor 1 for PM dept)
('00000005', 1, '00000001', 2, 3, '688cf7c1d297f6f57a61fdf4a67d22c88c4b0dc9655ef9eb2cc0c6a84cf23ea7', 0xE3B2563309C47C3D38591FF40C843113, 'Ella', 
'Thomas', 'C', '104 King St', 'Moncton', 'NB', 'E1A1A5', '1988-05-01', '111-111-115', '2009-05-01', '2009-05-01', 
'(506) 555-1111', '(506) 955-1111', 'ella.thomas@example.com', 'West Wing', NULL, NULL),

-- Project Manager (Supervisor 2 for PM dept)
('00000010', 1, '00000001', 2, 3, 'c08724bac9e850482980b50b84710655f82c8c364b0619fc0febeaa04eed12f9', 0xB7B51BC0DC6D995B6D3D6A42D8D2DAD6, 'Jack', 
'Wells', 'G', '109 King St', 'Moncton', 'NB', 'E1A1B0', '1993-10-01', '111-111-120', '2014-10-01', '2014-10-01', 
'(506) 101-1111', '(506) 201-1111', 'jack.wells@example.com', 'HQ', NULL, NULL),

-- PM Employees under Ella (Supervisor 1)
('00000009', 1, '00000005', 2, 16, '6a1338e1a4126fe7ffe29e502eb1929ad593e7829b4672bac738faa98848fbfd', 0xA23F0532ACD00320B3089613DA1D898A, 'Ivy', 
'Turner', 'F', '108 King St', 'Moncton', 'NB', 'E1A1A9', '1992-09-01', '111-111-119', '2013-09-01', '2013-09-01', 
'(506) 999-1111', '(506) 999-2111', 'ivy.turner@example.com', 'North Office', NULL, NULL),

-- PM Employees under Jack (Supervisor 2)
('00000011', 1, '00000010', 2, 16, '139a68e3eabb3431239fc09da45ec70f8a382de36b92477a58f784e06b0c3c7c', 0xDDB9309EFD90F0632459ACFF4C31554F, 'Karen', 
'Young', NULL, '110 Queen St', 'Moncton', 'NB', 'E1A2A1', '1994-01-01', '111-111-121', '2015-01-01', '2015-01-01', 
'(506) 202-1111', '(506) 302-1111', 'karen.young@example.com', 'HQ', NULL, NULL),

-- SOFTWARE DEVELOPMENT DEPARTMENT (ID: 3)
-- Developer Manager (Supervisor 1 for SD dept)
('00000006', 1, '00000001', 3, 5, '473fd7135581560f373b891acbe8d89d984b7dadafdb444219ff95624f8a343e', 0x23C16FBA7D1015052DDB09AE1ED07F2B, 'Frank', 
'Olsen', NULL, '105 King St', 'Moncton', 'NB', 'E1A1A6', '1989-06-01', '111-111-116', '2010-06-01', '2010-06-01', 
'(506) 666-1111', '(506) 966-1111', 'frank.olsen@example.com', 'West Wing', NULL, NULL),

-- Developer Manager (Supervisor 2 for SD dept)
('00000007', 1, '00000001', 3, 5, '526914124ad8c6714f1781b73051523e54526ed593c922376435898dc844ca6c', 0xE6F540157C5CEAE8D99D685B19AD0281, 'Grace', 
'Martin', 'D', '106 King St', 'Moncton', 'NB', 'E1A1A7', '1990-07-01', '111-111-117', '2011-07-01', '2011-07-01', 
'(506) 777-1111', '(506) 977-1111', 'grace.martin@example.com', 'HQ', NULL, NULL),

-- SD Employees under Frank (Supervisor 1)
('00000012', 1, '00000006', 3, 10, '6afe6d7fc22bc59b1fa5b2c6749ddbb019b6e507a6ec7b0514cc631361b86635', 0x6E74EB8E9F01E582EE94FAE18D91DBD3, 'Leo', 
'Zimmer', 'H', '111 Queen St', 'Moncton', 'NB', 'E1A2A2', '1995-02-01', '111-111-122', '2016-02-01', '2016-02-01', 
'(506) 303-1111', '(506) 403-1111', 'leo.zimmer@example.com', 'HQ', NULL, NULL),

('00000015', 1, '00000006', 3, 10, '1a2e155400ceab995e31122c019a4b427b1d268bee726a745189cc257860dbf7', 0xDAA7C1DC69DF180D964FBC15A5D276C7, 'Olivia', 
'Grant', 'J', '114 Queen St', 'Moncton', 'NB', 'E1A2A5', '1998-05-01', '111-111-125', '2019-05-01', '2019-05-01', 
'(506) 606-1111', '(506) 706-1111', 'olivia.grant@example.com', 'West Wing', NULL, NULL),

-- SD Employees under Grace (Supervisor 2)
('00000013', 2, '00000007', 3, 14, '2c51fc521adf988f2f0a273acd43a68569791eec7f3acdac786f2ef439183bd6', 0xD74874CFA118F2C7581603F677AC2354, 'Mona', 
'Evans', 'I', '112 Queen St', 'Moncton', 'NB', 'E1A2A3', '1996-03-01', '111-111-123', '2017-03-01', '2017-03-01', 
'(506) 404-1111', '(506) 504-1111', 'mona.evans@example.com', 'East Wing', '2025-05-01', NULL),

('00000014', 1, '00000007', 3, 10, '54171a646c261ee55f7abb95a2b48ae1d248f78d6c8970d7e21a67a21e3bee17', 0xE9C27C4B179E24445B1F548F607F8D4A, 'Nathan', 
'Ford', NULL, '113 Queen St', 'Moncton', 'NB', 'E1A2A4', '1997-04-01', '111-111-124', '2018-04-01', '2018-04-01', 
'(506) 505-1111', '(506) 605-1111', 'nathan.ford@example.com', 'East Wing', NULL, NULL),

-- IT OPERATIONS DEPARTMENT (ID: 4)
-- Infrastructure Manager (Supervisor 1 for IT dept)
('00000016', 1, '00000001', 4, 6, '74d08c00d62f3170d5fe653b4a591f334e3b859b5185c4becba23ea0d8fc7510', 0x7D079DA13D2446ED8E6AA8F06F3810E1, 'Paul', 
'Harris', 'K', '115 Queen St', 'Moncton', 'NB', 'E1A2A6', '1999-06-01', '111-111-126', '2020-06-01', '2020-06-01', 
'(506) 707-1111', '(506) 807-1111', 'paul.harris@example.com', 'West Wing', NULL, NULL),

-- Infrastructure Manager (Supervisor 2 for IT dept)
('00000017', 1, '00000001', 4, 6, '050395ff9d11eb8964974c719478276267c049ec6a2c3d3b9c58be6150705dbb', 0x6B5B6A3F2D1FC0CE2BD0F6907F4122BE, 'Quinn', 
'Irwin', NULL, '116 Queen St', 'Moncton', 'NB', 'E1A2A7', '2000-07-01', '111-111-127', '2021-07-01', '2021-07-01', 
'(506) 808-1111', '(506) 908-1111', 'quinn.irwin@example.com', 'HQ', NULL, NULL),

-- IT Employees under Paul (Supervisor 1)
('00000018', 1, '00000016', 4, 12, '78368eaa4aa09b1a06adcc027b579cfce2ca0888044e8833b3967ac68993e8bc', 0x289FF3C6B4FC4171B5B4D96D41F16446, 'Rachel', 
'Jones', 'L', '117 Queen St', 'Moncton', 'NB', 'E1A2A8', '2001-08-01', '111-111-128', '2022-08-01', '2022-08-01', 
'(506) 909-1111', '(506) 109-1111', 'rachel.jones@example.com', 'North Office', NULL, NULL),

-- IT Employees under Quinn (Supervisor 2)
('00000019', 1, '00000017', 4, 17, '1d30dea60908174511d284a26d06083c948a3fecbef0e4739bff7c675eadaa39', 0xA3D2C7FA5746B2D96F1DA147D3A165C6, 'Sam', 
'Kim', 'M', '118 Queen St', 'Moncton', 'NB', 'E1A2A9', '2002-09-01', '111-111-129', '2023-09-01', '2023-09-01', 
'(506) 110-1111', '(506) 210-1111', 'sam.kim@example.com', 'North Office', NULL, NULL),

('00000020', 1, '00000017', 4, 13, '4700c0d519cc4eeb0ae1e0d84211fd10223d974c21297040015eb1749a26f8e2', 0xBBAEF8439A15704733F8E7D62186566C, 'Tina', 
'Lopez', 'N', '119 Queen St', 'Moncton', 'NB', 'E1A2B0', '2003-10-01', '111-111-130', '2024-10-01', '2024-10-01', 
'(506) 211-1111', '(506) 311-1111', 'tina.lopez@example.com', 'HQ', NULL, NULL),

-- BUSINESS SOLUTIONS DEPARTMENT (ID: 5) - Adding this department
-- Product Manager (Supervisor 1 for BS dept)
('00000021', 1, '00000001', 5, 4, '24388386653dd1434991cb75d3669a8b7b8361c87986110575b4e88e35c66504', 0xE0187C6A4AD19746F368CFAA1FB93FCF, 'Victor', 
'Morgan', 'O', '120 Queen St', 'Moncton', 'NB', 'E1A2B1', '1985-11-01', '111-111-131', '2010-11-01', '2010-11-01', 
'(506) 212-1111', '(506) 312-1111', 'victor.morgan@example.com', 'HQ', NULL, NULL),

-- Product Manager (Supervisor 2 for BS dept)
('00000022', 1, '00000001', 5, 4, 'a5b6d7c1bfbc023930e33f8b0f2658b782a80a19251e9885c6468cb823037930', 0x7D1384BECD09A7091F3CC453EE18A5FE, 'Wendy', 
'Parker', 'P', '121 Queen St', 'Moncton', 'NB', 'E1A2B2', '1986-12-01', '111-111-132', '2011-12-01', '2011-12-01', 
'(506) 213-1111', '(506) 313-1111', 'wendy.parker@example.com', 'HQ', NULL, NULL),

-- BS Employees under Victor (Supervisor 1)
('00000023', 1, '00000021', 5, 9, 'b12db4f06d40c58197aee70dedda8ddfe23663d043306af1469f760af9da55e3', 0x5A6885A5C69A9D8FB337B21AC80D8016, 'Xavier', 
'Roberts', 'Q', '122 Queen St', 'Moncton', 'NB', 'E1A2B3', '1990-01-15', '111-111-133', '2015-01-15', '2015-01-15', 
'(506) 214-1111', '(506) 314-1111', 'xavier.roberts@example.com', 'East Wing', NULL, NULL),

-- BS Employees under Wendy (Supervisor 2)
('00000024', 1, '00000022', 5, 18, 'c08724bac9e850482980b50b84710655f82c8c364b0619fc0febeaa04eed12f9', 0xB7B51BC0DC6D995B6D3D6A42D8D2DAD6, 'Yasmin', 
'Stevens', 'R', '123 Queen St', 'Moncton', 'NB', 'E1A2B4', '1991-02-15', '111-111-134', '2016-02-15', '2016-02-15', 
'(506) 215-1111', '(506) 315-1111', 'yasmin.stevens@example.com', 'West Wing', NULL, NULL);
 


--END SEED DATA MR--

--SELECT * FROM PurchaseOrder
--SEED DATA JG--
INSERT INTO dbo.PurchaseOrder (PurchaseOrderId, EmployeeId, Status, DateCreated)
VALUES

('00000101', '00000001', 0, '2025-01-01'),
('00000102', '00000001', 0, '2025-01-02'),
('00000103', '00000001', 0, '2025-01-03'),
('00000104', '00000001', 0, '2025-01-04'),
('00000105', '00000001', 0, '2025-01-05'),
('00000106', '00000001', 0, '2025-01-06'),
('00000107', '00000001', 0, '2025-01-07'),
('00000108', '00000001', 0, '2025-01-08'),
('00000109', '00000001', 0, '2025-01-09'),
('00000110', '00000001', 0, '2025-01-10'),
('00000111', '00000001', 0, '2025-01-11'),
('00000112', '00000001', 0, '2025-01-12'),
('00000113', '00000001', 0, '2025-01-13'),
('00000114', '00000001', 0, '2025-01-14'),
('00000115', '00000001', 0, '2025-01-15'),
('00000116', '00000001', 0, '2025-01-16'),
('00000117', '00000001', 0, '2025-01-17'),
('00000118', '00000001', 0, '2025-01-18'),
('00000119', '00000004', 0, '2025-01-19'),
('00000120', '00000004', 0, '2025-01-21'),
('00000121', '00000004', 0, '2025-01-22'),
('00000122', '00000004', 0, '2025-01-23'),
('00000123', '00000004', 0, '2025-01-24'),
('00000124', '00000010', 0, '2025-01-25'),
('00000125', '00000010', 0, '2025-01-26'),
('00000126', '00000010', 0, '2025-01-27'),
('00000127', '00000010', 0, '2025-01-28'),
('00000128', '00000002', 0, '2025-01-29'),
('00000129', '00000012', 0, '2025-01-30'),
('00000130', '00000012', 0, '2025-02-01'),
('00000131', '00000012', 0, '2025-02-02'),
('00000132', '00000012', 0, '2025-02-03'),
('00000133', '00000012', 0, '2025-02-04'),
('00000134', '00000012', 0, '2025-02-05'),
('00000135', '00000012', 0, '2025-02-06'),
('00000136', '00000012', 0, '2025-02-07'),
('00000137', '00000012', 0, '2025-02-08'),
('00000138', '00000012', 0, '2025-02-09'),
('00000139', '00000012', 0, '2025-02-10'),
('00000140', '00000002', 0, '2025-02-11'),
('00000141', '00000002', 0, '2025-02-12'),
('00000142', '00000002', 0, '2025-02-13'),
('00000143', '00000002', 0, '2025-02-14');
--('00000144', '00000025', 0, '2025-02-15'),
--('00000145', '00000025', 0, '2025-02-16'),
--('00000146', '00000025', 0, '2025-02-17');


--SELECT * FROM Employee;
-------


--SELECT * FROM PurchaseOrder

INSERT INTO PurchaseOrderItem (
    PurchaseOrderId, Name, Description, Quantity,
    Justification, PurchaseLocation, ItemStatus, Price
)
VALUES
('00000101', 'Tablet', 'Adjustable tablet', 2, 'QA equipment', 'Amazon', 'Pending', 63.62),
('00000101', 'Docking Station', 'USB-C docking station', 2, 'Meeting room setup', 'Staples', 'Pending', 258.69),
('00000101', 'Desk Lamp', 'Compact desk lamp', 5, 'Replacement unit', 'Newegg', 'Pending', 105.61),
('00000102', 'Speaker', 'Compact speaker', 4, 'Team expansion', 'Amazon', 'Pending', 65.06),
('00000102', 'Headset', 'Compact headset', 2, 'Ergonomic setup', 'Staples', 'Pending', 193.08),
('00000102', 'Chair', 'Wireless chair', 3, 'QA equipment', 'Newegg', 'Pending', 78.98),
('00000103', 'Camera', 'Compact camera', 1, 'Meeting room setup', 'Amazon', 'Pending', 242.35),
('00000103', 'Stand', 'High-end stand', 5, 'Ergonomic setup', 'Tech World', 'Pending', 164.96),
('00000103', 'SSD', 'USB-C ssd', 4, 'Replacement unit', 'Amazon', 'Pending', 189.68),
('00000104', 'Router', 'High-end router', 3, 'QA equipment', 'Office Depot', 'Pending', 186.75),
('00000104', 'Tablet', 'Compact tablet', 2, 'Ergonomic setup', 'Staples', 'Pending', 102.3),
('00000104', 'Docking Station', 'Compact docking station', 5, 'Replacement unit', 'Tech World', 'Pending', 254.2),
('00000105', 'Desk Lamp', 'Adjustable desk lamp', 3, 'Team expansion', 'Office Depot', 'Pending', 114.92),
('00000105', 'Speaker', 'Adjustable speaker', 5, 'Team expansion', 'Tech World', 'Pending', 235.25),
('00000105', 'Headset', 'Adjustable headset', 4, 'Team expansion', 'Newegg', 'Pending', 97.8),
('00000106', 'Chair', 'USB-C chair', 3, 'Meeting room setup', 'Office Depot', 'Pending', 262.66),
('00000106', 'Camera', 'USB-C camera', 3, 'Replacement unit', 'Office Depot', 'Pending', 67.81),
('00000106', 'Stand', 'Compact stand', 3, 'Replacement unit', 'Amazon', 'Pending', 225.74),
('00000107', 'SSD', 'Adjustable ssd', 5, 'Meeting room setup', 'Newegg', 'Pending', 217.49),
('00000107', 'Router', 'High-end router', 1, 'Ergonomic setup', 'Newegg', 'Pending', 217.34),
('00000107', 'Tablet', 'USB-C tablet', 1, 'Meeting room setup', 'Staples', 'Pending', 195.35),
('00000108', 'Docking Station', 'USB-C docking station', 3, 'Replacement unit', 'Amazon', 'Pending', 225.72),
('00000108', 'Desk Lamp', 'Wireless desk lamp', 5, 'Ergonomic setup', 'Staples', 'Pending', 250.85),
('00000108', 'Speaker', 'USB-C speaker', 3, 'Replacement unit', 'Tech World', 'Pending', 150.07),
('00000109', 'Headset', 'Adjustable headset', 2, 'Replacement unit', 'Office Depot', 'Pending', 243.8),
('00000109', 'Chair', 'High-end chair', 2, 'Ergonomic setup', 'Office Depot', 'Pending', 147.57),
('00000109', 'Camera', 'High-end camera', 4, 'Team expansion', 'Tech World', 'Pending', 248.78),
('00000110', 'Stand', 'Adjustable stand', 3, 'Team expansion', 'Tech World', 'Pending', 270.21),
('00000110', 'SSD', 'USB-C ssd', 1, 'Ergonomic setup', 'Tech World', 'Pending', 95.26),
('00000110', 'Router', 'High-end router', 3, 'QA equipment', 'Staples', 'Pending', 281.63),
('00000111', 'Tablet', 'High-end tablet', 3, 'QA equipment', 'Office Depot', 'Pending', 180.66),
('00000111', 'Docking Station', 'Compact docking station', 5, 'Meeting room setup', 'Amazon', 'Pending', 238.02),
('00000111', 'Desk Lamp', 'Wireless desk lamp', 1, 'Ergonomic setup', 'Amazon', 'Pending', 274.16),
('00000112', 'Speaker', 'Adjustable speaker', 2, 'Meeting room setup', 'Tech World', 'Pending', 294.79),
('00000112', 'Headset', 'Adjustable headset', 5, 'Ergonomic setup', 'Staples', 'Pending', 237.25),
('00000112', 'Chair', 'Wireless chair', 5, 'Ergonomic setup', 'Newegg', 'Pending', 104.09),
('00000113', 'Camera', 'USB-C camera', 5, 'Replacement unit', 'Newegg', 'Pending', 274.43),
('00000113', 'Stand', 'Adjustable stand', 5, 'QA equipment', 'Newegg', 'Pending', 282.15),
('00000113', 'SSD', 'Wireless ssd', 2, 'Team expansion', 'Office Depot', 'Pending', 128.97),
('00000114', 'Router', 'Wireless router', 3, 'Replacement unit', 'Amazon', 'Pending', 162.19),
('00000114', 'Tablet', 'Wireless tablet', 1, 'Team expansion', 'Tech World', 'Pending', 97.08),
('00000114', 'Docking Station', 'Wireless docking station', 3, 'Ergonomic setup', 'Amazon', 'Pending', 221.73),
('00000115', 'Desk Lamp', 'High-end desk lamp', 2, 'Ergonomic setup', 'Amazon', 'Pending', 236.58),
('00000115', 'Speaker', 'Compact speaker', 1, 'Ergonomic setup', 'Tech World', 'Pending', 219.88),
('00000115', 'Headset', 'Wireless headset', 5, 'QA equipment', 'Office Depot', 'Pending', 195.6),
('00000116', 'Chair', 'USB-C chair', 5, 'Ergonomic setup', 'Newegg', 'Pending', 282.15),
('00000116', 'Camera', 'USB-C camera', 2, 'QA equipment', 'Staples', 'Pending', 76.67),
('00000116', 'Stand', 'Adjustable stand', 5, 'Replacement unit', 'Staples', 'Pending', 193.2),
('00000117', 'SSD', 'High-end ssd', 3, 'Ergonomic setup', 'Amazon', 'Pending', 74.86),
('00000117', 'Router', 'Adjustable router', 3, 'Replacement unit', 'Staples', 'Pending', 224.14),
('00000117', 'Tablet', 'USB-C tablet', 4, 'Meeting room setup', 'Newegg', 'Pending', 123.63),
('00000118', 'Docking Station', 'High-end docking station', 1, 'Replacement unit', 'Newegg', 'Pending', 165.18),
('00000118', 'Desk Lamp', 'USB-C desk lamp', 5, 'Ergonomic setup', 'Amazon', 'Pending', 89.01),
('00000118', 'Speaker', 'Adjustable speaker', 3, 'Replacement unit', 'Newegg', 'Pending', 176.98),
('00000119', 'Headset', 'High-end headset', 3, 'Replacement unit', 'Amazon', 'Pending', 293.52),
('00000119', 'Chair', 'Compact chair', 3, 'Team expansion', 'Amazon', 'Pending', 196.04),
('00000119', 'Camera', 'Wireless camera', 2, 'Team expansion', 'Amazon', 'Pending', 119.99),
('00000120', 'Stand', 'Adjustable stand', 2, 'QA equipment', 'Tech World', 'Pending', 95.82),
('00000120', 'SSD', 'Adjustable ssd', 2, 'Replacement unit', 'Newegg', 'Pending', 224.93),
('00000120', 'Router', 'Wireless router', 5, 'QA equipment', 'Tech World', 'Pending', 277.61),
('00000121', 'Tablet', 'High-end tablet', 4, 'Ergonomic setup', 'Tech World', 'Pending', 182.14),
('00000121', 'Docking Station', 'Adjustable docking station', 2, 'Ergonomic setup', 'Tech World', 'Pending', 174.95),
('00000121', 'Desk Lamp', 'Compact desk lamp', 5, 'Meeting room setup', 'Amazon', 'Pending', 228.42),
('00000122', 'Speaker', 'Wireless speaker', 1, 'QA equipment', 'Office Depot', 'Pending', 92.26),
('00000122', 'Headset', 'High-end headset', 5, 'Team expansion', 'Newegg', 'Pending', 198.58),
('00000122', 'Chair', 'Compact chair', 3, 'Meeting room setup', 'Staples', 'Pending', 153.42),
('00000123', 'Camera', 'USB-C camera', 1, 'Meeting room setup', 'Office Depot', 'Pending', 192.42),
('00000123', 'Stand', 'Wireless stand', 2, 'Team expansion', 'Newegg', 'Pending', 135.89),
('00000123', 'SSD', 'Adjustable ssd', 2, 'QA equipment', 'Tech World', 'Pending', 272.52),
('00000124', 'Router', 'High-end router', 4, 'Ergonomic setup', 'Amazon', 'Pending', 287.96),
('00000124', 'Tablet', 'Compact tablet', 3, 'QA equipment', 'Staples', 'Pending', 261.26),
('00000124', 'Docking Station', 'High-end docking station', 5, 'QA equipment', 'Newegg', 'Pending', 272.99),
('00000125', 'Desk Lamp', 'Adjustable desk lamp', 4, 'Replacement unit', 'Office Depot', 'Pending', 287.18),
('00000125', 'Speaker', 'Wireless speaker', 1, 'Replacement unit', 'Amazon', 'Pending', 53.24),
('00000125', 'Headset', 'Compact headset', 2, 'QA equipment', 'Office Depot', 'Pending', 98.9),
('00000126', 'Chair', 'Adjustable chair', 5, 'Meeting room setup', 'Staples', 'Pending', 54.89),
('00000126', 'Camera', 'USB-C camera', 5, 'Meeting room setup', 'Tech World', 'Pending', 207.81),
('00000126', 'Stand', 'Compact stand', 1, 'Meeting room setup', 'Amazon', 'Pending', 61.08),
('00000127', 'SSD', 'USB-C ssd', 3, 'Ergonomic setup', 'Tech World', 'Pending', 248.38),
('00000127', 'Router', 'Adjustable router', 3, 'Meeting room setup', 'Tech World', 'Pending', 286.88),
('00000127', 'Tablet', 'Compact tablet', 2, 'QA equipment', 'Office Depot', 'Pending', 112.84),
('00000128', 'Docking Station', 'Adjustable docking station', 5, 'Meeting room setup', 'Tech World', 'Pending', 93.65),
('00000128', 'Desk Lamp', 'Compact desk lamp', 3, 'Ergonomic setup', 'Tech World', 'Pending', 175.08),
('00000128', 'Speaker', 'USB-C speaker', 4, 'Team expansion', 'Office Depot', 'Pending', 57.58),
('00000129', 'Headset', 'Adjustable headset', 2, 'QA equipment', 'Newegg', 'Pending', 71.55),
('00000129', 'Chair', 'Wireless chair', 1, 'Meeting room setup', 'Newegg', 'Pending', 156.47),
('00000129', 'Camera', 'High-end camera', 5, 'Replacement unit', 'Tech World', 'Pending', 61.99),
('00000130', 'Stand', 'Adjustable stand', 4, 'Replacement unit', 'Tech World', 'Pending', 275.5),
('00000130', 'SSD', 'Adjustable ssd', 4, 'Team expansion', 'Newegg', 'Pending', 180.6),
('00000130', 'Router', 'High-end router', 2, 'Ergonomic setup', 'Amazon', 'Pending', 84.54),
('00000131', 'Tablet', 'Adjustable tablet', 4, 'Meeting room setup', 'Office Depot', 'Pending', 232.86),
('00000131', 'Docking Station', 'USB-C docking station', 3, 'Team expansion', 'Office Depot', 'Pending', 79.11),
('00000131', 'Desk Lamp', 'High-end desk lamp', 1, 'Ergonomic setup', 'Newegg', 'Pending', 89.01),
('00000132', 'Speaker', 'Compact speaker', 4, 'Replacement unit', 'Office Depot', 'Pending', 56.15),
('00000132', 'Headset', 'USB-C headset', 4, 'Team expansion', 'Tech World', 'Pending', 230.0),
('00000132', 'Chair', 'High-end chair', 3, 'Ergonomic setup', 'Office Depot', 'Pending', 153.77),
('00000133', 'Camera', 'High-end camera', 1, 'Ergonomic setup', 'Staples', 'Pending', 181.15),
('00000133', 'Webcam', 'High-end camera', 1, 'Meeting setup', 'Staples', 'Pending', 299.15),
('00000134', 'Stand', 'Adjustable stand', 5, 'QA equipment', 'Office Depot', 'Pending', 100.66),
('00000134', 'SSD', 'Adjustable ssd', 1, 'Team expansion', 'Staples', 'Pending', 114.36),
('00000135', 'Monitor', 'HD 16inch', 1, 'To work faster', 'Bestbuy', 'Pending', 112.36),
('00000135', 'Wireless Mouse', 'Logitech mx4', 1, 'Mine broke', 'Staples', 'Pending', 199.36),
('00000135', 'Docking Station', 'Anker plus', 1, 'My mac doesnt have USB A', 'Bestbuy', 'Pending', 59.50),
('00000135', 'Microphone', 'Lapele mic', 2, 'Recording the new ad', 'Amazon', 'Pending', 224.44),
('00000136', 'Docking Station', 'Anker plus', 1, 'My mac doesnt have USB A', 'Bestbuy', 'Pending', 59.50),
('00000136', 'Printer paper', 'A4', 2, 'John office is over', 'Walmart', 'Pending', 24.99),
('00000137', 'Pens', 'Blue and red', 10, 'For the customers', 'Dolarama', 'Pending', 10.50),
('00000137', 'Monitor Riser', 'Any works', 1, 'My neck is hurting', 'Amazon', 'Pending', 22.35),
('00000138', 'Noise-Cancelling Headphones', 'Over-ear, Bluetooth, with built-in mic', 1, 'For virtual meetings and focus', 'Bestbuy', 'Pending', 99.35),
('00000138', 'Whiteboard (3x4 ft)', 'Magnetic dry erase board with markers and eraser included', 1, 'Team brainstorming in meeting room', 'Amazon', 'Pending', 55.99),

('00000139', 'Microsoft 365 Business License', 'Any works', 1, 'Includes Word, Excel, Outlook, Teams', 'Amazon', 'Approved', 99.99),
('00000139', 'Printer Ink Cartridges', 'Black cartridges compatible with HP LaserJet', 1, 'Restocking ink for shared office printer', 'HP Store', 'Pending', 45.99),

('00000140', 'Fire Extinguisher', 'ABC-rated with wall mount and inspection tag', 1, 'Safety requirement per building policy', 'Amazon', 'Pending', 105.29),
('00000140', 'Standing Desk Converter', 'Adjustable riser for dual monitor setup', 1, 'Ergonomic upgrade for seated desks', 'Amazon', 'Pending', 245.95),

('00000141', 'Pack of pens', 'Blue and red', 100, 'For meeting notes', 'Amazon', 'Pending', 11.99),
('00000141', 'Speakers', 'JBL Boombox 2', 1, 'For the office anual party', 'Bestbuy', 'Pending', 399.99),
('00000141', 'Plastic cups', '50ml with lid', 100, 'For coffee machine', 'Amazon', 'Pending', 15.99),


('00000142', 'Cable organizer', 'Colored ones', 100, 'For organizing my desk', 'Amazon', 'Pending', 12.99),
('00000142', 'Speakers', 'Logitech sound master', 1, 'For announcements', 'Bestbuy', 'Pending', 499.99),
('00000142', 'Plastic cups', '50ml with lid', 100, 'For coffee machine', 'Amazon', 'Pending', 15.99),


('00000143', 'Sunglasses', 'With polarized filter', 1, 'For use when visiting clients', 'Oakley website', 'Pending', 199.85),
('00000143', 'Monitor', 'Foldable portable', 1, 'For using on the field trip', 'Bestbuy', 'Pending', 299.99),
('00000143', 'Chairs', 'Black ones', 20, 'For the new break roon', 'Ikea', 'Pending', 59.99);

--('00000144', 'Webcam', '1080p HD webcam', 2, 'Required for remote meetings', 'Amazon', 'Pending', 397.45),
--('00000144', 'Monitor', '24-inch Full HD monitor', 3, 'Standard screen for workstation', 'Staples', 'Pending', 128.25),
--('00000144', 'Mouse', 'Wireless ergonomic mouse', 3, 'Comfortable for daily use', 'Tech Warehouse', 'Pending', 105.21),

--('00000145', 'Chair', 'Ergonomic office chair', 3, 'Promotes good posture', 'Staples', 'Pending', 346.29),
--('00000145', 'Printer', 'All-in-one printer', 2, 'Shared office printer', 'Amazon', 'Pending', 483.72),
--('00000145', 'Monitor', '24-inch Full HD monitor', 5, 'Standard screen for workstation', 'Amazon', 'Pending', 229.08),

--('00000146', 'Desk', 'Adjustable standing desk', 3, 'Supports ergonomic setup', 'Office Depot', 'Pending', 435.17),
--('00000146', 'Docking Station', 'USB-C docking station', 1, 'Connects multiple devices', 'Staples', 'Pending', 71.87),
--('00000146', 'Webcam', '1080p HD webcam', 1, 'Required for remote meetings', 'Best Buy', 'Pending', 330.75);




--SELECT * FROM PurchaseOrderItem
--SEED DATA JG--

--SELECT * FROM Job;
--SELECT * FROM Department;
--SELECT * FROM Employee;
--SELECT * FROM PurchaseOrder;
--SELECT * FROM PurchaseOrderItem;


------------ STORED PROCS --------------

GO
CREATE OR ALTER PROC spAddEmployee
	@Id					CHAR(8)			OUTPUT,
	@EmploymentStatusId	Int,
	@SupervisorId		CHAR(8)			= NULL,
	@DepartmentId		INT				= NULL,
	@JobAssignmentId	INT,
	@Password			VARCHAR(255),
	@PasswordSalt		BINARY(16),
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@MiddleInitial		CHAR(1)			= NULL,
	@StreetAddress		VARCHAR(255),
	@City				VARCHAR(255),
	@Province			VARCHAR(255),
	@PostalCode			VARCHAR(7),
	@DOB				DATETIME,
	@SIN				VARCHAR(11),
	@SeniorityDate		DATETIME,
	@JobStartDate		DATETIME,
	@WorkPhone			VARCHAR(14),
	@CellPhone			VARCHAR(14),
	@EmailAddress		VARCHAR(255),
	@OfficeLocation		VARCHAR(255),
	@RetirementDate		DATETIME = NULL,
	@TerminationDate	DATETIME = NULL,
	@RecordVersion		ROWVERSION	OUTPUT
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION

		INSERT INTO Employee(Id, EmploymentStatusId, SupervisorId, DepartmentId, JobAssignmentId, Password, PasswordSalt, FirstName, LastName, MiddleInitial,
			StreetAddress, City, Province, PostalCode, DOB, SIN, SeniorityDate, JobStartDate, WorkPhone, CellPhone, EmailAddress,
			OfficeLocation, RetirementDate, TerminationDate
		)
		VALUES(@Id, @EmploymentStatusId, @SupervisorId, @DepartmentId, @JobAssignmentId, @Password, @PasswordSalt, @FirstName, @LastName, @MiddleInitial, @StreetAddress,
		@City, @Province, @PostalCode, @DOB, @SIN, @SeniorityDate, @JobStartDate, @WorkPhone, @CellPhone, @EmailAddress, @OfficeLocation, @RetirementDate, @TerminationDate);

		-- SET @Id = SCOPE_IDENTITY();

		SELECT @RecordVersion = RecordVersion
		FROM Employee
		WHERE Id = @Id;
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		;THROW
	END CATCH
END


GO
CREATE OR ALTER PROC spGetAllEmployees
AS
BEGIN
	SELECT * FROM Employee;
END


GO
CREATE OR ALTER PROC spGetLastEmployee
AS
BEGIN
	SELECT TOP 1 * FROM Employee ORDER BY ID DESC;
END

GO
CREATE OR ALTER PROC spGetEmployee
	@Id		CHAR(8)
AS
BEGIN
	SELECT * FROM Employee WHERE Id = @Id;
END

GO

GO
CREATE OR ALTER PROC spDeleteEmployee
	@Id				CHAR(8),
	@RecordVersion ROWVERSION
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION
		DELETE FROM Employee
		WHERE Id = @Id AND RecordVersion = @RecordVersion;

		IF @@ROWCOUNT = 0
		BEGIN
			 -- No rows updated = concurrency conflict
			THROW 50002, 'Couldn''t Delete Employee. Employee has been modified or deleted by another user.', 1;
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		THROW;
	END CATCH
END

GO
CREATE OR ALTER PROC spGetAllJobs
AS
BEGIN
	SELECT * FROM Job;
END

GO
CREATE OR ALTER PROC spGetJob
	@Id		INT
AS
BEGIN
	SELECT * FROM Job WHERE Id = @Id;
END


GO
CREATE OR ALTER PROC spGetAllEmployeesDisplay
AS
BEGIN
	SELECT
		Employee.Id AS Id,
		Employee.FirstName + ' ' + ISNULL(Employee.MiddleInitial + ' ', '') + ' ' + Employee.LastName AS FullName,
		empSupervisor.FirstName + ' ' + ISNULL(empSupervisor.MiddleInitial + ' ', '') + ' ' + empSupervisor.LastName AS Supervisor,
		Department.Name AS Department,
		Job.Title AS Job,
		Employee.OfficeLocation AS OfficeLocation
	FROM Employee INNER JOIN Department
	ON
		Employee.DepartmentId = Department.Id
	INNER JOIN Job
	ON
		Employee.JobAssignmentId = Job.Id
	LEFT JOIN Employee AS empSupervisor
	ON
		Employee.SupervisorId = empSupervisor.Id
	ORDER BY
		FullName ASC;
END



GO
CREATE OR ALTER PROC spGetEmployeeJob
	@Id		CHAR(8)
AS
BEGIN
	SELECT Job.Title FROM Employee INNER JOIN Job
	ON
		Employee.JobAssignmentId = Job.Id
	WHERE
	Employee.Id = @Id;
END


GO
CREATE OR ALTER PROC spGetAllDepartments
AS
BEGIN
	SELECT * FROM Department ORDER BY Name ASC;
END

GO
CREATE OR ALTER PROC spGetDepartment
	@Id		INT
AS
BEGIN
	SELECT * FROM Department WHERE Id = @Id;
END

GO
CREATE OR ALTER PROC spCreateDepartment
	@Id		INT		OUTPUT,
	@Name	VARCHAR(128),
	@Description	VARCHAR(512),
	@InvocationDate	DATETIME
AS
BEGIN
	INSERT INTO Department(Name, Description, InvocationDate)
	VALUES(@Name, @Description, @InvocationDate);

	SET @Id = SCOPE_IDENTITY();
END

GO
CREATE OR ALTER PROC spUpdateDepartment
	@Id				INT,
	@Name			VARCHAR(128),
	@Description	VARCHAR(512),
	@InvocationDate	DATETIME,
	@RecordVersion	ROWVERSION
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
		
		UPDATE Department
		SET
			Name = @Name,
			Description = @Description,
			InvocationDate = @InvocationDate
		WHERE
			Id = @Id AND RecordVersion = @RecordVersion;

		IF @@ROWCOUNT = 0
		BEGIN

			 -- No rows updated = concurrency conflict
			THROW 50002, 'Couldn''t Update Department. Department has been modified or deleted by another user.', 1;
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
		END;
		
		THROW;
	END CATCH
END

--------------------------------------------------
GO
CREATE OR ALTER PROC spUpdateEmployee
	@Id					CHAR(8),
	@SupervisorId		CHAR(8)		=	NULL,
	@DepartmentId		INT			=	NULL,
	@JobAssignmentId	INT,
	@Password			VARCHAR(255),
	@PasswordSalt		BINARY(16),
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@MiddleInitial		CHAR(1)		=	NULL,
	@StreetAddress		VARCHAR(255),
	@City				VARCHAR(255),
	@Province			VARCHAR(255),
	@PostalCode			VARCHAR(7),
	@DOB				DATETIME,
	@SIN				VARCHAR(11),
	@SeniorityDate		DATETIME,
	@JobStartDate		DATETIME,
	@WorkPhone			VARCHAR(14),
	@CellPhone			VARCHAR(14),
	@EmailAddress		VARCHAR(255),
	@OfficeLocation		VARCHAR(255),

	@EmploymentStatusId	INT,
	@RetirementDate		DATETIME = NULL,
	@TerminationDate	DATETIME = NULL,
	@RecordVersion		ROWVERSION
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
		
		UPDATE Employee
		SET
			SupervisorId = @SupervisorId,
			DepartmentId = @DepartmentId,
			JobAssignmentId = @JobAssignmentId,
			Password = @Password,
			PasswordSalt = @PasswordSalt,
			FirstName = @FirstName,
			LastName = @LastName,
			MiddleInitial = @MiddleInitial,
			StreetAddress = @StreetAddress,
			City = @City,
			Province = @Province,
			PostalCode = @PostalCode,
			DOB = @DOB,
			SIN = @SIN,
			SeniorityDate = @SeniorityDate,
			JobStartDate = @JobStartDate,
			WorkPhone = @WorkPhone,
			CellPhone = @CellPhone,
			EmailAddress = @EmailAddress,
			OfficeLocation = @OfficeLocation,

			RetirementDate = @RetirementDate,
			TerminationDate = @TerminationDate,
			EmploymentStatusId = @EmploymentStatusId

		WHERE
			Id = @Id AND RecordVersion = @RecordVersion;;

		IF @@ROWCOUNT = 0
		BEGIN

			 -- No rows updated = concurrency conflict

			THROW 50002, 'Couldn''t Update Employee. Employee has been modified or deleted by another user.', 1;
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
		END;
		
		THROW;
	END CATCH
END

GO
CREATE OR ALTER PROC spDeleteDepartment
	@Id				INT,
	@RecordVersion	ROWVERSION
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION
		DELETE FROM Department
		WHERE Id = @Id AND RecordVersion = @RecordVersion; 

		IF @@ROWCOUNT = 0
		BEGIN

			 -- No rows updated = concurrency conflict

			THROW 50003, 'Couldn''t Delete Department. Department has been modified or deleted by another user.', 1;
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		
		THROW;
	END CATCH
END

GO
CREATE OR ALTER PROCEDURE spGetSupervisorsWithFewerThan10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.Id,
        s.FirstName,
        s.LastName,
        j.Title AS JobTitle,
        COUNT(e.Id) AS SupervisedCount
    FROM
        Employee s
    INNER JOIN Job j ON s.JobAssignmentId = j.Id
    LEFT JOIN Employee e ON s.Id = e.SupervisorId
    WHERE
        j.Title IN (
            'CEO', 'HR Manager', 'Project Manager', 'Product Manager',
            'Developer Manager', 'Infrastructure Manager'
        )
    GROUP BY
        s.Id, s.FirstName, s.LastName, j.Title
    HAVING
        COUNT(e.Id) < 10
    ORDER BY
        SupervisedCount ASC;
END

GO
CREATE OR ALTER PROC spGetDepartmentByName
	@Name	VARCHAR(128)
AS
BEGIN
	SELECT * FROM Department WHERE LOWER(Name) = LOWER(@Name);
END

GO
CREATE OR ALTER PROCEDURE spGetEmployeeDetails
    @EmployeeId CHAR(8)
AS
BEGIN
    SELECT 
        e.Id,
        (e.FirstName + ' ' + e.LastName) AS FullName,
        d.Name AS Department,
        (s.FirstName + ' ' + s.LastName) AS SupervisorFullName
    FROM Employee e
    LEFT JOIN Department d ON e.DepartmentId = d.Id
    LEFT JOIN Employee s ON e.SupervisorId = s.Id
    WHERE e.Id = @EmployeeId
END
GO

GO
CREATE OR ALTER PROCEDURE spGetEmployeeBySIN
    @SIN VARCHAR(11),
    @ExcludeId CHAR(8) = NULL
AS
BEGIN
    SELECT *
    FROM Employee
    WHERE SIN = @SIN
      AND (@ExcludeId IS NULL OR Id <> @ExcludeId)
END


GO
--GetAll--
CREATE OR ALTER PROCEDURE spGetAllPurchaseOrders
AS
BEGIN
    SELECT * FROM PurchaseOrder
	ORDER BY DateCreated DESC
END
GO
--EXEC spGetAllPurchaseOrders

--GetSummary--

CREATE OR ALTER PROCEDURE spGetPurchaseOrderSummaries
AS
BEGIN
    SELECT 
        po.PurchaseOrderId,
        po.DateCreated,
        po.Status,
        po.EmployeeId,
        e.FirstName + ' ' + e.LastName AS EmployeeFullName
    FROM PurchaseOrder po
    INNER JOIN Employee e ON po.EmployeeId = e.Id
    ORDER BY po.DateCreated DESC
END
GO




--GetById--
CREATE OR ALTER PROCEDURE spGetPurchaseOrderById
    @PurchaseOrderId CHAR(8)
AS
BEGIN
    SELECT * FROM PurchaseOrder WHERE PurchaseOrderId = @PurchaseOrderId
END
GO
--EXEC spGetPurchaseOrderById @PurchaseOrderId = '00000101'


--GetById--
CREATE OR ALTER PROCEDURE spGetPurchaseOrderItemsByPOId
    @PurchaseOrderId CHAR(8)
AS
BEGIN
    SELECT * FROM PurchaseOrderItem WHERE PurchaseOrderId = @PurchaseOrderId
END
GO
--EXEC spGetPurchaseOrderItemsByPOId @PurchaseOrderId = 101


--Create PO
CREATE OR ALTER PROCEDURE spCreatePurchaseOrder
    @PurchaseOrderId CHAR(8),
    @EmployeeId CHAR(8),
    @Status INT,
    @DateCreated DATETIME
AS
BEGIN
    BEGIN TRY
        INSERT INTO PurchaseOrder (
			PurchaseOrderId,
            EmployeeId,
            Status,
            DateCreated
        )
        VALUES (
			@PurchaseOrderId,
            @EmployeeId,
            @Status,
            @DateCreated
        ) 
    END TRY
    BEGIN CATCH
        ;THROW
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE spCreateFullPurchaseOrder
    @PurchaseOrderId CHAR(8),
    @EmployeeId CHAR(8),
    @Status INT,
    @DateCreated DATETIME,
	@Name                VARCHAR(255),
    @Description         VARCHAR(255),
    @Justification       VARCHAR(255),
    @PurchaseLocation    VARCHAR(255),
    @ItemStatus          INT,
	@Quantity            INT,
    @Price               MONEY
AS
BEGIN
	SET NOCOUNT ON;
    BEGIN TRY
	BEGIN TRANSACTION;
	   EXEC spCreatePurchaseOrder
	    @PurchaseOrderId = @PurchaseOrderId ,
		@EmployeeId = @EmployeeId,
		@Status = @Status,
		@DateCreated = @DateCreated;

		EXEC spCreatePOItem
			@PurchaseOrderId,
            @Name,
            @Description,
			@Quantity,
            @Justification,
            @PurchaseLocation,
            @ItemStatus,
            @Price
		COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000) = Error_Message();
		RAISERROR(@ErrorMessage, 16,1);
        ;THROW
    END CATCH
END
GO

--Create po item--

CREATE OR ALTER PROCEDURE spCreatePOItem
    @PurchaseOrderId     CHAR(8),
    @Name                VARCHAR(255),
    @Description         VARCHAR(255),
    @Price               MONEY,
    @Quantity            INT,
    @Justification       VARCHAR(255),
    @PurchaseLocation    VARCHAR(255),
	@ItemStatus			 VARCHAR(10),
    @RecordVersion		 ROWVERSION
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM PurchaseOrder
            WHERE PurchaseOrderId = @PurchaseOrderId
              AND RecordVersion = @RecordVersion
              AND Status != 2
        )
        BEGIN
            THROW 50002, 'Couldn''t add item. The purchase order has been modified or closed by another user.', 1;
        END


        INSERT INTO PurchaseOrderItem (
            PurchaseOrderId,
            Name,
            Description,
            Price,
            Quantity,
            Justification,
            PurchaseLocation,
			ItemStatus

        )
        VALUES (
            @PurchaseOrderId,
            @Name,
            @Description,
            @Price,
            @Quantity,
            @Justification,
            @PurchaseLocation,
			@ItemStatus
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

--Get last pO
CREATE OR ALTER PROC spGetLastPO
AS
BEGIN
	SELECT TOP 1 * FROM PurchaseOrder ORDER BY PurchaseOrderId DESC;
END
GO

-------
CREATE OR ALTER PROCEDURE spGetItemsByEmployeeId
    @EmployeeId CHAR(8)
AS
BEGIN
    SELECT 
        i.*
    FROM 
        PurchaseOrderItem i
    INNER JOIN 
        PurchaseOrder p ON i.PurchaseOrderId = p.PurchaseOrderId
    WHERE 
        p.EmployeeId = @EmployeeId
END
GO
---------

CREATE OR ALTER PROCEDURE spGetMatchingPOItem
    @PurchaseOrderId CHAR(8),
    @Name VARCHAR(100),
    @Description VARCHAR(255),
    @Price DECIMAL(10,2),
    @Justification VARCHAR(255),
    @PurchaseLocation VARCHAR(100)
AS
BEGIN

    SELECT * FROM PurchaseOrderItem

    WHERE PurchaseOrderId = @PurchaseOrderId
      AND Name = @Name
      AND Description = @Description
      AND Price = @Price
      AND Justification = @Justification
      AND PurchaseLocation = @PurchaseLocation
END
GO

--------
CREATE OR ALTER PROCEDURE spUpdateItemQuantity
    @ItemId INT,
    @QuantityToAdd INT
AS
BEGIN
    UPDATE PurchaseOrderItem
    SET Quantity = Quantity + @QuantityToAdd
    WHERE Id = @ItemId
END
GO

-----

CREATE OR ALTER PROCEDURE spGetEmployeeDetails
    @EmployeeId CHAR(8)
AS
BEGIN
    SELECT 
        e.Id,
        (e.FirstName + ' ' + e.LastName) AS FullName,
        d.Name AS Department,
        (s.FirstName + ' ' + s.LastName) AS SupervisorFullName
    FROM Employee e
    LEFT JOIN Department d ON e.DepartmentId = d.Id
    LEFT JOIN Employee s ON e.SupervisorId = s.Id
    WHERE e.Id = @EmployeeId
END
GO


--SEARCH
CREATE OR ALTER PROCEDURE spSearchPurchaseOrders
    @EmployeeId CHAR(8) = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @PurchaseOrderId CHAR(8) = NULL,
    @Status INT = NULL,
    @IncludeDepartment BIT = 0,
    @EmployeeName VARCHAR(100) = NULL,
    @IsCeo BIT = 0

AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EffectiveEndDate DATETIME = NULL;
    DECLARE @DepartmentId INT = NULL;

    IF @EndDate IS NOT NULL
        SET @EffectiveEndDate = DATEADD(MILLISECOND, -3, DATEADD(DAY, 1, CAST(@EndDate AS DATETIME)));

    IF @IncludeDepartment = 1 AND @EmployeeId IS NOT NULL
    BEGIN
        SELECT @DepartmentId = DepartmentId
        FROM Employee
        WHERE Id = @EmployeeId;
    END


    SELECT 
        po.PurchaseOrderId,
        po.EmployeeId,
        po.DateCreated,
        po.Status,
        po.RecordVersion,
        e.FirstName + ' ' + 
        ISNULL(NULLIF(e.MiddleInitial, '') + ' ', '') + 
        e.LastName AS EmployeeFullName
    FROM PurchaseOrder po
    INNER JOIN Employee e ON po.EmployeeId = e.Id
    WHERE
        (
            (@IsCeo = 1 AND (
                e.SupervisorId = '00000001' OR e.Id = @EmployeeId
            ))

            OR (
                @IsCeo = 0 AND (
                    (@IncludeDepartment = 1 AND e.DepartmentId = @DepartmentId)
                    OR po.EmployeeId = @EmployeeId
                )
            )

        )
        AND (@PurchaseOrderId IS NULL OR po.PurchaseOrderId = @PurchaseOrderId)
        AND (@StartDate IS NULL OR po.DateCreated >= @StartDate)
        AND (@EndDate IS NULL OR po.DateCreated <= @EffectiveEndDate)
        AND (@Status IS NULL OR po.Status = @Status)
        AND (
            @EmployeeName IS NULL OR 
            (e.FirstName + ' ' + ISNULL(e.MiddleInitial + ' ', '') + e.LastName) LIKE '%' + @EmployeeName + '%'
        )
    ORDER BY po.DateCreated DESC;
END;
GO







-- SELECT * FROM Employee WHERE EmploymentStatus = 'Active';
GO
CREATE OR ALTER PROC spVerifyLogin
	@Id CHAR(8),
	@Password VARCHAR(255)
AS
BEGIN

	SELECT * FROM Employee WHERE Id = @Id AND Password = @Password

	AND EmploymentStatusId = 1;

END


EXEC spGetAllEmployeesDisplay;

GO
CREATE OR ALTER PROC spSearchEmployee
	@DepartmentId INT = NULL,
	@EmployeeID CHAR(8) = NULL,
	@LastName VARCHAR(50) = NULL
AS
BEGIN


	SELECT
		Employee.Id,
		Employee.FirstName,
		Employee.MiddleInitial,
		Employee.LastName,
		Employee.StreetAddress + ', ' + Employee.City + ', ' + Employee.Province + ' ' + Employee.PostalCode AS HomeAddress,
		Employee.WorkPhone,
		Employee.CellPhone,
		Employee.EmailAddress,

		Employee.OfficeLocation,
		Department.Name,
		Job.Title AS JobTitle
	FROM Employee

	LEFT JOIN Department
	ON
		Employee.DepartmentId = Department.Id
	LEFT JOIN Job

	ON
		Employee.JobAssignmentId = Job.Id
	WHERE
		(@DepartmentId IS NULL OR Employee.DepartmentId = @DepartmentId)

		AND (
			@EmployeeID IS NULL OR Employee.Id = @EmployeeID
		)
		
		AND (
			@LastName IS NULL OR Employee.LastName LIKE '%' + @LastName + '%'

		)
		ORDER BY
			Employee.LastName ASC, Employee.FirstName ASC;
END
GO

CREATE OR ALTER PROC spGetEmployeeSalt
	@Id  CHAR(50)
AS
BEGIN
	SELECT PasswordSalt FROM Employee WHERE Id = @Id;
END
GO

--EXEC spSearchEmployee @EmployeeID = '00000002';

--EXEC spGetEmployeeSalt @Id = '00000001'


--EXEC spGetAllEmployeesDisplay;
--EXEC spGetEmployeeJob @Id = '00000006';

-- EXEC spVerifyLogin @Id = '00000005', @Password = 'e0a281dc3915f1a54eb47317796fa931a7c2d2595facfef8e832697304ddeffc';


-- SELECT * FROM PurchaseOrderItem
-- SELECT * FROM PurchaseOrder
-- SELECT * FROM Employee;
-- SELECT * FROM Employee WHERE SupervisorId = '00000001'
-- EXEC spGetSupervisorsWithFewerThan10


GO
CREATE OR ALTER PROCEDURE GetSupervisorsByDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        E.Id,
        E.FirstName,
        E.LastName,
        J.Title AS JobTitle,
        E.DepartmentId,
        D.Name AS DepartmentName,
        COUNT(Subordinate.Id) AS SupervisedCount
    FROM 
        Employee E
    INNER JOIN 
        Employee Subordinate ON Subordinate.SupervisorId = E.Id
    INNER JOIN 
        Job J ON E.JobAssignmentId = J.Id
    LEFT JOIN
        Department D ON E.DepartmentId = D.Id
    WHERE 
        Subordinate.DepartmentId = @DepartmentId
    GROUP BY
        E.Id, E.FirstName, E.LastName, E.DepartmentId, J.Title, D.Name
    HAVING
        COUNT(Subordinate.Id) < 10
    ORDER BY 
        E.FirstName, E.LastName ASC;
END;
GO
--EXEC GetSupervisorsByDepartment @DepartmentId = 4;
--SELECT * FROM Employee;
--SELECT * FROM Department;


CREATE OR ALTER PROCEDURE spUpdatePOItem
    @Id                 INT,
    @Name               VARCHAR(255),
    @Description        VARCHAR(255),
    @Quantity           INT,
    @Justification      VARCHAR(255),
    @PurchaseLocation   VARCHAR(255),
    @Price              MONEY,
    @ItemStatus         VARCHAR(20) = NULL,
    @RecordVersion      ROWVERSION
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE PurchaseOrderItem
        SET
            Name = @Name,
            Description = @Description,
            Quantity = @Quantity,
            Justification = @Justification,
            PurchaseLocation = @PurchaseLocation,
            Price = @Price,
            ItemStatus = COALESCE(@ItemStatus, ItemStatus)

        WHERE
            Id = @Id AND RecordVersion = @RecordVersion;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50002, 'Couldn''t update PurchaseOrderItem. It has been modified or deleted by another user.', 1;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO




CREATE OR ALTER PROCEDURE spGetItemById
    @ItemId INT
AS
BEGIN
    SELECT * FROM PurchaseOrderItem WHERE Id = @ItemId
END
GO





--SELECT * FROM Employee, Job
	--WHERE Employee.JobAssignmentId = Job.Id

--	SELECT * FROM Job

	--select  * from Department

--SELECT * FROM Employee
--select  * from Department
--SELECT * FROM Job

CREATE OR ALTER PROCEDURE spGetPurchaseOrdersByDepartment
    @DepartmentId INT
AS
BEGIN
    SELECT po.PurchaseOrderId AS PONumber,
        po.DateCreated AS POCreationDate,
        s.FirstName + ' ' + s.LastName AS SupervisorName,
        po.Status AS Status
    FROM PurchaseOrder po JOIN Employee e ON po.EmployeeId = e.Id
    LEFT JOIN Employee s ON e.SupervisorId = s.Id
    JOIN Department d ON e.DepartmentId = d.Id
    WHERE d.Id = @DepartmentId
	AND (po.Status = 0 OR po.Status = 1)
    ORDER BY po.DateCreated ASC;
END
GO


CREATE OR ALTER PROCEDURE spSetItemStatus
    @ItemId INT,
    @NewStatus VARCHAR(20),
    @DenialReason VARCHAR(500) = NULL,
    @RecordVersion ROWVERSION
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM PurchaseOrderItem WHERE Id = @ItemId)
        BEGIN
            THROW 50001, 'Item not found.', 1;
        END

        DECLARE @RowsAffected INT;

        IF @NewStatus = 'Denied'
        BEGIN
            UPDATE PurchaseOrderItem
            SET ItemStatus = @NewStatus,
                DenialReason = @DenialReason
            WHERE Id = @ItemId AND RecordVersion = @RecordVersion;

            SET @RowsAffected = @@ROWCOUNT;
        END
        ELSE
        BEGIN
            UPDATE PurchaseOrderItem
            SET ItemStatus = @NewStatus,
                DenialReason = NULL
            WHERE Id = @ItemId AND RecordVersion = @RecordVersion;

            SET @RowsAffected = @@ROWCOUNT;
        END

        IF @RowsAffected = 0
        BEGIN
            THROW 50002, 'Concurrency conflict: the item was modified by another user.', 1;
        END

        IF @NewStatus IN ('Approved', 'Denied')
        BEGIN
            DECLARE @PurchaseOrderId CHAR(8);
            SELECT @PurchaseOrderId = PurchaseOrderId FROM PurchaseOrderItem WHERE Id = @ItemId;

            IF NOT EXISTS (
                SELECT 1
                FROM PurchaseOrderItem
                WHERE PurchaseOrderId = @PurchaseOrderId
                  AND Id != @ItemId
                  AND ItemStatus IN ('Approved', 'Denied')
            )
            BEGIN
                UPDATE PurchaseOrder
                SET Status = 1
                WHERE PurchaseOrderId = @PurchaseOrderId;
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO


CREATE PROCEDURE spUpdatePurchaseOrder
    @PurchaseOrderId CHAR(10),
    @EmployeeId CHAR(10),
    @Status INT,
    @DateCreated DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PurchaseOrder
    SET
        EmployeeId = @EmployeeId,
        Status = @Status,
        DateCreated = @DateCreated
    WHERE
        PurchaseOrderId = @PurchaseOrderId;
END;
GO


CREATE OR ALTER PROCEDURE spDeletePOItem
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM PurchaseOrderItem
        WHERE Id = @ItemId;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50003, 'Couldn''t delete PurchaseOrderItem. It may not exist or was already removed.', 1;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END
GO




--SELECT * FROM PurchaseOrderItem
--SELECT * FROM PurchaseOrder
--UPDATE PurchaseOrderItem SET ItemStatus = 'Pending' WHERE PurchaseOrderId = '00000141'
--UPDATE PurchaseOrder SET Status = 0 WHERE PurchaseOrderId = 00000141
-- SELECT * FROM Employee

CREATE OR ALTER PROCEDURE spSearchSupervisorsPOs
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @PurchaseOrderId CHAR(8) = NULL,
    @Status INT = NULL,
    @EmployeeName VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EffectiveEndDate DATETIME = NULL;

    IF @EndDate IS NOT NULL
        SET @EffectiveEndDate = DATEADD(MILLISECOND, -3, DATEADD(DAY, 1, CAST(@EndDate AS DATETIME)));

    SELECT 
        po.PurchaseOrderId,
        po.EmployeeId,
        po.DateCreated,
        po.Status,
        po.RecordVersion,
        e.FirstName + ' ' + 
        ISNULL(NULLIF(e.MiddleInitial, '') + ' ', '') + 
        e.LastName AS EmployeeFullName
    FROM PurchaseOrder po
    INNER JOIN Employee e ON po.EmployeeId = e.Id
    WHERE 
        e.SupervisorId = '00000001'
        AND (@PurchaseOrderId IS NULL OR po.PurchaseOrderId = @PurchaseOrderId)
        AND (@StartDate IS NULL OR po.DateCreated >= @StartDate)
        AND (@EndDate IS NULL OR po.DateCreated <= @EffectiveEndDate)
        AND (@Status IS NULL OR po.Status = @Status)
        AND (
            @EmployeeName IS NULL OR 
            (e.FirstName + ' ' + ISNULL(e.MiddleInitial + ' ', '') + e.LastName) LIKE '%' + @EmployeeName + '%'
        )
    ORDER BY po.DateCreated DESC;
END;
GO

GO
CREATE OR ALTER PROCEDURE spAddEmployeeReviewForQuarter
    @EmployeeId CHAR(8),
    @RatingOptionsId INT,
    @Comment VARCHAR(600),
    @Quarter INT,
    @Year INT,
    @ReviewDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ExistingReview INT;
    DECLARE @ReviewQuarter INT;
    DECLARE @ReviewYear INT;
    DECLARE @CurrentDate DATETIME = GETDATE();
    DECLARE @ErrorMessage NVARCHAR(200);
    
    -- Validate that review date is not in the future
    IF @ReviewDate > @CurrentDate
    BEGIN
        SET @ErrorMessage = 'Review date cannot be in the future.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Validate Quarter input
    IF @Quarter < 1 OR @Quarter > 4
    BEGIN
        SET @ErrorMessage = 'Invalid quarter. Must be 1, 2, 3, or 4.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Calculate the actual quarter and year from provided ReviewDate
    SET @ReviewQuarter = DATEPART(QUARTER, @ReviewDate);
    SET @ReviewYear = YEAR(@ReviewDate);
    
    -- Verify that the ReviewDate matches the specified Quarter and Year
    IF @ReviewQuarter != @Quarter OR @ReviewYear != @Year
    BEGIN
        SET @ErrorMessage = 'Review date does not match the specified quarter and year.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Check if employee exists
    IF NOT EXISTS (SELECT 1 FROM Employee WHERE Id = @EmployeeId)
    BEGIN
        SET @ErrorMessage = 'Employee does not exist.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Check if rating option exists
    IF NOT EXISTS (SELECT 1 FROM RatingOptions WHERE Id = @RatingOptionsId)
    BEGIN
        SET @ErrorMessage = 'Invalid rating option.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Check if the employee already has a review for this quarter/year
    SELECT @ExistingReview = COUNT(*)
    FROM EmployeeReview
    WHERE EmployeeId = @EmployeeId
      AND YEAR(ReviewDate) = @Year
      AND DATEPART(QUARTER, ReviewDate) = @Quarter;
    
    IF @ExistingReview > 0
    BEGIN
        SET @ErrorMessage = 'Employee already has a review for this quarter.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Insert the new review
    INSERT INTO EmployeeReview (EmployeeId, RatingOptionsId, Comment, ReviewDate)
    VALUES (@EmployeeId, @RatingOptionsId, @Comment, @ReviewDate);
    
    
END;
GO

-- Find employees who have not had a review for a specific quarter/year
CREATE OR ALTER PROCEDURE spFindEmployeesWithoutReviewInQuarter
    @Quarter INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StartDate DATETIME;
    DECLARE @EndDate DATETIME;
    DECLARE @ErrorMessage NVARCHAR(200);
    
    -- Validate Quarter input
    IF @Quarter < 1 OR @Quarter > 4
    BEGIN
        SET @ErrorMessage = 'Invalid quarter. Must be 1, 2, 3, or 4.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Set date range for the quarter
    IF @Quarter = 1
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 1, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 3, 31);
    END
    ELSE IF @Quarter = 2
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 4, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 6, 30);
    END
    ELSE IF @Quarter = 3
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 7, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 9, 30);
    END
    ELSE IF @Quarter = 4
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 10, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 12, 31);
    END
    
    -- Find active employees who don't have a review in the specified quarter
    SELECT 
        e.Id,
        e.FirstName,
        e.LastName,
        j.Title AS JobTitle,
        e.SeniorityDate,
        'Q' + CAST(@Quarter AS VARCHAR) + ' ' + CAST(@Year AS VARCHAR) AS MissingReviewPeriod
    FROM 
        Employee e
    LEFT JOIN 
        Job j ON e.JobAssignmentId = j.Id
    WHERE 
        e.SeniorityDate <= @EndDate 
        AND (e.TerminationDate IS NULL OR e.TerminationDate >= @StartDate)
        AND (e.RetirementDate IS NULL OR e.RetirementDate >= @StartDate)
        AND NOT EXISTS (
            SELECT 1
            FROM EmployeeReview er
            WHERE er.EmployeeId = e.Id
              AND YEAR(er.ReviewDate) = @Year
              AND DATEPART(QUARTER, er.ReviewDate) = @Quarter
        )
    ORDER BY 
        e.LastName, e.FirstName;
END;
GO

CREATE OR ALTER PROCEDURE spFindEmployeesWithoutReviewInQuarterBySupervisor
	@Id	CHAR(8),
    @Quarter INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StartDate DATETIME;
    DECLARE @EndDate DATETIME;
    DECLARE @ErrorMessage NVARCHAR(200);
    
    -- Validate Quarter input
    IF @Quarter < 1 OR @Quarter > 4
    BEGIN
        SET @ErrorMessage = 'Invalid quarter. Must be 1, 2, 3, or 4.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Set date range for the quarter
    IF @Quarter = 1
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 1, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 3, 31);
    END
    ELSE IF @Quarter = 2
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 4, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 6, 30);
    END
    ELSE IF @Quarter = 3
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 7, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 9, 30);
    END
    ELSE IF @Quarter = 4
    BEGIN
        SET @StartDate = DATEFROMPARTS(@Year, 10, 1);
        SET @EndDate = DATEFROMPARTS(@Year, 12, 31);
    END
    
    -- Find active employees who don't have a review in the specified quarter
    SELECT 
        e.Id,
        e.FirstName,
        e.LastName,
        j.Title AS JobTitle,
        e.SeniorityDate,
        'Q' + CAST(@Quarter AS VARCHAR) + ' ' + CAST(@Year AS VARCHAR) AS MissingReviewPeriod
    FROM 
        Employee e
    LEFT JOIN 
        Job j ON e.JobAssignmentId = j.Id
    WHERE 
        e.SeniorityDate <= @EndDate 
        AND (e.TerminationDate IS NULL OR e.TerminationDate >= @StartDate)
        AND (e.RetirementDate IS NULL OR e.RetirementDate >= @StartDate)
        AND NOT EXISTS (
            SELECT 1
            FROM EmployeeReview er
            WHERE er.EmployeeId = e.Id
              AND YEAR(er.ReviewDate) = @Year
              AND DATEPART(QUARTER, er.ReviewDate) = @Quarter
        )
		AND (
			e.SupervisorId = @Id -----------------------------------------------------------------------------------------------------------------------------
		)
    ORDER BY 
        e.LastName, e.FirstName;
END;

GO
-- Supporting procedures
CREATE OR ALTER PROC spGetAllEmpReviewsDetailed
AS
BEGIN
    SELECT
        rv.Id, 
        e.FirstName + ' ' + e.LastName AS FullName, 
        r.Rating, 
        rv.Comment, 
        rv.ReviewDate,
        'Q' + CAST(DATEPART(QUARTER, rv.ReviewDate) AS VARCHAR) + ' ' + CAST(YEAR(rv.ReviewDate) AS VARCHAR) AS ReviewPeriod
    FROM EmployeeReview AS rv 
    INNER JOIN Employee AS e ON rv.EmployeeId = e.Id
    INNER JOIN RatingOptions AS r ON rv.RatingOptionsId = r.Id
    ORDER BY rv.ReviewDate DESC;
END
GO

GO
CREATE OR ALTER PROC spGetAllEmpReviews
AS
BEGIN
    SELECT * FROM EmployeeReview ORDER BY ReviewDate DESC;
END
GO

GO
CREATE OR ALTER PROC spGetEmpReviewById
	@Id INT
AS
BEGIN
    SELECT * FROM EmployeeReview WHERE EmployeeReview.Id = @Id ORDER BY ReviewDate DESC ;
END
GO

GO
CREATE OR ALTER PROC spGetRatingOptions
AS
BEGIN
	SELECT * FROM RatingOptions;
END

GO
CREATE OR ALTER PROC spGetRatingOptionsById
	@Id INT
AS
BEGIN
	SELECT * FROM RatingOptions WHERE Id = @Id;
END
GO

GO
CREATE OR ALTER PROC spGetAllEmploymentStatus
AS
BEGIN
	SELECT * FROM EmploymentStatus;
END
GO

GO
CREATE OR ALTER PROC spGetEmploymentStatus
	@Id INT
AS
BEGIN
	SELECT * FROM EmploymentStatus WHERE Id = @Id;
END
GO

GO
CREATE OR ALTER PROC spFindAllReviewsMadeBySupervisor
	@Id CHAR(8)
AS
BEGIN
	SELECT EmployeeReview.Id, Employee.FirstName, Employee.LastName, S.FirstName AS SupervisorFName, S.LastName AS SupervisorLName,
	RatingOptions.Rating, EmployeeReview.Comment, EmployeeReview.ReviewDate
	FROM EmployeeReview INNER JOIN Employee ON
		Employee.Id = EmployeeReview.EmployeeId
	INNER JOIN RatingOptions ON
		RatingOptions.Id = EmployeeReview.RatingOptionsId
	INNER JOIN Employee as S ON
		Employee.SupervisorId = S.Id
	WHERE Employee.SupervisorId = @Id;
END
GO
-- EXEC spFindEmployeesWithoutReviewInQuarterBySupervisor @Id = '00000002', @Quarter = 2, @Year = 2025;

--EXEC spGetAllEmploymentStatus;
--EXEC spGetEmploymentStatus @Id = 1;

-- EXEC spGetRatingOptions;
-- EXEC spGetRatingOptionsById @Id = 1;
-- Find employees without reviews
-- EXEC spFindEmployeesWithoutReviewInQuarter @Quarter = 1, @Year = 2025;

-- Get all employee reviews with details
-- EXEC spGetAllEmpReviewsDetailed;

/*
EXEC AddEmployeeReviewForQuarter 
   @EmployeeId = '00000002', 
   @RatingOptionsId = 2, 
   @Comment = 'Good performance this quarter', 
   @Quarter = 1, 
   @Year = 2025, 
   @ReviewDate = '2025-02-01';
*/

--SELECT * FROM Employee, Job
	--WHERE Employee.JobAssignmentId = Job.Id

--	SELECT * FROM Job

	--select  * from Department

--SELECT * FROM Employee
--select  * from Department
--SELECT * FROM Job

--SELECT EmployeeReview.Id, EmployeeReview.EmployeeId, RatingOptions.Rating, Employee.SupervisorId, EmployeeReview.Comment, EmployeeReview.ReviewDate FROM EmployeeReview INNER JOIN Employee ON
--Employee.Id = EmployeeReview.EmployeeId
--INNER JOIN RatingOptions ON
--RatingOptions.Id = EmployeeReview.RatingOptionsId;

--EXEC spFindAllReviewsMadeBySupervisor @Id = '00000002';
--DELETE FROM EmployeeReview
--WHERE Id = 2;
--USE MJRecords;
--GO
--SELECT * FROM Employee WHERE Id = '00000003';
--2025-05-16 10:26:24.723
--2025-05-16 10:39:10.323