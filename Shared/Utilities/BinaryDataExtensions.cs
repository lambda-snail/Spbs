namespace Shared.Utilities;

public static class BinaryDataExtensions
{
    public static bool TryParseAsOBject<T>(this BinaryData data, out T? obj)
    {
        try
        {
            obj = data.ToObjectFromJson<T>();
            return true;
        }
        catch (Exception e)
        {
            obj = default;
            return false;
        }
    }
}