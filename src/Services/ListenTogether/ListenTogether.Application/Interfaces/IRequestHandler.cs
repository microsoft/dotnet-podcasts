namespace ListenTogether.Application.Interfaces;

public interface IRequestHandler<in TRequest, TResult>
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}