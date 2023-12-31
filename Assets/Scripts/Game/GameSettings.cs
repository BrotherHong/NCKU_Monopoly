using System.Collections;
using System.Collections.Generic;


public enum CameraDirection
{
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
    public static readonly int TARGET_CREDIT = 130;
    public static readonly int MAX_EMOTION = 10;
    public static readonly int MAX_POWER = 100;
    public static CameraDirection cameraDirection = CameraDirection.PLAYER;
}
