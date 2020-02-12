using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows our player to target objects that have the TargetToShoot script on them.
/// </summary>
public class PlayerTargeting : MonoBehaviour {

    public Transform target;
    public Transform visionOrigin;
    public float viewDistance = 5;
    [Range(0, 180)] public float viewFOV = 90;

    public float delayBetweenScans = .5f;
    public float delayBetweenUpdatingTarget = .2f;

    float timeUntilScan = 0;
    float timeUntilUpdatingTarget = 0;

    public bool isAiming { get; private set; }

    private List<ThingToShoot> possibleTargets = new List<ThingToShoot>();

    void Update() {
        isAiming = Input.GetButton("Fire2");


        timeUntilScan -= Time.deltaTime;
        timeUntilUpdatingTarget -= Time.deltaTime;

        if(timeUntilScan <= 0) ScanSceneForTargets();
        if(timeUntilUpdatingTarget <= 0) SetTargetToClosest();
    }
    /// <summary>
    /// Finds all targetable objects in the scene. NOTE: this is expensive; don't do every frame!
    /// Anything too far away or outside the player's FOV is ignored and not included in the set
    /// of possible targets.
    /// </summary>
    void ScanSceneForTargets() {

        timeUntilScan = delayBetweenScans; // reset timer

        ThingToShoot[] targets = FindObjectsOfType<ThingToShoot>();
        possibleTargets.Clear();

        float fovThreshold = AnimMath.Map(viewFOV, 0, 180, 1, 0);

        foreach(ThingToShoot target in targets) {
            Vector3 dis = target.transform.position - visionOrigin.position;
            bool tooFarAway = (dis.sqrMagnitude > viewDistance * viewDistance);
            bool outsideOfFOV = ( Vector3.Dot(transform.forward, dis.normalized) ) < fovThreshold;

            if (!tooFarAway && !outsideOfFOV) possibleTargets.Add(target);

        }
        //print(possibleTargets.Count);
    }
    void SetTargetToClosest() {

        timeUntilUpdatingTarget = delayBetweenUpdatingTarget; // reset timer

        target = null;
        float closestDistanceSquared = 0;

        for(int i = 0; i < possibleTargets.Count; i++) {

            ThingToShoot thing = possibleTargets[i];
            float d2 = (thing.transform.position - visionOrigin.position).sqrMagnitude;

            if(d2 < closestDistanceSquared || target == null) {
                if (CanISeeThisThing(thing)) {
                    target = thing.transform;
                    closestDistanceSquared = d2;
                }
            }
        }

    }

    private bool CanISeeThisThing(ThingToShoot thing) {

        Vector3 dir = (thing.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            if (hit.transform == thing.transform) return true;
        }

        return false;
    }


    void OnDrawGizmosSelected() {

        if (visionOrigin == null) visionOrigin = transform;

        Gizmos.matrix = Matrix4x4.Translate(visionOrigin.position) * Matrix4x4.Rotate(visionOrigin.rotation);
        Gizmos.DrawFrustum(Vector3.zero, viewFOV, viewDistance, 0, 1);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
