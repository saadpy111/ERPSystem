using Accounting.Domain.Enums;
using System;
using Accounting.Application.Posting.Interfaces;

namespace Accounting.Application.Posting.Requests
{
    public class PostingRequest : IPostingRequest
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
    }
}
