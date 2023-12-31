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
=======
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExecutor : MonoBehaviour
{
    [SerializeField] PlatformHelper platformHelper;

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

    public void OnPlayerBackToStart(Player player)
    {
        player.Power = GameSettings.MAX_POWER;
        player.Emotion += 3;
        foreach (Course course in player.CurrentCourse)
        {
            player.Credit += course.Credit;
            player.CourseHistory.Add(course);
            course.ResetOwner();
        }
        player.CurrentCourse.Clear();
        FixPlayerStats(player);
    }

    public void SelectCourse(int courseIndex)
    {
        Player player = GameStats.GetCurrentPlayer();
        Block block = platformHelper.GetBlock(player.StandingPos);
        Course course = block.Courses[(courseIndex == 1 ? block.course1 : block.course2)];

        if (player.Power < GameSettings.SELECT_COURSE_POWER_COST) return;
        if (course.Owner != null) return; 
        
        course.Owner = player.Name;
        player.CurrentCourse.Add(course);
        player.Power -= GameSettings.SELECT_COURSE_POWER_COST;
        FixPlayerStats(player);
    }
}
