using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;

//public class SetVolume: MonoBehaviour
//{
//    [SerializeField] Slider volumeSlider;

//    void Start()
//    {
//        if (!PlayerPrefs.HasKey("musicVolume"))
//        {
//            PlayerPrefs.SetFloat("musicVolume", 1);
//            Load();
//        }
//    }

//    public void ChageVolume()
//    {
//        AudioListener.volume = volumeSlider.value;
//        Save();
//    }

//    private void Load()
//    {
//        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

//    }

//    private void Save()
//    {
//        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
//    }
//}

public class SetVolume: MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    private void Awake()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}