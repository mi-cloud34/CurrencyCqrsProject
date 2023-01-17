

using Application.Services;
using Core.Persistance.Repositories;
using Domain.Entities;
using Persistance.Contexts;

namespace Persistance.Repositories;

public class CurrencyModelRepository : EfRepositoryBase<Currency, BaseDbContext>, ICurrencyModelRepository
{
    public CurrencyModelRepository(BaseDbContext context) : base(context)
    {
    }
}