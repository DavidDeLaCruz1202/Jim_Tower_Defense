using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider_master;
    [SerializeField] private Slider slider_music;
    [SerializeField] private Slider slider_soundfx;

    public TextMeshProUGUI volumeText_master;
    public TextMeshProUGUI volumeText_music;
    public TextMeshProUGUI volumeText_soundfx;


    private void Start()
    {
        CheckPreferences();
        SetVolumes();
    }

    public void SetVolume_Master()
    {
        float master_volume = slider_master.value;
        mixer.SetFloat("master", Mathf.Log10(master_volume) * 20);
        float volPercent = slider_master.value * 100;
        volumeText_master.SetText($"{volPercent.ToString("N0")}" + "%"); // Prevents decimal points
    
        // Save this volume to load it back in next time
        PlayerPrefs.SetFloat("volume_master", master_volume);
    }

    public void SetVolume_Music()
    {
        float music_volume = slider_music.value;
        mixer.SetFloat("music", Mathf.Log10(music_volume) * 20);
        float volPercent = slider_music.value * 100;
        volumeText_music.SetText($"{volPercent.ToString("N0")}" + "%"); // Prevents decimal points
    
        // Save this volume to load it back in next time
        PlayerPrefs.SetFloat("volume_music", music_volume);
    }

    public void SetVolume_SoundFX()
    {
        float soundfx_volume = slider_soundfx.value;
        mixer.SetFloat("soundfx", Mathf.Log10(soundfx_volume) * 20);
        float volPercent = slider_soundfx.value * 100;
        volumeText_soundfx.SetText($"{volPercent.ToString("N0")}" + "%"); // Prevents decimal points
    
        // Save this volume to load it back in next time
        PlayerPrefs.SetFloat("volume_soundfx", soundfx_volume);
    }

    private void SetVolumes()
    {
        SetVolume_Master();
        SetVolume_Music();
        SetVolume_SoundFX();
    }

    // Updates volume sliders to previously saved settings
    private void CheckPreferences()
    {
        if (PlayerPrefs.HasKey("volume_master"))
            slider_master.value = PlayerPrefs.GetFloat("volume_master");

        if (PlayerPrefs.HasKey("volume_music"))
            slider_music.value = PlayerPrefs.GetFloat("volume_music");
        
        if (PlayerPrefs.HasKey("volume_soundfx"))
            slider_soundfx.value = PlayerPrefs.GetFloat("volume_soundfx");
    }
}
