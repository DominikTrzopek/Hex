using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Camera_move : MonoBehaviour
{
    const int rotationX = 45;

    public Camera cam;
    public bool move;
    public bool doneScrolling = false;

    public Vector3 endPosition;
    public float camera_speed = 20f;
    public float scroll_speed = 20f;
    public float maxY = 12f, minY = 4f;
    public GameObject pivot;
    const float height = 40f;
    Vector2 moveMulti;
    float cameraRotation = 0;
    public float cameraRotationSpeed;
    float cameraSize = 8f;
    int mapSize;

    void Awake()
    {
        mapSize = TCPConnection.instance.serverInfo.mapSize;
    }

    void Update()
    {
        QualitySettings.shadowDistance = 100;
        Vector2 panlimitX = new Vector2(-10, mapSize * 1.5f);
        Vector2 panlimitZ = new Vector2(-10, mapSize * 1.5f);
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
        else if(IsMouseOverGameWindow)
        {
            moveMulti = new Vector2(1, 1);

            Vector3 pos = cam.transform.position;
            float speed = camera_speed * Time.deltaTime * transform.position.y / 50f;
            if (Input.GetKey("w"))
            {
                pos += pivot.transform.forward * speed * moveMulti.y;
                move = false;
            }
            if (Input.GetKey("s"))
            {
                pos -= pivot.transform.forward * speed * moveMulti.y;
                move = false;
            }
            if (Input.GetKey("d"))
            {
                pos += pivot.transform.right * speed * moveMulti.x;
                move = false;
            }
            if (Input.GetKey("a"))
            {
                pos -= pivot.transform.right * speed * moveMulti.x;
                move = false;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            cam.orthographicSize -= scroll * scroll_speed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, panlimitX.x, panlimitX.y);
            pos.z = Mathf.Clamp(pos.z, panlimitZ.x, panlimitZ.y);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minY, maxY);
            transform.position = pos;
        }

        if(move)
            MoveCamera();
    }

    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    void MoveCamera()
    {
        float speed = 2.5f;
        float sizeStep = 0.25f;
        transform.position = Vector3.Lerp(transform.position, endPosition, speed * Time.deltaTime);
        float sizeDiff = cam.orthographicSize - cameraSize;
        if(cam.orthographicSize > cameraSize + sizeStep && !doneScrolling)
        {
            cam.orthographicSize -= sizeStep;
            doneScrolling = true;
        }
        else if (cam.orthographicSize < cameraSize - sizeStep && !doneScrolling)
        {
            cam.orthographicSize += sizeStep;
            doneScrolling = true;
        }
    }
}
