using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
    public Sprite[] sprites;
    public float timeBetween;


    private int _index;
    private SpriteRenderer _rend;
    private void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimRoutine());
    }

    private IEnumerator AnimRoutine()
    {
        while (true)
        {
            _index++;
            _index = _index >= sprites.Length ? 0 : _index;
            _rend.sprite = sprites[_index];
            yield return new WaitForSeconds(timeBetween);
        }
    }
}
