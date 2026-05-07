namespace StarFetch.Models.Games;

public record MosaicRoundData(
    Movie AnswerMovie,
    string PosterUrl,
    List<string> Choices
);
