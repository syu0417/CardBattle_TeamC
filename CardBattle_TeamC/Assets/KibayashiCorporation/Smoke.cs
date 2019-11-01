using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    // Start is called before the first frame update
    void start()
    {
        
    }

    private void OnEnable()
    {
        AudioManager.Instance.Play_SE_Smoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleSystemStopped()//パーティクル終了
    {
        this.gameObject.SetActive(false);
    }

}
