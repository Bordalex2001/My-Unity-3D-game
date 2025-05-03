using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    private float AmbientVolume;
    private float EffectsVolume;
    private float MusicVolume;
    private float MasterVolume;

    void Start()
    {
        audioMixer.GetFloat(nameof(AmbientVolume), out AmbientVolume);
        audioMixer.GetFloat(nameof(EffectsVolume), out EffectsVolume);
        audioMixer.GetFloat(nameof(MusicVolume), out MusicVolume);
        audioMixer.GetFloat(nameof(MasterVolume), out MasterVolume);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
        {
            ChangeMasterVolume(isLouder: true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            ChangeMasterVolume(isLouder: false);
        }
    }

    private void ChangeMasterVolume(bool isLouder)
    {
        if (audioMixer.GetFloat(nameof(MasterVolume), out MasterVolume))
        {
            //20..0..-10..-20..-40..-60..-80
            float step = 4.5f + Mathf.Abs(MasterVolume + 5f) * .25f;
            MasterVolume = Mathf.Clamp(isLouder ? MasterVolume + step : MasterVolume - step, -80, 20);
            audioMixer.SetFloat(nameof(MasterVolume), MasterVolume);
        }
    }
}