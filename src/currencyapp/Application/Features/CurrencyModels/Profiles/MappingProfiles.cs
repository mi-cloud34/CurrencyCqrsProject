using Application.Features.CurrencyModels.Dtos;
using Application.Features.CurrencyModels.Models;
using AutoMapper;
using Core.Persistance.Paging;
using Domain.Entities;

namespace Application.Features.CurrencyModels.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {

        CreateMap<CurrencyModel, CurrencyModelListDto>().ReverseMap();
        CreateMap<IPaginate<CurrencyModel>, CurrencyModelListModel>().ReverseMap();
    }
}