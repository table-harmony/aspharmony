using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Utils;

public interface ITextModelService {
    Task<string> GetResponseAsync(string prompt);
}

public class GeminiService : ITextModelService {
    private readonly GenerativeModel _model;

    public GeminiService(IConfiguration configuration) {
        string apiKey = configuration["Gemini:ApiKey"] ?? 
            throw new Exception("Gemini API Key not found in configuration.");

        _model = new() { 
            ApiKey = apiKey,
            Model = Model.Gemini15Flash,
        };
    }

    public async Task<string> GetResponseAsync(string prompt) {
        try {
            var response = await _model.GenerateContent(prompt);
            return response.Text ?? "";
        } catch {
            return "I apologize, but I'm unable to answer at the moment. Please try again later.";
        }
    }
}

public interface IImageModelService {
    public readonly record struct Image(string Prompt, int Width = 1000, int Height = 1500, string Format = "webp");

    Task<Stream> GenerateImageAsync(Image image);
}

public class StabilityService : IImageModelService {
    private readonly HttpClient _httpClient;

    public StabilityService(IConfiguration configuration) {
        string apiKey = configuration["StabilityAI:ApiKey"] ??
            throw new Exception("StabilityAI API Key not found in configuration.");

        _httpClient = new HttpClient {
            BaseAddress = new Uri("https://api.stability.ai/")
        };
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("image/*"));
    }

    public async Task<Stream> GenerateImageAsync(IImageModelService.Image image) {
        using var content = new MultipartFormDataContent {
            { new StringContent(image.Prompt), "\"prompt\"" },
            { new StringContent(image.Format), "\"output_format\"" },
            { new StringContent(image.Width.ToString()), "\"width\"" },
            { new StringContent(image.Height.ToString()), "\"height\"" }
        };

        var response = await _httpClient.PostAsync("v2beta/stable-image/generate/core", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }

}


public interface ITextToSpeechService {
    public readonly record struct SpeechRequest(
        string Text,
        string ModelId = "eleven_monolingual_v1",
        string VoiceId = "9BWtsMINqrJLrRacOk9x",
        float Stability = 0.5f,
        float SimilarityBoost = 0.5f
    );

    Task<Stream> GenerateSpeechAsync(SpeechRequest request);
}

public class ElevenLabsService : ITextToSpeechService {
    private readonly HttpClient _httpClient;

    public ElevenLabsService(IConfiguration configuration) {
        string apiKey = configuration["ElevenLabs:ApiKey"] ??
            throw new Exception("ElevenLabs API Key not found in configuration.");

        _httpClient = new HttpClient {
            BaseAddress = new Uri("https://api.elevenlabs.io/v1/"),
            Timeout = TimeSpan.FromMinutes(5)
        };
        _httpClient.DefaultRequestHeaders.Add("xi-api-key", apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("audio/mpeg"));
    }

    public async Task<Stream> GenerateSpeechAsync(ITextToSpeechService.SpeechRequest request) {
        var payload = new {
            text = request.Text,
            model_id = request.ModelId,
            voice_settings = new {
                stability = request.Stability,
                similarity_boost = request.SimilarityBoost
            }
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(payload), 
            Encoding.UTF8, 
            "application/json"
        );

        var response = await _httpClient.PostAsync($"text-to-speech/{request.VoiceId}", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
}