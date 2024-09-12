using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmdbWebApi.Dtos;
using OmdbWebApi.Entities;
using OmdbWebApi.Repositories.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OmdbWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository,IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
         var items=  await  _movieRepository.GetAllAsync();
            var dto = _mapper.Map<IEnumerable< Movie>>(items);
            return Ok(dto);
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item =await _movieRepository.GetAsync(id);
            return Ok(item);
        }

     
    }
}
