using Application.Features.CurrencyModels.Models;
using Application.Services;
using AutoMapper;
using Core.Application.Requests;
using Core.Persistance.Paging;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace Application.Features.CurrencyModels.Queries.CurrencyModelListQuery;

public class GetListCurrencyModelQuery : IRequest<CurrencyModelListModel> {
    public PageRequest PageRequest { get; set; }
    public string CacheKey => "currency-list";

    public class GetListCarQueryHandler : IRequestHandler<GetListCurrencyModelQuery, CurrencyModelListModel> {
        private readonly ICurrencyModelRepository _currencyModelRepository;
        private readonly IMapper _mapper;

        public GetListCarQueryHandler(ICurrencyModelRepository currencyModelRepository, IMapper mapper) {
            _currencyModelRepository = currencyModelRepository;
            _mapper = mapper;
        }

        public async Task<CurrencyModelListModel> Handle(GetListCurrencyModelQuery request, CancellationToken cancellationToken) {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://www.tcmb.gov.tr/kurlar/");

            var result = await httpClient.GetAsync("today.xml");

            string xmlString = await result.Content.ReadAsStringAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(MyClass));
            using(TextReader reader = new StringReader(xmlString)) {
                MyClass myClass = (MyClass)serializer.Deserialize(reader);
                myClass.Currencies.ForEach(x => Console.WriteLine(x.CurrencyName));

                List<CurrencyModel> currencyModels = new();

                myClass.Currencies.ForEach((x) => {
                    CurrencyModel currencyModel = new() {
                        CurrencyCode = x.CurrencyCode,
                        Unit = x.Unit,
                        Currency = x.CurrencyName,
                        ForexBuying = x.ForexBuying is "" ? 0 : decimal.Parse(x.ForexBuying, CultureInfo.InvariantCulture),
                        ForexSelling = x.ForexSelling is "" ? 0 : decimal.Parse(x.ForexSelling, CultureInfo.InvariantCulture),
                        BanknoteBuying = x.BanknoteBuying is "" ? 0 : decimal.Parse(x.BanknoteBuying, CultureInfo.InvariantCulture),
                        BanknoteSelling = x.BanknoteSelling is "" ? 0 : decimal.Parse(x.BanknoteSelling, CultureInfo.InvariantCulture),
                    };

                    currencyModels.Add(currencyModel);
                });
                await _currencyModelRepository.AddRangeAsync(currencyModels);
            }

            IPaginate<CurrencyModel> currencyy = await _currencyModelRepository.GetListAsync(index: request.PageRequest.Page, size: request.PageRequest.PageSize);
            CurrencyModelListModel mappedCarListModel = _mapper.Map<CurrencyModelListModel>(currencyy);
            return mappedCarListModel;
        }
    }
}
[XmlRoot("Tarih_Date")]
public class MyClass {

    [XmlAttribute("Tarih")]
    public string Tarih { get; set; }

    [XmlAttribute("Date")]
    public string Date { get; set; }

    [XmlAttribute("Bulten_No")]
    public string BultenNo { get; set; }

    [XmlElement("Currency")]
    public List<Currency> Currencies { get; set; }
}

[XmlType("Currency")]
public class Currency {
    [XmlAttribute("CrossOrder")]
    public string CrossOrder { get; set; }

    [XmlAttribute("Kod")]
    public string Kod { get; set; }

    [XmlAttribute("CurrencyCode")]
    public string CurrencyCode { get; set; }

    [XmlElement("Unit")]
    public int Unit { get; set; }

    [XmlElement("Isim")]
    public string Isim { get; set; }

    [XmlElement("CurrencyName")]
    public string CurrencyName { get; set; }

    [XmlElement("ForexBuying")]
    public string ForexBuying { get; set; }

    [XmlElement("ForexSelling")]
    public string ForexSelling { get; set; }

    [XmlElement("BanknoteBuying")]
    public string BanknoteBuying { get; set; }

    [XmlElement("BanknoteSelling")]
    public string BanknoteSelling { get; set; }

    [XmlElement("CrossRateUSD")]
    public string CrossRateUSD { get; set; }

    [XmlElement("CrossRateOther")]
    public string CrossRateOther { get; set; }
}