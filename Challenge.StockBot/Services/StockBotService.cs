using System.Globalization;
using Challenge.Infrastructure;
using Challenge.Infrastructure.Models;
using CsvHelper;

namespace Challenge.StockBot;
public interface IStockService
{
    Task GetStockQuote(string stock);
}

public class StockBotService : IStockService
{
    private readonly IStooqServiceApi _stooqServiceApi;
    private readonly IMessageBrokerSender _messageBrokerSender;

    public StockBotService(IStooqServiceApi stooqServiceApi, IMessageBrokerSender messageBrokerSender)
    {
        _stooqServiceApi = stooqServiceApi;
        _messageBrokerSender = messageBrokerSender;
    }

    public async Task GetStockQuote(string stock)
    {
        var chatMessageResponse = new ChatBotMessage()
        {
            //TODO: read bot name from environment variable.
            BotName = "StockBot"
        };
        try
        {
            var stockCsvData = await _stooqServiceApi.GetStockData(stock);
            double stockCloseValue = GetStockCloseValue(stockCsvData);
            chatMessageResponse.Message = $"{stock.ToUpper()} quote is ${stockCloseValue} per share";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            chatMessageResponse.Message = $"Error processing stock price for stock {stock}";
        }
        //TODO: read queue name from environment variable.
        await _messageBrokerSender.SendToQueue(chatMessageResponse, "chat.queue");
    }

    private double GetStockCloseValue(string stockCsvData)
    {
        using (var stringReader = new StringReader(stockCsvData))
        using (var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture))
        {
            csvReader.Read();
            csvReader.ReadHeader();
            csvReader.Read();
            return csvReader.GetField<double>("Close");
        }
    }
}