using System.Net;
using RestSharp;

namespace Challenge.StockBot;

public interface IStooqServiceApi
{
    Task<string> GetStockData(string stock);
}

public class StooqServiceApi : IStooqServiceApi
{
    private readonly RestClient _restClient;

    public StooqServiceApi()
    {
        //TODO: read stooq url from an environment variable
        var options = new RestClientOptions("https://stooq.com")
        {
            ThrowOnAnyError = true,
            MaxTimeout = 5000
        };

        _restClient = new RestClient(options);
    }
    public async Task<string> GetStockData(string stock)
    {
        var requestUrl = $"q/l/?f=sd2t2ohlcv&h&e=csv&s={stock}";
        var request = new RestRequest(requestUrl);

        var response = await _restClient.ExecuteAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return response.Content;
        throw new Exception(
            $"Error on Get {requestUrl} , statuscode received: {response.StatusCode}");
    }
}