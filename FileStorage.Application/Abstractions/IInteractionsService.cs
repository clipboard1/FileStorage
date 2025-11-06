using FileStorage.Infrastructure;

namespace FileStorage.Application.Abstractions;

public interface IInteractionsService
{
    public Task<Result> StartInteraction(CancellationToken cancellation = default);
    public Task<Result> EndInteraction(CancellationToken cancellation = default);
}