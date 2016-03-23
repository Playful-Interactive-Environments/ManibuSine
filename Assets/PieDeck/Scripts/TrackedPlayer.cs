using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayer : ATrackingEntity
{
	public float Height;



	public override void SetPosition(Vector2 coords)
	{
		this.transform.position = new Vector3(coords.x, Height, coords.y);

	}

	

}
