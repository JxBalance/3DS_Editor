using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlayCtrl_T1 : BasePanel_T1
{
    [SerializeField]
    protected int introBoardCount = 0;
    [SerializeField]
    protected int currentBoardIndex = 0;

    public int CurrentBoardIndex
    {
        get { return currentBoardIndex; }
        set
        {
            if(currentBoardIndex != value && state == PanelState.Show)
            {
                ShowIntroBoardByIndex(value);
            }
            else
            {
                currentBoardIndex = value;
            }
        }
    }

    [SerializeField]
    protected Button ClosePanelButton;

    public int IntroBoardCount
    {
        get { return introBoardCount; }
    }

    protected virtual void ShowIntroPanel()
    {
        ShowIntroBoardByIndex(currentBoardIndex);
    }

    public virtual void CloseIntroPanel()
    {
        UIManagerGlobal._instance.CloseIntroPanel();
        GMSManagerGlobal._instance.SendIntroBoardObserverData();
    }

    private void ShowIntroBoardByIndex(int index)
    {
        if (index < 0 || index > introBoardCount - 1)
        {
            return;
        }
        
        currentBoardIndex = index;
        SetIntroBoardPanel(GamaManagerGlobal._instance.IntroBoardList[index]);
    }

    public void ShowIntroBoardByCurrent()
    {
        ShowIntroBoardByIndex(currentBoardIndex);
    }

    /// <summary>
    /// 设置面板信息
    /// </summary>
    /// <param name="nameText"></param>
    /// <param name="textInfo"></param>
    /// <param name="texture2DInfo"></param>
    private void SetIntroBoardPanel(IntroBoard board)
    {
        SetNameTitleText(board.BoardTitle);
        SetMoreInfo(board.BoardText, board.BoardTexture);
        OnOpenPanel();
    }

}
