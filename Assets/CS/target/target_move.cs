using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_move : MonoBehaviour
{
    private float distance;
    private Vector3 mouse;
    private Vector3 target;

    public Transform cameraObj;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);

        transform.position = raycastHit.point;
    }
}
