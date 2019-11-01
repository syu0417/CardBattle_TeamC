using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(RawImage), typeof(VideoPlayer), typeof(AudioSource))]
public class TitleVideoPlay : MonoBehaviour
{
    RawImage image;
    VideoPlayer player;
    float Alpha = 0;
    void Awake()
    {
        image = GetComponent<RawImage>();
        player = GetComponent<VideoPlayer>();
        var source = GetComponent<AudioSource>();
        player.EnableAudioTrack(0, true);
        player.SetTargetAudioSource(0, source);
        this.UpdateAsObservable().Where(_ => Alpha < 255.0f).Subscribe(_ => Alpha += 0.01f);//フェードイン
    }
    void Update()
    {
        
        if (player.isPrepared)
        {
            image.color = new Color(255, 255, 255, Alpha);//フェードイン
            image.texture = player.texture;
        }
    }
}
