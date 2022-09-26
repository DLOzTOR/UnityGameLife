using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    float horizontal;
    float vertical;
    Transform transf;
    public float runSpeed = 5f;
    private float[] CameraLimites = new float[4];//top bottom left right
    float w, h;
    float camkof;

    void camBorders()
    {
        transf = GetComponent<Transform>();
        CameraLimites[0] = gameLife.width - (GetComponent<Camera>().orthographicSize);
        CameraLimites[1] = GetComponent<Camera>().orthographicSize - 1f; 
        CameraLimites[2] = GetComponent<Camera>().orthographicSize* camkof - 1f;
        CameraLimites[3] = gameLife.height-(GetComponent<Camera>().orthographicSize * camkof);
    }
    public void zoomIN()
    {
        if(GetComponent<Camera>().orthographicSize > 3) GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize - 1;
    }
    public void zoomOUT()
    {
        if (GetComponent<Camera>().orthographicSize * 2 <= CameraLimites[0] + CameraLimites[1] && GetComponent<Camera>().orthographicSize * camkof * 2 <= CameraLimites[3] + CameraLimites[2]) GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize + 1;
        else GetComponent<Camera>().orthographicSize = w > h ? ((gameLife.width + 1f) / camkof) / 2 : (gameLife.height + 1f) / 2;
    }
    void Update()
    {
        w = GetComponent<Camera>().pixelWidth;
        h = GetComponent<Camera>().pixelHeight;
        camkof = w / h;
        camBorders();
        if (Input.GetKey(KeyCode.Minus)) zoomOUT();
        if (Input.GetKey(KeyCode.Equals)) zoomIN();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        transf.position += new Vector3(horizontal * runSpeed, vertical * runSpeed) * Time.deltaTime;
        transf.position = new Vector3(Mathf.Clamp(transf.position.x, CameraLimites[2], CameraLimites[3]), Mathf.Clamp(transf.position.y, CameraLimites[1], CameraLimites[0]), transf.position.z);
    }
}
