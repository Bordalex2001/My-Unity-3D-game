using System;
using UnityEngine;

public class MenuQualityScript : MonoBehaviour
{
    [SerializeField]
    private Material[] daySkyboxes = new Material[0];
    [SerializeField]
    private Material[] nightSkyboxes = new Material[0];
    private Material defaultSkybox;

    private TMPro.TMP_Dropdown graphicsDropdown;
    private TMPro.TMP_Dropdown fogDropdown;
    private TMPro.TMP_Dropdown daySkyDropdown;
    private TMPro.TMP_Dropdown nightSkyDropdown;

    void Start()
    {
        Transform layout = transform.Find("Content/Quality/Layout");
        graphicsDropdown = layout.Find("Graphics/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        InitGraphicsDropdown();
        fogDropdown = layout.Find("Fog/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        InitFogDropdown();
        daySkyDropdown = layout.Find("DaySky/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        daySkyDropdown.ClearOptions();
        foreach (Material mat in daySkyboxes)
        {
            daySkyDropdown.options.Add(new(mat.name));
        }
        nightSkyDropdown = layout.Find("NightSky/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        nightSkyDropdown.ClearOptions();
        foreach (Material mat in nightSkyboxes)
        {
            nightSkyDropdown.options.Add(new(mat.name));
        }
        GameEventSystem.AddListener(OnGameStateChanged, nameof(GameState));
    }

    public void OnDaySkyDropdownChanged(int selectedIndex)
    {
        RenderSettings.skybox = daySkyboxes[selectedIndex];
    }

    public void OnNightSkyDropdownChanged(int selectedIndex)
    {
        RenderSettings.skybox = nightSkyboxes[selectedIndex];
    }

    private void InitGraphicsDropdown()
    {
        graphicsDropdown.ClearOptions();
        foreach (string name in QualitySettings.names)
        {
            graphicsDropdown.options.Add(new(name));
        }
        int currentLevelIndex = QualitySettings.GetQualityLevel();
        graphicsDropdown.value = currentLevelIndex;
    }

    public void OnGraphicsDropdownChanged(int selectedIndex)
    {
        QualitySettings.SetQualityLevel(selectedIndex, true);
    }

    private void InitFogDropdown()
    {
        fogDropdown.ClearOptions();
        fogDropdown.options.Add(new("Off"));
        foreach (string name in Enum.GetNames(typeof(FogMode)))
        {
            fogDropdown.options.Add(new(name));
        }
        if (RenderSettings.fog)
        {
            fogDropdown.value = (int)RenderSettings.fogMode;
        }
        else
        {
            fogDropdown.value = 0;
        }
    }

    public void OnFogDropdownChanged(int selectedIndex)
    {
        if (selectedIndex == 0)
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = (FogMode)selectedIndex;
        }
    }

    private void InitSkyDropdowns()
    {
        defaultSkybox = RenderSettings.skybox;

        daySkyDropdown.ClearOptions();
        foreach (Material mat in daySkyboxes)
        {
            daySkyDropdown.options.Add(new(mat.name));
        }

        nightSkyDropdown.ClearOptions();
        foreach (Material mat in nightSkyboxes)
        {
            nightSkyDropdown.options.Add(new(mat.name));
        }

        if (defaultSkybox != null)
        {
            daySkyDropdown.options.Add(new(defaultSkybox.name));
            nightSkyDropdown.options.Add(new(defaultSkybox.name));
        }
    }

    private void OnGameStateChanged(string type, object payload)
    {
        if (nameof(GameState.activeSceneIndex).Equals(payload))
        {
            gameObject.SetActive(GameState.activeSceneIndex == 0);
        }
    }
}