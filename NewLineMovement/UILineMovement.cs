using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UILineMovment : MonoBehaviour
{    private RectTransform rectTransform;

    [SerializeField] public List<PathPoint> PathPoints = new List<PathPoint>();
    private List<Vector3> positions;
    private List<float> timeToReach;
    private List<float> scale;

    private List<bool> isLocal;

    private void Start()
    {
        positions = new List<Vector3>();
        timeToReach = new List<float>();
        scale = new List<float>();
        
        isLocal = new List<bool>();
        foreach (var point in PathPoints)
        {
            positions.Add(point.position);
            timeToReach.Add(point.timeToReach);
            scale.Add(point.scale);
            
            isLocal.Add(point.isLocal);
        }

        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component

        for (int i = 0; i < positions.Count; i++)
        {
            if(isLocal[i])
                positions[i] = rectTransform.anchoredPosition3D + positions[i];
        }
        StartEndAnim();
    }

    public void StartEndAnim()
    {
        StartCoroutine(EndAnim());
    }

    private IEnumerator EndAnim()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 worldPosition = isLocal[i] ? rectTransform.TransformPoint(positions[i]) : positions[i];
            
            rectTransform.DOAnchorPos3D(worldPosition, timeToReach[i]);
            rectTransform.DOScale(scale[i], timeToReach[i]);
            yield return new WaitForSeconds(timeToReach[i]);
        }
        
        
        //yield return null;
        StartCoroutine(EndAnim());
    }
    [Serializable]
    public class PathPoint
    {
        public Vector3 position;
        public float timeToReach;
        public float scale = 1f;
        
        public bool isLocal;
    }
}
