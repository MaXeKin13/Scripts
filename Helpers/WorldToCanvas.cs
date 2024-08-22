public class WorldToCanvas : MonoBehaviour
{
    [field: SerializeField] private Camera cam;    // Camera containing the canvas
    [field: SerializeField] Transform target; // object in the 3D World
    private RectTransform icon;   // icon to place in the canvas
    private Canvas canvas; // canvas with "Render mode: Screen Space - Camera"

    [field: SerializeField] public Vector2 offset;


    private float screenHeight;
    private float screenWidth;
    private void Awake()
    {
        if (!cam)
            cam = Camera.main;
        icon = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }
    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        float x = screenPos.x - (screenWidth / 2);
        x += offset.x;
        float y = screenPos.y - (screenHeight / 2);
        y += offset.y;
        float s = canvas.scaleFactor;
        icon.anchoredPosition = new Vector2(x, y) / s;
    }
}