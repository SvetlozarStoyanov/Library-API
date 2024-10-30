using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Genres;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> GenreExistsAsync(long id)
        {
            Genre? genre = await unitOfWork.GenreRepository.GetByIdAsync(id);

            return genre != null;
        }

        public async Task<IEnumerable<GenreListDto>> GetAllGenresAsync()
        {
            List<GenreListDto> genres = mapper.Map<List<GenreListDto>>(await unitOfWork.GenreRepository
                .AllReadOnlyAsync());

            return genres;
        }

        public async Task<IEnumerable<GenreSelectDto>> GetAllGenresForSelectAsync()
        {
            List<GenreSelectDto> genres = mapper.Map<List<GenreSelectDto>>(await unitOfWork.GenreRepository
                .AllReadOnlyAsync());

            return genres;
        }

        public async Task<GenreDetailsDto> GetGenreByIdAsync(long id)
        {
            GenreDetailsDto? genre = mapper.Map<GenreDetailsDto?>(await unitOfWork.GenreRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Include(g => g.Books)
                .FirstOrDefaultAsync());

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id {id} was not found!");
            }

            return genre;
        }

        public GenreCreateDto CreateGenreCreateDtoAsync()
        {
            GenreCreateDto dto = new GenreCreateDto();

            return dto;
        }

        public async Task<long> CreateGenreAsync(GenreCreateDto dto)
        {
            bool nameAlreadyExists = await GenreNameIsTakenAsync(dto.Name, null);

            if (nameAlreadyExists)
            {
                throw new InvalidOperationException($"Genre name - {dto.Name} is already taken!");
            }
            Genre genre = mapper.Map<Genre>(dto);
            await unitOfWork.GenreRepository.AddAsync(genre);
            await unitOfWork.SaveChangesAsync();
            return genre.Id;
        }

        public async Task<GenreEditDto> CreateGenreEditDtoAsync(long id)
        {
            GenreEditDto? genre = mapper.Map<GenreEditDto?>(await unitOfWork.GenreRepository.AllAsQueryable().AsNoTracking()
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync());

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id {id} was not found!");
            }

            return genre;
        }

        public async Task UpdateGenreAsync(long id, GenreEditDto dto)
        {
            Genre? genre = await unitOfWork.GenreRepository.AllAsQueryable()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id {id} was not found!");
            }

            bool nameAlreadyExists = await GenreNameIsTakenAsync(dto.Name, id);

            if (nameAlreadyExists)
            {
                throw new UniqueConstraintViolationException($"Genre name - {dto.Name} is already taken!");
            }

            genre.Name = dto.Name;
            genre.Description = dto.Description;
            unitOfWork.GenreRepository.Update(genre);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteGenreByIdAsync(long id)
        {
            Genre? genre = await unitOfWork.GenreRepository.GetByIdAsync(id);

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id {id} was not found!");
            }

            await unitOfWork.GenreRepository.DeleteAsync(genre.Id);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> GenreNameIsTakenAsync(string name, long? id)
        {
            if (id != null)
            {
                return await unitOfWork.GenreRepository.AllAsQueryable().AsNoTracking().AnyAsync(b => b.Name.ToLower() == name.ToLower() && b.Id != id);
            }
            return await unitOfWork.GenreRepository.AllAsQueryable().AsNoTracking().AnyAsync(b => b.Name.ToLower() == name.ToLower());
        }

    }
}
