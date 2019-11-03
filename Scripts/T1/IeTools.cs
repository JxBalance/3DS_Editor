using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IeTools : MonoBehaviour {

    private static volatile IeTools instance;
    private static object syncRoot = new object();

    public delegate void CameraMove();
    public event CameraMove OnUpdate;

    public static IeTools Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        GameObject go = new GameObject("CoroutineObject");
                        instance = go.AddComponent<IeTools>();
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return instance;
        }
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }
}
