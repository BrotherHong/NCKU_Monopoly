using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameExecutor : MonoBehaviour
{
    [SerializeField] PlatformHelper platformHelper;
    [SerializeField] PlayerController playerController;

    public void RollTheDice()
    {
        if (GameStats.currentState != GameState.WAIT_ROLL_DICE) return;
        GameStats.UI.DiceResult = new System.Random().Next(1, 6+1);
        GameSettings.cameraDirection = CameraDirection.DICE;
        GameStats.currentState = GameState.DICE_ROLLING;
    }

    public void ChangeToNextPlayer()
    {
        if (!IsGameOver())
        {
            GameStats.CurrentPlayerIndex = (GameStats.CurrentPlayerIndex + 1) % GameSettings.MAX_PLAYER;
            GameStats.currentState = GameState.CHECK;
            GameSettings.cameraDirection = CameraDirection.PLAYER;
        } else
        {
            SceneManager.LoadScene(2);
        }
    }

    private bool IsGameOver()
    {
        Player player = GameStats.GetCurrentPlayer();
        return player.Credit >= GameSettings.TARGET_CREDIT;
    }

    public void ExecuteRewards(Player player, Reward[] rewards)
    {
        foreach (Reward reward in rewards)
        {
            if (reward.Mode == "Add")
            {
                player.Emotion += reward.Emotion;
                player.Power += reward.Power;
            } else if (reward.Mode == "Teleport")
            {
                CornerBlock corner = CornerBlock.DORM;
                if (reward.TpTarget == "Dorm") corner = CornerBlock.DORM;
                else if (reward.TpTarget == "Hospital") corner = CornerBlock.HOSPITAL;
                else if (reward.TpTarget == "Library") corner = CornerBlock.LIRBARY;
                else if (reward.TpTarget == "Playground") corner = CornerBlock.PLAYGROUND;
                playerController.TeleportPlayerToCorner(GameStats.CurrentPlayerIndex, corner);
            }
        }
        FixPlayerStats(player);
    }

    private void FixPlayerStats(Player player)
    {
        player.Emotion = Math.Max(0, Math.Min(GameSettings.MAX_EMOTION, player.Emotion));
        player.Power = Math.Max(0, Math.Min(GameSettings.MAX_POWER, player.Power));
    }

    public void OnPlayerPassStart(Player player)
    {
        player.Power = GameSettings.MAX_POWER;
        player.Emotion += 5;
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

        int powerCost;
        bool grab = false;
        if (course.Owner == null)
        {
            powerCost = GameSettings.SELECT_COURSE_POWER_COST;
        } else
        {
            if (course.Owner == player.Name) return;
            powerCost = player.GetGrabCost();
            grab = true;
        }

        if (player.Power < powerCost) return;

        if (grab)
        {
            Player origOwner = GameStats.GetPlayerByName(course.Owner);
            player.GrabCount++;
            origOwner.CurrentCourse.Remove(course);
        }
        
        course.Owner = player.Name;
        player.CurrentCourse.Add(course);
        player.Power -= powerCost;
        FixPlayerStats(player);
    }

    public void ExecuteEmoPenalty(Player player)
    {
        player.ResetRoundStats();
        player.CurrentCourse.ForEach(c => c.ResetOwner());
        player.CurrentCourse.Clear();
    }
}
