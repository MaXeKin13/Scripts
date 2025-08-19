using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LerpTexture : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    [Range(0f, 1f)] public float blend = 0f;
    public float timeToBlend = 2f;

    private void Start()
    {
        if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

    }
    public void StartLerping()
    {
        DOTween.To(() => meshRenderer.material.GetFloat("_Blend"), x => meshRenderer.material.SetFloat("_Blend", x), 1f, timeToBlend);
    }
}
