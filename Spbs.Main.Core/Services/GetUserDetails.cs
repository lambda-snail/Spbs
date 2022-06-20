using MediatR;
using Spbs.Main.Core.Contracts;
using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Services;

/// <summary>
/// Representes a request for user details from mongodb. The user id is the same as in the identity database.
/// The response will have Success flag set to false if something went wrong and true otherwise. This means that even
/// if the user details document is not found, the operation can still be a success as long as no exception was thrown.
///
/// The caller should thus first check the Success flag to see if an error has occured, and then another check to see
/// if any data has been returned should be performed. 
/// </summary>
public class GetUserDetails
{
    public record Request(Guid UserId) : IRequest<Response>;
    public record Response(bool Success, User? User);

    public class GetUserDetailsRequestHandler : IRequestHandler<Request, Response>
    {
        private readonly IUserRepository _userRepository;

        public GetUserDetailsRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            User? user = null;
            try
            {
                user = await _userRepository.GetUserById(request.UserId);
            }
            catch (Exception exception)
            {
                return new Response(Success: false, User: null);
            }

            return new Response(Success: true, User : user);
        }
    }
}