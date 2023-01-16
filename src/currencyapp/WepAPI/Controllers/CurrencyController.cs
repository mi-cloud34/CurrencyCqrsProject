using Application.Features.CurrencyModels.Dtos;
using Application.Features.CurrencyModels.Models;
using Application.Features.CurrencyModels.Queries.CurrencyModelListQuery;
using Application.Features.CurrencyModels.Queries.GetByIdCurrencyModel;
using Core.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController :BaseController
    {
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdCurrencModelQuery getByIdCurrencModelQuery)
        {
            CurrencyModelDto result = await Mediator.Send(getByIdCurrencModelQuery);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            GetListCurrencyModelQuery getListCurrencyModelQuery = new() { PageRequest = pageRequest };
            CurrencyModelListModel result = await Mediator.Send(getListCurrencyModelQuery);
            return Ok(result);
        }
    }
}
