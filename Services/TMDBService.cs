using StarFetch.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        string basePosterUrl = "https://image.tmdb.org/t/p/w500";

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var movieList = await response.Content.ReadFromJsonAsync<MovieListResponse>(_jsonOptions);

        foreach (var movie in movieList?.Results ?? [])
        {
            if (!string.IsNullOrEmpty(movie.PosterPath))
            {
                movie.PosterPath = $"{basePosterUrl}{movie.PosterPath}";
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
}

