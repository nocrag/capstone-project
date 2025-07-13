using DAL;
using Microsoft.Data.SqlClient;
using MJRecords.Model;
using MJRecords.Types;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace MJRecords.Repository
{
    public class PurchaseOrderItemRepo : IPurchaseOrderItemRepo
    {
        private readonly IDataAccess _db;

        public PurchaseOrderItemRepo(IDataAccess db)
        {
            _db = db;
        }

        public async Task<PurchaseOrderItem?> GetByIdAsync(int id)
        {
            var parms = new List<Parm>
            {
             new("@ItemId", SqlDbType.Int, id)
            };

            var dt = await _db.ExecuteAsync("spGetItemById", parms);

            if (dt.Rows.Count == 0)
                return null;

            return PopulatePurchaseOrderItem(dt.Rows[0]);
         }


        public async Task<PurchaseOrderItem?> SetItemStatusAsync(
            int itemId, 
            string newStatus, 
            byte[] recordVersion,
            string? denialReason = null

            )
        {
            var parms = new List<Parm>
                {
                    new("@ItemId", SqlDbType.Int, itemId),
                    new("@NewStatus", SqlDbType.VarChar, newStatus, 20),
                    new("@DenialReason", SqlDbType.VarChar, denialReason ?? (object)DBNull.Value, 500),
                    new("@RecordVersion", SqlDbType.Timestamp, recordVersion)
                };

            try
            {
                await _db.ExecuteNonQueryAsync("spSetItemStatus", parms);

                return await GetByIdAsync(itemId);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                {
                    throw new ValidationException("The purchase order was modified by another user. Please try again.");
                }

                throw new Exception("Database error occurred while adding item to PO: " + ex.Message);
            }

        }


        public async Task CreateAsync(PurchaseOrderItem poItem)
        {
            var parms = new List<Parm>
            {
                new("@PurchaseOrderId", SqlDbType.Char, poItem.PurchaseOrderId),
                new("@Name", SqlDbType.VarChar, poItem.Name, 255),
                new("@Description", SqlDbType.VarChar, poItem.Description, 255),
                new("@Justification", SqlDbType.VarChar, poItem.Justification, 255),
                new("@PurchaseLocation", SqlDbType.VarChar, poItem.PurchaseLocation, 255),
                new("@ItemStatus", SqlDbType.VarChar, poItem.Status.ToString(), 20),
                new("@Quantity", SqlDbType.Int, poItem.Quantity),
                new("@Price", SqlDbType.Money, poItem.Price)
            };

            await _db.ExecuteNonQueryAsync("spCreatePOItem", parms);
        }

        public async Task DeleteAsync(int itemId)
        {
            var parms = new List<Parm>
        {
            new("@ItemId", SqlDbType.Int, itemId)
        };

            try
            {
                await _db.ExecuteNonQueryAsync("spDeletePOItem", parms);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Failed to delete purchase order item.", ex);
            }
        }

        public async Task<List<PurchaseOrderItem>> GetItemsByPurchaseOrderIdAsync(string purchaseOrderId)
        {
            var parms = new List<Parm>
            {
                new("@PurchaseOrderId", SqlDbType.Char, purchaseOrderId)
            };

            var dt = await _db.ExecuteAsync("spGetPurchaseOrderItemsByPOId", parms);

            return dt.AsEnumerable().Select(row => new PurchaseOrderItem
            {
                Id = Convert.ToInt32(row["Id"]),
                PurchaseOrderId = row["PurchaseOrderId"].ToString()!,
                Name = row["Name"].ToString()!,
                Description = row["Description"].ToString()!,
                Justification = row["Justification"].ToString()!,
                PurchaseLocation = row["PurchaseLocation"].ToString()!,
                Status = Enum.Parse<PurchaseOrderItemStatusEnum>(row["ItemStatus"].ToString()!),
                Quantity = Convert.ToInt32(row["Quantity"]),
                Price = Convert.ToDecimal(row["Price"]),
                DenialReason = row["DenialReason"].ToString()!,
                ModificationReason = row["ModificationReason"].ToString()!,
                RecordVersion = (byte[])row["RecordVersion"]
            }).ToList();
        }


        public async Task<List<PurchaseOrderItem>> GetItemsByEmployeeIdAsync(string employeeId)
        {
            var parms = new List<Parm>
            {
                new("@EmployeeId", SqlDbType.Char, employeeId)
            };

            var dt = await _db.ExecuteAsync("spGetItemsByEmployeeId", parms);

            return dt.AsEnumerable().Select(PopulatePurchaseOrderItem).ToList();
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
                RecordVersion = (byte[])row["RecordVersion"],
                ModificationReason = row["ModificationReason"] == DBNull.Value ? null : row["ModificationReason"].ToString()

            };
        }

        public async Task AddItemToPurchaseOrderAsync(string purchaseOrderId, ItemCreateDto itemDto)
        {
            var parms = new List<Parm>
            {
                new("@PurchaseOrderId", SqlDbType.Char, purchaseOrderId),
                new("@Name", SqlDbType.VarChar, itemDto.Name, 100),
                new("@Description", SqlDbType.VarChar, itemDto.Description, 255),
                new("@Price", SqlDbType.Decimal, itemDto.Price),
                new("@Quantity", SqlDbType.Int, itemDto.Quantity),
                new("@Justification", SqlDbType.VarChar, itemDto.Justification, 255),
                new("@PurchaseLocation", SqlDbType.VarChar, itemDto.PurchaseLocation, 100),
                new("@ItemStatus", SqlDbType.VarChar, itemDto.Status.ToString(), 20),
                //new("@RecordVersion", SqlDbType.Timestamp, itemDto.RecordVersion)

            };

            try
            {
                await _db.ExecuteNonQueryAsync("spCreatePOItem", parms);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                {
                    throw new ValidationException("The purchase order was modified or closed by another user. Please reload and try again.");
                }

                throw new Exception("Database error occurred while adding item to PO: " + ex.Message);
            }
        }


        public async Task<List<PurchaseOrderItem>> GetMatchingItemsAsync(string purchaseOrderId, ItemCreateDto newItem)
        {
            var parms = new List<Parm>
            {
            new("@PurchaseOrderId", SqlDbType.Char, purchaseOrderId),
            new("@Name", SqlDbType.VarChar, newItem.Name),
            new("@Description", SqlDbType.VarChar, newItem.Description),
            new("@Price", SqlDbType.Decimal, newItem.Price),
            new("@Justification", SqlDbType.VarChar, newItem.Justification),
            new("@PurchaseLocation", SqlDbType.VarChar, newItem.PurchaseLocation)
        };

            var dt = await _db.ExecuteAsync("spGetMatchingPOItem", parms); // updated SP name!

            var list = new List<PurchaseOrderItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(PopulatePurchaseOrderItem(row));
            }

            return list;
        }


        public async Task UpdateItemQuantityAsync(int itemId, int additionalQuantity)
        {
            var parms = new List<Parm>
            {
                new("@ItemId", SqlDbType.Int, itemId),
                new("@QuantityToAdd", SqlDbType.Int, additionalQuantity)
            };

            await _db.ExecuteNonQueryAsync("spUpdateItemQuantity", parms);
        }

        public PurchaseOrderItem Update(PurchaseOrderItem item)
        {
            try
            {
                List<Parm> parms = new()
                {
                    new("@Id", SqlDbType.Int, item.Id),
                    new("@Name", SqlDbType.VarChar, item.Name, 100),
                    new("@Description", SqlDbType.VarChar, item.Description, 255),
                    new("@Price", SqlDbType.Decimal, item.Price),
                    new("@Quantity", SqlDbType.Int, item.Quantity),
                    new("@Justification", SqlDbType.VarChar, item.Justification, 255),
                    new("@PurchaseLocation", SqlDbType.VarChar, item.PurchaseLocation, 100),
                    new("@ItemStatus", SqlDbType.VarChar, item.Status.ToString(), 20),
                    new("@DenialReason", SqlDbType.VarChar, item.DenialReason ?? "", 500),
                    new("@ModificationReason", SqlDbType.VarChar, item.ModificationReason ?? "", 500),
                    new("@RecordVersion", SqlDbType.Timestamp, item.RecordVersion),

                };

                _db.ExecuteNonQuery("spUpdatePOItem", parms);
                    return item;

            }
            catch (SqlException ex)
            {
                if (ex.Number == 50002)
                {
                    item.AddError(new ValidationrError(
                        "This item has been modified by another user. Try again.",
                        ErrorType.Business));
                }
                else
                {
                    item.AddError(new ValidationrError("A database error occurred: " + ex.Message, ErrorType.Business));
                }

                return item;
            }
        }


    }
}
