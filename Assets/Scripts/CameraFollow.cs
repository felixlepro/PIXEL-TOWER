
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float MouseFollowAmount;
    float time;

    void LateUpdate()
    {
        time += Time.deltaTime;
        Vector3 mousePosition = Input.mousePosition;
        if (time > 1)
        {
            Vector3 smoothedPosition = Vector3.Lerp(target.position, Camera.main.ScreenToWorldPoint(mousePosition), MouseFollowAmount);
            transform.position = smoothedPosition + new Vector3(0, 0, -1);

        }
        else transform.position = target.position + new Vector3(0, 0, -1);
        //Vector3 smoothedPosition = (mousePosition - target.position) * MouseFollowAmount + transform.position;
    }
}
