using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Authors;
using Library.Core.Dto.Books;
using Library.Core.Dto.Genres;
using Library.Core.Dto.Series;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Library.Core.Services.DbServices
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> BookExistsAsync(long id)
        {
            Book? book = await unitOfWork.BookRepository.GetByIdAsync(id);

            return book != null;
        }

        public async Task<bool> BookQuantityIsSufficientAsync(long id, int quantity)
        {
            Book? book = await unitOfWork.BookRepository.AllAsQueryable().AsNoTracking()
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found!");
            }

            return book.Quantity >= quantity;
        }

        public async Task<IEnumerable<BookListDto>> GetFilteredBooksAsync(BooksFilterDto dto)
        {
            if (dto.Page < 1 || dto.ItemsPerPage < 1)
            {
                throw new ArgumentException("Invalid page parameters.");
            }

            #region Filter in service
            //IQueryable<Book> books = unitOfWork.BookRepository
            //    .AllAsQueryable().AsNoTracking();

            //if (!string.IsNullOrEmpty(dto.SearchTerm))
            //{
            //    books = books
            //        .Where(b => b.Title.ToLower().Contains(dto.SearchTerm.ToLower()));
            //}

            //if (dto.AuthorIds != null && dto.AuthorIds.Count() > 0)
            //{
            //    books = books
            //        .Where(b => dto.AuthorIds.Any(aId => b.Authors.Select(a => a.Id).Contains(aId)));
            //}

            //if (dto.GenreIds != null && dto.GenreIds.Count() > 0)
            //{
            //    books = books
            //        .Where(b => dto.GenreIds.Any(gId => b.Genres.Select(g => g.Id).Contains(gId)));
            //}

            //if (dto.SeriesIds != null && dto.SeriesIds.Count() > 0)
            //{
            //    books = books
            //        .Where(b => dto.SeriesIds.Any(sId => b.Series.Select(s => s.Id).Contains(sId)));
            //}

            //if (dto.CountryIds != null && dto.CountryIds.Count() > 0)
            //{
            //    books = books
            //        .Where(b => dto.CountryIds.Contains(b.CountryId));
            //}

            //books = books
            //    .Skip(dto.ItemsPerPage * (dto.Page - 1))
            //    .Take(dto.ItemsPerPage);

            //List<BookListDto> resultBooks = mapper.Map<List<BookListDto>>(await books
            //    .Include(b => b.Authors)
            //    .Include(b => b.Genres)
            //    .Include(b => b.Series)
            //    .ToListAsync());
            #endregion

            #region Filter via repository
            List<Expression<Func<Book, bool>>> filters = new List<Expression<Func<Book, bool>>>();

            if (!string.IsNullOrEmpty(dto.SearchTerm))
            {
                filters.Add(b => b.Title.ToLower().Contains(dto.SearchTerm.ToLower()));
            }

            if (dto.AuthorIds != null && dto.AuthorIds.Count() > 0)
            {
                filters.Add(b => dto.AuthorIds.Any(aId => b.Authors.Select(a => a.Id).Contains(aId)));
            }

            if (dto.GenreIds != null && dto.GenreIds.Count() > 0)
            {
                filters.Add(b => dto.GenreIds.Any(gId => b.Genres.Select(g => g.Id).Contains(gId)));
            }

            if (dto.SeriesIds != null && dto.SeriesIds.Count() > 0)
            {
                filters.Add(b => dto.SeriesIds.Any(sId => b.Series.Select(s => s.Id).Contains(sId)));
            }

            if (dto.CountryIds != null && dto.CountryIds.Count() > 0)
            {
                filters.Add(b => dto.CountryIds.Contains(b.CountryId));
            }

            IReadOnlyCollection<Book> books = await unitOfWork.BookRepository.AllReadOnlyAsync(filters);
            List<BookListDto> resultBooks = mapper.Map<List<BookListDto>>(books
                .Skip(dto.ItemsPerPage * (dto.Page - 1))
                .Take(dto.ItemsPerPage)
                .ToList());
            #endregion


            return resultBooks;
        }

        public async Task<IEnumerable<BookListDto>> GetAllBooksAsync()
        {
            List<BookListDto> books = mapper.Map<List<BookListDto>>(await unitOfWork.BookRepository
                         .AllReadOnlyAsync());

            return books;
        }

        public async Task<BookDetailsDto> GetBookByIdAsync(long id)
        {
            BookDetailsDto? book = mapper.Map<BookDetailsDto?>(await unitOfWork.BookRepository
                .AllReadOnlyAsync(b => b.Id == id));

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found!");
            }

            return book;
        }

        public BookCreateDto CreateBookCreateDtoAsync()
        {
            BookCreateDto dto = new BookCreateDto()
            {
                PublicationDate = DateTime.Now
            };

            return dto;
        }

        public async Task<long> CreateBookAsync(BookCreateDto dto)
        {
            if (dto.ISBN != null)
            {
                dto.ISBN = TrimISBN(dto.ISBN);
                if (!ISBNIsValid(dto.ISBN))
                {
                    throw new ArgumentException("ISBN is invalid!");
                }
                if (await BookWithISBNAlreadyExistsAsync(dto.ISBN, null))
                {
                    throw new ArgumentException("ISBN already exists!");
                }
            }

            Language? language = await unitOfWork.LanguageRepository.GetByIdAsync(dto.LanguageId);

            if (language == null)
            {
                throw new NotFoundException($"Language with id {dto.LanguageId} was not found!");
            }

            Country? country = await unitOfWork.CountryRepository.GetByIdAsync(dto.CountryId);

            if (country == null)
            {
                throw new NotFoundException($"Country with id {dto.LanguageId} was not found!");
            }

            Book book = mapper.Map<Book>(dto);

            BookAcquisition acquisition = new BookAcquisition();
            acquisition.Quantity = dto.Quantity;
            acquisition.Time = DateTime.Now;
            acquisition.Type = BookAcquisitionType.Initial;

            book.Acquisitions.Add(acquisition);
            await unitOfWork.BookRepository.AddAsync(book);
            await unitOfWork.SaveChangesAsync();
            return book.Id;
        }

        public BookExperimentalCreateDto CreateBookExperimentalCreateDto()
        {
            BookExperimentalCreateDto dto = new BookExperimentalCreateDto()
            {
                PublicationDate = DateTime.Now,
                Authors = new HashSet<AuthorCreateDto>() { new AuthorCreateDto() },
                Genres = new HashSet<GenreCreateDto>() { new GenreCreateDto() },
                Series = new HashSet<SeriesCreateDto>() { new SeriesCreateDto() }
            };

            return dto;
        }

        public async Task<long> ExperimentalCreateBookAsync(BookExperimentalCreateDto dto)
        {
            if (dto.ISBN != null)
            {
                dto.ISBN = TrimISBN(dto.ISBN);
                if (!ISBNIsValid(dto.ISBN))
                {
                    throw new ArgumentException("ISBN is invalid!");
                }
                if (await BookWithISBNAlreadyExistsAsync(dto.ISBN, null))
                {
                    throw new ArgumentException("ISBN already exists!");
                }
            }

            Language? language = await unitOfWork.LanguageRepository.GetByIdAsync(dto.LanguageId);

            if (language == null)
            {
                throw new NotFoundException($"Language with id {dto.LanguageId} was not found!");
            }

            Country? country = await unitOfWork.CountryRepository.GetByIdAsync(dto.CountryId);

            if (country == null)
            {
                throw new NotFoundException($"Country with id {dto.CountryId} was not found!");
            }

            Book book = mapper.Map<Book>(dto);

            await AddBookAuthorsAsync(book, dto.Authors.DistinctBy(a => $"{a.FirstName} {(a.MiddleName != null ? $"{a.MiddleName} " : "")}{a.LastName}".ToLower()));
            await AddBookGenresAsync(book, dto.Genres.DistinctBy(a => a.Name.ToLower()));
            await AddBookSeriesAsync(book, dto.Series.DistinctBy(a => a.Title.ToLower()));
            BookAcquisition delivery = new BookAcquisition();
            delivery.Quantity = dto.Quantity;
            delivery.Time = DateTime.Now;
            delivery.Type = BookAcquisitionType.Initial;

            await unitOfWork.BookRepository.AddAsync(book);
            await unitOfWork.SaveChangesAsync();
            return book.Id;
        }

        public async Task<BookEditDto> CreateBookEditDtoAsync(long id)
        {
            BookEditDto? book = mapper.Map<BookEditDto>(await unitOfWork.BookRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync());

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found!");
            }

            return book;
        }

        public async Task UpdateBookAsync(long id, BookEditDto dto)
        {
            if (dto.ISBN != null)
            {
                dto.ISBN = TrimISBN(dto.ISBN);
                if (!ISBNIsValid(dto.ISBN))
                {
                    throw new ArgumentException("ISBN is invalid!");
                }
                if (await BookWithISBNAlreadyExistsAsync(dto.ISBN, id))
                {
                    throw new ArgumentException("ISBN already exists!");
                }
            }

            Book? book = await unitOfWork.BookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found!");
            }

            Language? language = await unitOfWork.LanguageRepository.GetByIdAsync(dto.LanguageId);

            if (language == null)
            {
                throw new NotFoundException($"Language with id {dto.LanguageId} was not found!");
            }

            Country? country = await unitOfWork.CountryRepository.GetByIdAsync(dto.CountryId);

            if (country == null)
            {
                throw new NotFoundException($"Country with id {dto.CountryId} was not found!");
            }

            book.Title = dto.Title;
            book.ISBN = dto.ISBN;
            book.Description = dto.Description;
            book.PageCount = dto.PageCount;
            book.LanguageId = dto.LanguageId;
            book.CountryId = dto.CountryId;
            if (dto.PublicationDate != null)
            {
                try
                {
                    book.PublicationDate = dto.PublicationDate;
                }
                catch (Exception e)
                {
                    throw new InvalidDataException("Date is invalid!");
                }
            }
            else
            {
                book.PublicationDate = null;
            }
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ArchiveBookByIdAsync(long id)
        {
            Book? book = await unitOfWork.BookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new NotFoundException($"Book with id {id} was not found!");
            }

            book.Status = BookStatus.Archived;
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.SaveChangesAsync();
        }

        private bool ISBNIsValid(string isbn)
        {
            Regex isbnRegex = new Regex(@"^[0-9]{10}$|^[0-9]{13}$");
            return isbnRegex.IsMatch(isbn);
        }

        private static string TrimISBN(string isbn)
        {
            isbn = isbn.Replace("-", string.Empty);
            isbn = isbn.Replace(" ", string.Empty);
            return isbn;
        }

        private async Task<bool> BookWithISBNAlreadyExistsAsync(string isbn, long? id)
        {
            if (id != null)
            {
                return await unitOfWork.BookRepository.AllAsQueryable().AsNoTracking().AnyAsync(b => b.ISBN == isbn && b.Id != id);
            }
            return await unitOfWork.BookRepository.AllAsQueryable().AsNoTracking().AnyAsync(b => b.ISBN == isbn);
        }

        private async Task AddBookAuthorsAsync(Book book, IEnumerable<AuthorCreateDto> authorDtos)
        {
            List<Author> authors = await unitOfWork.AuthorRepository.AllAsQueryable().ToListAsync();
            foreach (AuthorCreateDto authorDto in authorDtos)
            {
                DateTime dateOfBirth = authorDto.DateOfBirth;

                Author? foundAuthor = authors
                    .FirstOrDefault(a => $"{a.FirstName} {(a.MiddleName != null ? $"{a.MiddleName} " : "")}{a.LastName}".ToLower() == $"{authorDto.FirstName} {(authorDto.MiddleName != null ? $"{authorDto.MiddleName} " : "")}{authorDto.LastName}".ToLower()
                    && a.DateOfBirth == dateOfBirth);

                if (foundAuthor != null)
                {
                    book.Authors.Add(foundAuthor);
                }
                else
                {
                    Author newAuthor = mapper.Map<Author>(authorDto);
                    book.Authors.Add(newAuthor);
                }
            }
        }
        private async Task AddBookGenresAsync(Book book, IEnumerable<GenreCreateDto> genreDtos)
        {
            List<Genre> genres = await unitOfWork.GenreRepository.AllAsQueryable().ToListAsync();
            foreach (GenreCreateDto genreDto in genreDtos)
            {
                Genre? foundGenre = genres.FirstOrDefault(a => a.Name.ToLower() == genreDto.Name.ToLower());
                if (foundGenre != null)
                {
                    book.Genres.Add(foundGenre);
                }
                else
                {
                    Genre? newGenre = mapper.Map<Genre>(genreDto);
                    book.Genres.Add(newGenre);
                }
            }
        }

        private async Task AddBookSeriesAsync(Book book, IEnumerable<SeriesCreateDto> seriesDtos)
        {
            List<Series> series = await unitOfWork.SeriesRepository.AllAsQueryable().ToListAsync();
            foreach (SeriesCreateDto seriesDto in seriesDtos)
            {
                Series? foundSeries = series.FirstOrDefault(a => a.Title.ToLower() == seriesDto.Title.ToLower());
                if (foundSeries != null)
                {
                    book.Series.Add(foundSeries);
                }
                else
                {
                    Series newSeries = mapper.Map<Series>(seriesDto);
                    book.Series.Add(newSeries);
                }
            }
        }
    }
}
