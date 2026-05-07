namespace StarFetch.Models.Games;

public class MosaicGameState
{
    public int TotalScore { get; set; }
    public int RoundsPlayed { get; set; }
    public int CurrentRoundScore { get; set; }
    public string PlayerRank => CalculateRank(TotalScore);

    private static string CalculateRank(int score)
    {
        return score switch
        {
            < 1000 => "?? Clueless Extra",
            < 2500 => "?? Eager Intern",
            < 5000 => "?? Script Supervisor",
            < 8000 => "??? Assistant Director",
            < 12000 => "?? Casting Director",
            < 17000 => "?? Lead Actor",
            < 23000 => "?? Award Winner",
            < 30000 => "?? Auteur Director",
            < 40000 => "?? Hollywood Icon",
            _ => "?? Legendary Cinephile"
        };
    }
}
