using System.Globalization;
using System.Xml.Linq;

namespace Seb.Server.Infrastructure.Clients;

public interface ICurrencyExchangeClient
{
    Task<CurrencyResult> FetchCurrencyExchangeRates();
}

public record CurrencyResult(IReadOnlyCollection<CurrencyRateModel> Rates, DateTime DateStamp);

public record CurrencyRateModel(string Name, string Code, decimal Rate);
public class CurrencyExchangeClient : ICurrencyExchangeClient
{
    private readonly HttpClient _httpClient;

    public CurrencyExchangeClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CurrencyResult> FetchCurrencyExchangeRates()
    {
        var response = await _httpClient.GetAsync("");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();
        var document = XDocument.Parse(content);

        var currencyNodes = document.Nodes().ToArray();
        var currentNode = (XElement?)((XElement)((XElement)currencyNodes[0]).FirstNode!.NextNode!.NextNode!).FirstNode;

        var timeStamp = DateTime.Parse(currentNode!.Attribute("time")!.Value);

        currentNode = (XElement?)currentNode.FirstNode;
        var resolvedCurrencies = new List<CurrencyRateModel>();
        while (currentNode != null)
        {
            var currencyCode = currentNode.Attribute("currency")!.Value;
            var currencyRate = decimal.Parse(currentNode.Attribute("rate")!.Value, CultureInfo.InvariantCulture);
            var currencyName = _currencyNameMap[currencyCode];

            resolvedCurrencies.Add(new CurrencyRateModel(currencyName, currencyCode, currencyRate));

            currentNode = (XElement?)currentNode.NextNode;
        }

        return new CurrencyResult(resolvedCurrencies, timeStamp);
    }
 
    private readonly Dictionary<string, string> _currencyNameMap = new()
    {
        { "USD", "United States Dollar" },
        { "JPY", "Japanese yen" },
        { "BGN", "Bulgarian lev" },
        { "CZK", "Czech koruna" },
        { "DKK", "Danish krone" },
        { "GBP", "Pound sterling" },
        { "HUF", "Hungarian forint" },
        { "PLN", "Polish zloty" },
        { "RON", "Romanian leu" },
        { "SEK", "Swedish krona" },
        { "CHF", "Swiss franc" },
        { "ISK", "Icelandic krona" },
        { "NOK", "Norwegian krone" },
        { "TRY", "Turkish lira" },
        { "AUD", "Australian dollar" },
        { "BRL", "Brazilian real" },
        { "CAD", "Canadian dollar" },
        { "CNY", "Chinese yuan renminbi" },
        { "HKD", "Hong Kong dollar" },
        { "IDR", "Indonesian rupiah" },
        { "ILS", "Israeli shekel" },
        { "INR", "Indian rupee" },
        { "KRW", "South Korean won" },
        { "MXN", "Mexican peso" },
        { "MYR", "Malaysian ringgit" },
        { "NZD", "New Zealand dollar" },
        { "PHP", "Philippine peso" },
        { "SGD", "Singapore dollar" },
        { "THB", "Thai baht" },
        { "ZAR", "South African rand" }
    };
}