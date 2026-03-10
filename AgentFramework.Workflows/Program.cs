using AgentFramework.Common;
using AgentFramework.Workflows.Agents;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System.ClientModel;
using System.Text;

var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty;
var key = Environment.GetEnvironmentVariable("OPENAI_KEY") ?? string.Empty;

var client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(key));

var orchestratorAgent = OrchestratorAgent.Create(client);
var generalAgent = GeneralAgent.Create(client);
var timeAgent = TimeAgent.Create(client);
var replyAgent = ReplyAgent.Create(client);

var workflow = AgentWorkflowBuilder
    .CreateHandoffBuilderWith(initialAgent: orchestratorAgent)
    .WithHandoffs(from: orchestratorAgent, to: [generalAgent, timeAgent])
    .WithHandoff(from: generalAgent, to: orchestratorAgent)
    .WithHandoff(from: timeAgent, to: orchestratorAgent)
    .Build();

var workflowAgent = workflow.AsAIAgent();

var chatSession = await workflowAgent.CreateSessionAsync();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();

    var answerBuilder = new StringBuilder();

    await foreach (var response in workflowAgent.RunStreamingAsync(request!, chatSession))
    {
        if (!string.IsNullOrWhiteSpace(response.Text))
        {
            answerBuilder.Append(response.Text);
        }
        else
        {
            foreach (var contents in response.Contents)
            {
                if (contents is UsageContent usageContent)
                {
                    // NOP
                }

                if (contents is FunctionCallContent functionCallContent)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{response.AuthorName}-FunctionCall > ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(functionCallContent.Name);
                    foreach (var argument in functionCallContent.Arguments)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"\t{argument.Key} > ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(argument.Value);
                    }
                }

                if (contents is FunctionResultContent functionResultContent)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{response.AuthorName}-FunctionResult > ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(functionResultContent.Result);
                }
            }
        }
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(answerBuilder);

    Console.WriteLine();

    chatSession.Debug();
}