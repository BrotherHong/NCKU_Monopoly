using System.Collections;
using System.Collections.Generic;

public enum GameState {
    ROLL_DICE,
    MOVE,
    EXECUTE_FEATURE,
    NEXT_PLAYER,
}

public static class GameStats
{
    public static int CurrentPlayerIndex = 0;
    public static GameState currentState = GameState.ROLL_DICE;

    private static List<Player> _players = new List<Player>();

    public static List<Player> GetPlayerList()
    {
        return _players;
    }

    public static Player GetCurrentPlayer()
    {
        return _players[CurrentPlayerIndex];
    }

    public static class UI
    {
        public static int DiceResult = 0;
    }
}
