using UnityEngine;

/*
    This file is part of KamiKaze.

    KamiKaze is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
    KamiKaze is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/agpl-3.0.html>.
*/

public class PlayerMovement : MonoBehaviour
{

	//vars
	[Header("Basic movement stuff")]
	public float speed = 6f;
	public float jumpPower = 2f;
	public float gravity = 10f;

	[Header("Advanced tweaks")]
	public float turnSmoothSpeed = 0.1f;
	public bool cursorLocked = true;

	float turnSmoothVel;

	//Objects
	[Header("References")]
	public CharacterController controller;
	public Transform cam;

	// Update is called once per frame
	void Update()
	{
		InputHandler();
	}

	//simple system to handle player input
	void InputHandler()
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
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothSpeed);// the creates a variable tomake sure the transition is smooth when switching angles
			transform.rotation = Quaternion.Euler(0f,angle,0f); // the applies the angle

			// move the player
			Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward; // this will take the current directiont he camera is facing
			controller.Move(moveDir.normalized * speed * Time.deltaTime);// this uses the direction the camera is facing in order to move forward
		}

		if (Input.GetKeyDown("escape"))
		{
			cursorLocked = !cursorLocked;
		}

		if (cursorLocked == true)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
