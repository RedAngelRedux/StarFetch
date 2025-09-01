using StarFetch.Components.Pages;
using StarFetch.Models;

namespace StarFetch.Services;

public class FavoritesService(ILocalStorageService localStorageService)
{
    private readonly ILocalStorageService _localStorageService = localStorageService;
    private const string FavoritesKey = "favorites";

    /// <summary>
    /// Returns the list of favorite movies from local storage.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Movie>> GetFavoritesAsync()
    {
        List<Movie>? favorites = [];

        try
        {
            favorites = await _localStorageService.GetItemAsync<List<Movie>>(FavoritesKey) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving favorites: {ex.Message}");
            favorites = [];
        }

        return favorites;
    }
    /// <summary>
    /// Saves the list of favorite movies to local storage.
    /// </summary>
    /// <param name="list">A List of Movies</param>
    /// <returns></returns>
    public async Task SaveFavoritesAsync(List<Movie> list)
    {
        try
        {
            await _localStorageService.RemoveItemAsync(FavoritesKey);
            await _localStorageService.SetItemAsync(FavoritesKey, list);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding favorite: {ex.Message}");
        }
    }
    public async Task RemoveFavoritesAsync(string item = "")
    {
        try
        {
            await _localStorageService.RemoveItemAsync(FavoritesKey);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing favorite: {ex.Message}");
        }
    }
    public async Task<bool> HasFavoriteAsync(string item = "")
    {
        var favorites = await GetFavoritesAsync();
        return favorites.Count > 0;
    }

    /// <summary>
    /// Adds a movie to the list of favorites in local storage if it is not already present.
    /// </summary>
    /// <param name="movie">The movie to be added</param>
    /// <returns></returns>
    public async Task AddToFavorites(Movie movie)
    {
        var favorites = await GetFavoritesAsync();
        if (!favorites.Any(m => m.Id == movie.Id))
        {
            favorites.Add(movie);
            await SaveFavoritesAsync(favorites);
        }
    }

    /// <summary>
    /// Removes a movie from the list of favorites in local storage if it is present.
    /// </summary>
    /// <param name="movie"></param>
    /// <returns></returns>
    public async Task RemoveFromFavorites(Movie movie)
    {
        List<Movie> favorites = await GetFavoritesAsync();
        var newFavorites = favorites.Where(m => m.Id != movie.Id);
        await SaveFavoritesAsync([.. newFavorites]); 
    }

    /// <summary>
    /// Determines whether the specified item is marked as a favorite.
    /// </summary>
    /// <remarks>This method asynchronously retrieves the list of favorite items and checks if the specified
    /// item's identifier exists in the list.</remarks>
    /// <param name="id">The unique identifier of the Movie to check.</param>
    /// <returns><see langword="true"/> if the item with the specified identifier is marked as a favorite; otherwise, <see
    /// langword="false"/>.</returns>
    public async Task<bool> IsFavorite(int id)
    {
        var favorites = await GetFavoritesAsync();
        return favorites.Any(m => m.Id == id);
    }
}
