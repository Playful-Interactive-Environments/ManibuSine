using UnityEngine;
using System.Collections;
using TUIO;
using UnityPharus;
using UnityTuio;


// NOTE: UnityTuioManager should be in the namespace UnityTuio, 
// but for some reason Unity4.3.4 can't handle this class being in a namespace, sigh...
//namespace UnityTuio
//{
	/// <summary>
	/// The UnityTuioManager keeps control over the UnityTuioListener and the UnityTuioEventProcessor.
	/// </summary>
	[AddComponentMenu("UnityTuio/UnityTuioManager")]
	public class UnityTuioManager : MonoBehaviour
	{
		/// <summary>
		/// Overall TUIO Settings
		/// </summary>
		[System.Serializable]
		public class TuioSettings
		{
			[SerializeField] private int _udpPort = 3333;
			[SerializeField] private int _targetScreenWidth = 3840;
			[SerializeField] private int _targetScreenHeight = 2160;
			public int UDP_Port
			{
				get { return _udpPort; }
			}
			public int TargetScreenHeight 
			{
				get { return _targetScreenHeight; }
			}
			public int TargetScreenWidth 
			{
				get { return _targetScreenWidth; }
			}
		}

		[SerializeField] private TuioSettings m_tuioSettings = new TuioSettings();
		[SerializeField] private bool m_persistent = true;

		#region Singleton pattern
		private static UnityTuioManager m_instance;
		public static UnityTuioManager Instance 
		{
			get 
			{
				if (m_instance == null) 
				{
					m_instance = (UnityTuioManager) FindObjectOfType(typeof(UnityTuioManager));
					if (m_instance == null) 
					{
//						Debug.LogWarning (string.Format ("No instance of {0} available.", typeof(UnityTuioManager)));
					}
					else
					{
						m_instance.Awake();
					}
				}
				return m_instance;
			}
		}
		#endregion

		private bool m_initialized = false;
		private UnityTuioListener m_listener;
		private UnityTuioEventProcessor m_eventProcessor;
		public UnityTuioEventProcessor EventProcessor
		{
			get { return m_eventProcessor; }
		}

		#region unity messages
		void Awake ()
		{
			if (m_instance == null) 
			{
				m_instance = this;
			}
			else
			{
				if(m_instance != this)
				{
					Debug.Log (string.Format ("Other instance of {0} detected (will be destroyed)", typeof(UnityTuioManager)));
					GameObject.Destroy(this.gameObject);
					return;
				}
			}

			if (!m_initialized)
			{
				InitInstance();
			}
		}

		void Update()
		{
            //Listen for Tuio Data if Tuio is enabled
            if (UnityPharusManager.Instance.TuioStatus == 1)
			m_eventProcessor.Process();
			ListenForInputs();
		}

		void OnDestroy()
		{
			if(m_listener != null)
			{
				m_listener.Shutdown();
			}
		}
		#endregion

		#region public methods
		public void ReconnectTuioListener(float theDelay = -1f)
		{
			if(m_listener == null || m_listener.HasTuioContainers()) return;

			if(theDelay <= 0)
			{
				m_listener.Reconnect();
			}
			else
			{
				StartCoroutine(ReconnectTuioListenerDelayed(theDelay));
			}
		}
		#endregion

		#region private methods
		private void InitInstance()
		{
			m_initialized = true;

			Application.runInBackground = true;
			
			if(m_persistent)
			{
				GameObject.DontDestroyOnLoad(this.gameObject);
			}
			
			m_listener = new UnityTuioListener(m_tuioSettings.UDP_Port);
			m_eventProcessor = new UnityTuioEventProcessor(m_listener);
		}

		private IEnumerator ReconnectTuioListenerDelayed(float theDelay)
		{
			m_listener.Shutdown();
			yield return new WaitForSeconds(theDelay);
			m_listener.Reconnect();
		}

		private void ListenForInputs()
		{
			if(Input.GetKeyDown(KeyCode.R))
			{
				ReconnectTuioListener();
			}
		}
		#endregion

		#region static methods
		public static Vector2 TuioPointToScreenCoord (TuioPoint tuioPoint)
		{
			return new Vector2(tuioPoint.getScreenX(Instance.m_tuioSettings.TargetScreenWidth), Instance.m_tuioSettings.TargetScreenHeight - tuioPoint.getScreenY(Instance.m_tuioSettings.TargetScreenHeight));
		}
		public static Vector2 TuioPointToScreenCoord (float tuioPointX, float tuioPointY)
		{
			return new Vector2((int)Mathf.Round(tuioPointX * Instance.m_tuioSettings.TargetScreenWidth), Instance.m_tuioSettings.TargetScreenHeight - (int)Mathf.Round(tuioPointY * Instance.m_tuioSettings.TargetScreenHeight));
		}
		#endregion

		#region static properties
		public int TargetScreenWidth
		{
			get{ return Instance.m_tuioSettings.TargetScreenWidth; }
		}
		public int TargetScreenHeight
		{
			get{ return Instance.m_tuioSettings.TargetScreenHeight; }
		}
		#endregion
	}
//}