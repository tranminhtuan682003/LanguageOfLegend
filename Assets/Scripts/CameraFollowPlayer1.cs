using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;
    private float currentZoom = 10f;
    //xoay camera
    private float yawSpeed = 100f;
    private float currentYaw = 0f;
    void Start()
    {
        
    }
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw += Input.GetAxis("Horizontal")*yawSpeed*Time.deltaTime;
    }
    private void LateUpdate()
    {
        transform.position = player.position - offset * currentZoom;
        transform.LookAt(player.position + Vector3.up * pitch);

        //xoay camera
        transform.RotateAround(player.position, Vector3.up, currentYaw);
    }
}
