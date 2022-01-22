using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Blog.API.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Starting request of type: {RequestType}", typeof(TRequest).Name);
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError("Processing: {RequestType} - throws exception: {Message}", typeof(TRequest).Name, ex.Message);
                throw;
            }
            finally
            {
                _logger.LogInformation("Processing: {RequestType} - ended", typeof(TRequest).Name);
            }
        }
    }
}
