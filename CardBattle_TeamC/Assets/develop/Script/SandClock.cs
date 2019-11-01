using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SandClock : MonoBehaviour
{
    Slider _slider1;
    Slider _slider2;

    public bool Mytrun = false;

    public float FoalTime = 5f;


    private Animator animator;

    private const string key_isAni = "isAni";
    private float time = 0f;
    private float _hpcnt = 1f;
    private float timecnt = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _slider1 = GameObject.Find("Slider (1)").GetComponent<Slider>();
        _slider2 = GameObject.Find("Slider (2)").GetComponent<Slider>();

        _hpcnt = _hpcnt / FoalTime;
    }

    float _hp = 0f;

    

    // Update is called once per frame
    void Update()
    {
        if (timecnt < FoalTime)
        {
            time += 1f;
            if (time > 60f)
            {
                _hp += _hpcnt;
                
                time = 0f;
                timecnt += 1f;
            }

        }
        else
        {
            time += 1f;
            if (time > 60f)
            {
                timecnt += 1f;
            }

        }
        if (Mytrun)
        {
            //this.animator.SetBool(key_isAni, true);
            Reset();
            Mytrun = !Mytrun;
        }

        _slider1.value = _hp;
        _slider2.value = _hp;
    }

    public void Reset()
    {
        time = 0f;
        timecnt = 0f;
        _hp = 0f;
       // this.animator.SetBool(key_isAni, false);
        //Myrect.rotation = new Quaternion(0, 0, 0);
        //Debug.Break();
        //this.transform.rotation 
    }
}
