using FileStorage.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileStorage.API;

public class InteractionFilter : IAsyncActionFilter
{
    private readonly IInteractionsService _interactionsService;

    public InteractionFilter(IInteractionsService interactionsService)
    {
        _interactionsService = interactionsService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cancellation = context.HttpContext.RequestAborted;

        await _interactionsService.StartInteraction(cancellation);

        try
        {
            var executed = await next();
        }
        finally
        {
            await _interactionsService.EndInteraction(cancellation);
        }
    }
}

public class InteractionAttribute : TypeFilterAttribute
{
    public InteractionAttribute() : base(typeof(InteractionFilter))
    {
    }
}