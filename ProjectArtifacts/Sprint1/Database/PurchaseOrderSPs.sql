

--GetAll--
CREATE OR ALTER PROCEDURE spGetAllPurchaseOrders
AS
BEGIN
    SELECT * FROM PurchaseOrder
END
GO
--EXEC spGetAllPurchaseOrders

--GetSummary--

CREATE PROCEDURE spGetPurchaseOrderSummaries
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
    @PurchaseOrderId INT
AS
BEGIN
    SELECT * FROM PurchaseOrderItem WHERE PurchaseOrderId = @PurchaseOrderId
END
GO
--EXEC spGetPurchaseOrderItemsByPOId @PurchaseOrderId = 101


--Create PO
CREATE OR ALTER PROCEDURE spCreatePurchaseOrder
    @PurchaseOrderId INT OUTPUT,
    @EmployeeId CHAR(8),
    @Status VARCHAR(20),
    @DateCreated DATETIME
AS
BEGIN
    BEGIN TRY
        INSERT INTO PurchaseOrder (
            EmployeeId,
            Status,
            DateCreated
        )
        VALUES (
            @EmployeeId,
            @Status,
            @DateCreated
        ) SET @PurchaseOrderId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        ;THROW
    END CATCH
END
GO

--Create po item--

CREATE OR ALTER PROCEDURE spCreatePOItem
    @PurchaseOrderId     INT,
    @Name                VARCHAR(255),
    @Description         VARCHAR(255),
    @Quantity            INT,
    @Justification       VARCHAR(255),
    @PurchaseLocation    VARCHAR(255),
    @ItemStatus          VARCHAR(10),
    @Price               MONEY
AS
BEGIN
    BEGIN TRY
        INSERT INTO PurchaseOrderItem (
            PurchaseOrderId,
            Name,
            Description,
            Quantity,
            Justification,
            PurchaseLocation,
            ItemStatus,
            Price
        )
        VALUES (
            @PurchaseOrderId,
            @Name,
            @Description,
            @Quantity,
            @Justification,
            @PurchaseLocation,
            @ItemStatus,
            @Price
        );
    END TRY
    BEGIN CATCH
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
    SELECT TOP 1 * FROM PurchaseOrderItem
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
    @EmployeeId CHAR(8),
    @StartDate DATE = NULL,
    @EndDate DATE = NULL,
    @PurchaseOrderId CHAR(8) = NULL
AS
BEGIN
    SELECT * FROM PurchaseOrder
    WHERE EmployeeId = @EmployeeId
      AND (@PurchaseOrderId IS NULL OR PurchaseOrderId = @PurchaseOrderId)
      AND (@StartDate IS NULL OR DateCreated >= @StartDate)
      AND (@EndDate IS NULL OR DateCreated <= @EndDate)
END

