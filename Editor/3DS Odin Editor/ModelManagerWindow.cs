using Global;
using Sirenix.OdinInspector;
using UnityEngine;

public class ModelManagerWindow : BaseWindow
{
    [TitleGroup("模型管理")]
    public Model[] ModelList;
    protected override void OnHierarchyChange()
    {
        base.OnHierarchyChange();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void GetData()
    {
        if (gmGlobal)
        {
            this.ModelList = gmGlobal.ModelList;
        }
    }
    protected override void SetData()
    {
        if (gmGlobal)
        {
            //删除检测
            if (ModelList.Length < gmGlobal.ModelList.Length)
            {
                //元素遍历 找出被删除的元素
                for (int i = 0; i < gmGlobal.ModelList.Length; i++)
                {
                    //如果是最后一个元素被删除 此时ModelList的i元素不存在会报空
                    if (i == gmGlobal.ModelList.Length - 1)
                    {
                        //销毁一切该group内所有相关实例化物体及添加的脚本
                        GameObject.DestroyImmediate(gmGlobal.ModelList[i].ModelGameObject);
                        break;
                    }
                    //如果中间某一个元素被删除
                    if (ModelList[i] != gmGlobal.ModelList[i])
                    {
                        GameObject.DestroyImmediate(gmGlobal.ModelList[i].ModelGameObject);
                        break;
                    }
                }
            }
            //最后同步
            gmGlobal.ModelList = this.ModelList;
        }
    }

}
