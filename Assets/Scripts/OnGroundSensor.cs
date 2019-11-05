using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    // Start is called before the first frame update
    private float _radius;
    private Vector3 _buttomPoint;
    private Vector3 _topPoint;
    private CapsuleCollider _capsCol;
    private float _offset = 0.05f;

    private void Awake()
    {
        _capsCol = gameObject.GetComponentInParent<CapsuleCollider>();
        _radius = _capsCol.radius-0.05f;
    }

    void FixedUpdate()
    {
        _buttomPoint = transform.position + transform.up * (_radius-_offset);
        _topPoint = transform.position + transform.up * (_capsCol.height - _radius-_offset);
        Collider[] colls = Physics.OverlapCapsule(_buttomPoint, _topPoint, _radius, LayerMask.GetMask("Ground"));
        if (colls.Length > 0)
        {
            SendMessageUpwards("OnGround");
        }
        else {
            SendMessageUpwards("OnSky");
        }

    }

}
