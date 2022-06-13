using MediatR;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class GetPurchaseById
{
    public record Request(Guid PurchaseId) : IRequest<Response>;
    public record Response(Purchase Purchase);

    public class GetPurchaseByIdRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IPurchaseRepository _repository;

        public GetPurchaseByIdRequestHandler(IPurchaseRepository _repository)
        {
            this._repository = _repository;
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Purchase purchase = await _repository.GetPurchaseById(request.PurchaseId);
            return new Response(purchase);
        }
    }
}
