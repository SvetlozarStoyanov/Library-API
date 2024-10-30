using Library.Core.Contracts.DbServices;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.DataSeeding.Contracts;

namespace Library.Core.Services.DbServices
{
    public class SeedingService : ISeedingService
    {
        private readonly IDatabaseSeeder databaseSeeder;
        private readonly IUnitOfWork unitOfWork;

        public SeedingService(IUnitOfWork unitOfWork, IDatabaseSeeder databaseSeeder)
        {
            this.unitOfWork = unitOfWork;
            this.databaseSeeder = databaseSeeder;
        }

        public async Task<bool> CheckDatabaseIsSeededAsync()
        {
            return await databaseSeeder.CheckDatabaseIsSeededAsync();
        }

        public async Task SeedDatabaseAsync()
        {
            await databaseSeeder.SeedAsync();
        }
    }
}
