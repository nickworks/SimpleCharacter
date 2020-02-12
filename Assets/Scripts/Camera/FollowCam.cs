using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public Transform viewTarget;
    public Vector3 offset;
    
    void Update() {
        if (viewTarget == null) return;
        transform.position = AnimMath.Dampen(transform.position, viewTarget.position + offset, .01f);
    }
}
