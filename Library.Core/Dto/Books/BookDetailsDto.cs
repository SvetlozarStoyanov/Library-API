﻿using Library.Core.Dto.Authors;
using Library.Core.Dto.Genres;
using Library.Core.Dto.Series;

namespace Library.Core.Dto.Books
{
    public class BookDetailsDto
    {
        public BookDetailsDto()
        {
            Authors = new HashSet<AuthorNestedListDto>();
            Genres = new HashSet<GenreNestedListDto>();
            Series = new HashSet<SeriesNestedListDto>();
        }

        public long Id { get; init; }
        public string Title { get; init; }
        public string? ISBN { get; init; }
        public string Description { get; init; }
        public int PageCount { get; init; }
        public int Quantity { get; init; }
        public DateTime? PublicationDate { get; init; }
        public IEnumerable<AuthorNestedListDto> Authors { get; init; }
        public IEnumerable<GenreNestedListDto> Genres { get; init; }
        public IEnumerable<SeriesNestedListDto> Series { get; init; }
    }
}