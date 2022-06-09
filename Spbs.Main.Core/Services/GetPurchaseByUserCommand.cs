using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class GetPurchaseByUserCommand
{
    public record Request(string UserId, DateTime? Since = null, DateTime? Until = null) : IRequest<Response>;
    public record Response(bool Success, List<Purchase>? Purchases);

    public class GetPurchaseByUserRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<GetPurchaseByUserRequestHandler> _logger;
        private readonly IPurchaseRepository _repository;

        public GetPurchaseByUserRequestHandler(IPurchaseRepository repository, ILogger<GetPurchaseByUserRequestHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                List<Purchase> purchases = await _repository.GetPurchasesOfUser(request.UserId);
                return new Response(Success: true, Purchases: purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while fetching purchases of user {0}: {1}", request.UserId, ex.Message);
                return new Response(Success: false, Purchases: null);
            }
        }
    }
}