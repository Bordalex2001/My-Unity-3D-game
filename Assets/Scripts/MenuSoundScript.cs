using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSoundScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    private float AmbientVolume;
    private float EffectsVolume;
    private float MusicVolume;
    private float MasterVolume;

    private Slider masterSlider;
    private Slider ambientSlider;
    private Slider effectsSlider;
    private Slider musicSlider;

    void Start()
    {
        LoadVolumes();

        Transform layout = transform.Find("Content/Sound/Layout");
        masterSlider = layout.Find("Master/Slider").GetComponent<Slider>();
        masterSlider.value = DbToValue(MasterVolume);
        ambientSlider = layout.Find("Ambient/Slider").GetComponent<Slider>();
        ambientSlider.value = DbToValue(AmbientVolume);
        effectsSlider = layout.Find("Effects/Slider").GetComponent<Slider>();
        effectsSlider.value = DbToValue(EffectsVolume);
        musicSlider = layout.Find("Music/Slider").GetComponent<Slider>();
        musicSlider.value = DbToValue(MusicVolume);
    }

    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey(nameof(MasterVolume)))
        {
            MasterVolume = PlayerPrefs.GetFloat(nameof(MasterVolume));
            audioMixer.SetFloat(nameof(MasterVolume), MasterVolume);
        }
        else if (!audioMixer.GetFloat(nameof(MasterVolume), out MasterVolume))
        {
            Debug.LogError("LoadVolumes error loading " + nameof(MasterVolume));
        }

        if (PlayerPrefs.HasKey(nameof(AmbientVolume)))
        {
            AmbientVolume = PlayerPrefs.GetFloat(nameof(AmbientVolume));
            audioMixer.SetFloat(nameof(AmbientVolume), AmbientVolume);
        }
        else if (!audioMixer.GetFloat(nameof(AmbientVolume), out AmbientVolume))
        {
            Debug.LogError("LoadVolumes error loading " + nameof(AmbientVolume));
        }

        if (PlayerPrefs.HasKey(nameof(EffectsVolume)))
        {
            EffectsVolume = PlayerPrefs.GetFloat(nameof(EffectsVolume));
            audioMixer.SetFloat(nameof(EffectsVolume), EffectsVolume);
        }
        else if (!audioMixer.GetFloat(nameof(EffectsVolume), out EffectsVolume))
        {
            Debug.LogError("LoadVolumes error loading " + nameof(EffectsVolume));
        }

        if (PlayerPrefs.HasKey(nameof(MusicVolume)))
        {
            MusicVolume = PlayerPrefs.GetFloat(nameof(MusicVolume));
            audioMixer.SetFloat(nameof(MusicVolume), MusicVolume);
        }
        else if (!audioMixer.GetFloat(nameof(MusicVolume), out MusicVolume))
        {
            Debug.LogError("LoadVolumes error loading " + nameof(MusicVolume));
        }
    }

    public void OnMasterSliderChanged(float value)
    {
        audioMixer.SetFloat(nameof(MasterVolume), ValueToDb(value));
        PlayerPrefs.SetFloat(nameof(MasterVolume), ValueToDb(value));
    }

    public void OnAmbientSliderChanged(float value)
    {
        audioMixer.SetFloat(nameof(AmbientVolume), ValueToDb(value));
        PlayerPrefs.SetFloat(nameof(AmbientVolume), ValueToDb(value));
    }

    public void OnEffectsSliderChanged(float value)
    {
        audioMixer.SetFloat(nameof(EffectsVolume), ValueToDb(value));
        PlayerPrefs.SetFloat(nameof(EffectsVolume), ValueToDb(value));
    }

    public void OnMusicSliderChanged(float value)
    {
        audioMixer.SetFloat(nameof(MusicVolume), ValueToDb(value));
        PlayerPrefs.SetFloat(nameof(MusicVolume), ValueToDb(value));
    }

    private float ValueToDb(float value)
    {
        //[0..1] --> [-80..20]
        return -80f + 100f * Mathf.Sqrt(value);
    }

    private float DbToValue(float dB)
    {
        //[-80..20] --> [0..1]
        return Mathf.Pow((dB + 80f) / 100f, 2f);
    }
}
