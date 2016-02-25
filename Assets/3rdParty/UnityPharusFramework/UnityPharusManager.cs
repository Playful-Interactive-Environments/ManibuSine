using UnityEngine;
using System.Collections;
using PharusTransmission;
using System.IO;
using System;
using UnityEngine.UI;
using UnityTuio;

namespace UnityPharus
{
    /// <summary>
    /// The UnityPharusManager keeps control over the UnityPharusListener and the UnityPharusEventProcessor.
    /// </summary>
    [AddComponentMenu("UnityPharus/UnityPharusManager")]
    public class UnityPharusManager : MonoBehaviour
    {
        /// <summary>
        /// Overall PharusTransmission Settings
        /// </summary>
        [System.Serializable]
        public class PharusSettings
        {
            public enum EProtocolType
            {
                TCP,
                UDP
            }

            [SerializeField]
            private EProtocolType protocol = EProtocolType.UDP;
            [SerializeField]
            private string tcpRemoteIpAddress = "127.0.0.1";
            [SerializeField]
            private int tcpLocalPort = 44345;
            [SerializeField]
            private string udpMulticastIpAddress = "239.1.1.1";
            [SerializeField]
            private int udpLocalPort = 44345;
            [SerializeField]
            private int targetScreenWidth = 3840;
            [SerializeField]
            private int targetScreenHeight = 2160;
            [SerializeField]
            [Range(-1, 180)]
            [Tooltip("Use a negative value to prevent automatically server reconnecting")]
            private float checkServerReconnectIntervall = 5;
            [SerializeField]
            private int tracklinkEnabled = 0;
            [SerializeField]
            private int tuioEnabled = 0;
            [SerializeField] 
            private int targetResolution = 0;
            


            public EProtocolType Protocol
            {
                get { return this.protocol; }
                set { this.protocol = value; }
            }
            public string TCP_IP_Address
            {
                get { return this.tcpRemoteIpAddress; }
                set { this.tcpRemoteIpAddress = value; }
            }
            public int TCP_Port
            {
                get { return this.tcpLocalPort; }
                set { this.tcpLocalPort = value; }
            }
            public string UDP_Multicast_IP_Address
            {
                get { return this.udpMulticastIpAddress; }
                set { this.udpMulticastIpAddress = value; }
            }
            public int UDP_Port
            {
                get { return this.udpLocalPort; }
                set { this.udpLocalPort = value; }
            }
            public int TargetScreenWidth
            {
                get { return this.targetScreenWidth; }
                set { this.targetScreenWidth = value; }
            }
            public int TargetScreenHeight
            {
                get { return this.targetScreenHeight; }
                set { this.targetScreenHeight = value; }
            }
            public float CheckServerReconnectIntervall
            {
                get { return this.checkServerReconnectIntervall; }
            }
            public int TracklinkEnabled
            {
                get { return this.tracklinkEnabled; }
                set { this.tracklinkEnabled = value; }
            }
            public int TuioEnabled
            {
                get { return this.tuioEnabled; }
                set { this.tuioEnabled = value; }

            }
            public int TargetResolution
            {
                get { return targetResolution; }
                set { targetResolution = value; }
            }
        }

        #region event handlers
        public event EventHandler<EventArgs> OnTrackingInitialized;
        #endregion

        [SerializeField]
        private PharusSettings m_pharusSettings = new PharusSettings();
        [SerializeField]
        private bool m_persistent = true;
        public UnityPharusXMLConfig m_unityPharusXMLConfig;
        

        #region Singleton pattern
        private static UnityPharusManager m_instance;
        public static UnityPharusManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = (UnityPharusManager)FindObjectOfType(typeof(UnityPharusManager));
                    if (m_instance == null)
                    {
                        //						Debug.LogWarning (string.Format ("No instance of {0} available.", typeof(UnityPharusManager)));
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
        private UnityPharusListener m_listener;
        private UnityPharusEventProcessor m_eventProcessor;
        public UnityPharusEventProcessor EventProcessor
        {
            get { return m_eventProcessor; }
        }

        #region unity messages
        void Awake()
            
        {
            
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                if (m_instance != this)
                {
                    Debug.LogWarning(string.Format("Other instance of {0} detected (will be destroyed)", typeof(UnityPharusManager)));
                    GameObject.Destroy(this.gameObject);
                    return;
                }
            }

            if (!m_initialized)
            {
                StartCoroutine(InitInstance());
            }
            
        }

        void Update()
        {

            //Lister for Pharus Data if Tracklink is enbled
            if (m_eventProcessor != null && m_pharusSettings.TracklinkEnabled == 1)
            {
                m_eventProcessor.Process();
            }
        }

        void OnDestroy()
        {
            if (m_listener != null)
            {
                m_listener.Shutdown();
            }
        }
        #endregion

        #region public methods
        public void ReconnectListener(float theDelay = -1f)
        {
            if (theDelay <= 0)
            {
                m_listener.Reconnect();
            }
            else
            {
                StartCoroutine(ReconnectListenerDelayed(theDelay));

            }
        }
       
        #endregion

        #region private methods
        private IEnumerator InitInstance()
        {
            m_initialized = true;

            Application.runInBackground = true;

            //			Debug.Log ("UnityPharusManager InitInstance");
            if (m_pharusSettings.CheckServerReconnectIntervall > 0)
            {
                StartCoroutine(CheckServerAlive(m_pharusSettings.CheckServerReconnectIntervall));
            }

            if (m_persistent)
            {
                GameObject.DontDestroyOnLoad(this.gameObject);
            }

            // start: load config file
            yield return StartCoroutine(LoadConfigXML());
            //			Debug.Log ("UnityPharusManager config loaded, continue InitInstance");
            if (m_unityPharusXMLConfig != null)
            {
                string configUseUDP = null;
                string configTCPIP = null;
                string configTCPPort = null;
                string configUDPMulticastIP = null;
                string configUDPPort = null;
                string configTUIO = null;
                string configTracklink = null;
                string targetResolution = null;
                for (int i = 0; i < m_unityPharusXMLConfig.ConfigNodes.Length; i++)
                {
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "useUDP")
                    {
                        configUseUDP = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "tcp-ip")
                    {
                        configTCPIP = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "tcp-port")
                    {
                        configTCPPort = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "udp-multicast-ip")
                    {
                        configUDPMulticastIP = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "udp-port")
                    {
                        configUDPPort = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "tracklink")
                    {
                        configTracklink = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "tuio")
                    {
                        configTUIO = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                    if (m_unityPharusXMLConfig.ConfigNodes[i].Name == "targetResolution")
                    {
                        targetResolution = m_unityPharusXMLConfig.ConfigNodes[i].Value;
                        continue;
                    }
                   
                }
                bool configUseUDPBool;
                int configUDPPortInt;
                int configTCPPortInt;
                int configTracklinkInt;
                int configTuioInt;
                int configResolutionInt;

                if (int.TryParse(configTracklink, out configTracklinkInt) && configTracklink != null)
                {
                    m_pharusSettings.TracklinkEnabled = configTracklinkInt;

                }
                if (int.TryParse(configTUIO, out configTuioInt) && configTUIO != null)
                {
                    m_pharusSettings.TuioEnabled = configTuioInt;

                }
                if (int.TryParse(targetResolution, out configResolutionInt) && targetResolution != null)
                {
  
                    switch (configResolutionInt)
                    {
                        case 0:
                            m_pharusSettings.TargetScreenWidth = 3840;
                            m_pharusSettings.TargetScreenHeight = 2160;
                            break;
                        case  1:
                            m_pharusSettings.TargetScreenWidth = 1920;
                            m_pharusSettings.TargetScreenHeight = 1080;
                            break;
                        case 2:
                            m_pharusSettings.TargetScreenWidth = 1280;
                            m_pharusSettings.TargetScreenHeight = 720;
                            break;

                    }

                }

                if (configUseUDP != null && Boolean.TryParse(configUseUDP, out configUseUDPBool) && !configUseUDPBool &&
                    configTCPIP != null &&
                    configTCPPort != null && int.TryParse(configTCPPort, out configTCPPortInt))
                {
                    
                    m_pharusSettings.Protocol = PharusSettings.EProtocolType.TCP;
                    m_pharusSettings.TCP_IP_Address = configTCPIP;
                    m_pharusSettings.TCP_Port = configTCPPortInt;
                    Debug.Log(string.Format("config xml file found: overriding pharus settings TCP: {0}:{1}", configTCPIP, configTCPPort));
                }
                else if (configUseUDP != null && Boolean.TryParse(configUseUDP, out configUseUDPBool) && configUseUDPBool &&
                        configUDPMulticastIP != null &&
                        configUDPPort != null && int.TryParse(configUDPPort, out configUDPPortInt))
                {
                    m_pharusSettings.Protocol = PharusSettings.EProtocolType.UDP;
                    m_pharusSettings.UDP_Multicast_IP_Address = configUDPMulticastIP;
                    m_pharusSettings.UDP_Port = configUDPPortInt;
                    Debug.Log(string.Format("config xml file found: overriding pharus settings UDP: {0}:{1}", configUDPMulticastIP, configUDPPort));
                }
                else
                {
                    Debug.LogWarning("config xml file found with invalid config data");
                }
            }
            else
            {
                Debug.Log(string.Format("no config xml file found in resources: using default pharus settings ({0})", m_pharusSettings.Protocol.ToString()));
            }
            // end: load config file

            if (m_pharusSettings.Protocol == PharusSettings.EProtocolType.TCP)
            {
                m_listener = UnityPharusListener.NewUnityPharusListenerTCP(m_pharusSettings.TCP_IP_Address, m_pharusSettings.TCP_Port);
            }
            else if (m_pharusSettings.Protocol == PharusSettings.EProtocolType.UDP)
            {
                m_listener = UnityPharusListener.NewUnityPharusListenerUDP(m_pharusSettings.UDP_Multicast_IP_Address, m_pharusSettings.UDP_Port);
                
            }
            else
            {
                Debug.LogError("Invalid pharus settings!");
                yield break;
            }
            m_eventProcessor = new UnityPharusEventProcessor(m_listener);
            
            
            if (OnTrackingInitialized != null)
            {
                OnTrackingInitialized(this, new EventArgs());
            }
        }

        private IEnumerator ReconnectListenerDelayed(float theDelay)
        {
            m_listener.Shutdown();
            yield return new WaitForSeconds(theDelay);
            m_listener.Reconnect();
        }

        private IEnumerator CheckServerAlive(float theWaitBetweenCheck)
        {
            while (true)
            {
                yield return new WaitForSeconds(theWaitBetweenCheck);
                if (m_listener != null && !m_listener.IsCurrentlyConnecting && !m_listener.HasDataReceivedSinceLastCheck())
                {
                    Debug.LogWarning(string.Format("--- There might be a connection problem... (no data received in the past {0} seconds)---", theWaitBetweenCheck));
                    StartCoroutine(ReconnectListenerDelayed(1f));
                }
            }
        }

        private IEnumerator LoadConfigXML()
        {
            //			Debug.Log ("UnityPharusManager LoadConfigXML");
            string aPathToConfigXML = Path.Combine(Application.dataPath, "config.xml");
            aPathToConfigXML = "file:///" + aPathToConfigXML;
            WWW aWww = new WWW(aPathToConfigXML);
            //			Debug.Log ("UnityPharusManager loading file...");
            yield return aWww;
            //			Debug.Log ("UnityPharusManager file loading complete");

            if (aWww.isDone && string.IsNullOrEmpty(aWww.error))
            {
             //   				Debug.Log ("UnityPharusManager file could be loaded");
                m_unityPharusXMLConfig = UnityPharusXMLConfig.LoadFromText(aWww.text);
            }
            //			else
            //			{
            //				Debug.Log ("UnityPharusManager file could NOT be loaded");
            //			}
        }
        #endregion

        #region static methods
        public static Vector2 PharusPointToScreenCoord(Vector2f pharusTrackPosition)
        {
            return PharusPointToScreenCoord(pharusTrackPosition.x, pharusTrackPosition.y);
        }
        public static Vector2 PharusPointToScreenCoord(float x, float y)
        {
            return new Vector2((int)Mathf.Round(x * Instance.m_pharusSettings.TargetScreenWidth), Instance.m_pharusSettings.TargetScreenHeight - (int)Mathf.Round(y * Instance.m_pharusSettings.TargetScreenHeight));
        }
        #endregion

        #region static properties
        public int TargetScreenWidth
        {
            get { return Instance.m_pharusSettings.TargetScreenWidth; }
        }
        public int TargetScreenHeight
        {
            get { return Instance.m_pharusSettings.TargetScreenHeight; }
        }
        public int TuioStatus
        {
            get { return Instance.m_pharusSettings.TuioEnabled; }

        }
        #endregion
    }
}
