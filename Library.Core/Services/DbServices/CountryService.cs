using AutoMapper;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Countries;
using Library.Infrastructure.DataPersistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> PhoneCodeExistsAsync(string phoneCode)
        {
            bool phoneCodeExists = await unitOfWork.CountryRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(c => c.PhoneCode == phoneCode);

            return phoneCodeExists;
        }

        public async Task<IEnumerable<CountryListDto>> GetAllCountriesAsync()
        {
            List<CountryListDto> countries = mapper.Map<List<CountryListDto>>(
                    await unitOfWork.CountryRepository
                        .AllReadOnlyAsync());

            return countries!;
        }

        public async Task<IEnumerable<CountrySelectDto>> GetAllCountriesForSelectAsync()
        {
            List<CountrySelectDto> countries = mapper.Map<List<CountrySelectDto>>(
                    await unitOfWork.CountryRepository
                        .AllReadOnlyAsync());

            return countries!;
        }
    }
}
