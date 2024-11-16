using AutoMapper;
using LibraryAPI.DTOs;
using LibraryAPI.Models;

namespace LibraryAPI.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AuthorDTO, Author>();
        CreateMap<Author, AuthorDTO>();
        
        CreateMap<BookDTO, Book>();
        CreateMap<Book, BookDTO>();
        
        CreateMap<PublisherDTO, Publisher>();
        CreateMap<Publisher, PublisherDTO>();
    }
}