using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Player
{
    public string Name { get; set; }
    public int Credit { get; set; }
    public int Emotion { get; set; }
    public int Power { get; set; }
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
