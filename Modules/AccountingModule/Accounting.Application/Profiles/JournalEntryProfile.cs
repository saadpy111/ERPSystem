using Accounting.Application.Features.JournalEntries.Commands.CreateJournalEntry;
using Accounting.Application.Features.JournalEntries.DTOs;
using Accounting.Domain.Entities;
using AutoMapper;

namespace Accounting.Application.Profiles
{
    public class JournalEntryProfile : Profile
    {
        public JournalEntryProfile()
        {
            // Command -> Entity
            CreateMap<CreateJournalEntryCommand, JournalEntry>();
            CreateMap<JournalEntryLineDto, JournalEntryLine>();

            // Entity -> DTO
            CreateMap<JournalEntry, JournalEntryResponseDto>();
            CreateMap<JournalEntryLine, JournalEntryLineResponseDto>();
        }
    }
}
