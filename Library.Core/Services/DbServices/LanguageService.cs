using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Languages;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LanguageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> LanguageExistsAsync(long id)
        {
            Language? language = await unitOfWork.LanguageRepository.GetByIdAsync(id);

            return language != null;
        }

        public async Task<bool> LanguageWithCodeExists(string code)
        {
            bool languageExists = await unitOfWork.LanguageRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(l => l.Code == code);

            return languageExists;
        }

        public async Task<IEnumerable<LanguageListDto>> GetAllLanguagesAsync()
        {
            List<LanguageListDto> languages = mapper.Map<List<LanguageListDto>>(await unitOfWork.LanguageRepository
                .AllReadOnlyAsync());

            return languages;
        }

        public async Task<IEnumerable<LanguageSelectDto>> GetAllLanguagesForSelectAsync()
        {
            List<LanguageSelectDto> languages = mapper.Map<List<LanguageSelectDto>>(await unitOfWork.LanguageRepository
                .AllReadOnlyAsync());

            return languages;
        }

        public LanguageCreateDto CreateLanguageCreateDto()
        {
            LanguageCreateDto dto = new LanguageCreateDto();

            return dto;
        }

        public async Task<long> CreateLanguageAsync(LanguageCreateDto dto)
        {
            Language language = mapper.Map<Language>(dto);
            language.Code = language.Code.ToLower();

            if (await LanguageCodeExistsAsync(language.Code))
            {
                throw new UniqueConstraintViolationException("Language code already exists!");
            }

            await unitOfWork.LanguageRepository.AddAsync(language);
            await unitOfWork.SaveChangesAsync();

            return language.Id;
        }

        public async Task<LanguageEditDto> CreateLanguageEditDtoAsync(long id)
        {
            LanguageEditDto? dto = mapper.Map<LanguageEditDto?>(await unitOfWork.LanguageRepository
                .GetByIdAsync(id));

            if (dto == null)
            {
                throw new NotFoundException($"Language with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateLanguageAsync(long id, LanguageEditDto dto)
        {
            Language? language = await unitOfWork.LanguageRepository.GetByIdAsync(id);

            if (language == null)
            {
                throw new NotFoundException($"Language with id {id} not found!");
            }

            language.Name = dto.Name;
            language.Code = dto.Code.ToLower();

            if (await LanguageCodeExistsAsync(dto.Code))
            {
                throw new UniqueConstraintViolationException("Language code already exists!");
            }

            unitOfWork.LanguageRepository.Update(language);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> LanguageCodeExistsAsync(string code)
        {
            bool languageCodeExists = await unitOfWork.LanguageRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(l => l.Code == code);

            return languageCodeExists;
        }
    }
}
