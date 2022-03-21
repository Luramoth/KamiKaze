using System.Collections;
using System.Collections.Generic;
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
	private Vector3 inputVector;

	private float maxVelocity = 10f;

	public float walkSpeed = 20f;
	public float runSpeed = 35f;

	//objects
	private Rigidbody body;

	// Start is called before the first frame update
	void Start()
	{
		body = gameObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		inputVector = new Vector3
		(
				Input.GetAxisRaw("Horizontal"),
				0,
				Input.GetAxisRaw("Vertical")
		);

		inputVector = Vector3.Normalize(inputVector);
	}

	void FixedUpdate ()
	{
		body.AddForce(inputVector * runSpeed);

		body.velocity = Vector3.ClampMagnitude(body.velocity, maxVelocity);
	}
}
