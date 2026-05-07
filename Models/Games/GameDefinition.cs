namespace StarFetch.Models.Games;

public record GameDefinition(
    string Title,
    string Description,
    string Difficulty,
    string Route,
    string IconClass
);
