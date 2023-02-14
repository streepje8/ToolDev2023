using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Preview preview;
    public float sensitifity;

    private bool inDrag = false;
    private Vector3 dragStartPos;
    private Quaternion startRotation;
    
    private void Awake()
    {
        if (preview == null)
        {
            Debug.LogWarning("Skill Issue: Preview of CameraController is unassigned!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (preview.isBeeingHovered)
        {
            if (Input.GetMouseButton(0) && !inDrag)
            {
                dragStartPos = Input.mousePosition;
                startRotation = transform.rotation;
                inDrag = true;
            }
        }
        if (inDrag)
        {
            Vector3 delta = dragStartPos - Input.mousePosition;
            transform.rotation = startRotation * Quaternion.Euler(new Vector3(delta.y,delta.x,0) * (sensitifity * Time.deltaTime));
            if (!Input.GetMouseButton(0)) inDrag = false;
        }
    }
}
