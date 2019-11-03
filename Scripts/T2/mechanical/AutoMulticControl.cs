using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class AutoMulticControl :  MonoBehaviour {

    public string motionTrailFilePath;

    public float dx;
    public float dy;
    public float dz;

    public int controlPointRange;
    public List<int> controlPointList;
    public List<XElement> controlLayerList;
    public int DOF;
    public XElement root;

    public bool controlMove;

    public void setMotionTrailFilePath(string filePath)
    {
        motionTrailFilePath = filePath;

    }

    public XElement findXMLChildByFirstAttribute(XElement parent,string firstAttributeValue)
    {
        foreach (XElement child in parent.Elements())
        {
            if(child.HasAttributes)
            {
                if (child.FirstAttribute.Value == firstAttributeValue)
                {
                    return child;
                }
            }
        }
        Debug.Log("not found");
        return null;
    }

    void Start()
    {
        motionTrailFilePath = Application.streamingAssetsPath + "/undercarriage.xml";
        root = LoadXML(motionTrailFilePath);
        XElement inform = root.Element("information");
        XElement DOFInform = inform.Element("DOF");
        XElement rangeInform = inform.Element("range");

        DOF = int.Parse(DOFInform.Value);
        controlPointRange = int.Parse(rangeInform.Value);

        controlPointList = new List<int>();
        controlLayerList = new List<XElement>();
        controlPointList.Clear();
        controlLayerList.Clear();

        //初始化控制点
        for(int i = 0; i < DOF; i++)
        {
            controlPointList.Add(1);
        }

        //初始化相对位置
        Transform part0 = transform.GetChild(0);

        controlLayerList.Add(findXMLChildByFirstAttribute(root, "1"));
        for (int i = 1; i < DOF; i++)
        {
            controlLayerList.Add(findXMLChildByFirstAttribute(controlLayerList[i - 1], "1"));
        }

        XElement partNode      = findXMLChildByFirstAttribute(controlLayerList[DOF - 1], part0.name);

        XElement pXNode = partNode.Element("x");
        XElement pYNode = partNode.Element("y");
        XElement pZNode = partNode.Element("z");
        XElement oANode = partNode.Element("a");
        XElement oBNode = partNode.Element("b");
        XElement oGNode = partNode.Element("g");

        float positionX = float.Parse(pXNode.Value);
        float positionY = float.Parse(pYNode.Value);
        float positionZ = float.Parse(pZNode.Value);
        float rotationA = float.Parse(oANode.Value);
        float rotationB = float.Parse(oBNode.Value);
        float rotationG = float.Parse(oGNode.Value);

        Vector3 pos = part0.transform.position;

        dx = -positionY - pos.x;
        dy = -positionX - pos.y;
        dz =  positionZ - pos.z;

    }

    void Update()
    {
        if (controlMove)
        {
         
            //根据每个自由度的控制节点查找位置
            controlPointList[0] += 3; 
            if (controlPointList[0] >200)
            {
                controlPointList[0] = 200;
            }
            //获取最里层的LayerNode
            controlLayerList[0] = findXMLChildByFirstAttribute(root, controlPointList[0].ToString());  
            for(int i = 1; i < DOF; i++)
            {
            
                controlLayerList[i] = findXMLChildByFirstAttribute(controlLayerList[i - 1], controlPointList[i].ToString());
            }
        
            foreach (Transform child in transform)
            {//foreach2
                XElement partNode = findXMLChildByFirstAttribute(controlLayerList[DOF - 1], child.name);

                XElement pXNode = partNode.Element("x");
                XElement pYNode = partNode.Element("y");
                XElement pZNode = partNode.Element("z");
                XElement oANode = partNode.Element("a");
                XElement oBNode = partNode.Element("b");
                XElement oGNode = partNode.Element("g");
                //XElement oWNode = partNode.Element("w");

                float positionX = float.Parse(pXNode.Value);
                float positionY = float.Parse(pYNode.Value);
                float positionZ = float.Parse(pZNode.Value);
                float rotationA = float.Parse(oANode.Value);
                float rotationB = float.Parse(oBNode.Value);
                float rotationG = float.Parse(oGNode.Value);
                //float rotationW = float.Parse(oWNode.Value);

                //Quaternion quat = child.transform.rotation;
                Vector3 pos = child.transform.position;
                Vector3 rot = child.transform.eulerAngles;

                pos.x = -positionY - dx;
                pos.y = -positionX - dy;
                pos.z = positionZ - dz;
                rot.x = rotationB / Mathf.Deg2Rad;
                rot.y = rotationA / Mathf.Deg2Rad;
                rot.z = -rotationG / Mathf.Deg2Rad;

                Quaternion q = Quaternion.Euler(rot);
                child.transform.position = pos;
                child.transform.eulerAngles = rot;

            }//foreach2
        }
        else
        {

        }   
    }

    private XElement LoadXML(string path)
    {
        XElement xml = XElement.Load(path);
        return xml;
    }
}
