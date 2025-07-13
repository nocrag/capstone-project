using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public interface IPurchaseOrderItemService
    {

        /// <summary>
        /// Creates a new purchase order item and associates it with a specified purchase order.
        /// </summary>
        /// <param name="dto">The <see cref="ItemCreateDto"/> containing item details.</param>
        /// <param name="purchaseOrderId">The ID of the purchase order to associate the item with.</param>
        /// <returns>A task representing the asynchronous creation operation.</returns>
        Task CreateAsync(ItemCreateDto dto, string purchaseOrderId);

        /// <summary>
        /// Retrieves all purchase order items associated with a specific employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of 
        /// <see cref="PurchaseOrderItem"/> objects linked to the given employee.
        /// </returns>
        Task<List<PurchaseOrderItem>> GetItemsByEmployeeIdAsync(string employeeId);

        /// <summary>
        /// Retrieves all purchase order items associated with a specific purchase order ID.
        /// </summary>
        /// <param name="purchaseOrderId">The ID of the purchase order.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of 
        /// <see cref="PurchaseOrderItem"/> objects for the specified purchase order.
        /// </returns>
        Task<List<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(string purchaseOrderId);


        /// <summary>
        /// Adds a new item to an existing purchase order.
        /// </summary>
        /// <param name="purchaseOrderId">The ID of the purchase order to add the item to.</param>
        /// <param name="itemDto">The <see cref="ItemCreateDto"/> containing the item's details.</param>
        /// <returns>A task representing the asynchronous add operation.</returns>
        Task AddItemToPurchaseOrderAsync(string purchaseOrderId, ItemCreateDto itemDto);

        /// <summary>
        /// Retrieves a specific purchase order item by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the purchase order item.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the matching 
        /// <see cref="PurchaseOrderItem"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<PurchaseOrderItem?> GetByIdAsync(int id);

        /// <summary>
        /// Updates the details of an existing purchase order item.
        /// </summary>
        /// <param name="item">The <see cref="PurchaseOrderItem"/> object with updated information.</param>
        /// <returns>The updated <see cref="PurchaseOrderItem"/>.</returns>
        PurchaseOrderItem Update(PurchaseOrderItem item);

        /// <summary>
        /// Calculates and retrieves the subtotal, tax total, and grand total for a specific purchase order.
        /// </summary>
        /// <param name="purchaseOrderId">The ID of the purchase order.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing a tuple with:
        /// <list type="bullet">
        ///   <item><term>Subtotal</term><description>The sum of item prices before tax.</description></item>
        ///   <item><term>TaxTotal</term><description>The total tax amount.</description></item>
        ///   <item><term>GrandTotal</term><description>The combined total including tax.</description></item>
        /// </list>
        /// </returns>
        Task<(decimal Subtotal, decimal TaxTotal, decimal GrandTotal)> GetPurchaseOrderTotalsAsync(string purchaseOrderId);

        /// <summary>
        /// Validates the given <see cref="PurchaseOrderItem"/> based on business rules and constraints.
        /// </summary>
        /// <param name="item">The purchase order item to validate.</param>
        /// <remarks>
        /// This method may modify the item's state by adding validation errors to it if any issues are found.
        /// </remarks>
        public void ValidateModel(PurchaseOrderItem item);

        /// <summary>
        /// Updates the status of a specific purchase order item, with optional concurrency and denial reason handling.
        /// </summary>
        /// <param name="itemId">The ID of the purchase order item to update.</param>
        /// <param name="newStatus">The new status to assign (e.g., Pending, Approved, Denied).</param>
        /// <param name="recordVersion">
        /// The record version (row version/timestamp) used for concurrency control.
        /// </param>
        /// <param name="denialReason">An optional reason provided when the status is set to Denied.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the updated 
        /// <see cref="PurchaseOrderItem"/> if successful; otherwise, <c>null</c> if the item was not found.
        /// </returns>
        Task<PurchaseOrderItem?> SetItemStatusAsync(
            int itemId, 
            string newStatus,
            byte[] recordVersion,
            string? denialReason = null
            );


        /// <summary>
        /// Updates a purchase order item that is no longer needed, applying specific validation rules for this case.
        /// </summary>
        /// <param name="item">The <see cref="PurchaseOrderItem"/> to update.</param>
        /// <returns>The updated <see cref="PurchaseOrderItem"/>.</returns>
        /// <remarks>
        /// This method may apply different validation or logic if the item is marked as "No longer needed".
        /// </remarks>
        PurchaseOrderItem UpdateNoLongerNeeded(PurchaseOrderItem item);

        /// <summary>
        /// Deletes a purchase order item by its unique identifier.
        /// </summary>
        /// <param name="itemId">The ID of the purchase order item to delete.</param>
        public void Delete(int itemId);


        /// <summary>
        /// Updates a purchase order item with changes made by a supervisor.
        /// </summary>
        /// <param name="dto">
        /// A <see cref="SupervisorPOItemUpdateDto"/> containing the updated item details and any supervisor-specific changes.
        /// </param>
        /// <returns>The updated <see cref="PurchaseOrderItem"/>.</returns>
        PurchaseOrderItem UpdateBySupervisor(SupervisorPOItemUpdateDto dto);

    }
}
