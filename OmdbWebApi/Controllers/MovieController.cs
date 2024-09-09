using Microsoft.AspNetCore.Mvc;
using OmdbWebApi.Repositories.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OmdbWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
         var items=  await  _movieRepository.GetAllAsync(); 
            return Ok(items);
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MovieController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
