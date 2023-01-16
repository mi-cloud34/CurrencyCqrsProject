using Application.Features.CurrencyModels.Dtos;
using Core.Persistance.Paging;

namespace Application.Features.CurrencyModels.Models;

public class CurrencyModelListModel : BasePageableModel
{
    public IList<CurrencyModelListDto> Items { get; set; }
}