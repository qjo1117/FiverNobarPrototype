using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform    _target = null;
    public Vector3      _delta = Vector3.zero;

    public Transform    _cameraArm = null;
    public Transform    _characterBody = null;

    // 아직 안씀
    public LayerMask _layer;

    // Start is called before the first frame update
    void Start()
    {


    }

    private void PlayerFind()
	{
        _target = GameObject.FindObjectOfType<PlayerController>().transform;
        _delta = new Vector3(0.0f, 10.0f, -5.0f);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        ViewMode();

    }

    private void ViewMode()
	{
        Vector3 origin = _target.position;
        RaycastHit hit;
        if(Physics.Raycast(origin, _delta, out hit, _delta.magnitude, 1<< 9)) {
            float dist = (hit.point - _target.position).magnitude * 0.8f;
            transform.position = _target.position + (_delta.normalized * dist);
        }
        else {
            transform.position = _target.position + _delta;
        }
	}

    void LookAround()
    {
        // 일반적인 상황에 모두 적용시켜준다.
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = _cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        // 범위 제한
        x = x < 180.0f ? Mathf.Clamp(x, -1.0f, 70.0f) : Mathf.Clamp(x, 355.0f, 361f);

        _cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        _characterBody.rotation = Quaternion.Euler(camAngle.x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    void QuarterViewMode()
    {
        Vector3 delta = _cameraArm.up * _delta.y + _cameraArm.forward * _delta.z;

        RaycastHit hit;
        if (Physics.Raycast(_target.position, delta, out hit, _delta.magnitude, 1 << 9))
        {
            float dist = (hit.point - _target.position).magnitude * 0.8f;
            transform.position = _target.position + (delta.normalized * dist);
        }
        else
        {
            transform.position = _target.position + delta;
        }
    }
}
