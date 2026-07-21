using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class settingsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        // Optional: set the initial slider value
        // musicSlider.value = 1f;
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }

    private void SetMusicVolume(float value)
    {
        if (audioManager.Instance != null)
        {
            audioManager.Instance.SetMusicVolume(value);
        }
    }
}
