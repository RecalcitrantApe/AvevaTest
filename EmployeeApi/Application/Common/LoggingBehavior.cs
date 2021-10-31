using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeApi.Application.Common
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestName = request.GetType().Name;
            var requestGuid = Guid.NewGuid().ToString();

            var requestNameWithGuid = $"{requestName} [{requestGuid}]";

            this.logger.LogInformation($"Starting request: {requestNameWithGuid}");
            TResponse response;

            try
            {
                try
                {
                    this.logger.LogInformation($"Properties: {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
                }
                catch (NotSupportedException)
                {
                    this.logger.LogInformation($"Serialization error: {requestNameWithGuid} Could not serialize the request.");
                }

                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                this.logger.LogInformation($"Ending request {requestNameWithGuid}; Execution time: {stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}

