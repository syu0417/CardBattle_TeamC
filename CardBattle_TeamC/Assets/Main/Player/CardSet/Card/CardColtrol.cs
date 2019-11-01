using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardColtrol : MonoBehaviour
{
    public GameObject prehub;

    private bool OnMoveFlg = false;

    private Vector3 NewCardPos = new Vector3();

    [SerializeField]
    private int FloorX = 0;
    [SerializeField]
    private int FloorY = 0;

    const float FloorScale = 10.0f;
    const float ArrowObjY = 0.05f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 選択された場合
        if(this.transform.parent.GetComponent<CardSet>().GetisSelect() &&
            this.transform.parent.GetComponent<CardSet>().GetisMove() == false)
        {
            if(this.transform.childCount <= 0)
            {
                GeneMoveArrow();
            }
        }
        // 選択されてない場合
        else
        {
            // 選択用のオブジェクトがあるなら消す
            if(this.transform.childCount > 0)
            {
                DeleteChildren(this.transform.childCount);
            }
        }

        if(OnMoveFlg)
        {
            Move();

            OnMoveFlg = false;
        }
    }

    public void Move()
    {
        this.transform.position = NewCardPos;

        NewCardPos = Vector3.zero;
    }


    // 移動用のオブジェクトを生成する
    private void GeneMoveArrow()
    {
        // 一時格納用
        GameObject obj;

        // 移動制限のための格納用
        List<int> deletePanelNum = new List<int>();

        int Power = this.transform.parent.GetComponent<CardSet>().GetPower();

        if (Power == 1 || Power == 2)
        {
            for (int height = -1; height < (2); height++)
            {
                for (int width = -1; width < (2); width++)
                {
                    if (Cross(width,height) && (FloorY + height >= 0 && FloorY + height <= 4) &&
                        (FloorX + width >= 0 && FloorX + width <= 6))
                    {
                        obj = Instantiate(prehub, new Vector3(((FloorX + width) * FloorScale), ArrowObjY, ((FloorY + height) * FloorScale)), Quaternion.identity);

                        obj.transform.parent = this.transform;

                        obj.transform.GetComponent<MoveArrowControl>().SetFloorNum((FloorX + width), (FloorY + height));
                    }
                }
            }

            // ゴールに移動できるようにする
            if (FloorX + FloorY * 7 == 14)
            {
                obj = Instantiate(prehub, new Vector3(((FloorX - 1) * FloorScale), ArrowObjY, ((FloorY) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(35, 0);
            }
            else if (FloorX + FloorY * 7 == 20)
            {
                obj = Instantiate(prehub, new Vector3(((FloorX + 1) * FloorScale), ArrowObjY, ((FloorY) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(36, 0);
            }
        }

        else if(Power == 3)
        {
            for (int height = -2; height < 3; height++)
            {
                for (int width = -2; width < 3; width++)
                {
                    if (Cross(width, height) && (FloorY + height >= 0 && FloorY + height <= 4) &&
                        (FloorX + width >= 0 && FloorX + width <= 6)) 
                    {
                        obj = Instantiate(prehub, new Vector3(((FloorX + width) * FloorScale), ArrowObjY, ((FloorY + height) * FloorScale)), Quaternion.identity);

                        obj.transform.parent = this.transform;

                        obj.transform.GetComponent<MoveArrowControl>().SetFloorNum((FloorX + width), (FloorY + height));

                        // 移動制限がかかるかもだから調べる
                        if (height >= -1 && height <= 1 && width >= -1 && width <= 1)
                        {
                            // 通り道にカードがいるならそこまでしか移動できないようにする
                            Ray ray = new Ray(new Vector3(obj.transform.position.x, obj.transform.position.y - 10.0f,
                                obj.transform.position.z), new Vector3(0.0f, 1.0f, 0.0f));

                            foreach (RaycastHit hit in Physics.RaycastAll(ray, 30.0f))
                            {
                                if (hit.collider.tag == "Card")
                                {
                                    deletePanelNum.Add((FloorX + width) + (FloorY + height) * 7);

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // ゴールに移動できるようにする
            if (FloorX + FloorY * 7 == 14 || FloorX + FloorY * 7 == 15)
            {
                obj = Instantiate(prehub, new Vector3(((-1) * FloorScale), ArrowObjY, ((FloorY) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(35, 0);
            }
            else if (FloorX + FloorY * 7 == 20 || FloorX + FloorY * 7 == 19)
            {
                obj = Instantiate(prehub, new Vector3(((7) * FloorScale), ArrowObjY, ((FloorY) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(36, 0);
            }
            
            // 移動制限
            if (deletePanelNum.Count > 0) 
            {
                foreach(int Panel in deletePanelNum)
                {
                    Ray ray1 = new Ray(new Vector3(((Panel % 7 - 1) * FloorScale), -10.0f, ((Panel / 7) * FloorScale)), new Vector3(0.0f, 1.0f, 0.0f));
                    Ray ray2 = new Ray(new Vector3(((Panel % 7 + 1) * FloorScale), -10.0f, ((Panel / 7) * FloorScale)), new Vector3(0.0f, 1.0f, 0.0f));
                    Ray ray3 = new Ray(new Vector3(((Panel % 7) * FloorScale), -10.0f, ((Panel / 7 - 1) * FloorScale)), new Vector3(0.0f, 1.0f, 0.0f));
                    Ray ray4 = new Ray(new Vector3(((Panel % 7) * FloorScale), -10.0f, ((Panel / 7 + 1) * FloorScale)), new Vector3(0.0f, 1.0f, 0.0f));

                    //Debug.DrawRay(ray1.origin, ray1.direction * 1000, Color.red, 5.0f);
                    //Debug.DrawRay(ray2.origin, ray2.direction * 1000, Color.red, 5.0f);
                    //Debug.DrawRay(ray3.origin, ray3.direction * 1000, Color.red, 5.0f);
                    //Debug.DrawRay(ray4.origin, ray4.direction * 1000, Color.red, 5.0f);
                    
                    RaycastHit hit;

                    if (Physics.Raycast(ray1, out hit, 30.0f))
                    {
                        if(hit.collider.tag == "Arrow")
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    if (Physics.Raycast(ray2, out hit, 30.0f))
                    {
                        if (hit.collider.tag == "Arrow")
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    if (Physics.Raycast(ray3, out hit, 30.0f))
                    {
                        if (hit.collider.tag == "Arrow")
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    if (Physics.Raycast(ray4, out hit, 30.0f))
                    {
                        if (hit.collider.tag == "Arrow")
                        {
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }
        }

        else if(Power == 4)
        {
            for (int height = -1; height < 2; height++)
            {
                for (int width = -1; width < 2; width++)
                {
                    if((width != 0 || height != 0) && (FloorY + height >= 0 && FloorY + height <= 4) &&
                        (FloorX + width >= 0 && FloorX + width <= 6))
                    {
                        obj = Instantiate(prehub, new Vector3(((FloorX + width) * FloorScale), ArrowObjY, ((FloorY + height) * FloorScale)), Quaternion.identity);

                        obj.transform.parent = this.transform;

                        obj.transform.GetComponent<MoveArrowControl>().SetFloorNum((FloorX + width), (FloorY + height));
                    }
                }
            }

            // ゴールに移動できるようにする
            if (FloorX + FloorY * 7 == 14 || FloorX + FloorY * 7 == 7 || FloorX + FloorY * 7 == 21)
            {
                obj = Instantiate(prehub, new Vector3(((-1) * FloorScale), ArrowObjY, ((5/2) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(35, 0);
            }
            else if (FloorX + FloorY * 7 == 20 || FloorX + FloorY * 7 == 13 || FloorX + FloorY * 7 == 27)
            {
                obj = Instantiate(prehub, new Vector3(((7) * FloorScale), ArrowObjY, ((5/2) * FloorScale)), Quaternion.identity);

                obj.transform.parent = this.transform;

                obj.transform.GetComponent<MoveArrowControl>().SetFloorNum(36, 0);
            }
        }
    }

    // 移動用オブジェクト生成のための関数
    private bool Cross(int x, int y)
    {
        return (y == 0 && x != 0 || x == 0 && y != 0);
    }


    // 全ての子供を削除
    private void DeleteChildren(int childCount)
    {
        for (int cnt = 0; cnt < childCount; cnt++) 
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }

    public void SetisMove(bool Flg)
    {
        OnMoveFlg = Flg;
    }
    public void SetisMove(bool Flg, int floorNum)
    {
        OnMoveFlg = Flg;

        if (floorNum == 35 || floorNum == 36)
        {

        }
        else
        {
            // 次の床の位置に移動
            FloorY = floorNum / 7;
            FloorX = floorNum % 7;

            NewCardPos = new Vector3((FloorX * 10.0f), 0.1f, (FloorY * 10.0f));
        }
    }

    public int GetNowFloorNum()
    {
        return FloorY * 7 + FloorX;
    }

    public void SetNowFloorNum(int FloorNum)
    {
        FloorX = FloorNum % 7;
        FloorY = FloorNum / 7;
    }
}
