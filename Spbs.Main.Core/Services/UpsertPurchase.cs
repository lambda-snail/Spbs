using MediatR;
using Microsoft.Extensions.Logging;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class UpsertPurchase
{
    public record Request(Purchase Purchase) : IRequest<Response>;
    public record Response(bool Success);

    public class UpsertPurchaseRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IPurchaseRepository _repository;
        private readonly ILogger<UpsertPurchaseRequestHandler> _logger;
        
        public UpsertPurchaseRequestHandler(IPurchaseRepository repository, ILogger<UpsertPurchaseRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Purchase.OwnerId))
            {
                return new Response(Success: false);
            }

            try
            {
                if (request.Purchase.Id == Guid.Empty)
                {
                    await _repository.InsertPurchase(request.Purchase);                    
                }
                else
                {
                    await _repository.UpdatePurchase(request.Purchase);
                }
            }
            catch (Exception e)
            {
                return new Response(Success: false);
            }
            
            return new Response(Success: true);
        }
    }
}