using System.Collections;
using System.Collections.Generic;


public enum CameraDirection
{
    DICE,
    CLOSE_PLAYER,
    PLAYER,
    DORM,
    HOSTPITAL,
    LIBRARY,
    PARK,
}

public static class GameSettings
{
    public static readonly int MAX_PLAYER = 4;
    public static readonly int TARGET_CREDIT = 10;
    public static readonly int MAX_EMOTION = 20;
    public static readonly int MAX_POWER = 100;
    public static readonly int SELECT_COURSE_POWER_COST = 10;
    public static CameraDirection cameraDirection = CameraDirection.PLAYER;

    public static void Reset()
    {
        cameraDirection = CameraDirection.PLAYER;
    }
}
