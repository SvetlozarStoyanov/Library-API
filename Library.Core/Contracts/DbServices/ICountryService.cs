using Library.Core.Dto.Countries;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface ICountryService
    {
        /// <summary>
        /// Checks if <see cref="Country"/> with <see cref="Country.PhoneCode"/>
        /// equal to <paramref name="phoneCode"/> exists
        /// </summary>
        /// <param name="phoneCode"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> PhoneCodeExistsAsync(string phoneCode);

        /// <summary>
        /// Returns all <see cref="Country"/> with their <see cref="Country.PhoneCode"/>
        /// as <see cref="IEnumerable{T}"/> of <see cref="CountryListDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="CountryListDto"/></returns>
        Task<IEnumerable<CountryListDto>> GetAllCountriesAsync();

        /// <summary>
        /// Returns all <see cref="Country"/> with their <see cref="Country.PhoneCode"/>
        /// as <see cref="IEnumerable{T}"/> of <see cref="CountrySelectDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="CountrySelectDto"/></returns>
        Task<IEnumerable<CountrySelectDto>> GetAllCountriesForSelectAsync();
    }
}
