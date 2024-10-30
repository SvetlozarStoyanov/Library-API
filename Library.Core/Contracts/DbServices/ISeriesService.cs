using Library.Core.Dto.Series;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface ISeriesService
    {
        /// <summary>
        /// Returns true if <see cref="Series"/> with given <paramref name="id"/>
        /// exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> SeriesExistsAsync(long id);

        /// <summary>
        /// Returns all <see cref="Series"/> as <see cref="ICollection{T}"/> of <see cref="SeriesListDto"/>
        /// </summary>
        /// <returns> <see cref="IEnumerable{T}"/> of <see cref="SeriesListDto"/></returns>
        Task<IEnumerable<SeriesListDto>> GetAllSeriesAsync();

        /// <summary>
        /// Returns all <see cref="Series"/> as <see cref="ICollection{T}"/> of <see cref="SeriesSelectDto"/>
        /// </summary>
        /// <returns> <see cref="IEnumerable{T}"/> of <see cref="SeriesSelectDto"/></returns>
        Task<IEnumerable<SeriesSelectDto>> GetAllSeriesForSelectAsync();

        /// <summary>
        /// Returns a <see cref="SeriesDetailsDto"/> of <see cref="Series"/>
        /// with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SeriesDetailsDto"/> dto</returns>
        Task<SeriesDetailsDto> GetSeriesByIdAsync(long id);

        /// <summary>
        /// Creates <see cref="SeriesCreateDto"/> and returns it
        /// </summary>
        /// <returns><see cref="SeriesCreateDto"/> dto</returns>
        SeriesCreateDto CreateSeriesCreateDtoAsync();

        /// <summary>
        /// Creates <see cref="Series"/> from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> seriesId</returns>
        Task<long> CreateSeriesAsync(SeriesCreateDto dto);

        /// <summary>
        /// Creates <see cref="SeriesEditDto"/> with data from <see cref="Series"/>
        /// with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="SeriesEditDto"/></returns>
        Task<SeriesEditDto> CreateSeriesEditDtoAsync(long id);

        /// <summary>
        /// Edits <see cref="Series"/> with given <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateSeriesAsync(long id, SeriesEditDto dto);

        /// <summary>
        /// Deletes <see cref="Series"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteSeriesByIdAsync(long id);
    }
}
