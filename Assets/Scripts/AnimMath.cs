using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMath
{
    public static float Lerp(float min, float max, float p) {
        return (max - min) * p + min;
    }
    public static Vector3 Lerp(Vector3 min, Vector3 max, float p) {
        return (max - min) * p + min;
    }
    public static Quaternion Lerp(Quaternion min, Quaternion max, float p) {
        return Quaternion.Lerp(min, max, p);
    }

    public static float Map(float v, float min1, float max1, float min2, float max2) {

        float p = (v - min1) / (max1 - min1);
        return Lerp(min2, max2, p);

    }

    public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float p) {

        Vector3 p1 = Lerp(a, b, p);
        Vector3 p2 = Lerp(b, c, p);

        return Lerp(p1, p2, p);
    }
    public static float Smooth(float min, float max, float p) {
        p = p * p * (3 - 2 * p);
        return (max - min) * p + min;
    }

    public static float Dampen(float current, float target, float percentAfter1Second) {
        float p = 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime);
        return Lerp(current, target, p);
    }
    public static Vector3 Dampen(Vector3 current, Vector3 target, float percentAfter1Second) {
        float p = 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime);
        return Lerp(current, target, p);
    }
    public static Quaternion Dampen(Quaternion current, Quaternion target, float percentAfter1Second) {
        float p = 1 - Mathf.Pow(percentAfter1Second, Time.deltaTime);
        return Lerp(current, target, p);
    }

}
