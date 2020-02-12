using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCam : MonoBehaviour
{

    public float distance = 5;
    public float scrollSensitivity = 1;
    Camera cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Vector2 scroll = Input.mouseScrollDelta;
        distance += scroll.y * scrollSensitivity;
        distance = Mathf.Clamp(distance, 1, 10);

        Vector3 targetPos = new Vector3(0, 0, -distance);
        cam.transform.localPosition = AnimMath.Dampen(cam.transform.localPosition, targetPos, .01f);
    }
}
