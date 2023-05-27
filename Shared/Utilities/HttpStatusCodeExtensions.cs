using System.Net;

namespace Shared.Utilities;

public static class HttpStatusCodeExtensions
{
    public static bool IsSuccessStatusCode(this HttpStatusCode code)
    {
        return code >= HttpStatusCode.OK && code <= (HttpStatusCode)299;

    }
}