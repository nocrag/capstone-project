using MJRecords.Model;
using MJRecords.Repository;
using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Service
{
    public class PurchaseOrderItemService : IPurchaseOrderItemService
    {
        private readonly IPurchaseOrderItemRepo _itemRepo;
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;

        public PurchaseOrderItemService(IPurchaseOrderItemRepo repo, IPurchaseOrderRepo purchaseOrderRepo)
        {
            _itemRepo = repo;
            _purchaseOrderRepo = purchaseOrderRepo;
        }


        public async Task CreateAsync(ItemCreateDto dto, string purchaseOrderId)
        {
            var item = new PurchaseOrderItem
            {
                PurchaseOrderId = purchaseOrderId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Justification = dto.Justification,
                PurchaseLocation = dto.PurchaseLocation,
                Status = dto.Status
            };

            await _itemRepo.CreateAsync(item);
        }

        public void Delete(int itemId)
        {
            _itemRepo.DeleteAsync(itemId).GetAwaiter().GetResult();
        }

        public PurchaseOrderItem Update(PurchaseOrderItem item)
        {
            ValidateModel(item);
            if (item.Errors.Count > 0)
                return item;

            var matches = _itemRepo.GetMatchingItemsAsync(item.PurchaseOrderId, new ItemCreateDto
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Justification = item.Justification,
                Status = item.Status,
                PurchaseLocation = item.PurchaseLocation
            }).GetAwaiter().GetResult();

            var matchToMerge = matches
                .FirstOrDefault(m => m.Id != item.Id && m.Status == PurchaseOrderItemStatusEnum.Pending);

            if (matchToMerge != null)
            {
                _itemRepo.UpdateItemQuantityAsync(matchToMerge.Id, item.Quantity).GetAwaiter().GetResult();
                _itemRepo.DeleteAsync(item.Id).GetAwaiter().GetResult();
                return matchToMerge;
            }

            return _itemRepo.Update(item);
        }



        public async Task AddItemToPurchaseOrderAsync(string purchaseOrderId, ItemCreateDto itemDto)
        {
            var matches = await _itemRepo.GetMatchingItemsAsync(purchaseOrderId, itemDto);

            var pendingMatch = matches.FirstOrDefault(m => m.Status == PurchaseOrderItemStatusEnum.Pending);

            if (pendingMatch != null)
            {
                await _itemRepo.UpdateItemQuantityAsync(pendingMatch.Id, itemDto.Quantity);
            }
            else
            {
                await _itemRepo.AddItemToPurchaseOrderAsync(purchaseOrderId, itemDto);
            }
        }



        public PurchaseOrderItem UpdateNoLongerNeeded(PurchaseOrderItem item)
        {
            return _itemRepo.Update(item);

        }

        public async Task<PurchaseOrderItem?> SetItemStatusAsync(
             int itemId,
             string newStatus,
             byte[] recordVersion,
             string? denialReason = null)
        {
            var item = await _itemRepo.GetByIdAsync(itemId);
            if (item == null)
                throw new Exception("Item not found.");

            if (newStatus == PurchaseOrderItemStatusEnum.Pending.ToString())
            {
                var potentialMatches = await _itemRepo.GetMatchingItemsAsync(item.PurchaseOrderId, new ItemCreateDto
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Justification = item.Justification,
                    PurchaseLocation = item.PurchaseLocation,
                    Status = PurchaseOrderItemStatusEnum.Pending
                });

                var matchToMerge = potentialMatches.FirstOrDefault(m => m.Id != item.Id);

                if (matchToMerge != null)
                {
                    var newQuantity = matchToMerge.Quantity + item.Quantity;
                    await _itemRepo.UpdateItemQuantityAsync(matchToMerge.Id, newQuantity);
                    await _itemRepo.DeleteAsync(item.Id);
                    return matchToMerge;
                }
            }

            if (newStatus == PurchaseOrderItemStatusEnum.Denied.ToString() ||
                newStatus == PurchaseOrderItemStatusEnum.Pending.ToString())
            {
                item.ModificationReason = null;
            }

            item.Status = Enum.Parse<PurchaseOrderItemStatusEnum>(newStatus);
            item.DenialReason = denialReason;
            item.RecordVersion = recordVersion;

            var updatedItem = _itemRepo.Update(item);

            if (item.Status == PurchaseOrderItemStatusEnum.Approved || item.Status == PurchaseOrderItemStatusEnum.Denied)
            {
                var po =  _purchaseOrderRepo.GetById(item.PurchaseOrderId);
                if (po.Status == PurchaseOrderStatusEnum.Pending)
                {
                    po.Status = PurchaseOrderStatusEnum.UnderReview;
                    _purchaseOrderRepo.Update(po);
                }
            }


            if (item.Status == PurchaseOrderItemStatusEnum.Pending)
            {
                var allItems = await _itemRepo.GetItemsByPurchaseOrderIdAsync(item.PurchaseOrderId);
                bool allArePending = allItems.All(i => i.Status == PurchaseOrderItemStatusEnum.Pending);

                if (allArePending)
                {
                    var po = _purchaseOrderRepo.GetById(item.PurchaseOrderId);
                    if (po.Status == PurchaseOrderStatusEnum.UnderReview)
                    {
                        po.Status = PurchaseOrderStatusEnum.Pending;
                         _purchaseOrderRepo.Update(po);
                    }
                }
            }

            return updatedItem;

        }



        public async Task<PurchaseOrderItem?> GetByIdAsync(int id)
        {
            return await _itemRepo.GetByIdAsync(id);
        }

        public async Task<List<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(string purchaseOrderId)
        {
            return  await _itemRepo.GetItemsByPurchaseOrderIdAsync(purchaseOrderId);
        }

        public async Task<List<PurchaseOrderItem>> GetItemsByEmployeeIdAsync(string employeeId)
        {
            return await _itemRepo.GetItemsByEmployeeIdAsync(employeeId);
        }

       

        public void ValidateModel(PurchaseOrderItem pI)
        {
            List<ValidationResult> results = new();
            Validator.TryValidateObject(pI, new ValidationContext(pI), results, true);

            foreach (ValidationResult e in results)
            {
                pI.AddError(new(e.ErrorMessage, ErrorType.Model));
            }
        }

        public async Task<(decimal Subtotal, decimal TaxTotal, decimal GrandTotal)> GetPurchaseOrderTotalsAsync(string purchaseOrderId)
        {
            var items = await _itemRepo.GetItemsByPurchaseOrderIdAsync(purchaseOrderId);

            decimal subtotal = items.Sum(i => i.Price * i.Quantity);
            decimal tax = Math.Round(subtotal * 0.15m, 2);
            decimal grandTotal = subtotal + tax;

            return (subtotal, tax, grandTotal);
        }


        public PurchaseOrderItem UpdateBySupervisor(SupervisorPOItemUpdateDto dto)
        {
            var existing = _itemRepo.GetByIdAsync(dto.Id).GetAwaiter().GetResult();

            if (existing == null)
            {
                var notFoundItem = new PurchaseOrderItem();
                notFoundItem.AddError(new ValidationrError("Item not found.", ErrorType.Business));
                return notFoundItem;
            }
            existing.Quantity = dto.Quantity;
            existing.Price = dto.Price;
            existing.PurchaseLocation = dto.PurchaseLocation;

            if (string.IsNullOrWhiteSpace(dto.ModificationReason))
            {
                existing.AddError(new ValidationrError("Modification reason is required.", ErrorType.Model));
                return existing;
            }

            existing.ModificationReason = dto.ModificationReason;
            existing.RecordVersion = dto.RecordVersion;


            var updatedItem = _itemRepo.Update(existing);

            return updatedItem;


        }

    }
}
