﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireframeTest : MonoBehaviour {
    void OnPreRender()
    {
        GL.wireframe = true;
    }
    void OnPostRender()
    {
        GL.wireframe = false;
    }
}
