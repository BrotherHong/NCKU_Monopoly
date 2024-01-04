using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void RefreshUnselectedCourse()
    {
        List<int> indexes = Enumerable.Range(0, Courses.Count()).ToList();
        indexes = indexes.OrderBy(i => new System.Random().Next()).ToList(); // shuffle
        Course c1 = Courses[course1];
        Course c2 = Courses[course2];
        if (c1.Owner != null) indexes.Remove(course1);
        if (c2.Owner != null) indexes.Remove(course2);

        int idx = 0;
        if (c1.Owner == null) course1 = indexes[idx++];
        if (c2.Owner == null) course2 = indexes[idx++];
    }

    public override string ToString()
    {
        return $"{Type}/{Name}";
    }
}
