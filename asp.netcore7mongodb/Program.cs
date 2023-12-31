using asp.netcore7mongodb.Entities;
using asp.netcore7mongodb.Entities.IOptions;
using asp.netcore7mongodb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MoviesDatabaseSettings>(builder.Configuration.GetSection("MoviesDatabaseSettings"));
builder.Services.AddSingleton<MoviesService>();




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



app.MapGet("/", () => "Movies API!");

app.MapGet("/api/movies", async (MoviesService moviesService) => await moviesService.Get());

app.MapGet("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
    var movie = await moviesService.Get(id);
    return movie is null ? Results.NotFound() : Results.Ok(movie);
});

app.MapPost("/api/movies", async (MoviesService moviesService, Movie movie) =>
{
    await moviesService.Create(movie);
    return Results.Ok();
});

app.MapPut("/api/movies/{id}", async (MoviesService moviesService, string id, Movie updatedMovie) =>
{
    var movie = await moviesService.Get(id);
    if (movie is null) return Results.NotFound();

    updatedMovie.Id = movie.Id;
    await moviesService.Update(id, updatedMovie);

    return Results.NoContent();
});

app.MapDelete("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
    var movie = await moviesService.Get(id);
    if (movie is null) return Results.NotFound();

    await moviesService.Remove(movie.Id);

    return Results.NoContent();
});

app.Run();
