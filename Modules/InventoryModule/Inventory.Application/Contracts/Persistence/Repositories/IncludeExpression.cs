using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Contracts.Persistence.Repositories
{
    public class IncludeExpression<T, TProperty>
    {
        public Expression<Func<T, TProperty>> Include { get; set; }
        public List<LambdaExpression>? ThenIncludes { get; set; }

        public IncludeExpression(Expression<Func<T, TProperty>> include)
        {
            Include = include;
            ThenIncludes = new List<LambdaExpression>();
        }

        public IncludeExpression<T, TProperty> AddThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> thenInclude)
        {
            ThenIncludes!.Add(thenInclude);
            return this;
        }
    }

}
