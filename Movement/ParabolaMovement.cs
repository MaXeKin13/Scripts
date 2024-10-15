using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class ParabolaMovement
{

    public static void Movement(Transform obj, Transform target, float arcHeight, float speed, float timeToComplete, UnityEvent FinishEvent = null)
    {


        arcHeight = 1f;
        Vector3 _startPosition = obj.position;


        float distance = Vector3.Distance(_startPosition, target.position);

        float _progress = 0f;
        // This is one divided by the total flight duration, to help convert it to 0-1 progress.
        float _stepScale = speed / distance;

        int delayTime = (int)(Time.deltaTime * 1000);

        obj.GetComponent<MonoBehaviour>().StartCoroutine(StartMovement());

        IEnumerator StartMovement()
        {
            //yield return new WaitForSeconds(0.25f);
            while (_progress < 1.0f)
            {
                //.transform.position;
                // Increment our progress from 0 at the start, to 1 when we arrive.
                _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);

                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

                // Travel in a straight line from our start position to the target.        
                Vector3 nextPos = Vector3.Lerp(_startPosition, target.position, _progress);

                // Then add a vertical arc in excess of this.
                nextPos.y += parabola * arcHeight;

                // Continue as before.
                //transform.LookAt(nextPos, transform.forward);
                obj.position = nextPos;

                Debug.Log("Moving");

                yield return null;
                //await Task.Delay(delayTime);

            }
        }


    }
}
