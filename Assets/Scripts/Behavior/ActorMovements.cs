using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorStates
{
  ONGROUND,
  JUMP
}



public class ActorMovements : MonoBehaviour
{
  [SerializeField] private float _walkSpeed;
  [SerializeField] private Animator _animator;
  
  [Header("Jump")]
  [SerializeField] private AnimationCurve _jumpCurve;
  [SerializeField] private float _jumpSpeed;
  [SerializeField] private float _jumpForce;

  [Header("Rotate to object")]
  [SerializeField] private Transform _rotateToObject;

  [SerializeField] private Transform _groundCheckPoint;

  private Transform _actorTransform;
  private ActorStates _actualState;
  
  private bool _isJumping = false;
  private bool _onGround = false;

  private Vector3 _startPos;
  private float _jumpProgress = 0f;

  private void Awake()
  {
    _actorTransform = GetComponent<Transform>();
  }

  private void Update()
  {
    Rotate();
    OnGroundCheck();
    JumpAnimation();
  }

  public void Walk(float direction)
  {
    Vector3 startPosition = _actorTransform.position;

    float xPos = Mathf.Clamp(startPosition.x + direction * _walkSpeed * Time.deltaTime, -11.0f, 21.0f);
    _actorTransform.position = new Vector3(xPos, startPosition.y, startPosition.z);

    _animator.SetFloat("Move", direction);
  }

  private void JumpAnimation()
  {
    if (!_isJumping)
      return;

    _jumpProgress += _jumpSpeed * Time.deltaTime;
    float jumpEvalution = _jumpCurve.Evaluate(_jumpProgress);
    float deltaYPos = _startPos.y + (jumpEvalution * _jumpForce);

    _actorTransform.position = new Vector3(_actorTransform.position.x,
                                           deltaYPos,
                                           _actorTransform.position.z);

    if (_jumpProgress >= 1f)
      _isJumping = false;
  }

  public void JumpInit()
  {
    if (!_onGround)
      return;

    _isJumping = true;
    _jumpProgress = 0f;
    _startPos = transform.position;
  }

  private void OnGroundCheck()
  {
    float distance = 0.3f;

    Ray ray = new Ray(_groundCheckPoint.position, Vector3.down);
    RaycastHit hit;

    _onGround = false;
    if (Physics.Raycast(ray, out hit, distance))
    {
      if (hit.collider.gameObject.layer == 12)
      {
        _onGround = true;
      }
    }

    _animator.SetBool("OnGround", _onGround);
  }

  private void Rotate()
  {
    Vector3 lookAtPoint = new Vector3(_rotateToObject.position.x, 
                           transform.position.y, 
                           transform.position.z);

    transform.LookAt(lookAtPoint);
  }

  private void OnCollisionEnter(Collision collision)
  {
    _isJumping = false;
  }
}
