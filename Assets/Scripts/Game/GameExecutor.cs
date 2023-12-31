using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExecutor : MonoBehaviour
{

    public void RollTheDice()
    {
        if (GameStats.currentState != GameState.ROLL_DICE) return;
        GameStats.UI.DiceResult = new System.Random().Next(1, 6+1);
        GameStats.currentState = GameState.MOVE;
    }

    public void ChangeToNextPlayer()
    {
        GameStats.CurrentPlayerIndex = (GameStats.CurrentPlayerIndex + 1) % GameSettings.MAX_PLAYER;
        GameStats.currentState = GameState.ROLL_DICE;
        GameSettings.cameraDirection = CameraDirection.PLAYER;
    }

    public void ExecuteRewards(Player player, Reward[] rewards)
    {
        foreach (Reward reward in rewards)
        {
            player.Emotion += reward.Emotion;
            player.Power += reward.Power;
        }
        FixPlayerStats(player);
    }

    private void FixPlayerStats(Player player)
    {
        player.Emotion = Math.Max(0, Math.Min(GameSettings.MAX_EMOTION, player.Emotion));
        player.Power = Math.Max(0, Math.Min(GameSettings.MAX_POWER, player.Power));
    }
}
