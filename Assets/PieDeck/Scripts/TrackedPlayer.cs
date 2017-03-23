using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayer : ATrackingEntity
{
    //add offset to adjust tracking area without changing the whole setup
    private float offsetX = 5.275f; // this offsets fixes the offset problem 0 = AEC | 5.275f = PIE;
    private float offsetY = 0;
	public float Height;
	public override void SetPosition(Vector2 coords)
	{
        if (this == null)  // prevent exception at restart
            return;
		this.transform.position = new Vector3(coords.x * 0.01f + offsetX, Height, coords.y * 0.01f + offsetY);
	}
}