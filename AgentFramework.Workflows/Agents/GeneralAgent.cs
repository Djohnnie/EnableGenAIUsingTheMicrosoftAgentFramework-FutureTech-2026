using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentFramework.Workflows.Agents;

internal static class GeneralAgent
{
    private const string NAME = nameof(GeneralAgent);
    private const string DESCRIPTION = nameof(GeneralAgent);
    private const string INSTRUCTIONS = @"
        You should answer all general questions.
        You should handoff the answer back to the OrchestratorAgent.";
    private const string MODEL = "gpt-5-chat";

    public static AIAgent Create(AzureOpenAIClient client)
    {
        var chatClient = client.GetChatClient(MODEL).AsIChatClient();
        var agentClient = new ChatClientAgent(chatClient, NAME, DESCRIPTION, INSTRUCTIONS);
        return agentClient;
    }
}