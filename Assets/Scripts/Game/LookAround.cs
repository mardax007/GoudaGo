using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    private float startPosY;
    private float currentPosY;
    private float differenceY;

    public float sensitivity;

    void Update()
    {
        try
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPosY = touch.position.x;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                currentPosY = touch.position.x;

                differenceY = startPosY - currentPosY;



                transform.Rotate(0, differenceY / (0.04f * sensitivity), 0);

                startPosY = touch.position.x;
            }
        }
        catch
        {
            // no touch
        }
    }
}
