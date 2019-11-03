using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel_T1 : MonoBehaviour {

    protected RectTransform panelRectTransform;
    public Text NameTitleText;
    public Text TextInfo;
    public Image ImageInfo;

    public Text OnlyTextInfo;
    public Image OnlyImageInfo;

    public Text MessageText;
    protected int messageCount = 0;

    //public bool isShowPanel = false;

    public enum PanelState
    {
        Close = 0,
        Show = 1,
        Pause = 2
    }
    public PanelState state = PanelState.Close;


    protected void SetPanel(RectTransform panel)
    {
        panelRectTransform = panel;
    }
    /// <summary>
    /// 界面打开
    /// </summary>
    public virtual void OnOpenPanel()
    {
        CanvasGroup cg = panelRectTransform.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        //isShowPanel = true;
        state = PanelState.Show;
    }
    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPausePanel()
    {
        CanvasGroup cg = panelRectTransform.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        //isShowPanel = true;
        state = PanelState.Pause;
    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResumePanel()
    {
        if (state == PanelState.Close)
        {
            UIManagerGlobal._instance.PopPanel();
            return;
        }

        CanvasGroup cg = panelRectTransform.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        //isShowPanel = true;
        state = PanelState.Show;
    }

    /// <summary>
    /// 界面关闭
    /// </summary>
    public virtual void OnClosePanel()
    {
        CanvasGroup cg = panelRectTransform.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        //isShowPanel = false;
        state = PanelState.Close;
    }

    /// <summary>
    /// 设置标题名称
    /// </summary>
    /// <param name="nameText"></param>
    protected virtual void SetNameTitleText(string nameText)
    {
        NameTitleText.text = nameText;
    }

    /// <summary>
    /// 设置图文信息
    /// </summary>
    /// <param name="textInfo"></param>
    /// <param name="texture2DInfo"></param>
    protected virtual void SetMoreInfo(string textInfo, Texture2D texture2DInfo)
    {
        TextInfo.gameObject.SetActive(false);
        ImageInfo.gameObject.SetActive(false);
        OnlyTextInfo.gameObject.SetActive(false);
        OnlyImageInfo.gameObject.SetActive(false);

        //仅图片模式
        if (textInfo == "" && texture2DInfo)
        {
            OnlyImageInfo.gameObject.SetActive(true);
            SetTexture2DAdapt(OnlyImageInfo, texture2DInfo);
        }
        //仅文字模式
        else if (textInfo != "" && !texture2DInfo)
        {
            OnlyTextInfo.gameObject.SetActive(true);
            SetText(OnlyTextInfo, textInfo);
        }
        //图文模式
        else if (textInfo != "" && texture2DInfo)
        {
            TextInfo.gameObject.SetActive(true);
            ImageInfo.gameObject.SetActive(true);
            SetTexture2DAdapt(ImageInfo, texture2DInfo);
            SetText(TextInfo, textInfo);
        }
    }

    /// <summary>
    /// 设置图片适配
    /// </summary>
    /// <param name="image"></param>
    /// <param name="texture2D"></param>
    protected virtual void SetTexture2DAdapt(Image image, Texture2D texture2D)
    {
        if (texture2D)
        {
            Sprite sp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f));
            image.sprite = sp;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }
    /// <summary>
    /// 设置文字
    /// </summary>
    /// <param name="text"></param>
    /// <param name="str"></param>
    protected virtual void SetText(Text text, string str)
    {
        text.text = str;
    }


    /// <summary>
    /// 显示主界面信息协程
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    protected IEnumerator DoShowMainMessage(string message, float showTime)
    {
        MessageText.text = message;
        messageCount++;
        yield return new WaitForSeconds(showTime);
        messageCount--;
        if (messageCount == 0)
        {
            MessageText.text = "";
        }
    }

    /// <summary>
    /// 显示主界面文字信息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    public virtual void ShowMainMessage(string message, float showTime = 2)
    {
        StartCoroutine(DoShowMainMessage(message, showTime));
    }
}
