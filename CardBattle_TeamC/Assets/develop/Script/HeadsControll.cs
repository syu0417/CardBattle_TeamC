using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsControll : MonoBehaviour
{
    private ChangeHead changehead;

    private int Power2;
    private int Power3;
    // Start is called before the first frame update
    void Start()
    {
        Power2 = 0;
        Power3 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Power2++;
            if (Power2 > this.GetComponent<ChangeHead>().Heads2.Length)
            {
                Power2 = 0;
            }
            this.GetComponent<ChangeHead>().ChangeHeads2(Power2);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Power3++;
            if (Power3 > this.GetComponent<ChangeHead>().Heads2.Length)
            {
                Power3 = 0;
            }
            this.GetComponent<ChangeHead>().ChangeHeads3(Power3);
        }
    }
}
