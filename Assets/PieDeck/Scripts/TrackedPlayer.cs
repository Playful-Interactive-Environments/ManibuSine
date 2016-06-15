using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayer : ATrackingEntity
{
	public float Height;
	public override void SetPosition(Vector2 coords)
	{
		this.transform.position = new Vector3(coords.x * 0.002f, Height, coords.y * 0.002f);
	}

	
}
