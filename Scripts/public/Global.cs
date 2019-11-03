using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Global
{
    public class ResourcesPath
    {
        public const string GameManagerGameObject = "GameObject/GameManager";
        public const string GameObserverGameObject = "GameObject/GameObserver";
        public const string StartUICanvasGameObject = "GameObject/DesktopStartCanvas";
        public const string InfoBoardParentGameObject = "GameObject/InfoBoardParent";
        public const string UnitParentGameObject = "GameObject/UnitParent";
        public const string EventSystemGameObject = "GameObject/EventSystem";
        public const string DesktopGameCanvasGameObject = "GameObject/DesktopGameCanvas";
        public const string VRTK_SDKManagerGameObject = "GameObject/[VRTK_SDKManager]";
        public const string VRTK_ScriptsGameObject = "GameObject/[VRTK_Scripts]";


        public const string UnitGameObject = "GameObject/UnitLabel";
        public const string UnitButtonGameObject = "GameObject/UnitButtonT1";
        public const string DRUnitButtonT2GameObject = "GameObject/T2/DRUnitButtonT2"; //作动 驱动零件button 
        public const string DAUnitButtonT2GameObject = "GameObject/T2/DAUnitButtonT2"; //拆装 零件button

    }

    public class BaseManager
    {
        protected GamaManagerGlobal GM
        {
            get { return GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamaManagerGlobal>(); }
        }
    }

    [System.Serializable]
    public class BasicSetting
    {
        [Rename("课件名称")] public string SceneName = "";
        [Multiline] public string SceneRemarks = "";

        public ClientMode[] ClientArray = {ClientMode.教员端, ClientMode.学员端, ClientMode.观察端, ClientMode.观察端};
    }

    [System.Serializable]
    public class Model
    {
        private bool CanTransform
        {
            get
            {
                if (mModelGameObject.GetComponent<MultiControlerBase>() == null)
                {
                    return true;
                }
                else
                {
                    modelInfo.ResetTransform();
                    return false;
                }
            }
        }

        [SerializeField] [HideInInspector] private GameObject mModelGameObject;
        [SerializeField] [HideInInspector] private string mModelName = "";
        [SerializeField] [HideInInspector] private Vector3 mModelPosition = Vector3.zero;
        [SerializeField] [HideInInspector] private Vector3 mModelEulerangle = Vector3.zero;


        //模型脚本
        [HideInInspector] public ModelInfoT1 modelInfo;
#if UNITY_EDITOR
        [HorizontalGroup("Split", 55), PropertyOrder(-1)]
        [PreviewField(50, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel]
        [EnableIf("enableAddModel", true)]
        public GameObject ModelGameObject
        {
            get { return mModelGameObject; }
            set
            {
                if (!value) return;
                mModelGameObject = GameObject.Instantiate(value);
                ModelInfoT1 modelInfo = mModelGameObject.AddComponent<ModelInfoT1>();
                modelInfo.mModelName = mModelName;
                modelInfo.mInitialPosition = mModelGameObject.transform.position;
                modelInfo.mInitialEulerAngles = mModelGameObject.transform.eulerAngles;
                mModelGameObject.transform.position = mModelPosition;
                mModelGameObject.transform.eulerAngles = mModelEulerangle;
                enableAddModel = false;
            }
        }

        [FoldoutGroup("Split/$名称", false)]
        public string 名称
        {
            get { return mModelName; }
            set
            {
                mModelName = value;
                if (modelInfo)
                {
                    modelInfo.mModelName = mModelName;
                }
            }
        }

        [FoldoutGroup("Split/$名称", false)]
        [EnableIf("CanTransform")]
        public Vector3 位置
        {
            get { return mModelPosition; }
            set
            {
                mModelPosition = value;
                if (mModelGameObject)
                {
                    mModelGameObject.transform.position = mModelPosition;
                }
            }
        }

        [FoldoutGroup("Split/$名称", false)]
        [EnableIf("CanTransform")]
        public Vector3 旋转
        {
            get { return mModelEulerangle; }
            set
            {
                mModelEulerangle = value;
                if (mModelGameObject)
                {
                    mModelGameObject.transform.eulerAngles = mModelEulerangle;
                }
            }
        }
#endif

        private bool enableAddModel = true;


    }

    [System.Serializable]
    public class IntroBoard
    {
        [Rename("标题")] public string BoardTitle = "";
        [Rename("文字介绍")] public string BoardText = "";
        [Rename("图片介绍")] public Texture2D BoardTexture;
    }


    /// <summary>
    /// 部件识别 部件组
    /// </summary>
    [System.Serializable]
    public class UnitGroup : BaseManager
    {
        /// <summary>
        /// 组内是否存在元素bool方法 用于切换编辑器按钮显示状态
        /// </summary>
        private bool ExistGameObject
        {
            get
            {
                if (unitMembers.Count == 0 || unitMembers == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 部件组成员列表
        /// </summary>
        [HideInInspector] public List<UnitMemberT1> unitMembers = new List<UnitMemberT1>();

        //PC界面点击按钮
        [HideInInspector] public GameObject button;

        //部件组脚本
        [HideInInspector] public UnitGroupT1 unitGroup;
#if UNITY_EDITOR
        [DisableIf("ExistGameObject")]
        [HorizontalGroup("Split")]
        [Button(ButtonSizes.Large)]
        private void 设为部件组()
        {
            if (UnityEditor.Selection.gameObjects.Length != 0)
            {
                bool canAdd = true;
                foreach (GameObject go in UnityEditor.Selection.gameObjects)
                {
                    if (go.GetComponent<UnitMemberT1>())
                    {
                        canAdd = false;
                    }
                }
                if (canAdd)
                {
                    GameObject unitLabelGameObject =
                        GameObject.Instantiate(Resources.Load(ResourcesPath.UnitGameObject),
                            GM.UnitParentGameObject.transform) as GameObject;
                    unitGroup = unitLabelGameObject.GetComponent<UnitGroupT1>();

                    unitMembers.Clear();
                    foreach (GameObject go in UnityEditor.Selection.gameObjects)
                    {
                        go.AddComponent<MeshCollider>();
                        UnitMemberT1 mem = go.AddComponent<UnitMemberT1>();
                        mem.@group = unitGroup;
                        unitGroup.unitMembers.Add(mem);
                        unitMembers = unitGroup.unitMembers;
                        //Debug.Log("unitMembers包含" + unitMembers.Count + "个元素");
                    }

                    button = GameObject.Instantiate(Resources.Load(ResourcesPath.UnitButtonGameObject) as GameObject,
                        GM.DesktopGameCanvasGameObject.transform.Find("UnitPanel").Find("Bottom").Find("List")
                            .Find("Panel"));
                    button.transform.localScale = new Vector3(1, 1, 1);
                    button.GetComponent<ForUnitButtonT1>().group = unitGroup;
                }
            }
            else
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "未选择可编辑的部件", "确定");
            }
        }

        [EnableIf("ExistGameObject")]
        [HorizontalGroup("Split")]
        [Button(ButtonSizes.Large)]
        private void 选中部件组()
        {
            if (unitMembers.Count == 0)
            {
                UnityEditor.Selection.objects = null;
            }
            else
            {
                List<UnityEngine.Object> objList = new List<UnityEngine.Object>();
                foreach (var unitMember in unitMembers)
                {
                    objList.Add(unitMember.gameObject);
                }
                UnityEditor.Selection.objects = objList.ToArray();
            }
        }
#endif

        [EnableIf("ExistGameObject")] [Rename("部件组名称")] public string unitName = "";
        [EnableIf("ExistGameObject")] [Rename("部件组说明")] public string unitContent = "";
        [EnableIf("ExistGameObject")] [Rename("高亮颜色")] public Color unitColor = Color.red;
        [EnableIf("ExistGameObject")] [Rename("插入图片")] public Texture2D unitLoadtexture2D;
        [EnableIf("ExistGameObject")] [Rename("插入语音")] public AudioClip unitAudioClip;
        [EnableIf("ExistGameObject")] [TableList] [ListDrawerSettings(ShowIndexLabels = true)]
        public List<ObservePoint> 观察点列表 = new List<ObservePoint>();


        private List<Vector3> observePointPositionList
        {
            get
            {
                List<Vector3> pList = new List<Vector3>();
                foreach (var observePoint in 观察点列表)
                {
                    pList.Add(observePoint.position);
                }
                return pList;
            }
        }

        private List<Vector3> observePointRotationList
        {
            get
            {
                List<Vector3> rList = new List<Vector3>();
                foreach (var observePoint in 观察点列表)
                {
                    rList.Add(observePoint.eulerAngle);
                }
                return rList;
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void LoadUnitGroupData()
        {
            unitGroup.SaveUnitGroupValue(unitName, unitContent, unitColor, unitLoadtexture2D,
                observePointPositionList, observePointRotationList, unitMembers, button);
            unitGroup.MemberInfoSave();
        }

        /// <summary>
        /// 销毁一切该group内所有相关实例化物体及添加的脚本
        /// </summary>
        public void DestroyGroup()
        {
            //销毁组内物体添加的脚本和组建
            foreach (var mem in unitMembers)
            {
                GameObject.DestroyImmediate(mem.GetComponent<MeshCollider>());
                GameObject.DestroyImmediate(mem);
            }
            //销毁实例化的物体
            GameObject.DestroyImmediate(button); //PC端的物体按钮
            GameObject.DestroyImmediate(unitGroup.gameObject); //该组对应的空物体 包含该组的相关功能一并随物体销毁
        }
    }

    [System.Serializable]
    public class ObservePoint
    {
        [ReadOnly] public string 位置信息 = Vector3.zero.ToString();

        [HideInInspector] public Vector3 position;
        [HideInInspector] public Vector3 eulerAngle;

        private Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                位置信息 = value.ToString();
            }
        }

        private Vector3 EulerAngle
        {
            get { return eulerAngle; }
            set { eulerAngle = value; }
        }

#if UNITY_EDITOR
        [TableColumnWidth(200)]
        [HorizontalGroup("操作")]
        public void 设置()
        {
            UnityEngine.Object[] tempObjects = UnityEditor.Selection.objects;
            GameObject camGameObject = GameObject.FindGameObjectWithTag("MainCamera");
            UnityEditor.Selection.activeGameObject = camGameObject;
            UnityEditor.EditorApplication.ExecuteMenuItem("GameObject/Align With View");
            UnityEditor.Selection.objects = tempObjects;
            Position = camGameObject.transform.position;
            EulerAngle = camGameObject.transform.eulerAngles;
            //Debug.Log("设置观察点位置" + position);
        }
#endif
    }

    public enum ClientMode
    {
        教员端 = 0,
        学员端 = 1,
        观察端 = 2
    }

    public enum OperateMode
    {
        桌面式 = 0,
        头盔式 = 1,
    }


    public enum ExamMode
    {
        教学模式 = 0,
        考核模式 = 1,
    }


    #region T2

    /*********T2*********T2***********T2***********T2*************T2***************T2*******T2**********/
    [System.Serializable]
    public class Drive : BaseManager
    {

        // public GamaManagerGlobal GM;
        [HorizontalGroup("Split")] [Rename("作动机构")] [ReadOnly] public GameObject mechanismMode;

        [HideInInspector] public string motionTrailFilePath;
        [HideInInspector] public List<string> actuatorModelName; //各个驱动件的名称
        [HideInInspector] public MultiControlerBase multiControlerBase;

#if UNITY_EDITOR
        private bool ExistGameObject
        {
            get { return mechanismMode != null; }
        }
        [Button("选择作动机构")]
        [HorizontalGroup("Split")]
        [DisableIf("ExistGameObject")]
        public void SelectDriveModel()
        {
            UnityEditor.EditorWindow window = UnityEditor.EditorWindow.GetWindow<SelectModelT2>(true, "选择作动机构");
            window.minSize = new Vector2(300f, 400f);
            window.maxSize = new Vector2(300f, 400f);
            window.Show();
            SelectModelT2.Instance.CurrentDrive = this;
        }
#endif

        public List<string> ActuatorModelName
        {
            get { return actuatorModelName; }
        }

        [TabGroup("作动演示模式")] [Rename("速度")] public int Velocity;
        [TabGroup("作动演示模式")] [Rename("作动开关")] public GameObject DriveStart;

#if UNITY_EDITOR
        [TabGroup("作动演示模式")]
        [Button("添加")]
        public void AutoDriveAdd()
        {
            //模型名称添加到界面
        }


        [TabGroup("分步作动模式")]
        [Button("添加驱动件")]
        public void Add()
        {
            actuatorModelName = new List<string>();
            Transform[] selected = UnityEditor.Selection.transforms;
            if (selected.Length != 0)
            {
                string selected_part_name = selected[0].name;
                if (selected.Length == 0)
                {
                    //提示
                    UnityEditor.EditorUtility.DisplayDialog("提示", "未选择驱动零件", "确定");
                }
                else
                {
                    for (int i = 0; i < selected.Length; i++)
                    {
                        actuatorModelName.Add(selected[i].name);
                        已添加驱动零件列表 += selected[i].name;
                        已添加驱动零件列表 += "\n";
                    }
                }
                ;
            }

            //选择已导入的可作动的机构的零件的名称或直接在场景里选择
        }
#endif
        [TabGroup("分步作动模式")] [DisableInEditorMode] [MultiLineProperty(5)] [Rename("已添加驱动零件列表")] //添加所加入零件的名字
        public string 已添加驱动零件列表;

        [HideInInspector] public GameObject newButtonPC;
        [HideInInspector] public GameObject newButtonVR;

        [Button("添加")]
        public void DriveAdd()
        {
            //模型名称添加到界面
            if (mechanismMode)
            {
                //add controler
                //mechanismMode.AddComponent<MultiControlerBase>().drive = this;
                //mechanismMode.GetComponent<MultiControlerBase>().setMotionTrailFilePath(motionTrailFilePath);
                //Transform PCparent = GM.DesktopGameCanvasGameObject.transform.Find("DrivePanelT2").Find("List").Find("Panel");
                //Transform VRparent = GM.VRTK_SDKManagerGameObject.transform.Find("SDKSetups").Find("SteamVR").Find("[CameraRig]").Find("VRGameCanvasT1").Find("DrivePanelT2").Find("List").Find("Panel");
                //for (int i = 0; i < actuatorModelName.Count; i++)
                //{
                //    //newButtonPC = GameObject.Instantiate(Resources.Load(ResourcesPath.DRUnitButtonT2GameObject) as GameObject, PCparent);
                //    //newButtonPC.transform.GetChild(0).GetComponent<Text>().text = actuatorModelName[i];
                //    //newButtonPC.transform.GetComponent<DRUnitButtonT2>().setButtonId(i);

                //    //newButtonVR = GameObject.Instantiate(Resources.Load(ResourcesPath.DRUnitButtonT2GameObject) as GameObject, VRparent);
                //    //newButtonVR.transform.GetChild(0).GetComponent<Text>().text = actuatorModelName[i];
                //    //newButtonVR.transform.transform.GetComponent<DRUnitButtonT2>().setButtonId(i);
                //}
            }
        }
#if UNITY_EDITOR
        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("错误操作提示", UnityEditor.MessageType.Info);
        }
#endif
    }



    [System.Serializable]
    public class Disassembly
    {

        [Rename("拆装模型")] public GameObject DisassemblyModleName; //选择已导入的可作动的机构的名称**********************


        [InlineButton("Add")] [Rename("拆装零件")] public GameObject PartName; //选择已导入的机构的零件的名称或直接在场景里选择

        public void Add()
        {
            //模型名称添加到界面
        }



        [InfoBox("已添加驱动零件列表")] //添加所加入零件的名字
        // [EnumPaging]
        // Rename("选择拆装工具")]
        public SomeEnum 拆装工具;

        public enum SomeEnum
        {
            手,
            扳手,
            螺丝刀
        } //选择拆装工具


        [Rename("拆装模式")] public TextAsset AddDisassemblyScrip; //简化可取消该功能



        [InlineButton("Start")] [Title("从当前位置开始")] [HideLabel] public Vector3 StartPosition;

        private void Start()
        {

        }

        [InlineButton("end")] [Title("以当前位置结束")] [HideLabel] public Vector3 EndPosition;

        private void end()
        {

        }

        [Button("添加")]
        private void DissassemblyAdd()
        {

        }
#if UNITY_EDITOR
        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("错误操作提示", UnityEditor.MessageType.Info);
        }
#endif

    }


    /*********T2*********T2***********T2***********T2*************T2***************T2*******T2**********/

    #endregion


    #region T3

    [System.Serializable]
    public class PipeGroup
    {
        [Rename("管道组名称")] public string PipeGroupName = "";
        [Rename("选中管道")] public GameObject Pipes;

        public GameObjectList[] 管道列表;

    }

    [System.Serializable]
    public struct GameObjectList
    {
        [HorizontalGroup("Split", 55), PropertyOrder(-1)]
        [PreviewField(50, Sirenix.OdinInspector.ObjectFieldAlignment.Left), HideLabel]
        public UnityEngine.MonoBehaviour SomeObject;

        [FoldoutGroup("Split/$Name", true)] public string 名称;

        private string Name
        {
            get { return this.SomeObject ? this.SomeObject.name : ""; }
        }
    }

    [System.Serializable]
    public class Hydraulic
    {
        [Rename("流动状态")] public string HydraulicTitle = "";

        [EnumToggleButtons] public SomeEnum 流动形式;

        public enum SomeEnum
        {
            气体,
            液体
        }

        [Space(10)] [TableList] [ListDrawerSettings(ShowIndexLabels = true)]
        public List<AddedPipeGroup> 包含管道组 = new List<AddedPipeGroup>();

        [Space(10)] [TableList] public List<PipeLine> 流动线路 = new List<PipeLine>();

        /*
        [Space(10)]*/
        [Button("自动匹配流水点")]
        private void PipeLinePoint()
        {

        }

        /*
        [Space(10)]*/
        [Button("自动生成流水线")]
        private void CreatePipeLine()
        {

        }

    }

    [System.Serializable]
    public class AddedPipeGroup
    {
        [HorizontalGroup("管道组名称")] public SomeBitmaskEnum 管道组;

        [System.Flags]
        public enum SomeBitmaskEnum
        {

        }
    }

    [System.Serializable]
    public class PipeLine
    {
        [HorizontalGroup("流水点1")] public SomeBitmaskEnum 流水点1;

        [TableColumnWidth(30)] [HorizontalGroup("流向")] public string to;

        [HorizontalGroup("流水点2")] public SomeBitmaskEnum 流水点2;

        [System.Flags]
        public enum SomeBitmaskEnum
        {

        }
    }
    #endregion
}




