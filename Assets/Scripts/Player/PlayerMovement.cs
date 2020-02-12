using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public OrbitCam theCam;

    public Transform legBoneLeft;
    public Transform legBoneRight;

    bool isAnimating = false;

    CharacterController body;

    void Start()
    {
        body = GetComponent<CharacterController>();
    }

    
    void Update() {
        GetInputAndMove();
        AnimateThoseLegs();
    }

    private void GetInputAndMove() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        isAnimating = (v != 0 || h != 0);

        if (isAnimating && theCam != null) {
            Quaternion targetRot = Quaternion.Euler(0, theCam.yaw, 0);
            transform.rotation = AnimMath.Dampen(transform.rotation, targetRot, .01f);
        }


        Vector3 moveDis = transform.forward * v * moveSpeed; // how far to move forward/back
        moveDis += transform.right * h * moveSpeed; // how far to move side-to-side

        body.SimpleMove(moveDis); // does collision, applies gravity, applies deltaTime
    }

    private void AnimateThoseLegs() {

        float pitch = 0;

        if (isAnimating) {
            pitch = Mathf.Sin(Time.time * 10) * 30;
        }

        legBoneLeft.localRotation = Quaternion.Euler(pitch, 0, 0);
        legBoneRight.localRotation = Quaternion.Euler(-pitch, 0, 0);

    }

}
