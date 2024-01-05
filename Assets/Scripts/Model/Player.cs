using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Player
{
    private int _credit;
    private int _emotion;
    private int _power;

    public string Name { get; set; }
    public int Credit
    {
        get => _credit;
        set => _credit = Math.Max(0, Math.Min(GameSettings.TARGET_CREDIT, value));
    }
    public int Emotion
    {
        get => _emotion;
        set => _emotion = Math.Max(0, Math.Min(GameSettings.MAX_EMOTION, value));
    }
    public int Power
    {
        get => _power;
        set => _power = Math.Max(0, Math.Min(GameSettings.MAX_POWER, value));
    }
    public string ImagePath { get; set; }
    public int StandingPos { get; set; }
    public int GrabCount { get; set; }
    public List<Course> CurrentCourse { get; set; }
    public List<Course> CourseHistory { get; set; }

    public Player(string name, string imagePath)
    {
        Name = name;
        Credit = 0;
        Emotion = GameSettings.MAX_EMOTION;
        Power = GameSettings.MAX_POWER;
        ImagePath = imagePath;
        StandingPos = 0;
        GrabCount = 0;
        CurrentCourse = new List<Course>();
        CourseHistory = new List<Course>();
    }

    public void ResetRoundStats()
    {
        Emotion = GameSettings.MAX_EMOTION;
        Power = GameSettings.MAX_POWER;
        GrabCount = 0;
    }

    public bool IsEMO()
    {
        return Emotion == 0;
    }

    public string GetCurrentCoursesText()
    {
        StringBuilder sb = new StringBuilder();
        CurrentCourse.ForEach(c => sb.AppendLine(c.ToString()));
        return sb.ToString();
    }

    public int GetGrabCost()
    {
        return GameSettings.SELECT_COURSE_POWER_COST + ((GrabCount + 1) * 5);
    }
}
