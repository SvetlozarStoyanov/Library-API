using Library.Core.Dto.Languages;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface ILanguageService
    {
        /// <summary>
        /// Checks if <see cref="Language"/> with <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> LanguageExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Language"/> with <paramref name="code"/>
        /// exists
        /// </summary>
        /// <param name="code"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> LanguageWithCodeExists(string code);

        /// <summary>
        /// Returns all <see cref="Language"/> as <see cref="IEnumerable{T}"/> of <see cref="LanguageListDto"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<LanguageListDto>> GetAllLanguagesAsync();

        /// <summary>
        /// Returns all <see cref="Language"/> as <see cref="IEnumerable{T}"/> of <see cref="LanguageSelectDto"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<LanguageSelectDto>> GetAllLanguagesForSelectAsync();

        /// <summary>
        /// Creates a <see cref="LanguageCreateDto"/> and returns it
        /// </summary>
        /// <returns><see cref="LanguageCreateDto"/> dto</returns>
        LanguageCreateDto CreateLanguageCreateDto();

        /// <summary>
        /// Creates a <see cref="Language"/> with data from <paramref name="dto"/>
        /// and returns the <see cref="Language"/>'s <see cref="Language.Code"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="Language.Id"/></returns>
        Task<long> CreateLanguageAsync(LanguageCreateDto dto);

        /// <summary>
        /// Creates a <see cref="LanguageEditDto"/> from <see cref="Language"/>
        /// with <see cref="Language.Code"/> equal to <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="LanguageEditDto"/> dto</returns>
        Task<LanguageEditDto> CreateLanguageEditDtoAsync(long id);

        /// <summary>
        /// Updates a <see cref="Language"/> with <paramref name="id"/>
        /// with data from <paramref name="dto"/> 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateLanguageAsync(long id, LanguageEditDto dto);
    }
}
