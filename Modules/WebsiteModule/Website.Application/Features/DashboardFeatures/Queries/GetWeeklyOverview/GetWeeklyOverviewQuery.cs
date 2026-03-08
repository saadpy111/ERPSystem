using System.Collections.Generic;
using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.DashboardFeatures.Queries.GetWeeklyOverview
{
    public class GetWeeklyOverviewQuery : IRequest<List<WeeklyAnalyticsDto>>
    {
        public int? Days { get; set; }
    }
}
