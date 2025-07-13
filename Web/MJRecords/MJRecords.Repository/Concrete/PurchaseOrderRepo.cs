using DAL;
using System.Data;
using Microsoft.Data.SqlClient;
using MJRecords.Model;
using MJRecords.Types;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using MJRecords.Model;

namespace MJRecords.Repository
{
    public class PurchaseOrderRepo : IPurchaseOrderRepo
    {
        private readonly IDataAccess _db;

        public PurchaseOrderRepo(IDataAccess db)
        {
            _db = db;
        }

        public async Task<List<PurchaseOrder>> GetAllAsync()
        {
            DataTable dt = await _db.ExecuteAsync("spGetAllPurchaseOrders");

            return dt.AsEnumerable().Select(PopulatePurchaseOrder).ToList();
        }

        public List<PurchaseOrder> GetAll()
        {
            var dt = _db.Execute("spGetAllPurchaseOrders");
            return dt.AsEnumerable().Select(PopulatePurchaseOrder).ToList();
        }


        public async Task<List<POSByDepartmentSummaryDto>> GetPOsByDepartment(int departmentId)
        {

            var parms = new List<Parm>
            {
                new("@DepartmentId", SqlDbType.Int, departmentId)
            };

            var dt = await _db.ExecuteAsync("spGetPurchaseOrdersByDepartment", parms);

            return dt.AsEnumerable().Select(row => new POSByDepartmentSummaryDto
            {
                PONumber = row["PONumber"].ToString()!,
                POCreationDate = Convert.ToDateTime(row["POCreationDate"]),
                SupervisorName = row["SupervisorName"].ToString() ?? "Unknown",
                POStatus = (PurchaseOrderStatusEnum)Convert.ToInt32(row["Status"]),
                EmployeeId = row["EmployeeId"].ToString() ?? string.Empty,
            }).ToList();
        }


        public async Task<PurchaseOrder?> GetByIdAsync(string id)
        {
            var parms = new List<Parm>
            {
                new("@PurchaseOrderId", SqlDbType.Char, id)
            };

            var dt = await _db.ExecuteAsync("spGetPurchaseOrderById", parms);

            if (dt.Rows.Count == 0)
                return null;

            return PopulatePurchaseOrder(dt.Rows[0]);
        }


        public List<PurchaseOrderItem> GetItemsByOrderId(string purchaseOrderId)
        {

            var parms = new List<Parm>
            {
                new("@PurchaseOrderId", SqlDbType.Char, purchaseOrderId)
            };

            var dt = _db.Execute("spGetPurchaseOrderItemsByPOId", parms);

            return dt.AsEnumerable().Select(PopulatePurchaseOrderItem).ToList();
        }

        public PurchaseOrder? GetById(string id)
        {
            var parms = new List<Parm>
            {
                new Parm("@PurchaseOrderId", SqlDbType.Char, id)
            };

            var dt = _db.Execute("spGetPurchaseOrderById", parms);

            if (dt.Rows.Count == 0)
                return null;

            return PopulatePurchaseOrder(dt.Rows[0]);
        }


        public async Task<PurchaseOrder> CreateAsync(PurchaseOrder po, ItemCreateDto item)
        {

            var lastPO = await GetLastInsertedPO();
            int newPoId = lastPO != null ? Convert.ToInt32(lastPO.PurchaseOrderId) + 1 : 1;
            po.PurchaseOrderId = newPoId.ToString().PadLeft(8, '0');

            List<Parm> poParms = new()
            {
                new("@PurchaseOrderId", SqlDbType.Char, po.PurchaseOrderId),
                new("@EmployeeId", SqlDbType.Char, po.EmployeeId),
                new("@Status", SqlDbType.Int, (int)po.Status),
                new("@DateCreated", SqlDbType.DateTime, po.DateCreated),
                new("@Name", SqlDbType.VarChar, item.Name, 255),
                new("@Description", SqlDbType.VarChar, item.Description, 255),
                new("@Justification", SqlDbType.VarChar, item.Justification, 255),
                new("@PurchaseLocation", SqlDbType.VarChar, item.PurchaseLocation, 255),
                new("@ItemStatus", SqlDbType.VarChar, item.Status, 10),
                new("@Quantity", SqlDbType.Int, item.Quantity),
                new("@Price", SqlDbType.Money, item.Price)
            };

            try
            {
                await _db.ExecuteNonQueryAsync("spCreateFullPurchaseOrder", poParms);
            }
            catch (SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    po.AddError(new(error.Message, ErrorType.Business));

                }
             }

            return po;
        }
        
        
        public async Task<PurchaseOrder?> GetLastInsertedPO()
        {
            var dt = await _db.ExecuteAsync("spGetLastPO");

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return PopulatePurchaseOrder(row);
        }

        public void Update(PurchaseOrder po)
        {
            var parms = new List<Parm>
            {
                new Parm("@PurchaseOrderId", SqlDbType.Char, po.PurchaseOrderId),
                new Parm("@EmployeeId", SqlDbType.Char, po.EmployeeId),
                new Parm("@Status", SqlDbType.Int, po.Status, 20),
                new Parm("@DateCreated", SqlDbType.DateTime, po.DateCreated),

            };

            _db.Execute("spUpdatePurchaseOrder", parms);
        }

        public void Delete(string id)
        {
            var parms = new List<Parm>
            {
                new Parm("@PurchaseOrderId", SqlDbType.Char, id)
            };

            _db.ExecuteNonQuery("spPurchaseOrder_Delete", parms);
        }

        private static PurchaseOrder PopulatePurchaseOrder(DataRow row)
        {
            return new PurchaseOrder
            {
                PurchaseOrderId = (row["PurchaseOrderId"].ToString()!),
                EmployeeId = row["EmployeeId"].ToString()!,
                Status = (PurchaseOrderStatusEnum)Convert.ToInt32(row["Status"]),
                DateCreated = Convert.ToDateTime(row["DateCreated"]),
                RecordVersion = (byte[])row["RecordVersion"]
            };
        }

        private static POSummaryDto PopulatePurchaseOrderSummaryDto(DataRow row)
        {
            return new POSummaryDto
            {
                PurchaseOrderId = row["PurchaseOrderId"].ToString()!,
                Employee = row["Employee"].ToString(),
                DateCreated = Convert.ToDateTime(row["DateCreated"]),
                Status = (PurchaseOrderStatusEnum)Convert.ToInt32(row["Status"]),
                Subtotal = Convert.ToDecimal(row["Subtotal"]),
                TaxTotal = Convert.ToDecimal(row["TaxTotal"]),
                GrandTotal = Convert.ToDecimal(row["GrandTotal"])
            };
        }

        private static PurchaseOrderItem PopulatePurchaseOrderItem(DataRow row)
        {
            return new PurchaseOrderItem
            {
                Id = Convert.ToInt32(row["Id"]),
                PurchaseOrderId = row["PurchaseOrderId"].ToString()!,
                Name = row["Name"].ToString()!,
                Description = row["Description"].ToString()!,
                Quantity = Convert.ToInt32(row["Quantity"]),
                Price = Convert.ToDecimal(row["Price"]),
                Justification = row["Justification"].ToString()!,
                PurchaseLocation = row["PurchaseLocation"].ToString()!,
                Status = Enum.Parse<PurchaseOrderItemStatusEnum>(row["ItemStatus"].ToString()!),
                RecordVersion = (byte[])row["RecordVersion"]
            };
        }

        public async Task<List<PurchaseOrder>> SearchByCriteriaAsync(
            POSearchDto criteria,
            bool includeDepartment = false,
            bool isCeo = false,
            string employeeId = null
            )
        {
            var parms = new List<Parm>
        {
            new("@EmployeeId", SqlDbType.Char, (object?)employeeId ?? DBNull.Value),
            new("@StartDate", SqlDbType.Date, (object?)criteria.StartDate ?? DBNull.Value),
            new("@EndDate", SqlDbType.Date, (object?)criteria.EndDate ?? DBNull.Value),
            new("@PurchaseOrderId", SqlDbType.Char, (object?)criteria.PurchaseOrderId ?? DBNull.Value),
            new("@Status", SqlDbType.Int, (object?)criteria.Status ?? DBNull.Value),
            new("@IncludeDepartment", SqlDbType.Bit, includeDepartment ? 1 : 0),
            new("@IsCeo", SqlDbType.Bit, isCeo ? 1 : 0),
            new("@EmployeeName", SqlDbType.VarChar, (object?)criteria.EmployeeName ?? DBNull.Value),
         };

            var dt = await _db.ExecuteAsync("spSearchPurchaseOrders", parms);
            return dt.AsEnumerable().Select(PopulatePurchaseOrder).ToList();
        }


        public List<MonthlyExpenseDto> GetMonthlyExpensesForSupervisor(string supervisorId)
        {
            var parms = new List<Parm>
        {
            new Parm("@SupervisorId", SqlDbType.Char, supervisorId, 8)
        };

            var dt = _db.Execute("spGetMonthlyExpensesForSupervisor", parms);

            var result = new List<MonthlyExpenseDto>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new MonthlyExpenseDto
                {
                    Month = row["Month"].ToString(),
                    POCount = Convert.ToInt32(row["POCount"]),
                    Total = Convert.ToDecimal(row["Total"])
                });
            }

            return result;
        }


        public List<MonthlyExpenseDto> GetMonthlyExpensesForEmployee(string employeeId)
        {
            var parms = new List<Parm>
        {
            new Parm("@EmployeeId", SqlDbType.Char, employeeId, 8)
        };

            var dt = _db.Execute("spGetMonthlyExpensesAndCountsForEmployee", parms);

            var result = new List<MonthlyExpenseDto>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new MonthlyExpenseDto
                {
                    Month = row["Month"].ToString(),
                    POCount = Convert.ToInt32(row["POCount"]),
                    Total = Convert.ToDecimal(row["Total"]),
                });
            }

            return result;
        }


        public async Task<POSummaryApiDto?> GetPurchaseOrderSummaryApiAsync(string poId)
        {
            var parms = new List<Parm>
            {
                new Parm("@PurchaseOrderId", SqlDbType.VarChar, poId, 50)
             };

            var dt = await _db.ExecuteAsync("spGetPurchaseOrderApiSummary", parms);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];

            return new POSummaryApiDto
            {
                PurchaseOrderId = row["PurchaseOrderId"].ToString()!,
                Status = (PurchaseOrderStatusEnum)Convert.ToInt32(row["Status"]),
                SupervisorName = row["SupervisorName"].ToString() ?? "No supervisor",
                TotalItems = Convert.ToInt32(row["TotalItems"]),
                GrandTotal = Convert.ToDecimal(row["GrandTotal"])
            };
        }


        private static POSummaryApiDto PopulatePOSummaryApi(DataRow row)
        {
            return new POSummaryApiDto
            {
                PurchaseOrderId = row["PurchaseOrderId"].ToString()!,
                Status = (PurchaseOrderStatusEnum)Convert.ToInt32(row["Status"]),
                SupervisorName = row["SupervisorName"].ToString()!,
                TotalItems = Convert.ToInt32(row["TotalItems"]),
                GrandTotal = Convert.ToDecimal(row["GrandTotalWithTax"])
            };
        }

    }
}
