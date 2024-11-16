using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AuthorDto, Author>();
        CreateMap<Author, AuthorDto>();
        
        CreateMap<BookDto, Book>();
        CreateMap<Book, BookDto>();
        
        CreateMap<PublisherDto, Publisher>();
        CreateMap<Publisher, PublisherDto>();
    }
}