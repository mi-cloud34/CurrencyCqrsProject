using Application.Features.CurrencyModels.Dtos;
using Application.Services;
using AutoMapper;
using Core.Application.Pipelines.Caching;
using Domain.Entities;
using MediatR;

namespace Application.Features.CurrencyModels.Queries.GetByIdCurrencyModel;

public class GetByIdCurrencModelQuery : IRequest<CurrencyModelDto>, ICacheRemoverRequest
{
    public int Id { get; set; }
    public bool BypassCache { get; }
    public string CacheKey => "currency-byId";

    public class GetByIdBrandQueryHandler : IRequestHandler<GetByIdCurrencModelQuery, CurrencyModelDto>
    {
        private readonly ICurrencyModelRepository _currencyModelRepository;
        private readonly IMapper _mapper;

        public GetByIdBrandQueryHandler(ICurrencyModelRepository currencyModelRepository,
                                        IMapper mapper)
        {
            _currencyModelRepository = currencyModelRepository;

            _mapper = mapper;
        }


        public async Task<CurrencyModelDto> Handle(GetByIdCurrencModelQuery request, CancellationToken cancellationToken)
        {

            Currency? currencyModel = await _currencyModelRepository.GetAsync(b => b.Id == request.Id);
            CurrencyModelDto currencyModelDto = _mapper.Map<CurrencyModelDto>(currencyModel);
            return currencyModelDto;
        }
    }
}