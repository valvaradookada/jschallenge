namespace Challenge.Api.Domain;

public interface IChatCommand
{
    string Message { get; set; }
    bool ValidCommand { get; }
    string BotName { get; }
    string Command { get; }
}

public class ChatCommand : IChatCommand
{
    public string Message { get; set; }
    public bool ValidCommand => Message.StartsWith("/");
    public string BotName => Message.Substring(1, Message.IndexOf("=") - 1);
    public string Command => Message.Substring(Message.IndexOf("=") + 1);
}