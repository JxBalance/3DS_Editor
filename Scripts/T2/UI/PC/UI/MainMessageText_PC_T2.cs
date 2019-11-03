using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMessageText_PC_T2 : MonoBehaviour
{
    private string message = "message";
    private Text mText;
    private float mShowTime = 2f;
    private Transform canvasTransform;
    private bool isLast = false;
    private bool isDestroy = false;

    public string Message
    {
        set { message = value; }
    }

    public float ShowTime
    {
        set { mShowTime = value; }
    }


	// Use this for initialization
	void Start ()
	{
	    mText = GetComponent<Text>();
        canvasTransform = transform.parent.parent.GetComponent<Transform>();
	    mText.text = message;
        StartCoroutine(ShowAsCurrentText(mShowTime));
	}


    public IEnumerator ShowAsCurrentText(float showTime)
    {
        yield return new WaitForSeconds(showTime);
        while (mText.color.a > 0.05f)
        {  
            Color c = mText.color;
            mText.color = new Color(c.r,c.g,c.b,c.a - 0.05f);
            yield return new WaitForSeconds(0.02f);
        }
        if (!isLast)
        {
            DestroyThis();
        }
    }

    public IEnumerator ShowAsLastText(float showTime = 2f)
    {
        isLast = true;
        RectTransform rt = mText.GetComponent<RectTransform>();
        rt.DOMoveY(rt.position.y + rt.rect.height * canvasTransform.localScale.y, 0.2f);
        mText.color = new Color(mText.color.r, mText.color.g, mText.color.b, 0.5f);
        yield return new WaitForSeconds(showTime);
        while (mText.color.a > 0.05f)
        {
            if (isDestroy)
            {
                yield return null;
            }
            else
            {
                Color c = mText.color;
                mText.color = new Color(c.r, c.g, c.b, c.a - 0.05f);
                yield return new WaitForSeconds(0.02f);
            }
        }
        DestroyThis();
    }

    public void DestroyThis()
    {
        if (!isDestroy)
        {
            isDestroy = true;
            Destroy(mText.gameObject);
        }
    }
}
