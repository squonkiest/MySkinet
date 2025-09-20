using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.Infrastructure.Data
{
    internal class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsDistinct)
            {
                query = query.Distinct();
            }

            return query;
        }

        public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query,
            ISpecificaiton<T, TResult> specification)
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            var selectQuery = query as IQueryable<TResult>;

            if (specification.Select != null)
            {
                selectQuery = query.Select(specification.Select);
            }

            if (specification.IsDistinct)
            {
                selectQuery = selectQuery?.Distinct();
            }

            return selectQuery ?? query.Cast<TResult>();
        }
    }
}
