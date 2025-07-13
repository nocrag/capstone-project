using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MJRecords.Model;


namespace MJRecords.Service
{
    public interface IPurchaseOrderService
    {


        /// <summary>
        /// Retrieves all purchase orders from the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="PurchaseOrder"/> objects.</returns>
        Task<List<PurchaseOrder>> GetAllAsync();

        /// <summary>
        /// Retrieves summary information for all purchase orders.
        /// </summary>
        /// <returns>A list of <see cref="POSummaryDto"/> objects containing summary data.</returns>
        List<POSummaryDto> GetSummary();

        /// <summary>
        /// Retrieves purchase order summaries for a specific department.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of 
        /// <see cref="POSByDepartmentSummaryDto"/> objects for the specified department.
        /// </returns>
        Task<List<POSByDepartmentSummaryDto>> GetPOsByDepartment(int departmentId);


        /// <summary>
        /// Retrieves a purchase order by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the purchase order.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the matching <see cref="PurchaseOrder"/> 
        /// if found; otherwise, <c>null</c>.
        /// </returns>
        Task<PurchaseOrder?> GetByIdAsync(string id);


        /// <summary>
        /// Creates a new purchase order based on the provided data.
        /// </summary>
        /// <param name="dto">A <see cref="POCreateDto"/> object containing the details for the new purchase order.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the newly created <see cref="PurchaseOrder"/>.
        /// </returns>
        Task<PurchaseOrder> CreateAsync(POCreateDto dto);

        /// <summary>
        /// Calculates the subtotal amount of a given purchase order (sum of item prices).
        /// </summary>
        /// <param name="po">The <see cref="PurchaseOrder"/> for which to calculate the subtotal.</param>
        /// <returns>The subtotal amount as a <see cref="decimal"/>.</returns>
        decimal CalculateSubTotal(PurchaseOrder po);



        /// <summary>
        /// Calculates the total tax based on the provided subtotal.
        /// </summary>
        /// <param name="subTotal">The subtotal amount.</param>
        /// <returns>The calculated tax amount as a <see cref="decimal"/>.</returns>
        decimal CalculateTaxTotal(decimal subTotal);



        /// <summary>
        /// Calculates the grand total by adding the subtotal and tax.
        /// </summary>
        /// <param name="subTotal">The subtotal amount.</param>
        /// <param name="tax">The tax amount.</param>
        /// <returns>The grand total as a <see cref="decimal"/>.</returns>
        decimal CalculateGrandTotal(decimal subTotal, decimal tax);


        /// <summary>
        /// Closes a purchase order by its ID, marking it as completed.
        /// </summary>
        /// <param name="purchaseOrderId">The ID of the purchase order to close.</param>
        /// <returns>A task representing the asynchronous close operation.</returns>
        Task ClosePurchaseOrderAsync(string purchaseOrderId);


        /// <summary>
        /// Searches for purchase order summaries based on the provided criteria.
        /// </summary>
        /// <param name="criteria">The search criteria as a <see cref="POSearchDto"/> object.</param>
        /// <param name="includeDepartment">Whether to all department POs in the results.</param>
        /// <param name="isSupervisor">Indicates if the requester is a supervisor (may affect filtering logic).</param>
        /// <param name="employeeId">Optional employee ID to filter results for a specific employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of matching <see cref="POSummaryDto"/> objects.
        /// </returns>
        Task<List<POSummaryDto>> SearchSummariesAsync(
            POSearchDto criteria, 
            bool includeDepartment = false, 
            bool isSupervisor = false,
            string ? employeeId = null);


        /// <summary>
        /// Retrieves the monthly purchase order expenses for all employees under a specific department.
        /// </summary>
        /// <param name="supervisorId">The ID of the supervisor.</param>
        /// <returns>
        /// A list of <see cref="MonthlyExpenseDto"/> objects representing monthly expense data for the supervisor's team.
        /// </returns>
        List<MonthlyExpenseDto> GetMonthlyExpensesForSupervisor(string supervisorId);


        /// <summary>
        /// Retrieves the monthly purchase order expenses for a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>
        /// A list of <see cref="MonthlyExpenseDto"/> objects representing the employee's monthly expense data.
        /// </returns>
        List<MonthlyExpenseDto> GetMonthlyExpensesForEmployee(string employeeId);

        /// <summary>
        /// Retrieves purchase orders with statuses of Pending or Under Review for a specific department
        /// </summary>
        /// <param name="departmentId">The ID of the department to filter purchase orders.</param>
        /// <param name="supervisorId">The ID of the supervisor requesting the data.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of 
        /// <see cref="POSByDepartmentSummaryDto"/> objects matching the criteria.
        /// </returns>
        Task<List<POSByDepartmentSummaryDto>> GetPendingAndUnderReviewPOsByDepartmentAsync(int departmentId, string supervisorId);

        /// <summary>
        /// Retrieves summary information for a specific purchase order in a format for API responses.
        /// </summary>
        /// <param name="poId">The ID of the purchase order.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a <see cref="POSummaryApiDto"/> 
        /// if the purchase order is found; otherwise, <c>null</c>.
        /// </returns>
        Task<POSummaryApiDto?> GetPurchaseOrderSummaryApiAsync(string poId);


    }
}

