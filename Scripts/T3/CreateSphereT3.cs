using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphereT3 : MonoBehaviour
{

    public GameObject sphere;

    class minmax
    {
        public Vector3 min = new Vector3(10000f, 10000f, 10000f);
        public Vector3 max = new Vector3(-10000f, -10000f, -10000f);

    }

    class vertix
    {
        //public int num = 0;
        public int count = 0;
        public Vector3 position;
    };

    class group
    {
        public int sum = 0;
        public List<int> nums;
        public List<vertix> vertics;
    };

    public void GetVertix(GameObject pipeParts)
    {
        int i = 0, j = 0, k = 0;
        float x, y, z;

        List<Vector3> _VerticesList = new List<Vector3>(pipeParts.GetComponent<MeshFilter>().sharedMesh.vertices);            //读取MeshFileter上的所有顶点
        for (i = 0; i < _VerticesList.Count; i++)
        {
            x = pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).x;
            y = pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).y;
            z = pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).z;
        }

        List<vertix> sameVertix = new List<vertix>();               //将重复的顶点分为一个vertix
        for (i = 0; i < _VerticesList.Count; i++)
        {
            if (i == 0)
            {
                sameVertix.Add(new vertix());
                sameVertix[0].position = pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]);
                sameVertix[0].count++;
            }
            else
            {
                for (j = 0; j < sameVertix.Count; j++)
                {
                    if (sameVertix[j].count == 0)
                    {
                        break;
                    }
                    else if (sameVertix[j].position == pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]))
                    {
                        sameVertix[j].count++;
                        break;
                    }
                }
                if (j == sameVertix.Count)
                {
                    sameVertix.Add(new vertix());
                    sameVertix[j].position = pipeParts.transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]);
                    sameVertix[j].count++;
                }
            }
        }
        
        List<group> vertixGroup = new List<group>();                //将相邻的点分为一个顶点组
        //float max = 0f;
        //float min = 10000f;
        float temp = 0f;

        int sum = 0;
        //bool isBreak = false;
        for (i = 0; i < sameVertix.Count; i++)             //遍历所有vertix顶点
        {
            x = sameVertix[i].position.x;
            y = sameVertix[i].position.y;
            z = sameVertix[i].position.z;
            sum += sameVertix[i].count;

            if (i == 0)
            {
                vertixGroup.Add(new group());
                vertixGroup[0].nums = new List<int>();
                vertixGroup[0].nums.Add(i);
                vertixGroup[0].sum = 1;
                vertixGroup[0].vertics = new List<vertix>();
                vertixGroup[0].vertics.Add(sameVertix[i]);
            }
            else
            {                
                for (k = 0; k < vertixGroup.Count; k++)             //遍历存在的顶点组，若该顶点符合该组，则加入到当前顶点组中
                {   /*
                    for (j = 0; j < vertixGroup[k].sum; j++)
                    {
                        temp = Mathf.Pow((sameVertix[i].position.x - vertixGroup[k].vertics[j].position.x), 2) +
                                Mathf.Pow((sameVertix[i].position.y - vertixGroup[k].vertics[j].position.y), 2) +
                                Mathf.Pow((sameVertix[i].position.z - vertixGroup[k].vertics[j].position.z), 2);
                        if (temp < min)
                        {
                            min = temp;
                        }
                        if (temp > max)
                        {
                            if (temp > 10 * max && max != 0)
                            {
                                break;
                            }
                            max = temp;
                        }
                    }
                    if (j == vertixGroup[k].sum)
                    {
                        vertixGroup[k].nums.Add(i);
                        vertixGroup[k].sum++;
                        vertixGroup[k].vertics.Add(sameVertix[i]);
                        break;
                    }
                    */
                    j = 0;                                                                                      //仅跟每个组的第一个顶点进行比较，若差异较大则进入下一个顶点组进行比较
                    temp = Mathf.Pow((sameVertix[i].position.x - vertixGroup[k].vertics[j].position.x), 2) +
                            Mathf.Pow((sameVertix[i].position.y - vertixGroup[k].vertics[j].position.y), 2) +
                            Mathf.Pow((sameVertix[i].position.z - vertixGroup[k].vertics[j].position.z), 2);

                    if (temp > 0.5f)          //判定因子，若大于则进入下一顶点组
                    {
                        continue;
                    }
                    /*
                    if (temp > max)
                    {
                        if (temp > 0.01f)   
                        {
                            continue;
                        }
                        max = temp;
                    }
                    if (temp < min)
                    {
                        min = temp;
                    }
                    */
                    vertixGroup[k].nums.Add(i);                     //若将顶点分入该顶点组，则直接进入下一个顶点进行分类
                    vertixGroup[k].sum++;
                    vertixGroup[k].vertics.Add(sameVertix[i]);
                    break;
                }
                if (k == vertixGroup.Count)                         //若不属于存在的任何顶点组，则创建新的顶点组
                {
                    vertixGroup.Add(new group());
                    vertixGroup[k].nums = new List<int>();
                    vertixGroup[k].nums.Add(i);
                    vertixGroup[k].sum = 1;
                    vertixGroup[k].vertics = new List<vertix>();
                    vertixGroup[k].vertics.Add(sameVertix[i]);
                }

            }

        }

        VertixGroupSort(vertixGroup);                      //顶点组排序

        for (i = 0; i < vertixGroup.Count; i++)                 //分类输出测试
        {
            for (j = 0; j < vertixGroup[i].sum; j++)
            {
                x = vertixGroup[i].vertics[j].position.x;
                y = vertixGroup[i].vertics[j].position.y;
                z = vertixGroup[i].vertics[j].position.z;
                //Debug.Log("Group" + i + "    " + "Sum:" + vertixGroup[i].sum + "   Num:" + vertixGroup[i].nums[j] + "   Count:" + vertixGroup[i].vertics[j].count + "   Position:" + x + ", " + y + ", " + z + ")");
            }
        }

        CreateSphere(vertixGroup);              //在场景中创建圆点，一个圆点对应一个顶点组  

    }

    //顶点组排序
    void VertixGroupSort(List<group> vertixGroup)
    {
        int i, j;
        group top = new group();
        group end = new group();
        top.sum = -1;
        end.sum = -1;
        /*
        for (i = 0; i < vertixGroup.Count; i++)
        {
            Debug.Log("Group:"+i+"       Count:"+vertixGroup[i].vertics[0].count);
            if (vertixGroup[i].vertics[0].count == 2)
            {
                if (top.sum == -1)
                {
                    top = vertixGroup[i];
                    for (j = i; j > 0; j--)
                    {
                        vertixGroup[j] = vertixGroup[j - 1];
                    }
                    vertixGroup[0] = top;
                }
                else if(end.sum == -1)
                {
                    end = vertixGroup[i];
                    for (j = i; j < vertixGroup.Count-1; j++)
                    {
                        vertixGroup[j] = vertixGroup[j + 1];
                    }
                    vertixGroup[j] = end;
                }
            }
        }*/
        group temp = new group();
        for (i = 0; i < vertixGroup.Count; i++)
        {
            for (j = 0; j < vertixGroup.Count-i - 1; j++)
            {
                if (vertixGroup[j].vertics[0].count > vertixGroup[j + 1].vertics[0].count)
                {
                    temp = vertixGroup[j];
                    vertixGroup[j] = vertixGroup[j+1];
                    vertixGroup[j+1] = temp;
                }
            }
        }
        if (vertixGroup.Count > 2)
        {
            end = vertixGroup[1];
            for(i=1; i< vertixGroup.Count-1; i++)
            {
                vertixGroup[i] = vertixGroup[i + 1];
            }
            vertixGroup[vertixGroup.Count - 1] = end;
        }
        for (i = 0; i < vertixGroup.Count; i++)
        {
            Debug.Log("Group:" + i + "       Count:" + vertixGroup[i].vertics[0].count);
        }
    }

    //创建圆点
    void CreateSphere(List<group> vGroup)
    {
        minmax[] axisMinmax = new minmax[vGroup.Count];
        for (int i = 0; i < vGroup.Count; i++)
        {
            axisMinmax[i] = new minmax();
            for (int j = 0; j < vGroup[i].vertics.Count; j++)
            {
                if (vGroup[i].vertics[j].position.x > axisMinmax[i].max.x)
                {
                    axisMinmax[i].max.x = vGroup[i].vertics[j].position.x;

                }
                if (vGroup[i].vertics[j].position.y > axisMinmax[i].max.y)
                {
                    axisMinmax[i].max.y = vGroup[i].vertics[j].position.y;

                }
                if (vGroup[i].vertics[j].position.z > axisMinmax[i].max.z)
                {
                    axisMinmax[i].max.z = vGroup[i].vertics[j].position.z;

                }
                if (vGroup[i].vertics[j].position.x < axisMinmax[i].min.x)
                {
                    axisMinmax[i].min.x = vGroup[i].vertics[j].position.x;

                }
                if (vGroup[i].vertics[j].position.y < axisMinmax[i].min.y)
                {
                    axisMinmax[i].min.y = vGroup[i].vertics[j].position.y;

                }
                if (vGroup[i].vertics[j].position.z < axisMinmax[i].min.z)
                {
                    axisMinmax[i].min.z = vGroup[i].vertics[j].position.z;

                }
            }
        }
        float xMax, yMax, zMax, xMin, yMin, zMin, x, y, z;
        for (int i = 0; i < vGroup.Count; i++)
        {
            xMax = axisMinmax[i].max.x;
            yMax = axisMinmax[i].max.y;
            zMax = axisMinmax[i].max.z;
            xMin = axisMinmax[i].min.x;
            yMin = axisMinmax[i].min.y;
            zMin = axisMinmax[i].min.z;
            //Debug.Log("Group" + i + "       Max:(" + xMax + ", " + yMax + ", " + zMax + ")" + "     Min:(" + xMin + ", " + yMin + ", " + zMin + ")");

            x = (xMax - xMin) / 2 + xMin;
            y = (yMax - yMin) / 2 + yMin;
            z = (zMax - zMin) / 2 + zMin;

            GameObject go = Instantiate(sphere);
            go.transform.position = new Vector3(x, y, z);
            string newName = "sphere0" + i;
            go.name = newName;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
