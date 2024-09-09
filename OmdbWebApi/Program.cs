using Microsoft.EntityFrameworkCore;
using OmdbWebApi.Data;
using OmdbWebApi.Repositories.Abstract;
using OmdbWebApi.Repositories.Concretes;
using OmdbWebApi.Services.Concretes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<MovieService>();

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<MovieDbContext>(opt =>
{
    opt.UseSqlServer(conn);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
