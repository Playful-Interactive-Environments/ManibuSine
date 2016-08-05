using UnityEngine;
using UnityEngine.Networking;

public class CameraTool : MonoBehaviour {
    NetworkClient myClient;
    NetworkConnection myConnection;

    public class MyMsgType {
        public static short Score = MsgType.Highest + 1;
    };

    public class ScoreMessage : MessageBase {
        public int score;
        public Vector3 scorePos;
        public int lives;
    }

    public void SendScore(int score, Vector3 scorePos, int lives) {
        ScoreMessage msg = new ScoreMessage();
        msg.score = score;
        msg.scorePos = scorePos;
        msg.lives = lives;

        NetworkServer.SendToAll(MyMsgType.Score, msg);
    }

    public void SetUpConnetion(NetworkConnection connection) {
        myConnection = connection;
        myConnection.RegisterHandler(MyMsgType.Score, OnScore);
    }

    // Create a client and connect to the server port
    public void SetupClient() {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MyMsgType.Score, OnScore);
        myClient.Connect("127.0.0.1", 4444);
    }

    public void OnScore(NetworkMessage netMsg) {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        Debug.Log("OnScoreMessage " + msg.score);
    }

    public void OnConnected(NetworkMessage netMsg) {
        Debug.Log("Connected to server");
    }
}