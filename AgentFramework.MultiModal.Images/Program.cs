using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.ClientModel;

var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty;
var key = Environment.GetEnvironmentVariable("OPENAI_KEY") ?? string.Empty;

var name = "TranslationAgent";
var description = "Agent that knows about translating text between languages.";
var instructions = "You should respond to translation requests.";

var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(key));
var chatClient = openAIClient.GetChatClient("gpt-4o").AsIChatClient();
var agentClient = new ChatClientAgent(chatClient, name: name, description: description, instructions: instructions);

var chatSession = await agentClient.CreateSessionAsync();

var textContent = new TextContent("Translate the text in the image to Dutch.");
var urlContent = new UriContent("https://cdn.vectorstock.com/i/1000v/71/18/french-welcome-print-typography-vector-7817118.jpg", "image/jpeg");
var inlineImageContent = new DataContent(new byte[] { 0x01, 0x02, 0x03 }, "image/jpeg");

var response = await agentClient.RunAsync(new ChatMessage(ChatRole.User, [textContent, urlContent]), chatSession);

Console.WriteLine(response.Text);