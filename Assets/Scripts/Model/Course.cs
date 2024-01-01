using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Course
{
    public string Name;
    public string Type;
    public int Credit;
    public string Owner;

    public void ResetOwner()
    {
        Owner = null;
    }

    public override string ToString()
    {
        return $"{Name}/{Type}/{Credit}¾Ç¤À";
    }
}
