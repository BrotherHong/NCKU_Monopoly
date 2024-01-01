using System.Collections;
using System.Collections.Generic;


public enum WalkingEventType
{
    BG_FRIEND,
    CONSTRUCTION,
    BIKE_ACCIDENT,
    DOG_BITE,
}

public abstract class SpecialEvent
{
    public string Title;
    public string Message;
    public Reward[] Rewards;
}

[System.Serializable]
public class WalkingEvent : SpecialEvent
{

}
