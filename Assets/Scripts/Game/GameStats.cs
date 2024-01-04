using System.Collections;
using System.Collections.Generic;

public enum GameState {
    CHECK,
    WAIT_ROLL_DICE,
    DICE_ROLLING,
    PASS_START,
    MOVE,
    EXECUTE_FEATURE,
    NEXT_PLAYER,
}

public static class GameStats
{
    public static int CurrentPlayerIndex = 0;
    public static GameState currentState = GameState.CHECK;

    private static List<Player> _players = new List<Player>();

    public static List<Player> GetPlayerList()
    {
        return _players;
    }

    public static Player GetCurrentPlayer()
    {
        return _players[CurrentPlayerIndex];
    }

    public static Player GetPlayerByName(string name)
    {
        return _players.Find(p => p.Name == name);
    }

    public static class UI
    {
        public static int DiceResult = 0;
    }
}
