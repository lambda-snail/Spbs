using Microsoft.AspNetCore.Http;
using Serilog;
using Spbs.Ui.Auth;

namespace Spbs.Ui.Middleware;

public class UserInformationLogEnricher
{
    public static void PushSeriLogProperties(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        diagnosticContext.Set("UserId", httpContext.User?.GetUserId());
    }
}