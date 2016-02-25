using UnityEngine;
using System.Collections;

/// <summary>
/// Derive your player scripts from this.
/// Each GameType needs its own TuioPlayerScript for its player representations.
/// Each TuioPlayer has a SessionID which relates to a TuioContainer
/// </summary>
public abstract class ATuioPlayer : MonoBehaviour
{
	private long m_sessionID;

	public long SessionID {
		get {
			return m_sessionID;
		}
		set {
			m_sessionID = value;
		}
	}

	public virtual void MoveTo (Vector2 coords)
	{
		this.transform.position = new Vector3 (coords.x,coords.y, 0);
	}

}