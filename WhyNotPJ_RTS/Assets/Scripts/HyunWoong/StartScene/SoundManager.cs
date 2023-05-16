using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    private Slider[] _sliders;

    private void Awake()
    {
        _sliders = GetComponentsInChildren<Slider>();
    }

    public void MasterVolumeChange()
    {
        _audioMixer.SetFloat("Master", _sliders[0].value);
    }

    public void BGMVolumeChange()
    {

        _audioMixer.SetFloat("BGM", _sliders[1].value);
    }

    public void SFXVolumeChange()
    {

        _audioMixer.SetFloat("SFX", _sliders[2].value);
    }
}
