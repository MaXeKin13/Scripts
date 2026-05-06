using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CinemachineDollySpawner : MonoBehaviour
{
    [Header("References")]
    public CinemachineSmoothPath smoothPath;
    public GameObject prefab;

    [Header("Spacing")]
    [Tooltip("World-space distance between each spawned object")]
    public float spacing = 2f;

    [Header("Alignment")]
    [Tooltip("Align object's forward axis to the spline tangent")]
    public bool alignToSpline = true;
    [Tooltip("Additional rotation offset applied after alignment")]
    public Vector3 rotationOffset = Vector3.zero;

    [Header("Offset")]
    [Tooltip("Local offset from the spline position")]
    public Vector3 positionOffset = Vector3.zero;

    [Header("Snap to Terrain")]
    public bool snapToTerrain = false;
    // Holds all spawned instances so we can clear them on refresh
    private readonly List<GameObject> _spawnedObjects = new List<GameObject>();

    private void OnValidate()
    {
#if UNITY_EDITOR
        // OnValidate cannot instantiate objects directly — defer to next editor tick
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null) // guard against the component being destroyed before the call fires
                Refresh();
        };
#endif
    }

    [ContextMenu("Refresh Spawned Objects")]
    public void Refresh()
    {
        ClearSpawned();

        if (smoothPath == null || prefab == null || spacing <= 0f)
            return;

        float pathLength = smoothPath.PathLength;
        if (pathLength <= 0f) return;

        int count = Mathf.FloorToInt(pathLength / spacing);

        for (int i = 0; i <= count; i++)
        {
            float distance = i * spacing;

            Vector3 position = smoothPath.EvaluatePositionAtUnit(distance, CinemachinePathBase.PositionUnits.Distance);
            Vector3 tangent = smoothPath.EvaluateTangentAtUnit(distance, CinemachinePathBase.PositionUnits.Distance);

            // Apply local offset (relative to the spline orientation)
            Quaternion splineRot = tangent != Vector3.zero
                ? Quaternion.LookRotation(tangent)
                : Quaternion.identity;

            position += splineRot * positionOffset;

            Quaternion finalRot = alignToSpline
                ? splineRot * Quaternion.Euler(rotationOffset)
                : Quaternion.Euler(rotationOffset);

#if UNITY_EDITOR
            GameObject instance;
            if (Application.isPlaying)
            {
                instance = Instantiate(prefab, position, finalRot, transform);
            }
            else
            {

                instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
                if (instance != null)
                    instance.transform.SetPositionAndRotation(position, finalRot);


            }
#else
            GameObject instance = Instantiate(prefab, position, finalRot, transform);
#endif
            if (instance != null)
            {
                instance.hideFlags = HideFlags.DontSave;
                _spawnedObjects.Add(instance);
            }



            if (snapToTerrain)
            {
                // Cast downward from above the object to find the terrain surface
                Vector3 rayOrigin = instance.transform.position + Vector3.up * 100f;

                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 200f))
                {
                    Vector3 snappedPos = hit.point;

                    if (alignToSpline)
                    {
                        // Optionally align Y-axis to terrain normal, keep spline forward
                        Quaternion terrainRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                        instance.transform.SetPositionAndRotation(snappedPos, terrainRot * finalRot);
                    }
                    else
                    {
                        instance.transform.position = snappedPos;
                    }
                }
            }


        }
    }

    private void ClearSpawned()
    {
        foreach (var obj in _spawnedObjects)
        {
            if (obj == null) continue;
#if UNITY_EDITOR
            if (Application.isPlaying) Destroy(obj);
            else DestroyImmediate(obj);
#else
            Destroy(obj);
#endif
        }
        _spawnedObjects.Clear();
    }

    private void OnDestroy() => ClearSpawned();
}