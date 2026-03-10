using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using System.ClientModel;

Console.WriteLine("Hello, World!");

var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty;
var key = Environment.GetEnvironmentVariable("OPENAI_KEY") ?? string.Empty;

var name = "TranslationAgent";
var description = "Agent that knows about translating text between languages.";
var instructions = "You should respond to translation requests.";

var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(key));
var audioClient = openAIClient.GetAudioClient("whisper");
//var agentClient = new ChatClientAgent(chatClient, name: name, description: description, instructions: instructions);

//var chatSession = await agentClient.CreateSessionAsync();

//var textContent = new TextContent("Translate the text in the image to Dutch.");
//var urlContent = new UriContent("https://cdn.vectorstock.com/i/1000v/71/18/french-welcome-print-typography-vector-7817118.jpg", "image/jpeg");
//var inlineImageContent = new DataContent(new byte[] { 0x01, 0x02, 0x03 }, "image/jpeg");


var audioData = File.OpenRead(@"C:\_-_GITHUB_-_\EnableGenAIUsingTheMicrosoftAgentFramework-FutureTech-2026\AgentFramework.MultiModalModels\carl-sagan.mp3");
var textContent = new TextContent("Translate what is said in the audio clip to Dutch.");
//var inlineAudioContent = new DataContent(audioData, "audio/mpeg");

var response = await audioClient.TranscribeAudioAsync(audioData, "carl-sagan.mp3");

//var response = await agentClient.RunAsync(new ChatMessage(ChatRole.User, [textContent, inlineAudioContent]), chatSession);

Console.ReadKey();