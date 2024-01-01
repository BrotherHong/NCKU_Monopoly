using System.Collections;
using System.Collections.Generic;

public enum CornerBlock
{
    DORM,
    HOSPITAL,
    LIRBARY,
    PLAYGROUND,
}

[System.Serializable]
public class Platform
{
    public Block[] blocks;
    public Chance[] chances;
    public Destiny[] destinies;
    public WalkingEvent BoyGirlFriend;
    public WalkingEvent Construction;
    public WalkingEvent BikeAccident;
    public WalkingEvent DogBite;
}
