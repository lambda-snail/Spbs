using MediatR;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

public class UpsertUserDetails
{
    public record Request(User User) : IRequest<Response>;
    public record Response(bool Success);

    public class UpsertUserDetailsRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IUserRepository _userRepository;

        public UpsertUserDetailsRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.User.Id == Guid.Empty)
            {
                return NewFailureResponse();
            }
            
            try
            {
                await _userRepository.UpsertUserData(request.User);
            }
            catch (Exception exception)
            {
                return NewFailureResponse();
            }

            return NewSuccessResponse();
        }

        private Response NewFailureResponse()
        {
            return new Response(Success: false);
        }

        private Response NewSuccessResponse()
        {
            return new Response(Success: true);
        }
    }
}