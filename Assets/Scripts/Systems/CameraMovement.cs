using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float mouseWheelSpeed;
    private Vector3 lastFramePos;
    public void Initialize()
    {
        moveCameraToMapCenter();
    }
    private void Update()
    {
        Vector3 currFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePos - currFramePos;
            Camera.main.transform.Translate(diff);
        }
        float newCameraSize = Camera.main.orthographicSize - Input.mouseScrollDelta.y * mouseWheelSpeed;
        if (newCameraSize >= 2 && newCameraSize <= 15)
            Camera.main.orthographicSize = newCameraSize;

        lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(lastFramePos);
    }
    void moveCameraToMapCenter()
    {
        Vector2 center = Map.TileMap[(int)Map.MapSideSize/ 2, (int)Map.MapSideSize/ 2].TileGo.transform.position;
        Camera.main.transform.position = new Vector3(center.x, center.y, Camera.main.transform.position.z);
    }
}
