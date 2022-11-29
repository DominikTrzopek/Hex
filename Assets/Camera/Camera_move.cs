﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{
    const int rotationX = 45;
    public Camera cam;
    public float camera_speed = 20f;
    public float scroll_speed = 20f;
    public float maxY = 120f, minY = 0f;
    public GameObject pivot;
    const float height = 40f;
    Vector2 moveMulti;
    float cameraRotation = 0;
    public float cameraRotationSpeed;
    void Update()
    {
        QualitySettings.shadowDistance = 100;
        Vector2 panlimit = new Vector2(100 * 2f, 100 * 2f);
        float border = Screen.height / 6f;
        Vector2 cursorPoz = new Vector2(Mathf.Abs(Input.mousePosition.x - Screen.width / 2f), Mathf.Abs(Input.mousePosition.y - Screen.height / 2f));
        if (Input.GetMouseButton(2))
        {
            moveMulti = new Vector2(0, 0);
            transform.rotation = Quaternion.Euler(rotationX, cameraRotation, 0);
            if (Input.mousePosition.x - Screen.width / 2f > 0)
                cameraRotation += cameraRotationSpeed;
            else if (Input.mousePosition.x - Screen.width / 2f < 0)
                cameraRotation -= cameraRotationSpeed;
        }
        else
            moveMulti = new Vector2(1, 1);

            Vector3 pos = cam.transform.position;
            float speed = camera_speed * Time.deltaTime * transform.position.y / 50f;
            if (Input.mousePosition.y > Screen.height - border)
            {
                pos += pivot.transform.forward * speed * moveMulti.y;
            }
            if (Input.mousePosition.y < border)
            {
                pos -= pivot.transform.forward * speed * moveMulti.y;
            }
            if (Input.mousePosition.x > Screen.width - border)
            {
                pos += pivot.transform.right * speed * moveMulti.x;
            }
            if (Input.mousePosition.x < border)
            {
                pos -= pivot.transform.right * speed * moveMulti.x;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scroll_speed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, -panlimit.x, panlimit.x);
            pos.z = Mathf.Clamp(pos.z, -panlimit.y, panlimit.y);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        
        }


}