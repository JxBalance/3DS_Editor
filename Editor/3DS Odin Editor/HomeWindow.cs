using System;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class HomeWindow : ScriptableObject
{
    private MainWindow mainWindow;
    [NonSerialized]
    public bool ShowEditor;

    [ShowIf("ShowEditor", false)]
    public string titleText = "  中国商飞交互式三维虚拟课件开发平台";


    [ShowIf("ShowEditor", false),]
    public Texture2D titleIcon;

    [ShowIf("ShowEditor", false)]
    [HideReferenceObjectPicker]
    public List<Section> Examples = new List<Section>();

    [HideIf("ShowEditor")]
    [OnInspectorGUI]

    void OnInspectorGUI()
    {
        var title = new GUIStyle(SirenixGUIStyles.SectionHeaderCentered);
        var subTitle = new GUIStyle(SirenixGUIStyles.SectionHeader);
        GUILayout.Space(20);
        GUILayout.Label(new GUIContent(titleText, titleIcon), title,GUILayout.Height(200));
        GUILayout.Space(40);
        var padding = new GUIStyle() { padding = new RectOffset(20, 20, 20, 20) };
        foreach (var item in this.Examples)
        {
            if (item.VerticalResources) { GUILayout.BeginVertical(padding); } else { GUILayout.BeginHorizontal(padding); }
            {
                // bg
                EditorGUI.DrawRect(GUIHelper.GetCurrentLayoutRect().Padding(-10, -10, 5, 5), SirenixGUIStyles.BoxBackgroundColor);

                // Draw texts
                GUILayout.BeginVertical();
                {
                    GUILayout.Label(item.Title, subTitle);
                    GUILayout.Label(item.SubTitle, SirenixGUIStyles.MultiLineLabel);
                }
                GUILayout.EndVertical();

            }
            if (item.VerticalResources) { GUILayout.EndVertical(); } else { GUILayout.EndHorizontal(); }
        }
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(250);
            if (GUILayout.Button("创建课件", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("提示", "确定要新建课件吗？", "确定", "取消"))
                {
                    EditorApplication.delayCall += NewScene;
                }
                
            }
            GUILayout.Space(250);
        }
        GUILayout.EndHorizontal();

    }

    void NewScene()
    {
        //新建
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        GameObject GMGO = Instantiate(Resources.Load(ResourcesPath.GameManagerGameObject)) as GameObject;
        GamaManagerGlobal gm = GMGO.GetComponent<GamaManagerGlobal>();
        UIManagerGlobal um = gm.GetComponent<UIManagerGlobal>();

        gm.InitGameObjects(Instantiate(Resources.Load(ResourcesPath.GameObserverGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.StartUICanvasGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.UnitParentGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.EventSystemGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.DesktopGameCanvasGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.VRTK_SDKManagerGameObject)) as GameObject,
            Instantiate(Resources.Load(ResourcesPath.VRTK_ScriptsGameObject)) as GameObject);
        um.InitUICanvas(gm.StartUICanvasGameObject, gm.DesktopGameCanvasGameObject,
            gm.VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).gameObject,
            gm.VRTK_SDKManagerGameObject.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(0)
                .GetChild(0).gameObject);

        mainWindow.RestartWindow();
    }


    public HomeWindow(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    void Awake()
    {
        Examples.Add(new Section("位置识别",
            "位置识别模块主要为满足快速制作开发飞机外部及客舱课件中有关介绍、布局、交互等功能定制。其面向的课件有：飞机总体介绍（飞机基本信息、总体布局、基本尺寸），客舱介绍，客舱布局介绍，行李架等部件交互，机务和飞机的绕机检查演示与交互，全机天线位置识别，全机照明灯光的布局、位置识别，全机舱门口盖位置识别，舱门口盖打开/关闭交互等，E/E舱内布置，设备位置识别等。"));
        Examples.Add(new Section("机构作动与拆装",
            "机构作动课件包括起落架、襟缝翼收放机构作动课件和舱门机构作动课件。课件制作的主要目的是展示飞机机构作动原理。\n 拆卸/安装课件包括飞行控制板拆装课件、襟缝翼电子控制装置拆装课件与FQC拆装课件。课件着重拆装过程演示与交互，包括拆装顺序安排、拆装工具的使用，拆卸零件的安放等。课件提供拆装过程的模拟，用户可以通过鼠标、手柄以及其他交互方式，在过程展示课件及提示信息的指导下，选择合理的拆装工具、拆装顺序，通过螺钉等连接零件完成拆装过程。"));
        Examples.Add(new Section("气/液流动",
            "气/液流动课件主要展示各气/液流动系统的部件与管道在飞机内部的位置并介绍气液压系统运行的整体原理。设备的位置通过在半透明的飞机机体中高亮显示系统设备所在区域来体现设备在飞机中的大体布局，同时通过选择相应设备可以观察其具体位置并展示设备细节。系统原理展示通过在原理演示场景中观看系统原理的演示，系统原理的演示效果可以通过改变相关部件与设备的属性而发生相应的变化。"));
        Examples.Add(new Section("演示标注",
            "通过鼠标、键盘、3D操作器、手势、语音等交互手段实现讲解过程中实时对课件进行注释、标注。研究基于鼠标操作的对象模型节点的点选、模型高亮显示以及注释内容的显示，研究文字、简图形式的注释内容的实时输入和储存技术，多次培训时调用注释。"));       
    }


    public class Section
    {
        [HideInInspector]
        public bool VerticalResources;

        [HideLabel]
        [VerticalGroup("Split/Text")]
        [SuffixLabel("Title", true)]
        public string Title;

        [HideLabel]
        [Multiline(4)]
        [VerticalGroup("Split/Text")]
        public string SubTitle;

        public Section(string Title, string SubTitle)
        {
            this.Title = Title;
            this.SubTitle = SubTitle;
        }
    }

}
