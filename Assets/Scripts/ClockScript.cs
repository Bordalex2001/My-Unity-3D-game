using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI clock;

    void Start()
    {
        clock = GetComponent<TMPro.TextMeshProUGUI>();
        GameEventSystem.AddListener(OnGameStateChanged, nameof(GameState));
    }

    void Update()
    {
        int hours = Mathf.FloorToInt(GameState.gameTimeHour);
        int minutes = Mathf.FloorToInt(60 * (GameState.gameTimeHour - Mathf.FloorToInt(GameState.gameTimeHour)));

        clock.text = $"{hours:D2}:{minutes:D2}";
    }

    private void OnGameStateChanged(string type, object payload)
    {
        clock.enabled = GameState.isClockVisible;
    }

    private void OnDestroy()
    {
        GameEventSystem.RemoveListener(OnGameStateChanged, nameof(GameState));
    }
}