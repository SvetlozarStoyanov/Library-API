namespace Library.Infrastructure.Constants.CacheTimes
{
    public static class CacheTimes
    {
        public static class CachingDurationsInMinutes
        {
            public static readonly TimeSpan ShortTerm = TimeSpan.FromMinutes(10);
            public static readonly TimeSpan MediumTerm = TimeSpan.FromMinutes(30);
            public static readonly TimeSpan LongTerm = TimeSpan.FromMinutes(60);
        }        
        
        public static class CachingDurationsInHours
        {
            public static readonly TimeSpan ShortTerm = TimeSpan.FromHours(1);
            public static readonly TimeSpan MediumTerm = TimeSpan.FromHours(2);
            public static readonly TimeSpan LongTerm = TimeSpan.FromHours(10);
        }
    }
}
