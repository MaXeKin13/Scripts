using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.Events;

public class MaxUILineMovement : MonoBehaviour
{
    //viewable in Inspector
    public RectTransform trans;
    [SerializeField] public List<PathPoint> PathPoints = new List<PathPoint>();

    public bool startOnAwake;
    public bool loop;
    public bool autoProceed;

    //private
    private List<Vector3> positions;
    private List<float> delay;
    private List<float> timeToReach;
    private List<Vector3> rotation;

    private List<Vector3> scale;
    private List<Ease> easeType;
    private List<UnityEvent> onComplete;


    public int currentPos = 0;

    private float _regularLerp;
    private float _bezierLerp;

    public void Awake()
    {

        positions = new List<Vector3>();
        delay = new List<float>();
        timeToReach = new List<float>();

        rotation = new List<Vector3>();
        scale = new List<Vector3>();


        easeType = new List<Ease>();

        onComplete = new List<UnityEvent>();

        foreach (var point in PathPoints)
        {
            positions.Add(point.position);

            timeToReach.Add(point.timeToReach);
            delay.Add(point.delay);
            scale.Add(point.scale);
            rotation.Add(point.rotation);


            easeType.Add(point.easeType);

            onComplete.Add(point.onComplete);
        }

        trans.anchoredPosition = positions[0];


    }

    private void OnEnable()
    {
        if (startOnAwake)
            StartEndAnim();

    }




    public void StartEndAnim()
    {
        //starts at 1
        currentPos = 1;
        trans.localScale = scale[0];
        StartCoroutine(EndAnim());
    }

    public void ContinueAnim()
    {
        StartCoroutine(EndAnim());
    }

    private IEnumerator EndAnim()
    {
        
        if (currentPos == 1)
        {
            //delay for first position
            yield return new WaitForSeconds(delay[0]);
            //invoke onComplete if at first Pos
            PathPoints[currentPos - 1].onComplete?.Invoke(); 

            trans.anchoredPosition = PathPoints[0].position;
        }
        //moves to next position
        {
            var initQuaternion = trans.rotation;
            yield return new WaitForSeconds(delay[currentPos]);
            float duration = timeToReach[currentPos];
            float elapsedTime = 0f;

            //scale
            trans.DOScale(scale[currentPos], timeToReach[currentPos]);

            while (elapsedTime < duration)
            {
                float t = (elapsedTime / duration);


                // Quadratic Bezier interpolation for position// start pos is currentPos - 1
                var pos = QuadraticLerp(PathPoints[currentPos - 1].position, PathPoints[currentPos - 1].controlPoint1,
                        PathPoints[currentPos].controlPoint2,
                        PathPoints[currentPos].position,
                        t);

                // Linear interpolation for rotation
                var rot = Quaternion.Lerp(initQuaternion, Quaternion.Euler(PathPoints[currentPos].rotation), t);

                elapsedTime += Time.fixedDeltaTime;

                // Move the object
                trans.anchoredPosition = pos;

                // Rotate the object
                trans.rotation = rot;

                yield return new WaitForFixedUpdate();
            }

            _regularLerp = 0f;
            _bezierLerp = 0f;


            PathPoints[currentPos].onComplete?.Invoke();


            currentPos++;
            if (currentPos >= positions.Count)
                currentPos = 1;

        }
        //Finish


        if (autoProceed && currentPos != 1)
            ContinueAnim();
        if (loop && currentPos == 1)
            StartEndAnim();
    }
    //old manual way
    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3 * P0
        p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * P1
        p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * P2
        p += ttt * p3; // t^3 * P3

        return p;
    }

    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var ab = Vector3.Lerp(a, b, t);
        var bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

    public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var abbc = CubicLerp(a, b, c, t);
        var bccd = CubicLerp(b, c, d, t);
        return Vector3.Lerp(abbc, bccd, t);
    }




    [Serializable]
    public class PathPoint
    {
        public Vector3 position;
        public Vector3 controlPoint1;
        public Vector3 controlPoint2;
        [Space] public Vector3 rotation;
        [Space]
        [Tooltip("Delay before moving to THIS position")]
        public float delay;
        [Tooltip("Time to reach THIS position")]
        public float timeToReach;
        [Space]
        public Vector3 scale;

        public Ease easeType;

        public UnityEvent onComplete;

        //used to create copy of any original referenced pathpoint
        public static PathPoint ClonePathPoint(PathPoint original)
        {
            PathPoint newPoint = new PathPoint();

            newPoint.position = original.position;
            newPoint.controlPoint1 = original.controlPoint1;
            newPoint.controlPoint2 = original.controlPoint2;
            newPoint.rotation = original.rotation;
            newPoint.delay = original.delay;
            newPoint.timeToReach = original.timeToReach;
            newPoint.scale = original.scale;
            newPoint.easeType = original.easeType;
            newPoint.onComplete = original.onComplete;

            return newPoint;
        }
    }
}
