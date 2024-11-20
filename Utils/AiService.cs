using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;

namespace Utils;

public interface IAiService {
    Task<string> GetResponseAsync(string question);
}

public class GeminiService : IAiService {
    private readonly GenerativeModel _model;

    public GeminiService(IConfiguration configuration) {
        string? apiKey = configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
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