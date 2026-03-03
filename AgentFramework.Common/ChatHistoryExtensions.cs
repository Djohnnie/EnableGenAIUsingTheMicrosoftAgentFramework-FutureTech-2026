using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentFramework.Common;

public static class ChatHistoryExtensions
{
    public static void Debug(this AgentSession chatThread)
    {
        if (chatThread.StateBag.TryGetValue<InMemoryChatHistoryProvider.State>(nameof(InMemoryChatHistoryProvider), out var inMemoryState))
        {
            foreach (var message in inMemoryState.Messages)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"[{message.Role}] ");

                if (message.Role == ChatRole.Tool && message.Contents[0] is FunctionResultContent functionResult)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{functionResult.CallId} --> {functionResult.Result.ToString()}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{message.Text.Trim()}");
                }
            }
        }
    }
}