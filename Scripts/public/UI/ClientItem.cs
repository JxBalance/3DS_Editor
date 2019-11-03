using System.Collections;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.UI;

public class ClientItem : MonoBehaviour
{
    public Image Icon;
    public Text NameText;
    public Image ConnectImage;
    public Image DisconnectImage;

    public bool isConnect = false;
    public bool onlyIcon = false;

    private float timer = 2.5f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            OnDisconnect();
        }
    }


    public void InitItem(ClientInfo clientInfo)
    {
        NameText.text = clientInfo.clientMode.ToString();
        if (clientInfo.isConnect)
        {
            OnConnect();
        }
        else
        {
            OnDisconnect();
        }
    }

    public void OnConnect()
    {
        timer = 2.5f;
        isConnect = true;
        ColorChange(Color.green);
        if(onlyIcon) return;
        ConnectImage.gameObject.SetActive(true);
        DisconnectImage.gameObject.SetActive(false);
    }

    public void OnDisconnect()
    {
        timer = 0f;
        isConnect = false;
        ColorChange(Color.white);
        if (onlyIcon) return;
        ConnectImage.gameObject.SetActive(false);
        DisconnectImage.gameObject.SetActive(true);
    }


    private void ColorChange(Color color)
    {
        Icon.color = color;
        if(onlyIcon) return;
        NameText.color = color;
        ConnectImage.color = color;
        DisconnectImage.color = color;
    }

}
