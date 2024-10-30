//using AutoMapper;
//using Library.Core.Automapper.Config;
//using Library.Core.Contracts;
//using Library.Core.CustomExceptions;
//using Library.Core.Dto.Series;
//using Library.Core.Services;
//using Library.Infrastructure;
//using Library.Infrastructure.DataPersistence.Contracts;
//using Library.Infrastructure.DataPersistence.UnitOfWork;
//using Library.Infrastructure.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace Library.Tests.UnitTests
//{
//    [TestFixture]
//    public class SeriesServiceTests
//    {
//        private LibraryDbContext dbContext;
//        private IUnitOfWork unitOfWork;
//        private ISeriesService seriesService;
//        private IMapper mapper;

//        [OneTimeSetUp]
//        public async Task OneTimeSetup()
//        {
//            DbContextOptions<LibraryDbContext> contextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
//                .UseInMemoryDatabase("LibraryTestDb")
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

//            seriesService = new SeriesService(unitOfWork, mapper);
//        }


//        [OneTimeTearDown]
//        public void OneTimeTearDown()
//        {
//            dbContext.Dispose();
//        }

//        [Test]
//        [Order(0)]
//        public async Task Test_SeriesExistsAsync_ReturnsTrue_IfSeriesExists()
//        {
//            bool result = await seriesService.SeriesExistsAsync(1);

//            Assert.That(result, Is.True);
//        }

//        [Test]
//        [Order(1)]
//        public async Task Test_SeriesExistsAsync_ReturnsFalse_IfSeriesDoesNotExist()
//        {
//            bool result = await seriesService.SeriesExistsAsync(-1);

//            Assert.That(result, Is.False);
//        }

//        [Test]
//        [Order(2)]
//        public async Task Test_GetAllSeriesAsync_ReturnsAllSeries()
//        {
//            IEnumerable<SeriesListDto> result = await seriesService.GetAllSeriesAsync();

//            Assert.That(result.Count, Is.EqualTo(2));
//        }

//        [Test]
//        [Order(3)]
//        public async Task Test_GetSeriesByIdAsync_ReturnsCorrectSeries_WhenSeriesExists()
//        {
//            SeriesDetailsDto result = await seriesService.GetSeriesByIdAsync(1);

//            Assert.That(result.Title, Is.EqualTo("Harry Potter"));
//        }

//        [Test]
//        [Order(4)]
//        public async Task Test_GetSeriesByIdAsync_ThrowsException_WhenSeriesDoesNotExist()
//        {
//            Assert.That(async () => await seriesService.GetSeriesByIdAsync(-1), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        [Test]
//        [Order(5)]
//        public async Task Test_CreateSeriesCreateDtoAsync_CreatesDtoCorrectly()
//        {
//            SeriesCreateDto result = seriesService.CreateSeriesCreateDtoAsync();

//            Assert.That(result, Is.Not.Null);
//        }

//        [Test]
//        [Order(6)]
//        public async Task Test_CreateSeriesAsync_CreatesSeries_WhenDtoIsCorrect()
//        {
//            SeriesCreateDto dto = new SeriesCreateDto()
//            {
//                Title = "Goosebumps",
//                Description = "Description",
//            };

//            IEnumerable<SeriesListDto> series = await seriesService.GetAllSeriesAsync();
//            Assert.That(series.Count() == 2, Is.True);

//            await seriesService.CreateSeriesAsync(dto);

//            series = await seriesService.GetAllSeriesAsync();

//            Assert.That(series.Count() == 3, Is.True);
//        }

//        [Test]
//        [Order(7)]
//        public async Task Test_CreateSeriesEditDtoAsync_CreatesDtoCorrectly_IfSeriesExists()
//        {
//            SeriesEditDto result = await seriesService.CreateSeriesEditDtoAsync(1);

//            Assert.That(result, Is.Not.Null);

//            Assert.That(result.Title, Is.EqualTo("Harry Potter"));
//        }

//        [Test]
//        [Order(8)]
//        public async Task Test_CreateSeriesEditDtoAsync_ThrowsException_IfSeriesDoesNotExist()
//        {
//            Assert.That(async () => await seriesService.CreateSeriesEditDtoAsync(-1), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        [Test]
//        [Order(9)]
//        public async Task Test_UpdateSeriesAsync_UpdatesSeriesCorrectly_IfSeriesExists()
//        {
//            SeriesDetailsDto before = await seriesService.GetSeriesByIdAsync(1);
//            Assert.That(before.Title, Is.EqualTo("Harry Potter"));

//            SeriesEditDto dto = new SeriesEditDto()
//            {
//                Id = 1,
//                Title = "Harri Potter",
//                Description = "Description"
//            };

//            await seriesService.UpdateSeriesAsync(1, dto);

//            SeriesDetailsDto after = await seriesService.GetSeriesByIdAsync(1);
//            Assert.That(after.Title, Is.EqualTo("Harri Potter"));
//        }

//        [Test]
//        [Order(10)]
//        public async Task Test_UpdateSeriesAsync_ThrowsException_IfSeriesDoesNotExist()
//        {
//            SeriesEditDto dto = new SeriesEditDto()
//            {
//                Id = 1,
//                Title = "Harri Potter",
//                Description = "Description"
//            };

//            Assert.That(async () => await seriesService.UpdateSeriesAsync(-1, dto), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        [Test]
//        [Order(11)]
//        public async Task Test_DeleteSeriesByIdAsyncDeletesSeries_IfSeriesExists()
//        {
//            bool serieExistsBeforeDelete = await seriesService.SeriesExistsAsync(2);
//            Assert.That(serieExistsBeforeDelete, Is.EqualTo(true));

//            await seriesService.DeleteSeriesByIdAsync(2);

//            bool seriesExistsAfterDelete = await seriesService.SeriesExistsAsync(2);
//            Assert.That(seriesExistsAfterDelete, Is.EqualTo(false));
//        }

//        [Test]
//        [Order(12)]
//        public async Task Test_DeleteSeriesByIdAsync_ThrowsException_IfSeriesDoesNotExist()
//        {
//            Assert.That(async () => await seriesService.DeleteSeriesByIdAsync(-1), Throws.Exception.TypeOf<NotFoundException>());
//        }

//        private async Task SeedDatabase()
//        {
//            HashSet<Language> languages = new HashSet<Language>()
//            {
//                new Language()
//                {
//                    Code = "eng",
//                    Name = "English"
//                }
//            };

//            HashSet<Book> books = new HashSet<Book>()
//            {
//                new Book()
//                {
//                    Id = 1,
//                    Title = "Harry Potter and the Philosopher's Stone",
//                    ISBN = "9781566199094",
//                    Description = "Description",
//                    PageCount = 223,
//                    LanguageId = 2,
//                    PublicationDate = new DateOnly(1994, 6, 26)
//                },
//                new Book()
//                {
//                    Id = 2,
//                    Title = "Harry Potter and the Chamber of Secrets",
//                    ISBN = "9781566199094",
//                    Description = "Description",
//                    PageCount = 352,
//                    LanguageId = 2,
//                    PublicationDate = new DateOnly(1998, 07, 02)
//                },
//                new Book()
//                {
//                    Id = 3,
//                    Title = "The Fellowship of the Ring",
//                    Description = "Description",
//                    ISBN = null,
//                    PageCount = 423,
//                    LanguageId = 2,
//                    PublicationDate = new DateOnly(1954, 7, 29)
//                }
//            };

//            HashSet<Series> series = new HashSet<Series>()
//            {
//                new Series
//                {
//                    Id = 1,
//                    Title = "Harry Potter",
//                    Description = "Description"
//                },
//                new Series
//                {
//                    Id = 2,
//                    Title = "Lord of the Rings",
//                    Description = "Description"
//                },
//            };

//            HashSet<SeriesBook> seriesBooks = new HashSet<SeriesBook>()
//            {
//                new SeriesBook
//                {
//                    Id = 1,
//                    SeriesId = 1,
//                    BookId = 1
//                },
//                new SeriesBook {
//                    Id = 2,
//                    SeriesId = 1,
//                    BookId = 2
//                },
//                new SeriesBook
//                {
//                    Id = 3,
//                    SeriesId = 1,
//                    BookId = 3
//                },
//            };

//            await unitOfWork.LanguageRepository.AddRangeAsync(languages);
//            await unitOfWork.BookRepository.AddRangeAsync(books);
//            await unitOfWork.SeriesRepository.AddRangeAsync(series);
//            await unitOfWork.SeriesBookRepository.AddRangeAsync(seriesBooks);
//            await unitOfWork.SaveChangesAsync();
//        }
//    }
//}
