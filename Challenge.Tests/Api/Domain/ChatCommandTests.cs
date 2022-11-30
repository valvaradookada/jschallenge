using Challenge.Api.Domain;
using Xunit;

namespace Challenge.Tests.Api.Domain;

public class ChatCommandTests
{
    private readonly ChatCommand _chatCommand; 
    public ChatCommandTests()
    {
        _chatCommand = new ChatCommand();
    }

    [Theory]
    [InlineData("/bot=something", true)]
    [InlineData("bot=something", false)]
    public void ValidCommandTest(string message,bool expectedResult)
    {
        _chatCommand.Message = message;
        var actualResult = _chatCommand.ValidCommand;
        Assert.Equal(expectedResult,actualResult);
    }
    
    [Theory]
    [InlineData("/bot=somecommand", "bot","somecommand")]
    [InlineData("/b=","b","")]
    public void ParseCommandData(string message,string expectedBotName,string expectedCommand)
    {
        _chatCommand.Message = message;
        var actualBotName = _chatCommand.BotName;
        var actualCommand = _chatCommand.Command;
        Assert.Equal(actualBotName, expectedBotName);
        Assert.Equal(actualCommand, expectedCommand);
    }
    
}