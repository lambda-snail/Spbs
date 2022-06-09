using MediatR;
using Microsoft.Extensions.Logging;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class InsertPurchase
{
    public record Request(Purchase Purchase) : IRequest<Response>;
    public record Response(bool Success);

    public class InsertPurchaseRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IPurchaseRepository _repository;
        private readonly ILogger<InsertPurchaseRequestHandler> _logger;
        
        public InsertPurchaseRequestHandler(IPurchaseRepository repository, ILogger<InsertPurchaseRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Purchase.OwnerId) || string.IsNullOrWhiteSpace(request.Purchase.OwnerId) )
            {
                return new Response(Success: false);
            }

            try
            {
                await _repository.InsertPurchase(request.Purchase);
            }
            catch (Exception e)
            {
                return new Response(Success: false);
            }
            
            return new Response(Success: true);
        }
    }
}