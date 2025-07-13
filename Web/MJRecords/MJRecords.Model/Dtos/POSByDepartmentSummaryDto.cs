namespace MJRecords.Model
{
    public class POSByDepartmentSummaryDto
    {
        public string PONumber { get; set; }
        public DateTime POCreationDate { get; set; }
        public string SupervisorName { get; set; }
        public PurchaseOrderStatusEnum POStatus { get; set; }

        public PurchaseOrderStatusEnum Status { get; set; }
        public string EmployeeId { get; set; }

    }


}


