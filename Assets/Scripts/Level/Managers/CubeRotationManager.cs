using System;
using System.Collections;
using UnityEngine;

public class CubeRotationManager : MonoBehaviour
{
	[Header("Rotation")]
	[SerializeField] private Transform XJoint;
	[SerializeField] private Transform YJoint;
	[SerializeField] private float rotationSpeed = 1f;

	public void Rotate(Vector3 axis, Action rotated)
	{
		StartCoroutine(RotationTask(axis, rotated));
	}

	private IEnumerator RotationTask(Vector3 axis, Action rotated)
	{
		float progress = 0f;
		float lastAngle = 0f;
		float targetAngle = 90f;

		while (progress <= 1f)
		{
			yield return new WaitForFixedUpdate();
			progress += Time.fixedDeltaTime * rotationSpeed;
			var newAngle = Mathf.Lerp(0, targetAngle, progress);
			var rotation = newAngle - lastAngle;
			lastAngle = newAngle;
			transform.Rotate(axis, rotation, Space.World);
		}

		rotated?.Invoke();
	}
}
