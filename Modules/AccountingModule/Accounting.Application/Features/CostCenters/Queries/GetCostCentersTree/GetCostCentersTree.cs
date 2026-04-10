using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CostCenters.Queries.GetCostCentersTree
{
    public class CostCenterTreeNode
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!; // Combined or selected name
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public bool IsActive { get; set; }
        public List<CostCenterTreeNode> Children { get; set; } = new();
    }

    public class GetCostCentersTreeQuery : IRequest<Result<List<CostCenterTreeNode>>>
    {
    }

    public class GetCostCentersTreeQueryHandler : IRequestHandler<GetCostCentersTreeQuery, Result<List<CostCenterTreeNode>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCostCentersTreeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CostCenterTreeNode>>> Handle(GetCostCentersTreeQuery request, CancellationToken cancellationToken)
        {
            var all = await _unitOfWork.CostCenters.GetAllAsync();
            
            var nodes = all.Select(cc => new CostCenterTreeNode
            {
                Id = cc.Id,
                Code = cc.Code,
                Name = cc.NameAr, // Default to Arabic name
                NameAr = cc.NameAr,
                NameEn = cc.NameEn,
                ParentId = cc.ParentId,
                Level = cc.Level,
                IsActive = cc.IsActive
            }).ToList();

            var tree = new List<CostCenterTreeNode>();
            var dictionary = nodes.ToDictionary(n => n.Id);

            foreach (var node in nodes)
            {
                if (node.ParentId.HasValue && dictionary.ContainsKey(node.ParentId.Value))
                {
                    dictionary[node.ParentId.Value].Children.Add(node);
                }
                else
                {
                    tree.Add(node);
                }
            }

            return Result<List<CostCenterTreeNode>>.Ok(tree);
        }
    }
}
