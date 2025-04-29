using UnityEngine;

public class GameState
{
    public static float gameTimeHour { get; set; } = 12.0f;

    #region Skyboxes
    private static Material _daySkyBox;
    public static Material daySkyBox
    {
        get => _daySkyBox;
        set
        {
            if (value != _daySkyBox)
            {
                _daySkyBox = value;
                GameEventSystem.EmitEvent(nameof(GameState), nameof(daySkyBox));
            }
        }
    }

    private static Material _nightSkyBox;
    public static Material nightSkyBox
    {
        get => _nightSkyBox;
        set
        {
            if (value != _nightSkyBox)
            {
                _nightSkyBox = value;
                GameEventSystem.EmitEvent(nameof(GameState), nameof(nightSkyBox));
            }
        }
    }
    #endregion
}