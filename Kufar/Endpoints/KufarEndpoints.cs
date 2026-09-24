namespace Kufar;

public partial class KufarHandler
{
    public static class KufarEndpoints
    {
        public const string V1Counter = "messaging-api/v1/counter";
        public const string V4Conversations = "messaging-api/v4/conversations";
        public const string SavedSearchHasNewItems = "saved-search/v2/accounts/searches/has-new-items";

        public static string V4ConversationsWithPaging(int limit, int offset = 0) =>
            $"{V4Conversations}?limit={limit}&offset={offset}";
    }
}
