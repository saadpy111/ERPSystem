using Accounting.Application.Features.JournalEntries.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry
{
    public class CreateJournalEntryCommand : IRequest<Result<JournalEntryResponseDto>>
    {
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Reference { get; set; }
        public int CurrencyId { get; set; }
        public List<JournalEntryLineDto> Lines { get; set; } = new List<JournalEntryLineDto>();
    }
}
