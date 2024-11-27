using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Configuration;
using static Utils.ITranslationService;

namespace Utils;

public interface ITranslationService {
    public readonly record struct TranslationRequest(string Text, string Language);

    Task<string> TranslateAsync(TranslationRequest request);
    Task<string> TranslateHtmlAsync(TranslationRequest request);
}

public class GoogleService : ITranslationService {
    private readonly TranslationClient _client;

    public GoogleService(IConfiguration configuration) {
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

public class AiTranslateService(ITextModelService textModel) : ITranslationService {
    public async Task<string> TranslateAsync(TranslationRequest request) {
        string prompt = @$"Translate the following text from it's source language to {request.Language}: 
                    `{request.Text}`. Ensure accuracy and natural fluency in the translation.";

        return await textModel.GetResponseAsync(prompt);
    }

    public async Task<string> TranslateHtmlAsync(TranslationRequest request) {
        string prompt = @$"
            Translate the following SSML/HTML to {request.Language}:
            {request.Text}

            - Ensure no segments remain untranslated.
            - Keep all SSML/HTML tags intact.
            - Only translate the text content.
            - Ensure the translation is natural and appropriate for audiobook narration.";

        string response = await textModel.GetResponseAsync(prompt);

        const string xmlPrefix = "```xml";
        if (response.StartsWith(xmlPrefix, StringComparison.OrdinalIgnoreCase)) {
            response = response[xmlPrefix.Length..].Trim();
        }

        return response;
    }
}
