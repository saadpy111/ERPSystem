using System;
using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.DashboardFeatures.Queries.GetDashboardOverview
{
    public class GetDashboardOverviewQuery : IRequest<DashboardOverviewDto>
    {
        public int? Days { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
