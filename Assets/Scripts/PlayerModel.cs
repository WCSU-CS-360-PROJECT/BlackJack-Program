public class PlayerModel
{
    public int BBucks { get; private set; }
    public int HighScore { get; private set; }
    public int WinStreak { get; private set; }
    public int LongestWinStreak { get; private set; }

    public PlayerModel(int startingBBucks)
    {
        BBucks = startingBBucks;
        HighScore = 0;
        WinStreak = 0;
        LongestWinStreak = 0;
    }

    public void ResetForNewSession(int startingBBucks)
    {
        BBucks = startingBBucks;
        WinStreak = 0;
        LongestWinStreak = 0;
    }

    private void AdjustBalance(int delta)
    {
        BBucks += delta;
        if (BBucks < 0) BBucks = 0;
    }

    public void RecordWin(int netWin)
    {
        AdjustBalance(netWin);
        WinStreak++;
        if (WinStreak > LongestWinStreak)
            LongestWinStreak = WinStreak;
    }

    public void RecordLoss(int amountLost)
    {
        AdjustBalance(-amountLost);
        WinStreak = 0;
    }

    public void RecordPush()
    {
        // No change to BBucks or streak.
    }

    public void CashOut()
    {
        if (BBucks > HighScore)
        {
            HighScore = BBucks;
        }
    }

    public bool IsBroke()
    {
        return BBucks <= 0;
    }
}
