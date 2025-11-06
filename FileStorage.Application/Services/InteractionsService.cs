using FileStorage.Application.Abstractions;
using FileStorage.Infrastructure;

namespace FileStorage.Application.Services;

public class InteractionsService : IInteractionsService
{
    private readonly HttpClient _httpClient;
    private readonly ActivityServiceSettings _activitySettings;

    public InteractionsService(HttpClient httpClient, ActivityServiceSettings settings)
    {
        _httpClient = httpClient;
        _activitySettings = settings;
    }

    public async Task<Result> StartInteraction(CancellationToken cancellation = default)
    {
        var response = await _httpClient.PostAsync(_activitySettings.StartInteractionEndpoint, null, cancellation);

        if (!response.IsSuccessStatusCode)
            return Result.Failure(Result.ToDict("Activity request", response.ReasonPhrase ?? "Something went wrong"));

        return Result.Success();
    }

    public async Task<Result> EndInteraction(CancellationToken cancellation = default)
    {
        var response = await _httpClient.PostAsync(_activitySettings.EndInteractionEndpoint, null, cancellation);

        if (!response.IsSuccessStatusCode)
            return Result.Failure(Result.ToDict("Activity request", response.ReasonPhrase ?? "Something went wrong"));

        return Result.Success();
    }
}