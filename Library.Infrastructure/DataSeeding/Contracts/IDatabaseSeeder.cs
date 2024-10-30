namespace Library.Infrastructure.DataSeeding.Contracts
{
    public interface IDatabaseSeeder
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
        Task SeedAsync();
    }
}
