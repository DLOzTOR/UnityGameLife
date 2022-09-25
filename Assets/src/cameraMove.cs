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

    void camBorders()
    {
        float w = GetComponent<Camera>().pixelWidth, h = GetComponent<Camera>().pixelHeight;
        float camkof = w / h;
        transf = GetComponent<Transform>();
        CameraLimites[0] = gameLife.width - (GetComponent<Camera>().orthographicSize);
        CameraLimites[1] = GetComponent<Camera>().orthographicSize - 1f; 
        CameraLimites[2] = GetComponent<Camera>().orthographicSize* camkof - 1f;
        CameraLimites[3] = gameLife.height-(GetComponent<Camera>().orthographicSize * camkof);
    }

    void Update()
    {
        camBorders();
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        transf.position += new Vector3(horizontal * runSpeed, vertical * runSpeed) * Time.deltaTime;
        transf.position = new Vector3(Mathf.Clamp(transf.position.x, CameraLimites[2], CameraLimites[3]), Mathf.Clamp(transf.position.y, CameraLimites[1], CameraLimites[0]), transf.position.z);
    }
}
