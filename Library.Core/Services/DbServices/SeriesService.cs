using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Series;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class SeriesService : ISeriesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SeriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> SeriesExistsAsync(long id)
        {
            Series? series = await unitOfWork.SeriesRepository.GetByIdAsync(id);

            return series != null;
        }

        public async Task<IEnumerable<SeriesListDto>> GetAllSeriesAsync()
        {
            List<SeriesListDto> series = mapper.Map<List<SeriesListDto>>(await unitOfWork.SeriesRepository
                .AllReadOnlyAsync());

            return series;
        }

        public async Task<IEnumerable<SeriesSelectDto>> GetAllSeriesForSelectAsync()
        {
            List<SeriesSelectDto> series = mapper.Map<List<SeriesSelectDto>>(await unitOfWork.SeriesRepository
                .AllReadOnlyAsync());

            return series;
        }

        public async Task<SeriesDetailsDto> GetSeriesByIdAsync(long id)
        {
            SeriesDetailsDto? series = mapper.Map<SeriesDetailsDto?>(await unitOfWork.SeriesRepository
                .AllAsQueryable().AsNoTracking()
                .Where(s => s.Id == id)
                .Include(s => s.Books)
                .FirstOrDefaultAsync());

            if (series == null)
            {
                throw new NotFoundException($"Series with id {id} was not found!");
            }

            return series;
        }

        public SeriesCreateDto CreateSeriesCreateDtoAsync()
        {
            SeriesCreateDto dto = new SeriesCreateDto();

            return dto;
        }

        public async Task<long> CreateSeriesAsync(SeriesCreateDto dto)
        {
            Series? series = mapper.Map<Series>(dto);

            await unitOfWork.SeriesRepository.AddAsync(series);
            await unitOfWork.SaveChangesAsync();
            return series.Id;
        }

        public async Task<SeriesEditDto> CreateSeriesEditDtoAsync(long id)
        {
            SeriesEditDto? dto = mapper.Map<SeriesEditDto?>(await unitOfWork.SeriesRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Series with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateSeriesAsync(long id, SeriesEditDto dto)
        {
            Series? series = await unitOfWork.SeriesRepository.GetByIdAsync(id);

            if (series == null)
            {
                throw new NotFoundException($"Series with id {id} was not found!");
            }

            series.Title = dto.Title;
            series.Description = dto.Description;

            unitOfWork.SeriesRepository.Update(series);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSeriesByIdAsync(long id)
        {
            Series? series = await unitOfWork.SeriesRepository.GetByIdAsync(id);

            if (series == null)
            {
                throw new NotFoundException($"Series with id {id} was not found!");
            }

            await unitOfWork.SeriesRepository.DeleteAsync(series.Id);
            await unitOfWork.SaveChangesAsync();
        }


    }
}
