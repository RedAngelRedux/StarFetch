using StarFetch.Models;
using StarFetch.Models.Games;

namespace StarFetch.Services.Games;

public class MosaicGameService
{
    private readonly TMDBService _tmdbService;
    private readonly FavoritesService _favoritesService;
    private readonly Random _random = new();

    public MosaicGameService(TMDBService tmdbService, FavoritesService favoritesService)
    {
        _tmdbService = tmdbService;
        _favoritesService = favoritesService;
    }

    public async Task<MosaicRoundData> GetNewRoundAsync()
    {
        try
        {
            var nowPlayingResponse = await _tmdbService.GetNowPlayingMoviesAsync();
            var nowPlayingMovies = nowPlayingResponse.Results;

            var favorites = await _favoritesService.GetFavoritesAsync();
            var favoriteIds = favorites.Select(f => f.Id).ToHashSet();

            var weightedPool = new List<(Movie movie, int weight)>();

            foreach (var movie in nowPlayingMovies)
            {
                if (favoriteIds.Contains(movie.Id))
                {
                    weightedPool.Add((movie, 4));
                }
                else
                {
                    weightedPool.Add((movie, 2));
                }
            }

            foreach (var favorite in favorites)
            {
                if (!nowPlayingMovies.Any(m => m.Id == favorite.Id))
                {
                    weightedPool.Add((favorite, 1));
                }
            }

            if (weightedPool.Count < 5)
            {
                var popularResponse = await _tmdbService.GetPopularMoviesAsync();
                var popularMovies = popularResponse.Results.Take(20).ToList();

                foreach (var movie in popularMovies)
                {
                    if (!weightedPool.Any(m => m.movie.Id == movie.Id))
                    {
                        weightedPool.Add((movie, 1));
                    }
                }
            }

            var answerMovie = SelectWeightedRandom(weightedPool);

            var decoys = await GetDecoysAsync(answerMovie.Id);

            var choices = new List<string> { answerMovie.Title ?? "" };
            choices.AddRange(decoys);
            choices = choices.OrderBy(_ => _random.Next()).ToList();

            var posterUrl = answerMovie.PosterPath ?? "img/poster.png";

            return new MosaicRoundData(answerMovie, posterUrl, choices);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in GetNewRoundAsync: {ex.Message}");
            throw;
        }
    }

    private Movie SelectWeightedRandom(List<(Movie movie, int weight)> weightedPool)
    {
        if (weightedPool.Count == 0)
        {
            throw new InvalidOperationException("Weighted pool is empty");
        }

        var totalWeight = weightedPool.Sum(p => p.weight);
        var randomValue = _random.Next(totalWeight);

        var cumulative = 0;
        foreach (var (movie, weight) in weightedPool)
        {
            cumulative += weight;
            if (randomValue < cumulative)
            {
                return movie;
            }
        }

        return weightedPool.Last().movie;
    }

    private async Task<List<string>> GetDecoysAsync(int excludeId)
    {
        var popularResponse = await _tmdbService.GetPopularMoviesAsync();
        var candidates = popularResponse.Results
            .Where(m => m.Id != excludeId && !string.IsNullOrEmpty(m.Title))
            .ToList();

        return candidates
            .OrderBy(_ => _random.Next())
            .Take(4)
            .Select(m => m.Title!)
            .ToList();
    }
}
