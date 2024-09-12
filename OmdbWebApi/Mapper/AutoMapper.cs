using AutoMapper;
using OmdbWebApi.Dtos;
using OmdbWebApi.Entities;

namespace OmdbWebApi.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper() {
            CreateMap<Movie, MovieDto>();
        
        }
    }
}
