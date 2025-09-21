using Microsoft.AspNetCore.Mvc;
using Skinet.API.RequestHelpers;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;

namespace Skinet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository,
            ISpecification<T> specification, int pageIndex, int pageSize) where T : BaseEntity
        {
            IReadOnlyList<T> items = await repository.ListAsync(specification);
            int count = await repository.CountAsync(specification);
            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
            return Ok(pagination);
        }
    }
}
