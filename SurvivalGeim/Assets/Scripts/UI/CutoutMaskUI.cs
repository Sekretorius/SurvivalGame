using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    public static CutoutMaskUI instance;

    private Material mat;

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void SetMaterial()
    {
        material = mat;
    }

    public override Material materialForRendering {
        get {
            Material material =new Material( base.materialForRendering );
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            mat = material;
            return material; 
        }

    }
}

