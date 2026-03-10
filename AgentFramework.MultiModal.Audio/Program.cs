using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using System.ClientModel;

var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? string.Empty;
var key = Environment.GetEnvironmentVariable("OPENAI_KEY") ?? string.Empty;

var name = "TranslationAgent";
var description = "Agent that knows about translating text between languages.";
var instructions = "You should respond to translation requests.";

var openAIClient = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(key));
var audioClient = openAIClient.GetAudioClient("whisper");

var audioData = File.OpenRead(@"C:\_-_GITHUB_-_\EnableGenAIUsingTheMicrosoftAgentFramework-FutureTech-2026\AgentFramework.MultiModalModels\carl-sagan.mp3");
var textContent = new TextContent("Translate what is said in the audio clip to Dutch.");

var response = await audioClient.TranscribeAudioAsync(audioData, "carl-sagan.mp3");

Console.WriteLine(response.Value.Text);