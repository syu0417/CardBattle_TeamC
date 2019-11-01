using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModel : MonoBehaviour
{
    private ModelAnimation modelanimation;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            this.GetComponent<ModelAnimation>().Walk();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<ModelAnimation>().Jump();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            this.GetComponent<ModelAnimation>().Attack();
        }
        else
        {
            this.GetComponent<ModelAnimation>().Idle();
        }
    } 
}
