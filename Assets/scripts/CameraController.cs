using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // simle camera movement script
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        // mouse wheel for up and down rotation
        float upDown = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        upDown *= Time.deltaTime;

        transform.Translate(0, upDown, translation);
        transform.Rotate(0, rotation, 0);

    }



}
