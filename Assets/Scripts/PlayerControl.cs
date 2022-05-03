using UnityEngine;

/*
	This file is part of KamiKaze.

	KamiKaze is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
	KamiKaze is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
	You should have received a copy of the GNU Affero General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/agpl-3.0.html>.
*/

public class PlayerControl : MonoBehaviour
{
	//vars
	[Header("Basic movement stuff")]
	public float speed = 6f;
	public float jumpPower = 6f;
	public float gravity = 9.81f;

	public bool mouseLock = true;

	[Header("Advanced tweaks")]
	public float turnSmoothSpeed = 0.1f;

	private float turnSmoothVel;
	private Vector3 velocity;


	//Objects
	[Header("References")]
	public CharacterController controller;
	public Transform cam;

	// Update is called once per frame
	void Update()
	{
		ControlHandler();
	}

	//simple system to handle player input
	void ControlHandler()
	{
		if (controller.isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		//gather axis movements
		Vector3 inputAxis = new Vector3
		(
			Input.GetAxisRaw("Horizontal"),
			0f,
			Input.GetAxisRaw("Vertical")
		).normalized;

		//make character jump if space is pressed
		if (Input.GetButtonDown("Jump") && controller.isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpPower * -2.0f * -gravity);
		}

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

		//handle gravity
		velocity.y -= gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);

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
}
