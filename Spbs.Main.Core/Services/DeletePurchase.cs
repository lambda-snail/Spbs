using MediatR;
using Microsoft.Extensions.Logging;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class DeletePurchase
{
    public record Request(Purchase Purchase) : IRequest<Response>;
    public record Response(bool Success);

    public class DeletePurchaseRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IPurchaseRepository _repository;
        private readonly ILogger<DeletePurchaseRequestHandler> _logger;
        
        public DeletePurchaseRequestHandler(IPurchaseRepository repository, ILogger<DeletePurchaseRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Purchase.Id != Guid.Empty)
                {
                    await _repository.DeletePurchase(request.Purchase.Id);
                    return new Response(Success: true);
                }
            }
            catch (Exception e)
            {
                return new Response(Success: false);
            }
            
            return new Response(Success: false);
        }
    }
}