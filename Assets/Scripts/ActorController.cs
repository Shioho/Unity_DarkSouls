using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private GameObject _model;
    private PlayerInput _playerInput;
    private Animator _anim;
    private Rigidbody _rigidBody;

    private float _walkSpeed = 2.0f;
    private float _jumpVelocity = 4.0f;
    private float _rollVelocity = 3.0f;

    private Vector3 _strustVec;

    void Awake()
    {
        _model = transform.GetChild(0).gameObject;
        _anim = _model.GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    //get the run mag
    private float getTargetRunMag()
    {
        return _playerInput.getMoveScale();
    }
    void Update()
    {

        float mag = _playerInput.getDirMag();
        _anim.SetFloat("forward", Mathf.Lerp(_anim.GetFloat("forward"), getTargetRunMag() * mag, 0.5f));
        if (_rigidBody.velocity.magnitude > 5.0f)
        {
            _anim.SetTrigger("roll");
        }

        switch (_playerInput.getPlayerState())
        {
            case PlayerState.Jump:
                {
                    _anim.SetTrigger("jump");
                    break;
                }
            case PlayerState.Attack:
                {
                    _anim.SetTrigger("attack1hA");
                    break;


                }



        }

        if (mag > 0.1f)
        {
            _model.transform.forward = Vector3.Slerp(_model.transform.forward, _playerInput.getForwardVec(), 0.3f);
        }

    }

    void FixedUpdate()
    {
        //_rigidBody.position += _model.transform.forward * mag * _walkSpeed * Time.fixedDeltaTime;
        Vector3 movinfVec = _model.transform.forward * _playerInput.getDirMag() * getTargetRunMag() * _walkSpeed;
        _rigidBody.velocity = new Vector3(movinfVec.x, _rigidBody.velocity.y, movinfVec.z) + _strustVec;
        _strustVec = Vector3.zero;
    }

    public GameObject getModel()
    {
        return _model;
    }


    //========Callbak==========
    public void OnJumpEnter()
    {
        _strustVec = new Vector3(0, _jumpVelocity, 0);
    }

    public void OnRollEnter()
    {
        _strustVec = new Vector3(0, _rollVelocity, 0);
    }

    public void OnJabUpdate()
    {
        _strustVec = _model.transform.forward * _anim.GetFloat("jabVelocity");
    }

    public void OnGround()
    {
        _anim.SetBool("isOnGround", true);

    }

    public void OnSky()
    {
        _anim.SetBool("isOnGround", false);
    }

}
