using Core.Persistance.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICurrencyModelRepository : IAsyncRepository<CurrencyModel>, IRepository<CurrencyModel>
    {

    }
}
