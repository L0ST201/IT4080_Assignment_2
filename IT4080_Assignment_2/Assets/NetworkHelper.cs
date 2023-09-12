public static class NetworkHelper
{
    private static ulong? currentHostId = null;

    public static void SetHostId(ulong hostId)
    {
        currentHostId = hostId;
    }

    public static bool IsClientAlsoHost(ulong clientId)
    {
        if (!currentHostId.HasValue)
            return false;

        return clientId == currentHostId.Value;
    }
}
