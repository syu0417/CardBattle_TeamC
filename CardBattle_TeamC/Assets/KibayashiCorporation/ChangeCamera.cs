using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [Header("メインカメラ")]
    public GameObject MainCamera;

    [Header("サブカメラ")]
    public GameObject SubCamera;

    [Header("キャンバス")]
    public GameObject Canvas;
    // Start is called before the first frame update
    void Awake()
    {
        MainCamera.SetActive(false);
        SubCamera.SetActive(true);
        Canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeCameraFunc("Player");
        }
    }

    public void ChangeCameraFunc(string tag)
    {
        MainCamera.SetActive(true);
        SubCamera.SetActive(false);
        Canvas.SetActive(false);

        if(tag != "Player")
        {
            MainCamera.transform.localRotation = Quaternion.Euler(90.0f,270.0f,0.0f);
        }
    }
}
