using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public Transform cameraObj;

    void Start() { }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);

        Vector3 target = raycastHit.point;
        target.y = transform.position.y;

        transform.LookAt(target);
    }
}
