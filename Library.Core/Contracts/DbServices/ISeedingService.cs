namespace Library.Core.Contracts.DbServices
{
    public interface ISeedingService
    {
        /// <summary>
        /// Checks if the database is seeded.
        /// </summary>
        /// <returns><see cref="bool"/> isSeeded</returns>
        Task<bool> CheckDatabaseIsSeededAsync();

        /// <summary>
        /// Seeds database
        /// </summary>
        /// <returns></returns>
        Task SeedDatabaseAsync();
    }
}
