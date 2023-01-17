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

        CreateMap<Currency, CurrencyModelListDto>().ReverseMap();
        CreateMap<IPaginate<Currency>, CurrencyModelListModel>().ReverseMap();
    }
}