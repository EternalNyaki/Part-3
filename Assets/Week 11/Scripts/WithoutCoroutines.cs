using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class WithoutCoroutines : MonoBehaviour
{
    public GameObject missile;
    public float speed = 5;
    public float turningSpeedReduction = 0.75f;

    float time = 0;

    bool turning;
    float interpolation;
    Quaternion currentHeading;
    Quaternion newHeading;

    public void MakeTurn(float turn)
    {
        time = 0;
        turning = true;

        interpolation = 0;
        currentHeading = missile.transform.rotation;
        newHeading = currentHeading * Quaternion.Euler(0, 0, turn);
    }

    public void MoveForwards(float length)
    {
        turning = false;

        time = length;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            missile.transform.Translate(transform.right * speed * Time.deltaTime);
        }
        if (interpolation < 1 && turning)
        {
            interpolation += Time.deltaTime;
            missile.transform.rotation = Quaternion.Lerp(currentHeading, newHeading, interpolation);
            missile.transform.Translate(transform.right * (speed * turningSpeedReduction) * Time.deltaTime);
        }
    }
}
