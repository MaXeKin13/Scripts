using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnim : MonoBehaviour
{
    
    public float duration = 1f;
    public float scaleIncrease;

    private Transform trans;

    private float _ogScale;
    [SerializeField]
    public enum Type
    {
        Infinite,
        Once,
        OnlyUp,
        OnlyDown,
        
    }
    private void Start()
    {
        trans = GetComponent<Transform>();
        _ogScale = trans.localScale.x;
        Debug.Log(_ogScale);
    }

    private void ScaleAnimation()
    {
        //while (trans.localScale.x < scaleIncrease)
        {
            trans.DOScale(new Vector3(scaleIncrease, scaleIncrease, scaleIncrease), duration / 2).SetEase(Ease.InOutQuad);
        }
        
        
    }

    private void ScaleDecrease()
    {
        
            trans.DOScale(new Vector3(_ogScale, _ogScale, _ogScale), duration / 2).SetEase(Ease.InOutQuad);
        
    }

    private void Update()
    {
        if(trans.localScale.x == _ogScale)
            ScaleAnimation();
        if(trans.localScale.x >= scaleIncrease)
            ScaleDecrease();
    }
}
