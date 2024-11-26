using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using static Utils.ITranslationService;

namespace Utils;

public interface ITranslationService {
    public readonly record struct TranslationRequest(string Text, string Language);

    Task<string> TranslateAsync(TranslationRequest request);
    Task<string> TranslateHtmlAsync(TranslationRequest request);
}

public class TranslationService : ITranslationService {
    private readonly TranslationClient _client;

    public TranslationService(IConfiguration configuration) {
        string apiKey = configuration["Google:TranslationApiKey"] ??
            throw new Exception("Google Translation API Key not found in configuration.");

        _client = TranslationClient.CreateFromApiKey(apiKey);
    }

    public async Task<string> TranslateAsync(TranslationRequest request) {
        var response = await _client.TranslateTextAsync(
            text: request.Text,
            targetLanguage: request.Language
        );

        return response.TranslatedText;
    }

    public async Task<string> TranslateHtmlAsync(TranslationRequest request) {
        var response = await _client.TranslateHtmlAsync(
            html: request.Text,
            targetLanguage: request.Language
        );

        return response.TranslatedText;
    }
}