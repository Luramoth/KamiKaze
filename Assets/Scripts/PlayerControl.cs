using UnityEngine;

/*
	This file is part of KamiKaze.

	KamiKaze is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
	KamiKaze is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
	You should have received a copy of the GNU Affero General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/agpl-3.0.html>.
*/

// TO/DO: fix bug where player will float in air on slopes, possibly by making it so the palyer doesent float in the air and will instead be able to jump mid air for a few frames
// TO/DO: possibly add double jump?
// TODO: add dash
// TODO: add roll
// TODO: change movement system to state-based system
// !WARNING: CODE IS NON-FUNCTIONAL

public class PlayerControl : MonoBehaviour
{
	//vars
	[Header("Basic movement stuff")]
	public float walkSpeed = 6f;
	public float runSpeed = 10f;
	public float jumpPower = 3f;
	public float baseGravity = 19.62f;
	public bool mouseLock = true;
	public bool isRunning = false;
	public bool isJumping = false;
	public bool hasDoubleJumped = false;

	[Header("Advanced tweaks")]
	public float turnSmoothSpeed = 0.1f;
	private float turnSmoothVel;
	private Vector3 velocity;

	[Header("References")]
	public CharacterController controller;
	public Transform cam;

	public enum moveStates {walking,running,jumping,doubleJumping,falling,dashing};
	public moveStates moveState = moveStates.falling;

	// Update is called once per frame
	void Update()
	{
		switch(moveState)
		{
			case moveStates.walking:

				if (Input.GetButtonDown("Jump"))
				{
					moveState = moveStates.jumping;
					basicJump(baseGravity);
				}
				
				if (Input.GetKey("left shift"))
				{
					moveState = moveStates.running;
				}

				basicGravity(baseGravity);
				basicControl();
				basicMove(walkSpeed);

				break;
			case moveStates.running:

				if (Input.GetButtonDown("Jump"))
				{
					moveState = moveStates.dashing;
					dash();
				}
				
				if (!Input.GetKey("left shift"))
				{
					moveState = moveStates.walking;
				}

				basicGravity(baseGravity);
				basicControl();
				basicMove(runSpeed);

				break;
			case moveStates.jumping:

				if (Input.GetButtonDown("Jump"))
				{
					moveState = moveStates.doubleJumping;
					basicJump(baseGravity);
				}
				if (Input.GetKey("left shift"))
				{
					moveState = moveStates.dashing;
					dash();
				}
				if (controller.isGrounded)
				{
					moveState = moveStates.walking;
				}

				basicGravity(baseGravity);
				basicControl();
				basicMove(runSpeed);
				break;
			case moveStates.doubleJumping:

				if (Input.GetKey("left shift"))
				{
					moveState = moveStates.dashing;
					dash();
				}
				if (controller.isGrounded)
				{
					moveState = moveStates.walking;
				}

				basicGravity(baseGravity);
				basicControl();
				basicMove(runSpeed);
				break;
			case moveStates.falling:

				if (controller.isGrounded)
				{
					moveState = moveStates.walking;
				}

				basicGravity(baseGravity);
				basicControl();
				basicMove(walkSpeed);
				break;
			case moveStates.dashing:

				if (controller.isGrounded)
				{
					moveState = moveStates.walking;
				}
				break;
		}
	}

	//simple system to handle player input
	void basicControl()
	{
		if (controller.isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		//unlock cursor when escape is pressed
		if (Input.GetButtonDown("Cancel"))
		{
			mouseLock = !mouseLock;
		}

		if (mouseLock == true)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	bool canJump()
	{
		if (controller.isGrounded)// if the player is on the ground they cant possibly be on coyote time or double jumping so allow them to jump
		{
			return true;
		}
		else //  else check if they are double jumping or have coyote time
		{
			if (hasDoubleJumped == false)
			{
				hasDoubleJumped = true;

				return true;
			}
			else
			{
				return false;
			}
		}
	}

	void basicMove(float speed)
	{
		//gather axis movements
		Vector3 inputAxis = new Vector3
		(
			Input.GetAxisRaw("Horizontal"),
			0f,
			Input.GetAxisRaw("Vertical")
		).normalized;

		//apply movements if input is detected
		if (inputAxis.magnitude >= 0.1f)
		{
			// based ont he players movement direction, try to rotate the player model to match it
			float targetAngle = Mathf.Atan2(inputAxis.x,inputAxis.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //this find the target angle the player should be facing in
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothSpeed);// the creates a variable to make sure the transition is smooth when switching angles
			transform.rotation = Quaternion.Euler(0f,angle,0f); // the applies the angle

			// move the player
			Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward; // this will take the current direction the camera is facing
			controller.Move(moveDir.normalized * speed * Time.deltaTime);// this uses the direction the camera is facing in order to move forward
		}
	}

	void basicJump(float gravity)
	{
		velocity.y = Mathf.Sqrt(jumpPower * -2.0f * -gravity);
		isJumping = true;
	}

	void basicGravity(float gravity)
	{
		velocity.y -= gravity * Time.deltaTime; //handle gravity
		controller.Move(velocity * Time.deltaTime);// handle character vertical movement
	}

	void dash()
	{
		Debug.LogError("dash!");
	}
}