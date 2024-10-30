namespace Library.Infrastructure.Constants.CacheKeys
{
    public static class CacheKeys
    {
        public static class AuthorCacheKeys
        {
            public const string All = "authors-all";
            public const string ById = "authors-{0}";
        }

        public static class BookCacheKeys
        {
            public const string All = "books-all";
            public const string ById = "books-{0}";
        }

        public static class CountryCacheKeys
        {
            public const string All = "countries-all";
        }

        public static class GenreCacheKeys
        {
            public const string All = "genre-all";
            public const string ById = "genre-{0}";
        }

        public static class SeriesCacheKeys
        {
            public const string All = "series-all";
            public const string ById = "series-{0}";
        }

        public static class LanguageCacheKeys
        {
            public const string All = "languages-all";
            public const string ById = "languages-{0}";
        }
    }
}
