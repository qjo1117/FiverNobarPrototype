using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boom_State {
    Default,
    Daley,
};
public class PlayerController : MonoBehaviour {
    public float _speed = 10.0f;

    public float _velocity = 100.0f;

    [Header("Jump_variable")]
    public float jump_height = 100.0f;
    public float jump_speed = 10.0f;
    public float _currentJump = 0.0f;

    public bool _isGround = false;


    [Header("Boom")]
    public float _shootDelay = 0.0f;
    public float _shootMaxDelay = 1.0f;

    public float _lookRadius = 0.0f;

    public Boom _boom = null;
    public bool _isShootDelay = false;

    [Header("State")]
    public float            _stateDelay = 0.5f;
    public float            _currentStateDelay = 0.0f;
    public Boom_State       _eState = Boom_State.Default;


    public bool             _isState = false;
    public const float      _buttonDelay = 0.5f;

    Animator        _anim = null;
    Rigidbody       _rigid = null;

    public enum PlayerState {
        Idle,
        Forward,
        BackForward,
        Shoot,
        Jump,
	}

    public PlayerState      _state = PlayerState.Idle;
    public PlayerState State
	{
		set
		{
            if(_state == value) {
                return;
			}

            switch(value)
			{
                case PlayerState.Idle:
                    _anim.Play("Idle", -1, 0.314f);
                    break;
                case PlayerState.Forward:
                    _anim.Play("Forward", -1, 0.314f);
                    break;
                case PlayerState.BackForward:
                    _anim.Play("Backward", -1, 0.314f);
                    break;
                case PlayerState.Shoot:
                    _anim.CrossFade("Shoot", 0.314f, -1);
                    break;
                case PlayerState.Jump:
                    _anim.CrossFade("Jump", 0.314f, - 1);
                    break;
            }

            _state = value;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Check();
        InputController();
    }

    private void Check()
	{
        IsGround();

        _shootDelay += Time.deltaTime;
        if (_boom != null && _boom.gameObject.activeInHierarchy == true) {
            if (Input.GetKeyUp(KeyCode.Mouse0)) {
                if (_eState == Boom_State.Daley) {
                    _boom.Explosion();
                }

                _eState = Boom_State.Default;
                _currentStateDelay = 0.0f;
            }
        }
    }

    private void InputController()
	{

        InputMouseRotation();

        if (Input.anyKey == false) {
            State = PlayerState.Idle;
            return;
        }

        InputShoot();
        InputMove();
        InputJump();
    }


    private void InputShoot()
	{
        if (_isShootDelay == true && _shootMaxDelay > _shootDelay) {
            return;
		}

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            State = PlayerState.Shoot;
            Vector3 offset = transform.position + transform.up * 1.3f + transform.forward * 1.3f;

            _boom = Managers.Resource.NewPrefab("Boom").GetComponent<Boom>();
            _boom.CreateBoom(offset, transform.forward * _velocity);

            _isState = true;

            _shootDelay = 0.0f;
        }

        
        if (_eState == Boom_State.Default && Input.GetKey(KeyCode.Mouse0)) {
            _currentStateDelay += Time.deltaTime;
            if(_currentStateDelay > _stateDelay) {
                _eState = Boom_State.Daley;
            }
        }

    }

    // 기획서 1
    // 키보드 움직임에 대한 공평성 
    private void InputMove()    
	{
        Vector3 move = Vector3.zero;
        if(Input.GetKey(KeyCode.W)) {
            move.z += 1.0f;
            State = PlayerState.Forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            move.z -= 1.0f;
            State = PlayerState.BackForward;
        }
        if (Input.GetKey(KeyCode.A)) {
            move.x -= 1.0f;
            if(_state == PlayerState.Idle) {
                State = PlayerState.Forward;
            }
        }
        if (Input.GetKey(KeyCode.D)) {
            move.x += 1.0f;
            if (_state == PlayerState.Idle) {
                State = PlayerState.Forward;
            }
        }

        if(move.magnitude > 1.3f) {
            move = move.normalized;
		}


        transform.position += move * _speed * Time.deltaTime;
    }

    // 시선은 귀찮으닌깐 참고자료 찾는걸로
    private void InputMouseRotation()
    {
        Vector3 playerToScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 mousePos = Input.mousePosition;
        Vector3 tempPos = mousePos - playerToScreenPos;
        _lookRadius = Mathf.Atan2(tempPos.y, tempPos.x);

        transform.rotation = Quaternion.Euler(0f
            , (-_lookRadius * Mathf.Rad2Deg) + 90f
            , 0f);
    }

    private void InputJump()
	{
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            if (_isGround == true) {
                Managers.Log("됨");
                _rigid.AddForce(Vector3.up * jump_height);
                _isGround = false;
                State = PlayerState.Jump;
            }
        }
        
	}

    private void IsGround()
	{
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, -Vector3.up, out hit, 1.1f, 1 << 6)) {
            _isGround = true;
        }
    }


	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Vector3.up);

        Gizmos.DrawRay(transform.position,  transform.forward * 2.0f);
	}
}
