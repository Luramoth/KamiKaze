using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
	//vars
	[Header("Basic movement stuff")]
	public float speed = 6f;
	public float jumpPower = 20f;
	public float gravity = 10f;

	[Header("Advanced tweaks")]
	public float turnSmoothSpeed = 0.1f;
	public bool cursorLocked = true;

	float turnSmoothVel;

	//Objects
	[Header("References")]
	public CharacterController controller;
	public Transform cam;

	private PlayerInput playerInput;

	private void Awake() {
		playerInput = new PlayerInput();
	}

	private void OnEnable() {
		playerInput.Enable;
	}

	private void OnDisable() {
		playerInput.Disable;
	}

	private void Update() {
		InputHandler();
	}

	public void InputHandler(){
		Vector2 moveDirection = playerInput.characterControls.Move.ReadValue<Vector2>();

		// based ont he players movement direction, try to rotate the player model to match it
		float targetAngle = Mathf.Atan2(moveDirection.x,moveDirection.y) * Mathf.Rad2Deg + cam.eulerAngles.y; //this find the target angle the player should be facing in
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothSpeed);// the creates a variable tomake sure the transition is smooth when switching angles
		transform.rotation = Quaternion.Euler(0f,angle,0f); // the applies the angle
	}
}
