using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class CreateGoodsReceiptCommandHandler
        : IRequestHandler<CreateGoodsReceiptCommandRequest, CreateGoodsReceiptCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGoodsReceiptCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateGoodsReceiptCommandResponse> Handle(CreateGoodsReceiptCommandRequest request, CancellationToken cancellationToken)
        {
            var dto = request.GoodsReceipt;

            var goodsReceipt = new GoodsReceipt(dto.WarehouseId)
            {
                PurchaseOrderId = dto.PurchaseOrderId,
                LocationId = dto.LocationId,
                ReceivedDate = dto.ReceivedDate,
                ReceivedBy = dto.ReceivedBy,
                Remarks = dto.Remarks
            };

            foreach (var item in dto.Items)
            {
                goodsReceipt.AddItem(item.ProductId, item.ReceivedQuantity);
            }

            await _unitOfWork.GoodsReceiptRepository.AddAsync(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();

            var goodsReceiptDto = new GoodsReceiptDto
            {
                Id = goodsReceipt.Id,
                PurchaseOrderId = goodsReceipt.PurchaseOrderId,
                ReceivedDate = goodsReceipt.ReceivedDate,
                ReceivedBy = goodsReceipt.ReceivedBy.Value,
                Remarks = goodsReceipt.Remarks,
                CreatedAt = goodsReceipt.CreatedAt
            };

            return new CreateGoodsReceiptCommandResponse
            {
                Success = true,
                Message = "Goods receipt created successfully",
                GoodsReceipt = goodsReceiptDto
            };
        }
    }
}
