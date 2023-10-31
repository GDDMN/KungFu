using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private ActorMovements _actorMovements;
  
  private InputManager _inputManager;
  private float _direction;

  private void Awake()
  {
    _inputManager = new InputManager();
  }

  private void OnEnable()
  {
    _inputManager.Enable();
    _inputManager.Player.Jump.performed += context => JumpInput();
  }

  private void OnDisable()
  {
    _inputManager.Player.Jump.performed -= context => JumpInput();
    _inputManager.Disable();
  }

  private void Update()
  {
    WalkInput(_inputManager.Player.Move.ReadValue<float>());
  }

  private void WalkInput(float direction)
  {
    _actorMovements.Walk(direction);
  }

  private void JumpInput()
  {
    _actorMovements.JumpInit();
  }
}
