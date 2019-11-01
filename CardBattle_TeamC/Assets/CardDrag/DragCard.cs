using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCard : MonoBehaviour
{
    GameObject prehub;
    GameObject SelectFloor;

    // Start is called before the first frame update
    void Start()
    {
        prehub = (GameObject)Resources.Load("SelectFloor");
        SelectFloor = null;
    }

    // Update is called once per frame
    void Update()
    {
        getClickObject();
    }
    
    void OnMouseDrag()
    {
        print("DRAG");

        Vector3 objectPointInScreen
            = Camera.main.WorldToScreenPoint(this.transform.position);

        Vector3 mousePointInScreen
            = new Vector3(Input.mousePosition.x,
                          Input.mousePosition.y,
                          objectPointInScreen.z);

        Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
        this.transform.position = mousePointInWorld;
    }



    // 左クリックしたオブジェクトを取得する関数(3D)
    public GameObject getClickObject()
    {
        GameObject result = null;

        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                if(hit.collider.gameObject.tag == "Floor")
                {
                    if(SelectFloor == null)
                    {
                        SelectFloor = (GameObject)Instantiate(prehub, hit.collider.gameObject.transform.position, Quaternion.identity);
                        SelectFloor.transform.position = new Vector3(SelectFloor.transform.position.x, SelectFloor.transform.position.y + 2.0f,
                            SelectFloor.transform.position.z);
                    }
                    else
                    {
                        SelectFloor.transform.position = hit.collider.gameObject.transform.position;
                        SelectFloor.transform.position = new Vector3(SelectFloor.transform.position.x, SelectFloor.transform.position.y + 2.0f,
                            SelectFloor.transform.position.z);
                    }
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {

        }

        return result;
    }
}
