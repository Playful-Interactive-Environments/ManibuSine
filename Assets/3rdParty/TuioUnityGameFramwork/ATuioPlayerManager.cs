using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityTuio;
using UnityPharus;

abstract public class ATuioPlayerManager : AManager<ATuioPlayerManager> {

	protected List<ATuioPlayer> m_playerList;
	public GameObject m_playerPrefab;
	[SerializeField] private bool m_addUnknownPlayerOnUpdate = true;

	private bool TRACK_TUIO_CURSORS = true;
	private bool TRACK_TUIO_OBJECTS = false;
	private bool TRACK_TUIO_BLOBS = false;

	public List<ATuioPlayer> PlayerList
	{
		get { return m_playerList; }
	}

	protected override void Awake()
	{
		base.Awake();
		m_playerList = new List<ATuioPlayer>();
	}

	void OnEnable()
	{
		if(UnityTuioManager.Instance != null)
		{
			if(TRACK_TUIO_CURSORS)
			{
				UnityTuioManager.Instance.EventProcessor.CursorAdded += OnCursorAdded;
				UnityTuioManager.Instance.EventProcessor.CursorUpdated += OnCursorUpdated;
				UnityTuioManager.Instance.EventProcessor.CursorRemoved += OnCursorRemoved;
			}
			if(TRACK_TUIO_OBJECTS)
			{
				UnityTuioManager.Instance.EventProcessor.ObjectAdded += OnObjectAdded;
				UnityTuioManager.Instance.EventProcessor.ObjectUpdated += OnObjectUpdated;
				UnityTuioManager.Instance.EventProcessor.ObjectRemoved += OnObjectRemoved;
			}
			if(TRACK_TUIO_BLOBS)
			{
				UnityTuioManager.Instance.EventProcessor.BlobAdded += OnBlobAdded;
				UnityTuioManager.Instance.EventProcessor.BlobUpdated += OnBlobUpdated;
				UnityTuioManager.Instance.EventProcessor.BlobRemoved += OnBlobRemoved;
			}
		}
		if (UnityPharusManager.Instance != null)
		{
			if (UnityPharusManager.Instance.EventProcessor == null)
			{
				UnityPharusManager.Instance.OnTrackingInitialized += SubscribeTrackingEvents;
			}
			else
			{
				SubscribeTrackingEvents(this, null);
			}
		}
	}
	
	void OnDisable()
	{
		if(UnityTuioManager.Instance != null)
		{
			if(TRACK_TUIO_CURSORS)
			{
				UnityTuioManager.Instance.EventProcessor.CursorAdded -= OnCursorAdded;
				UnityTuioManager.Instance.EventProcessor.CursorUpdated -= OnCursorUpdated;
				UnityTuioManager.Instance.EventProcessor.CursorRemoved -= OnCursorRemoved;
			}
			if(TRACK_TUIO_OBJECTS)
			{
				UnityTuioManager.Instance.EventProcessor.ObjectAdded -= OnObjectAdded;
				UnityTuioManager.Instance.EventProcessor.ObjectUpdated -= OnObjectUpdated;
				UnityTuioManager.Instance.EventProcessor.ObjectRemoved -= OnObjectRemoved;
			}
			if(TRACK_TUIO_BLOBS)
			{
				UnityTuioManager.Instance.EventProcessor.BlobAdded -= OnBlobAdded;
				UnityTuioManager.Instance.EventProcessor.BlobUpdated -= OnBlobUpdated;
				UnityTuioManager.Instance.EventProcessor.BlobRemoved -= OnBlobRemoved;
			}
		}
		if (UnityPharusManager.Instance != null && UnityPharusManager.Instance.EventProcessor != null)
		{
			UnityPharusManager.Instance.EventProcessor.TrackAdded -= OnTrackAdded;
			UnityPharusManager.Instance.EventProcessor.TrackUpdated -= OnTrackUpdated;
			UnityPharusManager.Instance.EventProcessor.TrackRemoved -= OnTrackRemoved;
		}
	}
	
	#region tuio event handlers
	void OnObjectAdded (object sender, UnityTuioEventProcessor.TuioEventObjectArgs e)
	{
		AddPlayer(e.tuioObject.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioObject.Position));
	}
	void OnCursorAdded (object sender, UnityTuioEventProcessor.TuioEventCursorArgs e)
	{
		AddPlayer(e.tuioCursor.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioCursor.Position));
	}
	void OnBlobAdded (object sender, UnityTuioEventProcessor.TuioEventBlobArgs e)
	{
		AddPlayer(e.tuioBlob.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioBlob.Position));
	}
	
	void OnObjectUpdated (object sender, UnityTuioEventProcessor.TuioEventObjectArgs e)
	{
		UpdatePlayerPosition(e.tuioObject.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioObject.Position));
	}
	void OnCursorUpdated (object sender, UnityTuioEventProcessor.TuioEventCursorArgs e)
	{
		UpdatePlayerPosition(e.tuioCursor.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioCursor.Position));
	}
	void OnBlobUpdated (object sender, UnityTuioEventProcessor.TuioEventBlobArgs e)
	{
		UpdatePlayerPosition(e.tuioBlob.SessionID, UnityTuioManager.TuioPointToScreenCoord(e.tuioBlob.Position));
	}
	
	void OnObjectRemoved (object sender, UnityTuioEventProcessor.TuioEventObjectArgs e)
	{
		RemovePlayer(e.tuioObject.SessionID);
	}
	void OnCursorRemoved (object sender, UnityTuioEventProcessor.TuioEventCursorArgs e)
	{
		RemovePlayer(e.tuioCursor.SessionID);
	}
	void OnBlobRemoved (object sender, UnityTuioEventProcessor.TuioEventBlobArgs e)
	{
		RemovePlayer(e.tuioBlob.SessionID);
	}
	#endregion

	#region pharus event handlers
	private void OnTrackAdded(object sender, UnityPharusEventProcessor.PharusEventTrackArgs e)
	{
		AddPlayer(e.trackRecord.trackID, UnityPharusManager.PharusPointToScreenCoord(e.trackRecord.relPos));
	}
	private void OnTrackUpdated(object sender, UnityPharusEventProcessor.PharusEventTrackArgs e)
	{
		UpdatePlayerPosition(e.trackRecord.trackID, UnityPharusManager.PharusPointToScreenCoord(e.trackRecord.relPos));
	}
	private void OnTrackRemoved(object sender, UnityPharusEventProcessor.PharusEventTrackArgs e)
	{
		RemovePlayer(e.trackRecord.trackID);
	}
	#endregion

	#region private methods
	private void SubscribeTrackingEvents(object theSender, System.EventArgs e)
	{
		UnityPharusManager.Instance.EventProcessor.TrackAdded += OnTrackAdded;
		UnityPharusManager.Instance.EventProcessor.TrackUpdated += OnTrackUpdated;
		UnityPharusManager.Instance.EventProcessor.TrackRemoved += OnTrackRemoved;
	}


	#endregion

	#region player management
	public virtual void AddPlayer (long sessionID, Vector2 position)
	{
		ATuioPlayer player = (GameObject.Instantiate(m_playerPrefab, new Vector3(position.x,position.y,0), Quaternion.identity) as GameObject).GetComponent<ATuioPlayer>();
		player.SessionID = sessionID;
		m_playerList.Add(player);
	}

	public virtual void UpdatePlayerPosition (long sessionID, Vector2 position)
	{
		foreach (ATuioPlayer player in m_playerList) 
		{
			if(player.SessionID.Equals(sessionID))
			{
				player.MoveTo(position);
				return;
			}
		}
		
		if(m_addUnknownPlayerOnUpdate)
		{
			AddPlayer(sessionID, position);
		}
	}

	public virtual void RemovePlayer (long sessionID)
	{
		foreach (ATuioPlayer player in m_playerList.ToArray()) 
		{
			if(player.SessionID.Equals(sessionID))
			{
				GameObject.Destroy(player.gameObject);
				m_playerList.Remove(player);
			}	
		}
	}
	#endregion
}
