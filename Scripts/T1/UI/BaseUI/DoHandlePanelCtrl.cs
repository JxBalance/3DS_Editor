using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoHandlePanelCtrl : BasePanel_T1 {

    [SerializeField]
    protected Text NameTitle;

    [SerializeField]
    protected Button CloseButton;

    [SerializeField]
    protected Button UnitRecognitionButton;

    [SerializeField]
    protected Button ShowModelButton;

    [SerializeField]
    protected Button DriverControlerButton;

    [SerializeField]
    protected Button DisassemblyButton;


    protected UnitGroupT1 currentGroup;

    protected virtual void OnCloseButtonClick()
    {
        Debug.Log("Close");
        UIManagerGlobal._instance.OnCloseDoHandlePanelButtonClick();
        GMSManagerGlobal._instance.SendUnitRecognitionObserver();
    }

    protected virtual void OnUnitRecognitionButtonClick()
    {
        UIManagerGlobal._instance.OnUnitRecognitionButtonClick();
        GMSManagerGlobal._instance.SendUnitRecognitionObserver();
    }

    protected virtual void OnShowModelButtonClick()
    {
        UIManagerGlobal._instance.OnShowModelButtonClick();
        GMSManagerGlobal._instance.SendUnitRecognitionObserver();
    }
    public virtual void InitPanel(UnitGroupT1 group)
    {
        currentGroup = group;
        NameTitle.text = group.unitName;
    }
    #region T2
    protected virtual void OnDriveButtonClick()
    {
        if (currentGroup.multiControlerBase == null) return;
        
        UIManagerGlobal._instance.UpdateDrivePanel(currentGroup.multiControlerBase.drive);
        UIManagerGlobal._instance.OnDriveButtonClick();
    }

    #endregion
}
