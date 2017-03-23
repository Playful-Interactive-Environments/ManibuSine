using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class MobileClientIPConfig : MonoBehaviour
{
    private string clientIP;

    void Start()
    {

    }

    void Update()
    {

    }


    private bool ConfigureRenderClient()
    {
        string path = Path.Combine(Application.persistentDataPath, "RenderClientConfig.xml");
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(path);
        }
        catch (FileNotFoundException e)
        {
            return false;
        }

        foreach (XmlNode node in doc.DocumentElement.ChildNodes)
        {
            // read ip adress
            if (node.Name == "ip")
            {
                clientIP = node.InnerText;
            }
        }
        return true;
    }
}