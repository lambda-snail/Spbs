using MediatR;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class GetUserLocationContext
{
    public record Request() : IRequest<Response>;
    public record Response(bool Success, IDictionary<Guid, Location> Locations);

    public class RequestHandler : IRequestHandler<Request, Response>
    {
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly ILocationRepository _locationRepository;

        public RequestHandler(ILoggedInUserService loggedInUserService, ILocationRepository locationRepository)
        {
            _loggedInUserService = loggedInUserService;
            _locationRepository = locationRepository;
        }
        
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Guid userId = await _loggedInUserService.GetLoggedInUserId();
            if (userId == Guid.Empty) { return GetFailureResponse(); }

            IDictionary<Guid, Location> locations = await _locationRepository.GetAllLocations(userId);

            return new Response(Success: true, Locations: locations);
        }

        private Response GetFailureResponse()
        {
            return new Response(Success: false, Locations: new Dictionary<Guid, Location>());
        }
    }
}