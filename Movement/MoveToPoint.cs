using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveToPoint : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float overallSpeedMult = 1f;
    public AnimationCurve xMovementCurve;
    public AnimationCurve zMovementCurve;
    public AnimationCurve yMovementCurve;

    private Coroutine moveToPointCoroutine;

    // = null means it is optional
    public void StartMoveToPoint(Vector3 targetPos, Quaternion targetRot, Action onArrival = null)
    {
        if (moveToPointCoroutine != null)
        {
            StopCoroutine(moveToPointCoroutine);
        }
        moveToPointCoroutine = StartCoroutine(MoveToPointCoroutine(targetPos, targetRot, onArrival));
    }

    private IEnumerator MoveToPointCoroutine(Vector3 targetPos, Quaternion targetRot, Action onArrival = null)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;
        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / (moveSpeed * overallSpeedMult);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolate position using animation curves
            float newX = Mathf.Lerp(startPos.x, targetPos.x, xMovementCurve.Evaluate(t));
            float newY = Mathf.Lerp(startPos.y, targetPos.y, yMovementCurve.Evaluate(t));
            float newZ = Mathf.Lerp(startPos.z, targetPos.z, zMovementCurve.Evaluate(t));

            transform.localScale = Vector3.Lerp(startScale, startScale * .9f, t);
            transform.position = new Vector3(newX, newY, newZ);

            // Interpolate rotation
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // Ensure final position and rotation are set
        transform.position = targetPos;
        transform.rotation = targetRot;

        // Invoke the callback if provided
        onArrival?.Invoke();

        moveToPointCoroutine = null; // Reset coroutine reference when done
    }
}

