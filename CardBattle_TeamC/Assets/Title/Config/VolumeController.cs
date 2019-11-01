using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public enum VolumeType { BGM, SE }

    [SerializeField]
    VolumeType volumeType = 0;
    Slider slider;
    AudioManager audioManager;
    void Start()
    {
        slider = GetComponent<Slider>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void OnValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.BGM:
                audioManager.Bgm_Volume = slider.value;
                break;
            case VolumeType.SE:
                audioManager.Se_Volume = slider.value;
                break;
        }
    }
}