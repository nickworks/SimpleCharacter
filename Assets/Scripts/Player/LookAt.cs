using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    [Tooltip("If the target falls outside of this FOV, the bone will NOT point at the target.")]
    public float fieldOfView = 45;
    [Tooltip("If true, this bone will point down when not target is not in FOV.")]
    public bool isArm = false;
    [Tooltip("If true, this bone will only turn on its yaw axis. It will NOT lean forward / back.")]
    public bool isTorso = false;


    PlayerTargeting targetingScript;


    void Start()
    {
        targetingScript = GetComponentInParent<PlayerTargeting>();
    }

    
    void Update()
    {
        if (targetingScript == null) return;

        Vector3 lookAt = targetingScript.transform.forward;
        bool hasTarget = (targetingScript.target != null);

        if (hasTarget) {

            lookAt = targetingScript.target.position - transform.position;

            // no "vertical-ness" in the look vector
            // in otherwords, flatten the vector onto the x/z plane
            if (isTorso) lookAt.y = 0;

            lookAt.Normalize();
        }

        float angleDiff = Vector3.Angle(targetingScript.transform.forward, lookAt);
        
        Quaternion targetRotation = transform.parent.rotation;

        bool isArmButNotAiming = (isArm && !targetingScript.isAiming);

        if(hasTarget && angleDiff < fieldOfView && !isArmButNotAiming) {
            targetRotation = Quaternion.LookRotation(lookAt, Vector3.up);
        } else if (isArm) {
            targetRotation = Quaternion.LookRotation(transform.parent.up * -1, transform.parent.forward);
        }

        transform.rotation = AnimMath.Dampen(transform.rotation, targetRotation, .001f);

    }
}
