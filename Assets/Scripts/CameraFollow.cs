
using UnityEngine;

public class CameraFollow : MonoBehaviour {

     Transform target;
    public float mouseMaxFollowAmount;
    public float distanceFollow;
    float mouseFollowAmount;
    public float zoomMin;
    public float zoomMax;
    float zoom =8;
    Camera miniCam;

    private void Start()
    {
        target = GameManager.instance.player.transform;
      //  miniCam = GameObject.Find("MiniCamera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mouseFollowAmount = Vector3.Distance(Camera.main.ScreenToWorldPoint(mousePosition), target.position) / distanceFollow;          //met la distance dans une variable entre 0 et 1 qui va déterminer le follow amount
        mouseFollowAmount = Mathf.Clamp01(mouseFollowAmount);
        Vector3 smoothedPosition = Vector3.Lerp(target.position, Camera.main.ScreenToWorldPoint(mousePosition), (mouseFollowAmount * mouseMaxFollowAmount)/2 + 0.08f);
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
       // miniCam.orthographicSize = 3 * zoom;
        GetComponentInChildren<Camera>().orthographicSize = zoom;
    }
}
