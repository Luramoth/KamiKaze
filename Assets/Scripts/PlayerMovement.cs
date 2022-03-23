using UnityEngine;

/*
    This file is part of Microda.

    Microda is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
    Microda is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/agpl-3.0.html>.
*/

public class PlayerMovement : MonoBehaviour
{

	//vars
	[Header("Basic movement stuff")]
	public float speed = 6f;

	[Header("Advanced tweaks")]
	public float turnSmoothSpeed = 0.1f;

	private float turnSmoothVel;

	//Objects
	[Header("References")]
	public CharacterController controller;

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
			float targetAngle = Mathf.Atan2(inputAxis.x,inputAxis.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothSpeed);
			transform.rotation = Quaternion.Euler(0f,angle,0f);

			// move the player
			controller.Move(inputAxis * speed * Time.deltaTime);
		}
	}
}
