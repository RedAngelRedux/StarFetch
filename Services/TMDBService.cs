using StarFetch.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace StarFetch.Services;

public class TMDBService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private string _basePosterUrl = "https://image.tmdb.org/t/p/w500";

    public TMDBService(HttpClient httpClient,IConfiguration configuration)
    {
        _http = httpClient;
        _config = configuration;

        string? apiKey = _config["TmdbAccessKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentNullException("TMDB API key is not configured.");
        }
        else
        {
            _http.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
    }

    private async Task<MovieListResponse> FetchMoviesAsync(string url)
    {

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var movieList = await response.Content.ReadFromJsonAsync<MovieListResponse>(_jsonOptions);

        foreach (var movie in movieList?.Results ?? [])
        {
            if (!string.IsNullOrEmpty(movie.PosterPath))
            {
                movie.PosterPath = $"{_basePosterUrl}{movie.PosterPath}";
            }
            else
            {
                movie.PosterPath = "img/poster.png";
            }
        }

        return movieList ?? new MovieListResponse();
    }

    public async Task<MovieListResponse> GetNowPlayingMoviesAsync()
    {
        string url = $"{_http.BaseAddress}movie/now_playing?&region=US&language=en-US&include_adult=false";
        return await FetchMoviesAsync(url);
    }

    public async Task<MovieListResponse> GetPopularMoviesAsync()
    {
        string url = $"{_http.BaseAddress}movie/popular?&region=US&language=en-US&include_adult=false";
        return await FetchMoviesAsync(url);
    }

    /// <summary>
    /// Search for movies by title.
    /// </summary>
    /// <param name="query">User supplied by the user</param>
    /// <returns></returns>
    public async Task<MovieListResponse> SearchMoviesAsync(string query)
    {
        string url = $"{_http.BaseAddress}search/movie?query={Uri.EscapeDataString(query)}&region=US&language=en-US&include_adult=false";
        return await FetchMoviesAsync(url);
    }

    /// <summary>
    /// Asynchronously retrieves detailed information about a movie by its unique identifier.
    /// </summary>
    /// <remarks>The method fetches movie details from a remote API and updates the poster and backdrop paths 
    /// to include the base URL. If the poster or backdrop path is not available, default placeholder  images are
    /// used.</remarks>
    /// <param name="movieId">The unique identifier of the movie to retrieve details for.</param>
    /// <returns>A <see cref="MovieDetails"/> object containing detailed information about the specified movie. If the movie
    /// details cannot be retrieved, an exception is thrown.</returns>
    /// <exception cref="HttpIOException">Thrown if the HTTP request fails or the response is invalid.</exception>
    public async Task<MovieDetails> GetMovieDetailsAsync(int movieId)
    {    
        
        string url = $"{_http.BaseAddress}movie/{movieId}?";

        //MovieDetails movie = await _http.GetFromJsonAsync<MovieDetails>(url, _jsonOptions) 
        //    ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to fetch movie details.");

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        MovieDetails? movie = await response.Content.ReadFromJsonAsync<MovieDetails>(_jsonOptions)
            ?? throw new HttpIOException(HttpRequestError.InvalidResponse, "Failed to fetch movie details.");

        movie.PosterPath = !string.IsNullOrEmpty(movie.PosterPath) 
            ? $"{_basePosterUrl}{movie.PosterPath}" 
            : "img/poster.png";

        movie.BackdropPath = !string.IsNullOrEmpty(movie.BackdropPath) 
            ? $"{_basePosterUrl}{movie.BackdropPath}" 
            : "img/backdrop.jpg";

        return movie;
    }
}

