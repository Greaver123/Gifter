namespace Gifter.Common.Options
{
    public class StoreOptions
    {
        public const string Store = "Store";

        public string BaseDirectory { get; set; }

        public int UserStoreMaxSize { get; set; }

        public int FileMaxSize { get; set; }
    }
}
