using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public string Type;
    public string Name;
    public Course[] Courses;
    public string Color;
    public string Message;
    public int course1 = 0;
    public int course2 = 1;
    public SpecialEvent specialEvent;

    public override string ToString()
    {
        return $"{Type}/{Name}";
    }
}
