using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    public float angleOpened = 110; // X-axis Euler angle when opened.
    public float angleClosed = 0;   // X-axis Euler angle when closed.

    public float flapTime = 2; // Number of seconds for to open or close.

    Quaternion rotOpened; // Rotation when fully opened.
    Quaternion rotClosed; // Rotation when full closed.

    bool isFlapping = false; // Animate and lockout while true.
    bool isClosed = true; // Track open/closed state.

    // make changeSign as a member variable instead of a local variable
    float changeSign = -1;

    private void Start()
    {
        // Create and remember the open/closed quaternions.

        rotOpened = Quaternion.Euler(0, angleOpened, 0);
        rotClosed = Quaternion.Euler(0, angleClosed, 0);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse click");
        StartCoroutine(RotateCube());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("pressed space");
            StartCoroutine(RotateCube());
        }
    }



    IEnumerator RotateCube()
    {
        // Lockout any attempt to start another FlapLid while
        // one is already running.


        if (isFlapping)
        {
            // other coRoutines will be able to change direction and then break
            // also need to change isClosed to make sure that no error occurs when lid is fully closed or opened
            changeSign *= -1;
            isClosed = !isClosed;

            yield break;
        }

        // Start the animation and lockout others.

        isFlapping = true;

        // Vary this from zero to one, or from one to zero,
        // to interpolate between our quaternions.

        float interpolationParameter;

        // Set this according to whether we are going from zero
        // to one, or from one to zero.

        //float changeSign;

        // Set lerp parameter to match our state, and the sign
        // of the change to either increase or decrease the
        // lerp parameter during animation.

        if (isClosed)
        {
            interpolationParameter = 0;
            changeSign = 1;
        }
        else
        {
            interpolationParameter = 1;
            changeSign = -1;
        }

        while (isFlapping)
        {
            // Change our "lerp" parameter according to speed and time,
            // and according to whether we are opening or closing.

            interpolationParameter = interpolationParameter + changeSign * Time.deltaTime / flapTime;

            // At or past either end of the lerp parameter's range means
            // we are on our last step.

            if (interpolationParameter >= 1 || interpolationParameter <= 0)
            {
                // Clamp the lerp parameter.

                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);

                isFlapping = false; // Signal the loop to stop after this.
            }

            // Set the X angle to however much rotation is done so far.

            //transform.localRotation = Quaternion.Lerp(rotClosed, rotOpened, interpolationParameter);
            float rot = Mathf.Lerp(angleClosed, angleOpened, interpolationParameter);
            transform.localRotation = Quaternion.Euler(0, rot, 0);

            // Tell Unity to start us up again at some future time.

            yield return null;
        }

        // Toggle our open/closed state.

        isClosed = !isClosed;
    }
}
