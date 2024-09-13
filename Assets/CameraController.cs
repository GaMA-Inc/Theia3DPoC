using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float scrollSpeed = 20f;
    public float rotateSpeed = 100f;

    private Vector3 panOrigin;
    private bool isPanning;
    private bool isRotating;

    void Update()
    {
        // Panning
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            Vector3 move = new Vector3();
            if (Input.GetKey("w"))
            {
                move += Vector3.up;
            }
            if (Input.GetKey("s"))
            {
                move += Vector3.down;
            }
            if (Input.GetKey("a"))
            {
                move -= transform.right;
            }
            if (Input.GetKey("d"))
            {
                move += transform.right;
            }
            transform.Translate(move * panSpeed * Time.deltaTime, Space.World);
        }

        // Zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scroll * scrollSpeed * Time.deltaTime, Space.Self);

        // Rotating
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            panOrigin = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - panOrigin);
            transform.RotateAround(transform.position, transform.right, -pos.y * rotateSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.up, pos.x * rotateSpeed * Time.deltaTime);
            panOrigin = Input.mousePosition;
        }
    }
}
