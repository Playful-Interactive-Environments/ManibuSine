using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Net;

public class RenderClientConfig : MonoBehaviour {
    private static RenderClientConfig instance;

    private string rc_ip;
    public static string Rc_ip {
        get {
            if (instance == null)
                return "";
            return instance.rc_ip;
        }
    }
    private ClientChooser.ClientType rc_mode;
    public static ClientChooser.ClientType Rc_mode {
        get {
            if (instance == null)
                return ClientChooser.ClientType.NotSet;
            return instance.rc_mode;
        }
    }

    void OnLevelWasLoaded() {
        // not in use - because type is chosen by scene
        //if (ClientChooser.Instance == null)
        //    return;
        //ClientChooser.Instance.clientType = rc_mode;

        if (ServerManager.Instance == null)
            return;

        // check if ip is valid
        IPAddress address;
        if (IPAddress.TryParse(rc_ip, out address)) {
            switch (address.AddressFamily) {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    // only set ConnectionIP is valid IPv4
                    ServerManager.Instance.ConnectionIP = rc_ip;
                    break;
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    // IPv6
                    break;
                default:
                    break;
            }
        }

        // destroy RenderClientConfig after it has been used
        Destroy(gameObject);
    }

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        ConfigureRenderClient();
        //print("ip: " + rc_ip + " mode: " + rc_mode);

        if (rc_mode == ClientChooser.ClientType.RenderClientFloor)
            SceneManager.LoadScene("Client_Render_Floor");
        else if (rc_mode == ClientChooser.ClientType.RenderClientWall)
            SceneManager.LoadScene("Client_Render_Wall");
    }

    private bool ConfigureRenderClient() {
        string path = Path.Combine(Application.dataPath, "RenderClientConfig.xml");
        XmlDocument doc = new XmlDocument();
        try {
            doc.Load(path);
        }
        catch (FileNotFoundException e) {
            rc_mode = ClientChooser.ClientType.RenderClientFloor;
            return false;
        }

        foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
            // read ip adress
            if (node.Name == "ip") {
                rc_ip = node.InnerText;
            }
            // read render mode
            else if (node.Name == "mode") {
                if (node.InnerText == "Floor")
                    rc_mode = ClientChooser.ClientType.RenderClientFloor;
                else if (node.InnerText == "Wall")
                    rc_mode = ClientChooser.ClientType.RenderClientWall;
                else
                    rc_mode = ClientChooser.ClientType.NotSet;
            }
        }
        return true;
    }
}