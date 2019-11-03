using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainSystemScripts_T3 : MonoBehaviour {

    private Camera cam;
    public Camera camAirOutside;
    public Camera camHydraulic;
    public Camera camAirInside;
    public GameObject fpsController;

    public Material welcomeSkybox;
    public Material generalSkybox;

    public GameObject BlackBG;
    public GameObject BlackFullBG;
    public Camera nguiCam;
    public GameObject initUI;
    public GameObject partsTitle;
    public GameObject audioBoard;
    public GameObject textBoard;
    public GameObject textContents;
    public GameObject textScrollBar;
    public GameObject pictureBoard;
    public GameObject videoTexture;
    public AudioSource videoSound;

    public GameObject addAudioButton;
    public GameObject stopRecordButton;
    public AudioSource audioSound;

    public GameObject videoButton;
    public GameObject playButton;
    public GameObject pauseButton;

    public GameObject buttonAudio;
    public GameObject buttonText;
    public GameObject buttonVideo;
    public GameObject buttonPicture;
    public GameObject buttonReturn;

    public GameObject welcomeScene;
    public GameObject cabinScene;
    public GameObject pipeScene;
    public GameObject airInsideParts;
    public GameObject pipeParts;
    public GameObject airOutsideParts;

    public GameObject welcomeBoard;
    public GameObject prepareBoard;
    public GameObject pipeStartBoard;

    public TextAsset titleText;             //保存各类型注释名称的文本文档
    public TextAsset pictureText;
    public TextAsset videoText;
    public TextAsset audioText;

    public GameObject partSets;
    public GameObject[] parts = new GameObject[100];

    MovieTexture videoSource;   //用来记录视频文件

    static string audioNameText = "Audio Saved.txt";
    static string titleNameText = "Title Saved.txt";    //定义一个静态变量，代表预制文档的名字
    static string videoNameText = "Video Saved.txt";
    static string pictureNameText = "Picture Saved.txt";

    private int rayLength;

    LayerMask mask = 1 << 8;

    //T3!!!!!!!!!!!!!!!!!!!!!!!
    public Camera HydrauCamera;

    public GameObject chooseButon01;
    public GameObject chooseButon02;

    public GameObject airScene;
    public GameObject hydrauScene;

    public GameObject principleButton01;
    public GameObject principleButton02;
    
    public GameObject chooseView01;
    public GameObject chooseView02;

    public GameObject hydrauBoard;

    public GameObject fuel;
    public GameObject hydrau;
    public GameObject[] fuelPipe;
    public GameObject[] hydrauPipe;
    public Material defaultPipeMat;
    public Material transparentPipeMat;

    #region 存取文件
    //存入（创建）文件
    private void fnCreateFile(string sPath, string sName, string nData)
    {
        StreamWriter t_sStreamWriter;//文件流信息
        FileInfo t_fFileInfo = new FileInfo(sPath + "//" + sName);
        if (!t_fFileInfo.Exists)
        {
            t_sStreamWriter = t_fFileInfo.CreateText();//如果文件不存在则创建
        }
        else
        {
            fnDeleteFile(sPath, sName);     //如果此文件存在则先删除再重新创建
            t_sStreamWriter = t_fFileInfo.CreateText();
        }
        t_sStreamWriter.WriteLine(nData);//以行的形式写入信息
        t_sStreamWriter.Close();//关闭流
        t_sStreamWriter.Dispose();//销毁流
                                  //Debug.Log("FINISH?!!!");
    }

    //读取文件内容
    private string fnLoadFile(String sPath, string sName)
    {
        StreamReader t_sStreamReader = null;//使用流的形式读取
        try
        {
            t_sStreamReader = File.OpenText(sPath + "//" + sName);
        }
        catch (Exception ex)
        {
            return null;
        }
        string tText;
        tText = t_sStreamReader.ReadToEnd();

        t_sStreamReader.Close();//关闭流
        t_sStreamReader.Dispose();//销毁流
        return tText;
    }

    //删除文件
    private void fnDeleteFile(string sPath, string sName)
    {
        File.Delete(sPath + "//" + sName);
    }
    #endregion

    #region 读取集合文件
    string LoadSets(int i, TextAsset nameText)
    {
        /*
		string m_path;
		string m_name;
		string collects;
		m_path = Application.dataPath;//默认Assests文件夹
		m_name = nameText;
		collects = fnLoadFile (m_path, m_name);
		*/
        string partName = nameText.text;
        int n = partName.IndexOf(parts[i].name, 0);
        if (n != -1)
        {
            int m = partName.IndexOf(';', n);
            int k = partName.IndexOf('-', n);
            partName = partName.Substring(k + 1, m - k - 1);
        }
        return partName;
    }
    #endregion

    #region 按钮控制
    public void ButtonCtrl(GameObject button)
    {
        if (button == buttonAudio)
        {
            buttonAudio.GetComponent<UIButton>().isEnabled = false;
            buttonText.GetComponent<UIButton>().isEnabled = true;
            buttonPicture.GetComponent<UIButton>().isEnabled = true;
            buttonVideo.GetComponent<UIButton>().isEnabled = true;
            audioBoard.SetActive(true);
            textBoard.SetActive(false);
            pictureBoard.SetActive(false);
            videoTexture.SetActive(false);
            videoStop();
            InitializeAudio();
        }
        else if (button == buttonText)
        {
            buttonAudio.GetComponent<UIButton>().isEnabled = true;
            buttonText.GetComponent<UIButton>().isEnabled = false;
            buttonPicture.GetComponent<UIButton>().isEnabled = true;
            buttonVideo.GetComponent<UIButton>().isEnabled = true;
            audioBoard.SetActive(false);
            textBoard.SetActive(true);
            pictureBoard.SetActive(false);
            videoTexture.SetActive(false);
            videoStop();
            StopAudio();
        }
        else if (button == buttonPicture)
        {
            buttonAudio.GetComponent<UIButton>().isEnabled = true;
            buttonText.GetComponent<UIButton>().isEnabled = true;
            buttonPicture.GetComponent<UIButton>().isEnabled = false;
            buttonVideo.GetComponent<UIButton>().isEnabled = true;
            audioBoard.SetActive(false);
            textBoard.SetActive(false);
            pictureBoard.SetActive(true);
            videoTexture.SetActive(false);
            videoStop();
            StopAudio();
        }
        else if (button == buttonVideo)
        {
            buttonAudio.GetComponent<UIButton>().isEnabled = true;
            buttonText.GetComponent<UIButton>().isEnabled = true;
            buttonPicture.GetComponent<UIButton>().isEnabled = true;
            buttonVideo.GetComponent<UIButton>().isEnabled = false;
            audioBoard.SetActive(false);
            textBoard.SetActive(false);
            pictureBoard.SetActive(false);
            videoTexture.SetActive(true);
            StopAudio();
        }
        else if (button == buttonReturn)
        {
            buttonAudio.GetComponent<UIButton>().isEnabled = true;
            buttonText.GetComponent<UIButton>().isEnabled = false;
            buttonPicture.GetComponent<UIButton>().isEnabled = true;
            buttonVideo.GetComponent<UIButton>().isEnabled = true;
            audioBoard.SetActive(false);
            textBoard.SetActive(false);
            pictureBoard.SetActive(false);
            videoTexture.SetActive(false);
            videoStop();
            StopAudio();
            CloseNoteUI();
        }
    }
    #endregion

    #region 录音控制

    //录音变量
    int audioNumber = -1;       //记录录音数量
    public const int SamplingRate = 8000;   //录音码率
    private AudioClip clip;
    private byte[] recordData;
    private AudioClip[] testClip = new AudioClip[100];      //定义语音片段数组，用来缓存所有已录好的语音

    //添加语音
    public void AddAudio(GameObject button, bool flag)
    {
        Debug.Log("HERE?!!!!!!!! 01 " + System.DateTime.Now);
        if (flag)
        {
            Microphone.End(null);//这句可以不需要，但是在开始录音以前调用一次是好习惯 
            clip = Microphone.Start(null, false, 10, SamplingRate);
            Debug.Log("HERE?!!!!!!!! 02 " + System.DateTime.Now);
        }
        else
        {
            int audioLength;//录音的长度，单位为秒，ui上可能需要显示  
            int lastPos = Microphone.GetPosition(null);
            if (Microphone.IsRecording(null))   //录音小于10秒  
            {
                audioLength = lastPos / SamplingRate;//录音时长  
            }
            else
            {
                audioLength = 10;
            }
            Microphone.End(null);//此时录音结束，clip已可以播放了  

            if (audioLength < 1.0f)
            {
                return;//录音小于1秒就不处理了  
            }
            //如果要便于传输还需要进行压缩，压缩后的recordData就可以用于网络传输了  
            //recordData = AudioClipCompressor.CompressAudioClip(clip);  

            while (audioNumber != -1)
            {
                audioNumber++;
                string audioListName = "AudioList" + audioNumber;
                if (!GameObject.Find(audioListName))
                {
                    break;
                }
            }

            SavWav.Save("AudioNote" + audioNumber, clip);

            System.DateTime now = System.DateTime.Now;
            string nowTime = now.ToString();

            SaveAudioTime("AudioNote" + audioNumber, nowTime);

            AddAudioList();

            //本地可以用sqlit存储，  
            //int soundId = string.Format("{0}{1}",player.ID, System.DateTime.Now.ToString());  
            //DbSound dbs = new DbSound();  
            //dbs.InsertSound(soundId , audioLength, recordData);  
            //dbs.CloseSqlConnection();  
        }

    }

    //添加语音栏，排列在上一个可见栏的下方
    void AddAudioList()
    {
        GameObject prefabList;      //预设的语音栏
        int lastNum = audioNumber - 1;      //lastNum用来记录上一个录音栏的序号

        //判断序号为lastNumd的录音栏是否存在，若存在则获取此栏的序号，不存在则判断上一个栏
        GameObject lastList = GameObject.Find("AudioList" + lastNum);
        while (lastNum > 0)
        {
            if (lastList)
            {
                break;
            }
            lastNum--;
            lastList = GameObject.Find("AudioList" + lastNum);
        }

        float yAxis;
        //如果lastNum为0，则将新添加的栏放置在界面中第一个栏的位置
        if (lastNum <= 0)
        {
            yAxis = 14f;
            prefabList = Instantiate(Resources.Load<GameObject>("prefab/AudioList1"));
        }
        //lastNum大于0，则将新添加的栏放置在界面中lastNum对应的栏的下一位置
        else
        {
            yAxis = lastList.transform.localPosition.y - 54f;
            if (lastList.GetComponent<UISprite>().color.r == 1)
            {
                prefabList = Instantiate(Resources.Load<GameObject>("prefab/AudioList2"));
            }
            else
            {
                prefabList = Instantiate(Resources.Load<GameObject>("prefab/AudioList1"));
            }
        }
        //Debug.Log ("prefabList : " + prefabList.name);

        //初始化添加栏
        GameObject prefabParent = GameObject.Find("Audio Scroll View");
        prefabList.transform.parent = prefabParent.transform;
        prefabList.name = "AudioList" + audioNumber;
        prefabList.GetComponent<UIWidget>().transform.localPosition = new Vector3(-367.3f, yAxis, 0f);
        prefabList.GetComponent<UIWidget>().transform.localScale = new Vector3(1f, 1f, 1f);
        prefabList.GetComponent<UIWidget>().depth = 4;

        //获取录音时间，并显示在副标题中
        string audioTime = GetAudioTime("AudioNote" + audioNumber);
        foreach (Transform child in prefabList.transform)
        {
            if (child.name == "AudioLabel")
            {
                child.GetComponent<UILabel>().text = "录音" + audioNumber;
            }
            else if (child.name == "AudioSubLabel")
            {
                child.GetComponent<UILabel>().text = audioTime;
            }
        }

        //在添加栏时读取语音缓存，以便录音可以直接播放
        string m_path, m_name;
        m_name = "AudioNote" + audioNumber + ".wav";
        m_path = "file://" + Application.streamingAssetsPath + "/" + m_name;
        StartCoroutine(LoadAudio(m_path, audioNumber));
    }

    //保存录音时间
    void SaveAudioTime(string audioName, string nowTime)
    {
        string m_path;
        string m_name;
        m_path = Application.streamingAssetsPath;
        m_name = audioNameText;
        string timeCollect = fnLoadFile(m_path, m_name);
        //string timeCollect = audioText.text;
        int n = timeCollect.IndexOf(audioName, 0);
        if (n != -1)
        {
            int m = timeCollect.IndexOf(";", n);
            string reName = timeCollect.Substring(n, (m - n));
            if (audioName != "")
            {
                timeCollect = timeCollect.Replace(reName, (audioName + "-" + nowTime));
            }
            else
            {
                timeCollect = timeCollect.Replace((reName + ";"), "");
            }
        }
        else
        {
            if (nowTime != "")
            {
                timeCollect = timeCollect.Replace("#", (audioName + "-" + nowTime + ";#"));
            }
        }
        fnCreateFile(m_path, m_name, timeCollect);
    }

    //获取录音时间
    string GetAudioTime(string audioName)
    {
        string m_path;
        string m_name;
        m_path = Application.streamingAssetsPath;
        m_name = audioNameText;
        string audioCollect = fnLoadFile(m_path, m_name);
        //string audioCollect = audioText.text;
        int n = audioCollect.IndexOf(audioName, 0);
        if (n == -1)
        {
            return null;
        }
        else
        {
            int m = audioCollect.IndexOf(';', n);
            int k = audioCollect.IndexOf('-', n);
            return audioCollect.Substring(k + 1, m - k - 1);
        }

    }

    //录音界面初始化
    public void InitializeAudio()
    {
        if (audioNumber > -1)
        {
            Debug.Log("Not THERE...........");
            return;
        }

        Debug.Log("THERE?!?!?!?");

        //重新打开文件时，如果有存储的学习录音笔记，读取并显示
        string m_path;
        string m_name;
        m_path = Application.streamingAssetsPath;
        m_name = audioNameText;
        string audioNameCollect = fnLoadFile(m_path, m_name);
        //string audioNameCollect = audioText.text;

        int k = 0;
        while (k > -1)
        {
            int n = -1;
            int m = -1;
            n = audioNameCollect.IndexOf("AudioNote", k);
            m = audioNameCollect.IndexOf('-', k + 1);
            if (n < m)
            {
                audioNumber = int.Parse(audioNameCollect.Substring(n + 9, m - n - 9));  //int.Prase 转化字符串为数字
                k = m;
                Debug.Log("n= " + n + "  m= " + m + "  k= " + k + " ,     AudioNumber = " + audioNumber);
                //audioNumber++;
                AddAudioList();
            }
            else
            {
                if (k == 0)
                {
                    audioNumber = 0;
                }
                break;
            }
        }

    }

    //播放录音
    public void PlayAudio(string listName)
    {
        listName = listName.Replace("AudioList", "AudioNote");
        listName += ".wav";
        //Debug.Log (listName);
        string m_path;
        m_path = "file://" + Application.streamingAssetsPath + "/" + listName;

        int n = m_path.IndexOf(".wav", 0);      //获取录音序号
        string num = m_path.Substring(n - 1, 1);
        n = int.Parse(num);

        //StartCoroutine ( LoadAudio (m_path,n) );
        audioSound.clip = testClip[n];      //可直接播放缓存的对应序号语音
        audioSound.Play();
    }

    //停止播放录音
    public void StopAudio()
    {
        if (audioSound)
        {
            audioSound.Stop();
        }
    }

    //删除录音
    public void DeleteAudio(string listName)
    {
        listName = listName.Replace("AudioList", "AudioNote");

        string m_path;
        string m_name;
        m_path = Application.streamingAssetsPath;
        m_name = audioNameText;

        string audioCollect = fnLoadFile(m_path, m_name);

        //string audioCollect = audioText.text;
        int n = audioCollect.IndexOf(listName, 0);
        int m = audioCollect.IndexOf(';', n);
        string sub = audioCollect.Substring(n, (m + 1 - n));
        audioCollect = audioCollect.Replace(sub, "");

        fnCreateFile(m_path, m_name, audioCollect);

        //？？？？？？？？？？？？？无法实现删除资源？？？？？？？？？？？？？
        /*
		int k = listName.IndexOf ('e', 0);
		string gfdd = listName.Substring (k + 1, 1);
		k = int.Parse (gfdd);
		m_path = "file://" + m_path + "/" + m_name;
		StartCoroutine (DestroyAudio (m_path, k));
		*/

    }

    //读取录音文件，将录音缓存至testClip[]中相应序号的语音片段中
    IEnumerator LoadAudio(string url, int n)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Error: " + www.error);
        }
        testClip[n] = www.GetAudioClip();
    }

    IEnumerator DestroyAudio(string url, int n)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("Error: " + www.error);
        }
        //Destroy (www);
        testClip[n] = null;
    }

    #endregion

    #region 视频控制
    int pauseFlag = 0;  //用来判断视频是结束还是暂停

    //按钮点击触发事件
    public void VideoButtonClick(GameObject button)
    {
        if (!videoTexture.GetComponent<UITexture>().isActiveAndEnabled)
        {
        }
        else
        {
            if (!videoSource.isPlaying)
            {
                if (pauseFlag == 0)     //播放视频前若点过暂停，则继续播放(除去第一次播)
                {
                    videoSource.Play();
                    videoSound.Play();
                    playButton.SetActive(false);
                    pauseButton.SetActive(true);
                    pauseFlag++;
                }
                else                    //播放视频前没点过暂停，说明视频播放完毕，停止视频使其从头播放
                {
                    videoSource.Stop();
                    videoSound.Stop();
                    videoSource.Play();
                    videoSound.Play();
                    playButton.SetActive(false);
                    pauseButton.SetActive(true);
                }
            }
            else
            {
                videoSource.Pause();
                videoSound.Pause();
                playButton.SetActive(true);
                pauseButton.SetActive(false);
                pauseFlag--;
            }
        }

    }

    //按钮悬停触发事件
    public void VideoButtonHover(GameObject button, bool isHover)
    {
        if (isHover == true)
        {
            if (pauseFlag == 1)
            {
                if (!videoSource.isPlaying)
                {
                    pauseButton.SetActive(false);       //视屏播放结束时悬停，出现开始按钮
                    playButton.SetActive(true);
                }
                else
                {
                    pauseButton.SetActive(true);        //播放时悬停，出现暂停按钮	
                }
            }
        }
        else
        {
            if (pauseFlag == 1)
            {
                pauseButton.SetActive(false);       //播放时未悬停，暂停按钮消失	
            }
        }
    }

    //其他按钮事件触发视频停止
    void videoStop()
    {
        if (!videoSource)
        {
            return;
        }
        pauseFlag = 0;
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        videoSource.Stop();
        Resources.UnloadUnusedAssets();
    }
    #endregion

    #region 标注界面控制
    void ShowNoteUI(int n)
    {
        //第一人称视角暂停
        GetComponent<CameraWalk>().enabled = false;
        if (cam == camAirOutside)
        {
            cam.GetComponent<FirstView>().enabled = false;
        }
        else if (cam == camAirInside)
        {
            //fpsController.enabled = false;
            fpsController.GetComponent<UnityStandardAssets.Characters.FirstPerson.
                                        FirstPersonController>().enabled = false;
        }
        else
        {
            GameObject bBoard = GameObject.Find("Pipe Button Board");
            bBoard.GetComponent<UISprite>().alpha = 1f / 255f;
        }
        partSets.SetActive(false);

        //背景渐变变黑
        InvokeRepeating("bgChangeBlack", 0f, 0.05f);

        //设置标题
        string uiText;
        uiText = LoadSets(n, titleText);
        partsTitle.GetComponent<UILabel>().text = uiText;

        //设置文本
        TextAsset partsText = (TextAsset)Resources.Load(parts[n].name);
        if (partsText)
        {
            uiText = partsText.text;
        }
        else
        {
            uiText = "";
        }
        textContents.GetComponent<UILabel>().text = uiText;
        textScrollBar.GetComponent<UIScrollBar>().value = 0;

        //设置图片
        uiText = LoadSets(n, pictureText);
        pictureBoard.GetComponent<UISprite>().spriteName = uiText;
        //pictureBoard.GetComponent<UIWidget> ().height = 1;
        pictureBoard.GetComponent<UISprite>().MakePixelPerfect();
        float picHeight = pictureBoard.GetComponent<UISprite>().height;
        float picWidth = pictureBoard.GetComponent<UISprite>().width;
        if (picWidth > 650f)
        {
            picHeight = picHeight * 650f / picWidth;
            picWidth = 650f;
        }
        if (picHeight > 300f)
        {
            picWidth = picWidth * 300f / picHeight;
            picHeight = 300f;
        }
        pictureBoard.GetComponent<UISprite>().height = (int)picHeight;
        pictureBoard.GetComponent<UISprite>().width = (int)picWidth;

        //设置视频
        uiText = LoadSets(n, videoText);
        Debug.Log(uiText);
        videoSource = (MovieTexture)Resources.Load(uiText);
        videoTexture.GetComponent<UITexture>().mainTexture = videoSource;
        videoSound.clip = videoSource.audioClip;
        videoSource.loop = false;

        videoTexture.GetComponent<UITexture>().MakePixelPerfect();
        float videoHeight = videoTexture.GetComponent<UITexture>().height;
        float videoWidth = videoTexture.GetComponent<UITexture>().width;
        if (videoWidth > 720f)
        {
            videoHeight = videoHeight * 720f / videoWidth;
            videoWidth = 720f;
        }
        if (videoHeight > 360f)
        {
            videoWidth = videoWidth * 360f / videoHeight;
            videoHeight = 360f;
        }
        videoTexture.GetComponent<UITexture>().height = (int)videoHeight;
        videoTexture.GetComponent<UITexture>().width = (int)videoWidth;


    }

    public void CloseNoteUI()
    {
        initUI.SetActive(false);
        partsTitle.SetActive(false);
        InvokeRepeating("bgChangeWhite", 0f, 0.05f);

        //第一人称视角恢复
        GetComponent<CameraWalk>().enabled = true;
        if (cam == camAirOutside)
        {
            cam.GetComponent<FirstView>().enabled = true;
        }
        else if (cam == camAirInside)
        {
            fpsController.GetComponent<UnityStandardAssets.Characters.FirstPerson.
                                        FirstPersonController>().enabled = true;
        }
        else
        {
            GameObject bBoard = GameObject.Find("Pipe Button Board");
            bBoard.GetComponent<UISprite>().alpha = 1f;
        }
        partSets.SetActive(true);
    }

    //关闭标注界面，黑屏转场动画
    void bgChangeWhite()
    {
        BlackBG.GetComponent<UISprite>().alpha -= 18f / 255f;
        if (BlackBG.GetComponent<UISprite>().alpha <= 2f / 255f)
        {
            CancelInvoke();
        }
    }
    //打开标注界面，黑屏转场动画
    void bgChangeBlack()
    {
        BlackBG.GetComponent<UISprite>().alpha += 18f / 255f;
        if (BlackBG.GetComponent<UISprite>().alpha >= 180f / 255f)
        {
            initUI.SetActive(true);
            partsTitle.SetActive(true);
            textBoard.SetActive(true);
            CancelInvoke();
        }
    }

    //从 飞机场景 到 管道场景 的黑屏转场动画
    int backgroundFlag = 1;
    void bgChangeFullBlack()
    {
        BlackFullBG.GetComponent<UISprite>().alpha += (12f / 255f) * backgroundFlag;
        if (BlackFullBG.GetComponent<UISprite>().alpha >= 250f / 255f)
        {
            pipeScene.SetActive(true);
            airScene.SetActive(false);
            airOutsideParts.SetActive(false);
            pipeParts.SetActive(true);
            GameObject scripts = GameObject.Find("Scripts");
            scripts.GetComponent<CameraWalk>().enabled = false;
            cam = camHydraulic;
            rayLength = 5000;
            backgroundFlag = -1;
        }
        if (backgroundFlag == -1 && BlackFullBG.GetComponent<UISprite>().alpha < 170f / 255f)
        {
            pipeStartBoard.SetActive(true);
            backgroundFlag = 1;
            CancelInvoke();
        }
    }

    //从 管道场景 到飞机场景 的黑屏转场动画
    void bgBackFullBlack()
    {
        BlackFullBG.GetComponent<UISprite>().alpha += (12f / 255f) * backgroundFlag;
        if (BlackFullBG.GetComponent<UISprite>().alpha >= 250f / 255f)
        {
            pipeScene.SetActive(false);
            airScene.SetActive(true);
            airOutsideParts.SetActive(true);
            pipeParts.SetActive(false);
            GameObject scripts = GameObject.Find("Scripts");
            scripts.GetComponent<CameraWalk>().enabled = true;
            scripts.GetComponent<PipeController>().HideWaterPipe();
            cam = camAirOutside;
            backgroundFlag = -1;
        }
        if (backgroundFlag == -1 && BlackFullBG.GetComponent<UISprite>().alpha < 10f / 255f)
        {
            backgroundFlag = 1;
            CancelInvoke();
        }
    }

    //从 飞机外部场景 到 飞机内部场景 的黑屏转场动画
    int bgInsideFlag = 1;
    void bgChangeInsideBlack()
    {
        BlackFullBG.GetComponent<UISprite>().alpha += (12f / 255f) * bgInsideFlag;
        if (BlackFullBG.GetComponent<UISprite>().alpha >= 250f / 255f)
        {
            cabinScene.SetActive(true);
            airScene.SetActive(false);
            airOutsideParts.SetActive(false);
            airInsideParts.SetActive(true);
            GameObject scripts = GameObject.Find("Scripts");
            scripts.GetComponent<CameraWalk>().enabled = false;
            cam = camAirInside;

            bgInsideFlag = -1;
        }
        if (bgInsideFlag == -1 && BlackFullBG.GetComponent<UISprite>().alpha < 10f / 255f)
        {
            //pipeStartBoard.SetActive (true);
            bgInsideFlag = 1;
            CancelInvoke();
        }
    }

    //从 飞机内部场景 到 飞机外部场景 的黑屏转场动画
    int bgOutsideFlag = 1;
    void bgChangeOutsideBlack()
    {
        BlackFullBG.GetComponent<UISprite>().alpha += (12f / 255f) * bgOutsideFlag;
        if (BlackFullBG.GetComponent<UISprite>().alpha >= 250f / 255f)
        {
            cabinScene.SetActive(false);
            airScene.SetActive(true);
            airOutsideParts.SetActive(true);
            airInsideParts.SetActive(false);
            GameObject scripts = GameObject.Find("Scripts");
            scripts.GetComponent<CameraWalk>().enabled = true;
            cam = camAirOutside;
            rayLength = 20;
            bgOutsideFlag = -1;
        }
        if (bgOutsideFlag == -1 && BlackFullBG.GetComponent<UISprite>().alpha < 10f / 255f)
        {
            //pipeStartBoard.SetActive (true)
            bgOutsideFlag = 1;
            CancelInvoke();
        }
    }
    #endregion

    public void PipeBackToOuside()
    {
        BlackFullBG.SetActive(true);
        BlackFullBG.GetComponent<UISprite>().alpha = 1f / 255f;
        GetComponent<ChangeMouseCursor>().ChangeFlag(0);
        InvokeRepeating("bgBackFullBlack", 0f, 0.05f);
    }

    public void AddParts(GameObject newPerfab)
    {
        int i = 0;
        while (i >= 0)
        {
            if (!parts[i])
            {
                parts[i] = newPerfab;
                break;
            }
            i++;
            if (i > 20)
                break;
        }
        Debug.Log(parts);
    }

    //?Q!?!??!?!?!?!???????????????!?!??!?!?!??!?!?!
    void InitMicrophone()
    {
        Debug.Log(System.DateTime.Now);
        /*																			?Q!?!??!?!?!?!???????????????!?!??!?!?!??!?!?!
		//系统开始时调用一次麦克风，以防止系统运行期间调用麦克风卡顿
		AudioClip clip;
		clip = Microphone.Start(null,false,10,SamplingRate); 
		Debug.Log ("START"+" "+System.DateTime.Now);
		Microphone.End (null);
		*/
        prepareBoard.SetActive(false);
        CancelInvoke();

        InvokeRepeating("ShowWelcomeUI", 0.5f, 0.05f);
    }

    public void EnterTheScene()
    {
        InvokeRepeating("CloseWelcomeUI", 0f, 0.05f);
    }

    void ShowWelcomeUI()
    {
        welcomeBoard.GetComponent<UISprite>().alpha += 25f / 255f;
        if (welcomeBoard.GetComponent<UISprite>().alpha > 250f / 255f)
        {
            CancelInvoke();
        }
    }

    int welcomeFlag = 1;
    void CloseWelcomeUI()
    {
        welcomeBoard.GetComponent<UISprite>().alpha -= 25f / 255f;
        BlackFullBG.GetComponent<UISprite>().alpha += welcomeFlag * 18f / 255f;
        Debug.Log(BlackFullBG.GetComponent<UISprite>().alpha);
        if (BlackFullBG.GetComponent<UISprite>().alpha > 250f / 255f)
        {
            welcomeBoard.SetActive(false);
            welcomeScene.SetActive(false);
            airScene.SetActive(true);
            partSets.SetActive(true);
            RenderSettings.skybox = generalSkybox;
            welcomeFlag = -1;
        }
        if (BlackFullBG.GetComponent<UISprite>().alpha < 10f / 255f)
        {
            CancelInvoke();
        }
    }


    // Use this for initialization
    void Start()
    {
        /*
        prepareBoard.SetActive(true);
        InvokeRepeating("InitMicrophone", 0.2f, 0f);

        airScene.SetActive(false);
        partSets.SetActive(false);
        welcomeScene.SetActive(true);
        RenderSettings.skybox = welcomeSkybox;
        */

        cam = camAirOutside;
        rayLength = 20;

        DefaultHydrauCam();


    }

    Vector3 hydrauPosition;
    Vector3 hydrauRotation;
    void DefaultHydrauCam()
    {
        hydrauPosition = HydrauCamera.transform.localPosition;
        Debug.Log(hydrauPosition);
        hydrauRotation = HydrauCamera.transform.localEulerAngles;
        Debug.Log(hydrauRotation);

    }

    void ChangeScene01(GameObject button)
    {
        if (airScene.activeSelf == true)
        {
            Debug.Log("WAWAWWWW");
        }
        else
        {
            airScene.SetActive(true);
            hydrauScene.SetActive(false);
            hydrauBoard.SetActive(false);
        }
    }

    void ChangeScene02(GameObject button)
    {
        if(hydrauScene.activeSelf == true)
        {
            
        }
        else
        {
            hydrauScene.SetActive(true);
            airScene.SetActive(false);
            hydrauBoard.SetActive(true);
        }

    }

    void PrincipleFuel(GameObject button)
    {
        int i = 0;
        while (fuelPipe[i] != null)
        {
            fuelPipe[i].GetComponent<Renderer>().material = transparentPipeMat;
            if (i > 100)
            {
                Debug.Log("OVERFLOW!!!!!!!!!!!!");
                break;
            }
            i++;
        }
        i = 0;
        while (hydrauPipe[i] != null)
        {
            hydrauPipe[i].GetComponent<Renderer>().material = defaultPipeMat;
            if (i > 100)
            {
                Debug.Log("OVERFLOW!!!!!!!!!!!!");
                break;
            }
            i++;
        }
        fuel.SetActive(true);
    }

    void PrincipleHydrau(GameObject button)
    {
        int i = 0;
        while (hydrauPipe[i] != null)
        {
            hydrauPipe[i].GetComponent<Renderer>().material = transparentPipeMat;
            if (i > 100)
            {
                Debug.Log("OVERFLOW!!!!!!!!!!!!");
                break;
            }
            i++;
        }
        i = 0;
        while (fuelPipe[i] != null)
        {
            fuelPipe[i].GetComponent<Renderer>().material = defaultPipeMat;
            if (i > 100)
            {
                Debug.Log("OVERFLOW!!!!!!!!!!!!");
                break;
            }
            i++;
        }
        fuel.SetActive(false);

    }

    void FixedView(GameObject button)
    {
        if (HydrauCamera.GetComponent<FirstView>().enabled == false)
        {
            HydrauCamera.GetComponent<FirstView>().enabled = true;
        }
        else
        {
            HydrauCamera.GetComponent<FirstView>().enabled = false;
        }
    }

    void DefaultView(GameObject button)
    {
        HydrauCamera.transform.localPosition = hydrauPosition;
        HydrauCamera.transform.localEulerAngles = hydrauRotation;
    }

    void Awake()
    {
        /*
        UIEventListener.Get(buttonAudio).onClick = ButtonCtrl;
        UIEventListener.Get(buttonText).onClick = ButtonCtrl;
        UIEventListener.Get(buttonPicture).onClick = ButtonCtrl;
        UIEventListener.Get(buttonVideo).onClick = ButtonCtrl;
        UIEventListener.Get(buttonReturn).onClick = ButtonCtrl;

        //UIEventListener.Get (addAudioButton).onClick = AddAudio;
        UIEventListener.Get(addAudioButton).onPress = AddAudio;

        UIEventListener.Get(videoButton).onClick = VideoButtonClick;
        UIEventListener.Get(videoButton).onHover = VideoButtonHover;
        //UIEventListener.Get (videoButton).onHover = MovieControl;
        */
        UIEventListener.Get(chooseButon01).onClick = ChangeScene01;
        UIEventListener.Get(chooseButon02).onClick = ChangeScene02;
        UIEventListener.Get(principleButton01).onClick = PrincipleFuel;
        UIEventListener.Get(principleButton02).onClick = PrincipleHydrau;
        UIEventListener.Get(chooseView01).onClick = FixedView;
        UIEventListener.Get(chooseView02).onClick = DefaultView;
    }


    // Update is called once per frame
    void Update()
    {
        #region 射线检测
        //创建一条射线，产生的射线实在世界空间中,从相机的近裁剪面开始
        //并穿过屏幕position(x,y)像素坐标(positoon.z被忽略)
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //RayCastHit是一个结构体对象，用来储存射线返回的信息
        RaycastHit hit;

        //Debug.DrawLine (ray.origin,hit.point,Color.red,2);

        //如果射线碰撞到对象，把返回信息储存到hit中
        if (Physics.Raycast(ray, out hit, rayLength, mask.value))
        {
            Debug.Log(hit.transform.gameObject.name);
            //Debug.DrawLine (ray.origin,hit.point,Color.red,2);
            int i = 0;
            while (parts[i])
            {
                if (hit.transform.gameObject.name == parts[i].name)
                {
                    //如果射线与部件碰撞体碰撞，则改变鼠标样式
                    GetComponent<ChangeMouseCursor>().ChangeFlag(1);
                    //Debug.Log (parts[i].name);
                }
                i++;
            }

        }
        else
        {
            GetComponent<ChangeMouseCursor>().ChangeFlag(0);
        }
        //点击碰撞到对象，则显示对应部件标注界面
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                int i = 0;
                while (parts[i])
                {
                    if (hit.transform.gameObject.name == parts[i].name)
                    {
                        //若在漫游中点击管道部件，则转到管道原理演示场景
                        if (hit.transform.gameObject.name == "AirPipe")
                        {
                            GetComponent<ChangeMouseCursor>().ChangeFlag(0);
                            InvokeRepeating("bgChangeFullBlack", 0f, 0.05f);

                            break;
                        }
                        //若在漫游中点击内部场景部件，则转到飞机内部场景
                        else if (hit.transform.gameObject.name == "AirInside")
                        {
                            GetComponent<ChangeMouseCursor>().ChangeFlag(0);
                            InvokeRepeating("bgChangeInsideBlack", 0f, 0.05f);

                            break;
                        }
                        //若在飞机内部漫游中点击外部场景部件，则转到飞机外部场景
                        else if (hit.transform.gameObject.name == "AirOutside")
                        {
                            GetComponent<ChangeMouseCursor>().ChangeFlag(0);
                            InvokeRepeating("bgChangeOutsideBlack", 0f, 0.05f);

                            break;
                        }
                        //若漫游中点击正常部件，则显示标注演示界面
                        ShowNoteUI(i);
                        break;
                    }
                    i++;
                }
            }
        }
        #endregion

    }
        
}
