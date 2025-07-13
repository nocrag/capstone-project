using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MJRecords.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderItemService _purchaseOrderItemService;

        public PurchaseOrderController(
            IPurchaseOrderService purchaseOrderService
            , IPurchaseOrderItemService purchaseOrderItemService)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderItemService = purchaseOrderItemService;
        }



        /// <summary>
        /// Retrieves a summary of purchase orders for a specific department.
        /// </summary>
        /// <param name="departmentId">The ID of the department to retrieve purchase orders for.</param>
        /// <returns>
        /// A list of <see cref="POSByDepartmentSummaryDto"/> objects representing the purchase order summaries
        /// for the given department, or a bad request if the department ID is invalid.
        /// </returns>
        /// <response code="200">Returns the list of PO summaries.</response>
        /// <response code="400">Returned when the department ID is invalid.</response>
        // GET: api/<PurchaseOrderController>
        [Authorize]
        [HttpGet("Department")]
        public async Task<ActionResult<List<POSByDepartmentSummaryDto>>> GetSummaryByDepartment(int departmentId)
        {
            if (departmentId <= 0)
                return BadRequest("Invalid department ID.");

            var summaries = await _purchaseOrderService.GetPOsByDepartment(departmentId);

            if (summaries == null)
            {
                summaries = new List<POSByDepartmentSummaryDto>();
            }

            return Ok(summaries);
        }


        /// <summary>
        /// Retrieves all purchase order items associated with a specific purchase order ID.
        /// </summary>
        /// <param name="poId">The ID of the purchase order.</param>
        /// <returns>
        /// A list of <see cref="PurchaseOrderItem"/> objects related to the given purchase order ID,
        /// or a bad request if the PO ID is empty.
        /// </returns>
        /// <response code="200">Returns the list of purchase order items.</response>
        /// <response code="400">Returned when the PO ID is invalid.</response>

        // GET: api/<PurchaseOrderController>
        [Authorize]
        [HttpGet("Items")]
        public async Task<ActionResult<List<PurchaseOrder>>> GetItems(string poId)
        {
            if (poId == "")
                return BadRequest("Invalid PO ID.");

            var details = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(poId);

            if (details == null)
            {
                details = new List<PurchaseOrderItem>();
            }

            return Ok(details);
        }


        /// <summary>
        /// Retrieves the summary information of a specific purchase order by ID.
        /// </summary>
        /// <param name="poId">The ID of the purchase order to retrieve.</param>
        /// <returns>
        /// A <see cref="POSummaryApiDto"/> object containing summary details of the specified purchase order,
        /// or an error response if the ID is invalid or not found.
        /// </returns>
        /// <response code="200">Returns the purchase order summary.</response>
        /// <response code="400">Returned when the PO ID is missing or invalid.</response>
        /// <response code="404">Returned when the purchase order is not found.</response>
        [Authorize]
        [HttpGet("Summary")]
        public async Task<ActionResult<POSummaryApiDto>> GetPurchaseOrderSummary([FromQuery] string poId)
        {
            if (string.IsNullOrWhiteSpace(poId))
                return BadRequest("Invalid PO ID.");

            var summary = await _purchaseOrderService.GetPurchaseOrderSummaryApiAsync(poId);

            if (summary == null)
                return NotFound("Purchase Order not found.");

            return Ok(summary);
        }


    }
}
