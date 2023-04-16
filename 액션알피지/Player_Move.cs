using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Move : MonoBehaviour
{


    Player_Input _input;
    Player_Animator _animator;

    [SerializeField]
    float _player_Height_Distance = 1.0f;

    public float _walkSpeed;
    public float _moveSpeed;
    public float _sprintSpeed;
    public float _currentSpeed;
    public float _airSpeed;
    public float _gravitySpeed;
    public Rigidbody _rigidBody;

    Vector3 _moveDir;


    Transform _playerCamera;
    [SerializeField]
    float _jumpHeight = 2;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float _power;

    [SerializeField]
    float _controlGroundMove;


    public void Init(Player_Input _iP, Player_Animator _pA)
    {
        this._input = _iP;
        _animator = _pA;
        _playerCamera = Camera.main.transform;
        _currentSpeed = _moveSpeed;
    }

    public void SpeedSetting()
    {
        if (Game_Master.instance.PM._isSprintState && Game_Master.instance.PM._isGround)
        {
            _currentSpeed = _sprintSpeed;
            return;
        }

        else if (Game_Master.instance.PM._isWalkState && Game_Master.instance.PM._isGround)
        {
            _currentSpeed = _walkSpeed;
            return;
        }
        _currentSpeed = _moveSpeed;
    }




    public void CalDir()
    {
        _power = _input._vertical + _input._horizontal;
        _power = Mathf.Clamp(_power, 0, 1);

        _moveDir = _playerCamera.transform.forward * _input._vertical;
        _moveDir += _playerCamera.transform.right * _input._horizontal;
        _moveDir.y = 0;
    }

    public void CharRotate(float delta)
    {
        if (!Game_Master.instance.PM._isRotate || Game_Master.instance.PM._isInteracting)
        {
            return;
        }

        if (_animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Landing") && _animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop_Move"))
        {
            return;

        }

        Vector3 targetDir = _moveDir;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        float rotationspeed;
        if (Game_Master.instance.PM._isGround)
        {
            rotationspeed = rotationSpeed;
        }
        else
        {
            rotationspeed = rotationSpeed / 15f;
        }

        if (Game_Master.instance.PM.LockOnMode)
        {

            transform.LookAt(Game_Master.instance.PM._camera.CurrentLockonTarget);
            Quaternion temp = transform.rotation;
            temp.Set(0, temp.y, 0, temp.w);
            transform.rotation = temp;

            return;
        }

        Quaternion tatgetlock = Quaternion.LookRotation(targetDir);
        Quaternion targetrotation = Quaternion.Slerp(transform.rotation, tatgetlock, rotationspeed * delta);
        transform.rotation = targetrotation;
    }

    public void GroundMove()
    {
        _rigidBody.velocity = new Vector3(_moveDir.x, 0, _moveDir.z).normalized * _currentSpeed;
    }

    public void Jumping()
    {
        //Debug.Log("상승");

        Vector3 T = _rigidBody.velocity;
        T.x = transform.forward.x * _airSpeed * _power;
        T.z = transform.forward.z * _airSpeed * _power;
        T.y = _jumpHeight;
        _rigidBody.velocity = T;
    }
    public void Jumping_Move()
    {

        Vector3 T = _rigidBody.velocity;
        T.x = transform.forward.x * _airSpeed * _power;
        T.z = transform.forward.z * _airSpeed * _power;
        _rigidBody.velocity = T;

    }
    public void FallingStart()
    {

        Vector3 T = _rigidBody.velocity;
        T.x = transform.forward.x * _airSpeed * _power;
        T.z = transform.forward.z * _airSpeed * _power;
        T.y += Physics.gravity.y * Time.deltaTime;

    }

    public void Falling()
    {

        Vector3 T;
        T = _rigidBody.velocity;
        Ray ray = new Ray(transform.position, Vector3.down);
        //Debug.Log("추락");
        T.y += Physics.gravity.y * Time.deltaTime;
        T.y = Mathf.Clamp(T.y, -53, 0);
        if (Physics.SphereCast(ray, 0.2f, out RaycastHit hitInfo, (1.8f / 2f) + 0.2f))//원형미끌어짐생성
        {
            //Debug.Log(Vector3.Angle(Vector3.down, hitInfo.normal)); //충돌각
            if (Vector3.Angle(Vector3.down, hitInfo.normal) < 135) //135도이상이 나오면 걸어서올라갈수있는각도값일때 나옴 미끌어질필요없음
            {
                T.x += hitInfo.normal.x;
                T.z += hitInfo.normal.z;
                _rigidBody.velocity = T;
                return;
            }

        }
        T.x = transform.forward.x * _airSpeed * _power;
        T.z = transform.forward.z * _airSpeed * _power;
        _rigidBody.velocity = T;
        return;
    }
    public void Jump_Function()
    {
        if (Game_Master.instance.PM._isJump)
        {
            _animator._animator.CrossFade("Jump", 0f);
            Game_Master.instance.PM._isJump = false;
        }
    }
}
