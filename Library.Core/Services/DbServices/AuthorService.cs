using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Authors;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> AuthorExistsAsync(long id)
        {
            Author? author = await unitOfWork.AuthorRepository.GetByIdAsync(id);

            return author != null;
        }

        public async Task<IEnumerable<AuthorListDto>> GetAllAuthorsAsync()
        {
            List<AuthorListDto> authors = mapper.Map<List<AuthorListDto>>(await unitOfWork.AuthorRepository
                .AllReadOnlyAsync());

            return authors;
        }

        public async Task<IEnumerable<AuthorSelectDto>> GetAllAuthorsForSelectAsync()
        {
            List<AuthorSelectDto> authors = mapper.Map<List<AuthorSelectDto>>(await unitOfWork.AuthorRepository
                .AllReadOnlyAsync());

            return authors;
        }

        public async Task<AuthorDetailsDto> GetAuthorByIdAsync(long id)
        {
            AuthorDetailsDto? author = mapper.Map<AuthorDetailsDto?>(await unitOfWork.AuthorRepository
                .AllAsQueryable()
                .Where(a => a.Id == id)
                .Include(a => a.Books)
                .FirstOrDefaultAsync());

            if (author == null)
            {
                throw new NotFoundException($"Author with id {id} was not found!");
            }
            return author;
        }

        public AuthorCreateDto CreateAuthorCreateDtoAsync()
        {
            AuthorCreateDto dto = new AuthorCreateDto()
            {
                DateOfBirth = DateTime.Now
            };

            return dto;
        }

        public async Task<long> CreateAuthorAsync(AuthorCreateDto dto)
        {
            Author author = mapper.Map<Author>(dto);

            await unitOfWork.AuthorRepository.AddAsync(author);
            await unitOfWork.SaveChangesAsync();

            return author.Id;
        }

        public async Task<AuthorEditDto> CreateAuthorEditDtoAsync(long id)
        {
            AuthorEditDto? dto = mapper.Map<AuthorEditDto>(await unitOfWork.AuthorRepository.AllAsQueryable()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Author with {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateAuthorAsync(long id, AuthorEditDto dto)
        {
            Author? author = await unitOfWork.AuthorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException($"Author with id {id} was not found!");
            }

            author.FirstName = dto.FirstName;
            author.MiddleName = dto.MiddleName;
            author.LastName = dto.LastName;
            author.DateOfBirth = dto.DateOfBirth;
            author.DateOfDeath = dto.DateOfDeath;

            unitOfWork.AuthorRepository.Update(author);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAuthorByIdAsync(long id)
        {
            Author? author = await unitOfWork.AuthorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException($"Author with id {id} was not found!");
            }

            await unitOfWork.AuthorRepository.DeleteAsync(author.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
