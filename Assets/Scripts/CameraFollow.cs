
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float MouseFollowAmount;
    public float zoomMin;
    public float zoomMax;
    float zoom =8;

    void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 smoothedPosition = Vector3.Lerp(target.position, Camera.main.ScreenToWorldPoint(mousePosition), MouseFollowAmount);
        transform.position = smoothedPosition + new Vector3(0, 0, -1);

        zoom += -2*Input.GetAxis("Mouse ScrollWheel");
        if (zoom < zoomMin)
        {
            zoom = zoomMin;
        }
        else if(zoom > zoomMax)
        {
            zoom = zoomMax;
        }
        GameObject.Find("MiniCamera").GetComponent<Camera>().orthographicSize = 3 * zoom;
        GetComponentInChildren<Camera>().orthographicSize = zoom;
        //Vector3 smoothedPosition = (mousePosition - target.position) * MouseFollowAmount + transform.position;
    }
}
