USE MJRecords;
GO


CREATE OR ALTER PROC spAddEmployee
	@Id					CHAR(8)			OUTPUT,
	@SupervisorId		CHAR(8)			= NULL,
	@DepartmentId		INT				= NULL,
	@JobAssignmentId	INT,
	@Password			VARCHAR(255),
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
	@WorkPhone			VARCHAR(12),
	@CellPhone			VARCHAR(12),
	@EmailAddress		VARCHAR(255),
	@OfficeLocation		VARCHAR(255),
	@RecordVersion		ROWVERSION	OUTPUT
AS
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		INSERT INTO Employee(Id, SupervisorId, DepartmentId, JobAssignmentId, Password, FirstName, LastName, MiddleInitial,
			StreetAddress, City, Province, PostalCode, DOB, SIN, SeniorityDate, JobStartDate, WorkPhone, CellPhone, EmailAddress,
			OfficeLocation
		)
		VALUES(@Id, @SupervisorId, @DepartmentId, @JobAssignmentId, @Password, @FirstName, @LastName, @MiddleInitial, @StreetAddress,
		@City, @Province, @PostalCode, @DOB, @SIN, @SeniorityDate, @JobStartDate, @WorkPhone, @CellPhone, @EmailAddress, @OfficeLocation);

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
	@SupervisorId		CHAR(8)			NULL,
	@DepartmentId		INT				NULL,
	@JobAssignmentId	INT,
	@Password			VARCHAR(255),
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@MiddleInitial		CHAR(1)			NULL,
	@StreetAddress		VARCHAR(255),
	@City				VARCHAR(255),
	@Province			VARCHAR(255),
	@PostalCode			VARCHAR(7),
	@DOB				DATETIME,
	@SIN				VARCHAR(11),
	@SeniorityDate		DATETIME,
	@JobStartDate		DATETIME,
	@WorkPhone			VARCHAR(12),
	@CellPhone			VARCHAR(12),
	@EmailAddress		VARCHAR(255),
	@OfficeLocation		VARCHAR(255),
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
			OfficeLocation = @OfficeLocation
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

EXEC spCreateDepartment @Id = 0, @Name = 'Dept TESTING', @Description = 'TEST DESC', @InvocationDate = '2025-05-01';
EXEC spGetAllDepartments;
EXEC spUpdateDepartment @Id = 13, @Name = 'DEPT 2 TEST', @Description = 'MORE TESTING DESC', @InvocationDate = '2025-05-02', @RecordVersion = 0x0000000000000831;
EXEC spDeleteDepartment @Id = 13, @RecordVersion = 0x0000000000000834;

--GO
--CREATE OR ALTER PROC spDeleteDepartment
--	@Id		INT
--AS
--BEGIN
--	SET NOCOUNT ON;

--	BEGIN TRY
--		BEGIN TRANSACTION
--		DELETE FROM Department
--		WHERE Id = @Id;

--		COMMIT TRANSACTION;
--	END TRY
--	BEGIN CATCH
--		IF @@TRANCOUNT > 0
--			ROLLBACK TRANSACTION;
		
--		THROW;
--	END CATCH
--END

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
            'HR Manager', 'Project Manager', 'Product Manager',
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
CREATE OR ALTER PROC spGetEmployeeBySIN
	@SIN	VARCHAR(11)
AS
BEGIN
	SELECT * FROM Employee WHERE SIN = @SIN;
END

--EXEC spGetEmployeeBySIN @SIN = '111-111-123';

--EXEC spGetAllDepartments;
--EXEC spGetAllJobs;
--EXEC spGetAllEmployees;
--EXEC spGetDepartment @Id = 1;

--EXEC spGetSupervisorsWithFewerThan10;
--SELECT * FROM Employee WHERE SupervisorId = '00000002';
