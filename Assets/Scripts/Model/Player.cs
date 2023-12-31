using System.Collections;
using System.Collections.Generic;

public class Player
{
    public string Name { get; set; }
    public int Credit { get; set; }
    public int Emotion { get; set; }
    public int Power { get; set; }
    public string ImagePath { get; set; }
    public int StandingPos { get; set; }
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
        CurrentCourse = new List<Course>();
        CourseHistory = new List<Course>();
    }
}