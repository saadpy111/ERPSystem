using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Domain.Entities;
using SharedKernel.DomainEvents;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class ConfirmGoodsReceiptCommandHandler
        : IRequestHandler<ConfirmGoodsReceiptCommandRequest, ConfirmGoodsReceiptCommandResponse>
    {
        private readonly IGoodsReceiptRepository _goodsReceiptRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ConfirmGoodsReceiptCommandHandler(
            IGoodsReceiptRepository goodsReceiptRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<ConfirmGoodsReceiptCommandResponse> Handle(ConfirmGoodsReceiptCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                GoodsReceipt? goodsReceipt = await _goodsReceiptRepository.GetByIdAsync(request.GoodsReceiptId, includeItems: true);

                if (goodsReceipt == null)
                {
                    return new ConfirmGoodsReceiptCommandResponse
                    {
                        Success = false,
                        Message = "Goods receipt not found"
                    };
                }

                goodsReceipt.ConfirmReceipt();

                _goodsReceiptRepository.Update(goodsReceipt);
                await _unitOfWork.SaveChangesAsync();

                var goodsReceivedEvent = new GoodsReceivedEvent
                {
                    PurchaseOrderId = goodsReceipt.PurchaseOrderId,
                    GoodsReceiptId = goodsReceipt.Id,
                    WarehouseId = goodsReceipt.WarehouseId,
                    LocationId = goodsReceipt.LocationId,
                    Items = goodsReceipt.Items.Select(i => new ReceivedItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.ReceivedQuantity,
                    }).ToList()
                };

                await _mediator.Publish(goodsReceivedEvent, cancellationToken);

                return new ConfirmGoodsReceiptCommandResponse
                {
                    Success = true,
                    Message = "Goods receipt confirmed successfully"
                };
            }
            catch (System.Exception ex)
            {
                return new ConfirmGoodsReceiptCommandResponse
                {
                    Success = false,
                    Message = $"Error confirming goods receipt: {ex.Message}"
                };
            }
        }
    }
}
