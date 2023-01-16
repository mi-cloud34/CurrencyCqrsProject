﻿using Application.Features.CurrencyModels.Models;
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

namespace Application.Features.CurrencyModels.Queries.CurrencyModelListQuery;

public class GetListCurrencyModelQuery : IRequest<CurrencyModelListModel>
{
    public PageRequest PageRequest { get; set; }
    public string CacheKey => "currency-list";

    public class GetListCarQueryHandler : IRequestHandler<GetListCurrencyModelQuery, CurrencyModelListModel>
    {
        private readonly ICurrencyModelRepository _currencyModelRepository;
        private readonly IMapper _mapper;

        public GetListCarQueryHandler(ICurrencyModelRepository currencyModelRepository, IMapper mapper)
        {
            _currencyModelRepository = currencyModelRepository;
            _mapper = mapper;
        }

        public async Task<CurrencyModelListModel> Handle(GetListCurrencyModelQuery request, CancellationToken cancellationToken)
        {

            //var client = new System.Net.Http.HttpClient();
            //var currency = await client.GetStringAsync(url);


            //HttpClient client = new();
            //var response = client.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml").GetAwaiter().GetResult();
            //var xml = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //XmlDocument doc = new();
            //doc.LoadXml(xml);
            //string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            //Console.WriteLine(xml);
            //Console.WriteLine(json);
            //Console.ReadKey();

            //var url = "https://www.tcmb.gov.tr/";
            //using var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(url);

            //var result = httpClient.GetAsync("today.xml").Result;
            //var jsonString = result.Content.ReadAsStringAsync().Result;
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(jsonString);
            //string json = JsonConvert.SerializeXmlNode(jsonString);
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://www.tcmb.gov.tr/kurlar/");

            var result = await httpClient.GetAsync("today.xml");

            string xmlString = await result.Content.ReadAsStringAsync();
            XmlSerializer serializer = new XmlSerializer(typeof(MyClass));
            using (TextReader reader = new StringReader(xmlString))
            {
                MyClass myClass = (MyClass)serializer.Deserialize(reader);
                myClass.Currencies.ForEach(x => _currencyModelRepository.Add(JsonSerializer.Deserialize<List<CurrencyModel>>(x)));
            }
            //var currency = JsonSerializer.Deserialize<List<CurrencyModel>>(json);
            //foreach (var c in currency)
            //{
            //    _currencyModelRepository.Add(c);
            //    // var json = JsonConvert.DeserializeObject<CurrencyModel>(c);
            //    ////var json = JsonConvert.SerializeObject(c);

            //    // //var json = JsonSerializer.Serialize<CurrencyModel>(c)
            //}
            IPaginate<CurrencyModel> currencyy = await _currencyModelRepository.GetListAsync(index: request.PageRequest.Page, size: request.PageRequest.PageSize);
            CurrencyModelListModel mappedCarListModel = _mapper.Map<CurrencyModelListModel>(currencyy);
            return mappedCarListModel;
        }
    }
}
[XmlRoot("Tarih_Date")]
public class MyClass
{

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
public class Currency
{
    [XmlAttribute("CrossOrder")]
    public string CrossOrder { get; set; }

    [XmlAttribute("Kod")]
    public string Kod { get; set; }

    [XmlAttribute("CurrencyCode")]
    public string CurrencyCode { get; set; }

    [XmlElement("Unit")]
    public string Unit { get; set; }

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