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

	private float maxVelocity;

	public float walkSpeed = 20f;
	public float runSpeed = 35f;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
		MovePlayer();
	}

	//functions
	void GetInput()
	{

	}
	void MovePlayer()
	{
		inputVector = new Vector3
		(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"),
			0
		);

		inputVector = Vector3.Normalize(inputVector);
	}
}
