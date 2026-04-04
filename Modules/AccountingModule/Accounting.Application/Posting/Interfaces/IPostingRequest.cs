using Accounting.Domain.Enums;
using System;

namespace Accounting.Application.Posting.Interfaces
{
    public interface IPostingRequest
    {
        SourceType SourceType { get; set; }
        int SourceId { get; set; }
        int CurrencyId { get; set; }
        DateTime Date { get; set; }
        string? Description { get; set; }
        string? Reference { get; set; }
    }
}
