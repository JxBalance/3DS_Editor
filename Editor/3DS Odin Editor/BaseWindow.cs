using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseWindow : ScriptableObject
{
    [NonSerialized]
    public bool ShowEditor;

    [OnInspectorGUI]
    protected virtual void OnHierarchyChange()
    {
        GetManager();
        SetData();
    }
    protected virtual void Awake()
    {
        GetManager();
        GetData();
    }
    protected abstract void GetData();
    protected abstract void SetData();

    protected GamaManagerGlobal gmGlobal;
    protected virtual void GetManager()
    {
        if (!gmGlobal)
        {
            GameObject go = GameObject.FindGameObjectWithTag("GameManager");
            if (go)
            {
                gmGlobal = go.GetComponent<GamaManagerGlobal>();
            }
        }
    }
}
