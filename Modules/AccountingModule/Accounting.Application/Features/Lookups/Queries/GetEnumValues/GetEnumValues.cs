using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Models;
using MediatR;

namespace Accounting.Application.Features.Lookups.Queries.GetEnumValues
{
    public class EnumLookupDto
    {
        public int Value { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetEnumValuesQuery : IRequest<Result<List<EnumLookupDto>>>
    {
        public Type EnumType { get; set; }

        public GetEnumValuesQuery(Type enumType)
        {
            EnumType = enumType;
        }
    }

    public class GetEnumValuesQueryHandler : IRequestHandler<GetEnumValuesQuery, Result<List<EnumLookupDto>>>
    {
        public Task<Result<List<EnumLookupDto>>> Handle(GetEnumValuesQuery request, CancellationToken cancellationToken)
        {
            var values = Enum.GetValues(request.EnumType)
                .Cast<Enum>()
                .Select(e => new EnumLookupDto
                {
                    Value = Convert.ToInt32(e),
                    Name = e.ToString()
                })
                .ToList();

            return Task.FromResult(Result<List<EnumLookupDto>>.Ok(values));
        }
    }
}
