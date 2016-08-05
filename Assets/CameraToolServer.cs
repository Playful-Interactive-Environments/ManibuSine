using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CameraToolServer : NetworkBehaviour {

    const short MyBeginMsg = 1002;

    NetworkClient m_client;
    NetworkConnection m_connection;

    public void TestMsg() {
        SendReadyToBeginMessageConnection(42);
    }

    void OnServerReadyToBeginMessage(NetworkMessage netMsg) {
        var beginMessage = netMsg.ReadMessage<IntegerMessage>();
        Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.value);
    }

    public void SendReadyToBeginMessageConnection(int myId) {
        if (m_connection == null)
            return;

        var msg = new IntegerMessage(myId);

        print("SEND IT");
        m_connection.Send(MyBeginMsg, msg);
    }

    public void InitConnection(NetworkConnection client) {
        print("cts 2");
        print("c " + client.ToString());
        m_connection = client;
        NetworkServer.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
    }




    // original stuff with network client
    public void SendReadyToBeginMessage(int myId) {
        if (m_client == null)
            return;

        var msg = new IntegerMessage(myId);
        m_client.Send(MyBeginMsg, msg);
    }
    public void Init(NetworkClient client) {
        print("c " + client.ToString());
        m_client = client;
        NetworkServer.RegisterHandler(MyBeginMsg, OnServerReadyToBeginMessage);
    }
}
