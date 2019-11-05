using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    Roll,
    Jab,
    Attack,
}


public class PlayerInput : MonoBehaviour
{
    [Header("===== Key Setting =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyRun = "left shift";
    public string keyJump = "space";
    public string keyCUp = "up";
    public string keyCDown = "down";
    public string keyCLeft = "left";
    public string keyCRight = "right";

    public bool inputEnabled = true;

    private PlayerState _curState = PlayerState.Idle;

    private float _dUp;
    private float _dRight;
    private float _targetUp;
    private float _targetRight;
    private float _velocityUp;
    private float _velocityRight;
    private float _moveScale;
    private float _cUp;
    private float _cRight;


    public float getCameraRight() {
        return _cRight;
    }
    public float getCameraUp()
    {
        return _cUp;
    }

    // Update is called once per frame
    void Update()
    {
        //camera input dir
        _cUp = (Input.GetKey(keyCUp) ? 1.0f : 0) - (Input.GetKey(keyCDown) ? 1.0f : 0);
        _cRight = (Input.GetKey(keyCRight) ? 1.0f : 0) - (Input.GetKey(keyCLeft) ? 1.0f : 0);

        _targetUp = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        _targetRight = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (!inputEnabled)
        {
            _targetUp = 0;
            _targetRight = 0;
            return;
        }

        //set the state
        if (_targetUp != 0 || _targetRight != 0)
        {
            _moveScale = 1.0f;
            setPlayerState(PlayerState.Walk);
            if (Input.GetKey(keyRun))
            {
                _moveScale = 2.0f;
                setPlayerState(PlayerState.Run);
            }
        }
        else {
            setPlayerState(PlayerState.Idle);
        }


        if (Input.GetKey(keyJump))
        {
            setPlayerState(PlayerState.Jump);
            inputEnabled = false;
        }

        if (Input.GetMouseButton(0)) {
            setPlayerState(PlayerState.Attack);
        }

        _dUp = Mathf.SmoothDamp(_dUp, _targetUp, ref _velocityUp, 0.1f);
        _dRight = Mathf.SmoothDamp(_dRight, _targetRight, ref _velocityRight, 0.1f);

    }

    //get the move dir mag
    public float getDirMag()
    {
        Vector2 tempVec = Square2Circle(_dRight, _dUp);
        float mag = tempVec.magnitude;
        return mag;
    }


    //get the forward dir
    public Vector3 getForwardVec()
    {
        Vector3 vec = _dUp * transform.forward + _dRight * transform.right;
        vec.Normalize();
        return vec;
    }

    private Vector2 Square2Circle(float right, float up)
    {
        Vector2 output = Vector2.zero;

        output.x = right * Mathf.Sqrt(1 - (up * up) / 2.0f);
        output.y = up * Mathf.Sqrt(1 - (right * right) / 2.0f);
        return output;
    }

    public float getMoveScale() {
        return _moveScale;
    }


    private void setPlayerState(PlayerState state)
    {
        if (getPlayerState() == state) return;
        _curState = state;
    }

    public PlayerState getPlayerState()
    {
        return _curState;
    }


    //========Callback===========

    public void OnGround()
    {
        if (getPlayerState() == PlayerState.Roll||getPlayerState()==PlayerState.Jab) return;
        inputEnabled = true;
    }

    public void OnSky()
    {
        inputEnabled = false;
    }

    public void OnRollEnter() {
        setPlayerState(PlayerState.Roll);
        inputEnabled = false;
    }
    public void OnRollExit()
    {
        setPlayerState(PlayerState.Idle);
        inputEnabled = true;
    }
    public void OnFallEnter()
    {
        setPlayerState(PlayerState.Fall);
    }
    public void OnFallExit()
    {
        setPlayerState(PlayerState.Roll);
    }
    public void OnJabEnter()
    {
        inputEnabled = false;
        setPlayerState(PlayerState.Jab);
    }
    public void OnJabExit()
    {
        inputEnabled = true;
        setPlayerState(PlayerState.Idle);
    }

}
