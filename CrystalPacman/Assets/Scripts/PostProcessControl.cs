using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessControl : MonoBehaviour
{
    public float chromaValue = 0;

    PostProcessVolume pPStack;

    ChromaticAberration chroma;


    private void Start()
    {
        pPStack = GetComponent<PostProcessVolume>();
        pPStack.profile.TryGetSettings<ChromaticAberration>(out chroma);
    }

    private void Update()
    {
        SetChroma();
    }

    void SetChroma()
    {
        chroma.intensity.Override(Mathf.Lerp(chroma.intensity.value, chromaValue, Time.deltaTime));
    }
}
