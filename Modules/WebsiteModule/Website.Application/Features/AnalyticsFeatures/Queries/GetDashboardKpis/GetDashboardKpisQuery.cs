using System;
using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetDashboardKpis
{
    public class GetDashboardKpisQuery : IRequest<DashboardKpiDto>
    {
        public int? Days { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
