using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bloop : MonoBehaviour
{
    public float scaleUpValue = 1.1f;
    public float timeToScaleUp = 0.1f;
    public float timeToScaleDown = 0.1f;
    private float ogScale;
    private void Start()
    {
        ogScale = transform.localScale.x;
    }
    public void StartBloop()
    {
        transform.DOScale(transform.localScale * scaleUpValue, timeToScaleUp)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                transform.DOScale(transform.localScale / scaleUpValue, timeToScaleDown)
                    .SetEase(Ease.InCubic);
            });
    }
}
