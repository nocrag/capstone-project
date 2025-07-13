using MJRecords.Model;

namespace MJRecords.Repository
{
    public interface IPurchaseOrderItemRepo
    {

        Task CreateAsync(PurchaseOrderItem poItem);
        Task <List<PurchaseOrderItem>> GetItemsByPurchaseOrderIdAsync(string purchaseOrderId);

        Task<PurchaseOrderItem?> GetByIdAsync(int id);
        Task<List<PurchaseOrderItem>> GetItemsByEmployeeIdAsync(string employeeId);

        Task AddItemToPurchaseOrderAsync(string purchaseOrderId, ItemCreateDto itemDto);

        Task<List<PurchaseOrderItem>> GetMatchingItemsAsync(string purchaseOrderId, ItemCreateDto newItem);

        Task UpdateItemQuantityAsync(int itemId, int additionalQuantity);

        PurchaseOrderItem Update(PurchaseOrderItem item);
        Task DeleteAsync(int itemId);

        Task<PurchaseOrderItem?> SetItemStatusAsync(
            int itemId, 
            string newStatus, 
            byte[] recordVersion,
            string? denialReason = null);


    }
}

