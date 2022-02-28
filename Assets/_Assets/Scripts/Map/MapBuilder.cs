using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
	private float leftLimit;
	private float upLimit;
	private float rightLimit;
	private float bottomLimit;

	private bool init = false;

	public void Init(List<float> limits)
	{
		leftLimit = limits[0];
		upLimit = limits[1];
		rightLimit = limits[2];
		bottomLimit = limits[3];

		init = true;
	}

	void Update()
	{
		if (!init) return;
		//more things
	}
}
