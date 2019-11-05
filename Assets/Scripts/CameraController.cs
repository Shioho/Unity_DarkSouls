using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _playerHandle;
    private GameObject _cameraHandle;

    private PlayerInput _playerInput;

    private float _horizontalSpeed = 100.0f;
    private float _verticalSpeed = 80.0f;

    private float _tempAngle;
    private GameObject _model;
    private GameObject _mainCamera;
    private Vector3 _smoothVecolity;

    private void Awake()
    {
        _cameraHandle = transform.parent.gameObject;
        _playerHandle = _cameraHandle.transform.parent.gameObject;
        _playerInput = _playerHandle.GetComponent<PlayerInput>();
        _tempAngle = _cameraHandle.transform.localEulerAngles.x;
        _model = _playerHandle.transform.GetChild(0).gameObject;
        _mainCamera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        Vector3 tempModelEuler = _model.transform.eulerAngles;
        _playerHandle.transform.Rotate(Vector3.up, _playerInput.getCameraRight() * _horizontalSpeed * Time.deltaTime);


        _tempAngle -= _playerInput.getCameraUp() * _verticalSpeed * Time.fixedDeltaTime;
        _tempAngle = Mathf.Clamp(_tempAngle, -40, 30);
        _cameraHandle.transform.localEulerAngles = new Vector3(
            _tempAngle,
            0,
            0
        );

        _model.transform.eulerAngles = tempModelEuler;


        _mainCamera.transform.position = Vector3.SmoothDamp(_mainCamera.transform.position, transform.position, ref _smoothVecolity, 0.01f);
        _mainCamera.transform.eulerAngles = transform.eulerAngles;

    }
}
