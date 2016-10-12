using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayer : ATrackingEntity
{
    //add offset to adjust tracking area without changing the whole setup
    private float offsetX = 4.1f;//5.275f in pie;
    private float offsetY = 0;
	public float Height;
	public override void SetPosition(Vector2 coords)
	{
        if (this == null)  // prevent exception at restart
            return;
		this.transform.position = new Vector3(coords.x * 0.01f + offsetX, Height, coords.y * 0.01f + offsetY);
	}
}