
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float MouseFollowAmount;

    void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 smoothedPosition = Vector3.Lerp(target.position, Camera.main.ScreenToWorldPoint(mousePosition), MouseFollowAmount);
        transform.position = smoothedPosition + new Vector3(0, 0, -1);


        //Vector3 smoothedPosition = (mousePosition - target.position) * MouseFollowAmount + transform.position;
    }
}
