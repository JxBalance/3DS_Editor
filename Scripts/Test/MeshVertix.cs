using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshVertix : MonoBehaviour {

    public GameObject sphere;
    
    class minmax
    {
        public Vector3 min = new Vector3 (10000f,10000f,10000f);
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

    public List<Vector3> _VerticesList;

    // Use this for initialization
    void Start () {

        int i = 0, j = 0, k = 0;
        float x,y,z;

        

        _VerticesList = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);
        //Debug.Log("_rticesLise.length: "+_VerticesList.Count);
        for(i = 0;i < _VerticesList.Count; i++)
        {
            //Debug.Log(transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]));
            x = transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).x;
            y = transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).y;
            z = transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]).z;
            //Debug.Log(x+",  "+y+",  "+z);
        }
        /*
        Vector3[] vertix = GetComponent<MeshFilter>().sharedMesh.vertices;
        Vector3 boundsCenter = GetComponent<MeshFilter>().sharedMesh.bounds.center;
        //Debug.Log(boundsCenter);
        int[] a = GetComponent<MeshFilter>().sharedMesh.triangles;
        for(int i =0;i< GetComponent<MeshFilter>().sharedMesh.triangles.Length;i++)
        {
            Debug.Log(a[i]);
        }

        Vector3 bounds = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        Debug.Log(bounds);
        */

        
        //将重复的点分为一个vertix组
        List<vertix> sameVertix = new List<vertix>();
        for (i = 0; i < _VerticesList.Count; i++)
        {
            if (i == 0)
            {
                sameVertix.Add(new vertix());
                sameVertix[0].position = transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]);
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
                    else if (sameVertix[j].position == transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]))
                    {
                        sameVertix[j].count++;
                        break;
                    }
                }
                if(j == sameVertix.Count)
                {
                    sameVertix.Add(new vertix());
                    sameVertix[j].position = transform.localToWorldMatrix.MultiplyPoint3x4(_VerticesList[i]);
                    sameVertix[j].count++;
                }
            }
        }
        
        List<group> vertixGroup = new List<group>();

        //float max = 0f;
        //float min = 10000f;
        float temp = 0f;

        int sum = 0;
        //bool isBreak = false;
        for (i = 0; i < sameVertix.Count; i++)
        {
            x = sameVertix[i].position.x;
            y = sameVertix[i].position.y;
            z = sameVertix[i].position.z;
            sum += sameVertix[i].count;
            //Debug.Log("No:"+i+"  Count:"+sameVertix[i].count+"   Vertix:"+x+", "+y+", "+z+"       Sum:"+sum);
            
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

                for (k = 0; k < vertixGroup.Count; k++)
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
                    j = 0;
                    temp = Mathf.Pow((sameVertix[i].position.x - vertixGroup[k].vertics[j].position.x), 2) +
                            Mathf.Pow((sameVertix[i].position.y - vertixGroup[k].vertics[j].position.y), 2) +
                            Mathf.Pow((sameVertix[i].position.z - vertixGroup[k].vertics[j].position.z), 2);
                    if (temp > 0.5f)
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
                    vertixGroup[k].nums.Add(i);
                    vertixGroup[k].sum++;
                    vertixGroup[k].vertics.Add(sameVertix[i]);
                    if (k == 0)
                    {
                        //Debug.Log("Max:  " + max + "      Min:  " + min);
                    }
                    break;
                }
                if (k == vertixGroup.Count)
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

        for(i = 0; i < vertixGroup.Count; i++)
        {
            for (j = 0; j < vertixGroup[i].sum; j++)
            {
                x = vertixGroup[i].vertics[j].position.x;
                y = vertixGroup[i].vertics[j].position.y;
                z = vertixGroup[i].vertics[j].position.z;
                Debug.Log("Group"+i+"    " + "Sum:" + vertixGroup[i].sum + "   Num:" + vertixGroup[i].nums[j] + "   Position:" + x + ", " + y + ", " + z + ")");

            }

        }

        CreateSphere(vertixGroup);

    }

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
        float xMax,yMax,zMax,xMin,yMin,zMin,x,y,z;
        for (int i = 0; i < vGroup.Count; i++)
        {
            xMax = axisMinmax[i].max.x;
            yMax = axisMinmax[i].max.y;
            zMax = axisMinmax[i].max.z;
            xMin = axisMinmax[i].min.x;
            yMin = axisMinmax[i].min.y;
            zMin = axisMinmax[i].min.z;
            Debug.Log("Group"+i+"       Max:("+xMax+", "+yMax+", "+zMax+")"+ "     Min:(" + xMin + ", " + yMin + ", " + zMin + ")");

            x = (xMax - xMin) / 2 + xMin;
            y = (yMax - yMin) / 2 + yMin;
            z = (zMax - zMin) / 2 + zMin;

            GameObject go = Instantiate(sphere);
            go.transform.position = new Vector3(x,y,z);
        }


    }

	// Update is called once per frame
	void Update () {
		
	}

}
