//using AutoMapper;
//using Library.Core.Automapper.Config;
//using Library.Core.Contracts;
//using Library.Core.CustomExceptions;
//using Library.Core.Dto.Genres;
//using Library.Core.Services;
//using Library.Infrastructure;
//using Library.Infrastructure.DataPersistence.Contracts;
//using Library.Infrastructure.DataPersistence.UnitOfWork;
//using Library.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Library.Tests.UnitTests
//{
//    [TestFixture]
//    public class GenreServiceTests
//    {
//        private LibraryDbContext dbContext;
//        private IUnitOfWork unitOfWork;
//        private IGenreService genreService;
//        private IMapper mapper;
//        [OneTimeSetUp]
//        public async Task OneTimeSetup()
//        {
//            DbContextOptions<LibraryDbContext> contextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
//               .UseInMemoryDatabase("LibraryTestDb")
//                .Options;

//            dbContext = new LibraryDbContext(contextOptions, false);
//            dbContext.Database.EnsureDeleted();
//            dbContext.Database.EnsureCreated();

//            unitOfWork = new UnitOfWork(dbContext);
//            await SeedDatabase();

//            MapperConfiguration config = new MapperConfiguration(cfg =>
//            {
//                AutoMapperConfig.RegisterMappings(cfg);
//            });
//            mapper = config.CreateMapper();

//            genreService = new GenreService(unitOfWork, mapper);
//        }

//        [OneTimeTearDown]
//        public async Task OneTimeTeardown()
//        {
//            dbContext.Dispose();
//        }

//        [Test]
//        [Order(0)]
//        public async Task Test_GetAllGenresAsync_ReturnsAllGenres()
//        {
//            IEnumerable<GenreListDto> result = await genreService.GetAllGenresAsync();
//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        [Order(1)]
//        public async Task Test_GenreExistsAsync_ReturnsTrueWhenGenreExists()
//        {
//            bool result = await genreService.GenreExistsAsync(1);
//            Assert.That(result, Is.EqualTo(true));
//        }

//        [Test]
//        [Order(2)]
//        public async Task Test_GenreExistsAsync_ReturnsFalseWhenGenreDoesNotExist()
//        {
//            bool result = await genreService.GenreExistsAsync(384);
//            Assert.That(result, Is.EqualTo(false));
//        }

//        [Test]
//        [Order(3)]
//        public async Task Test_GetGenreByIdAsync_ReturnsCorrectGenreWhenItExists()
//        {
//            GenreDetailsDto result = await genreService.GetGenreByIdAsync(2);
//            Assert.That(result.Name, Is.EqualTo("Fantasy"));
//        }

//        [Test]
//        [Order(4)]
//        public async Task Test_GetGenreByIdAsync_ThrowsExceptionWhenGenreDoesNotExist()
//        {
//            Assert.That(async () => await genreService.GetGenreByIdAsync(26666), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        [Test]
//        [Order(5)]
//        public async Task Test_CreateGenreCreateDtoAsync_CreatesDtoCorrectly()
//        {
//            GenreCreateDto result = genreService.CreateGenreCreateDtoAsync();
//            Assert.That(result, Is.Not.Null);
//        }

//        [Test]
//        [Order(6)]
//        public async Task Test_CreateGenreAsync_CreatesGenreCorrectly()
//        {
//            IEnumerable<GenreListDto> allGenres = await genreService.GetAllGenresAsync();

//            int oldGenreCount = allGenres.Count();
//            GenreCreateDto createDto = new GenreCreateDto()
//            {
//                Name = "Comedy",
//                Description = "Description"
//            };
//            await genreService.CreateGenreAsync(createDto);
//            allGenres = await genreService.GetAllGenresAsync();

//            int newGenreCount = allGenres.Count();

//            Assert.That(newGenreCount, Is.GreaterThan(oldGenreCount));
//        }

//        [Test]
//        [Order(7)]
//        public async Task Test_CreateGenreEditDtoAsync_CreatesDtoCorrectly_WhenGenreExists()
//        {
//            GenreEditDto result = await genreService.CreateGenreEditDtoAsync(1);

//            Assert.That(result.Name, Is.EqualTo("Thriller"));
//        }

//        [Test]
//        [Order(8)]
//        public async Task Test_CreateGenreEditDtoAsync_ThrowsException_WhenGenreDoesNotExist()
//        {
//            Assert.That(async () => await genreService.CreateGenreEditDtoAsync(-1), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        [Test]
//        [Order(9)]
//        public async Task Test_UpdateGenreAsync_UpdatesGenreCorrectly_IfGenreExists_AndNameIsNotTaken()
//        {
//            GenreEditDto dto = new GenreEditDto()
//            {
//                Id = 1,
//                Name = "Thrilller",
//                Description = "Description",
//            };

//            await genreService.UpdateGenreAsync(1, dto);

//            GenreDetailsDto changed = await genreService.GetGenreByIdAsync(1);

//            Assert.That(changed.Name, Is.EqualTo("Thrilller"));
//        }

//        [Test]
//        [Order(10)]
//        public async Task Test_UpdateGenreAsync_ThrowsException_IfGenreExists_ButNameIsTaken()
//        {
//            GenreEditDto dto = new GenreEditDto()
//            {
//                Id = 1,
//                Name = "Fantasy",
//                Description = "Description"
//            };

//            Assert.That(async () => await genreService.UpdateGenreAsync(1, dto), Throws.Exception.TypeOf<UniqueConstraintViolationException>());
//        }

//        [Test]
//        [Order(11)]
//        public async Task Test_DeleteGenreByIdAsync_DeletesGenre_WhenItExists()
//        {
//            bool genreExistsBeforeDelete = await genreService.GenreExistsAsync(1);
//            Assert.That(genreExistsBeforeDelete, Is.EqualTo(true));

//            await genreService.DeleteGenreByIdAsync(1);

//            bool genreExistsAfterDelete = await genreService.GenreExistsAsync(1);
//            Assert.That(genreExistsAfterDelete, Is.EqualTo(false));
//        }

//        [Test]
//        [Order(12)]
//        public async Task Test_DeleteGenreByIdAsync_ThrowsException_WhenItDoesNotExist()
//        {
//            Assert.That(async () => await genreService.DeleteGenreByIdAsync(-1), Throws.Exception.TypeOf<NotFoundException>());
//        }


//        private async Task SeedDatabase()
//        {

//            HashSet<Language> languages = new HashSet<Language>()
//            {
//                new Language()
//                {
//                    Code = "eng",
//                    Name = "English"
//                },
//            };
//            HashSet<Book> books = new HashSet<Book>()
//            {
//                new Book()
//                {
//                    Id = 1,
//                    Title = "Harry Potter",
//                    ISBN = "9781566199094",
//                    Description = "",
//                    PageCount = 223,
//                    LanguageId = 2,
//                    PublicationDate = new DateOnly(1994, 6, 26)
//                },
//                new Book()
//                {
//                    Id = 2,
//                    Title = "A Game of Thrones",
//                    ISBN = "9781562198194",
//                    Description = "",
//                    PageCount = 694,
//                    LanguageId = 2,
//                    PublicationDate = new DateOnly(1996, 8, 6)
//                },
//            };
//            HashSet<Genre> genres = new HashSet<Genre>()
//            {
//                new Genre()
//                {
//                    Id = 1,
//                    Name = "Thriller",
//                    Description = "Description",
//                    Books = new HashSet<Book>()
//                },
//                new Genre()
//                {
//                    Id = 2,
//                    Name = "Fantasy",
//                    Description = "Description",
//                    Books = new HashSet<Book>()
//                },
//            };
//            HashSet<GenreBook> genreBooks = new HashSet<GenreBook>()
//            {
//                new GenreBook()
//                {
//                    GenreId = 2,
//                    BookId = 1,
//                },
//                new GenreBook()
//                {
//                    GenreId = 2,
//                    BookId = 2,
//                }
//            };

//            await unitOfWork.GenreRepository.AddRangeAsync(genres);
//            await unitOfWork.LanguageRepository.AddRangeAsync(languages);
//            await unitOfWork.BookRepository.AddRangeAsync(books);
//            await unitOfWork.GenreBookRepository.AddRangeAsync(genreBooks);

//            await unitOfWork.SaveChangesAsync();
//        }
//    }
//}