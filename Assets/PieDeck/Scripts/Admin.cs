using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Admin : AManager<Admin> {

	public Text CurrentPlayerText;
	public GameObject CurrentTrackedPlayer;
	public Button ButtonPlayerOne;
	public Button ButtonPlayerTwo;
    public Button ButtonDisconnect;
    public Button ButtonRecalibrate;
    public Button ButtonToggleChaperone;
    public Button ButtonRestartApplication;
    public InputField InputFieldLS;
    public InputField InputFieldMM;
    public GameObject PlayerOne;
	public GameObject PlayerTwo;
    public PlayerView P1View;
    public PlayerView P2View;

	#region ConnectClients
	public void DisconnectPlayer()
	{
		if (PlayerOne != null)
		{
			if (PlayerOne.GetComponent<NetworkPlayer>().ControllingPlayer == CurrentTrackedPlayer && PlayerOne != null)
			{
				PlayerOne.GetComponent<NetworkPlayer>().ControllingPlayer = null;
				CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne = false;
				ButtonPlayerOne.interactable = true;
			}
		}

		if (PlayerTwo != null)
		{
			if (PlayerTwo.GetComponent<NetworkPlayer>().ControllingPlayer == CurrentTrackedPlayer)
			{
				PlayerTwo.GetComponent<NetworkPlayer>().ControllingPlayer = null;
				CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo = false;
				ButtonPlayerTwo.interactable = true;
			}
		}

	}
	public void ChoosePlayerOne()
	{
		if (CurrentTrackedPlayer != null)
		{
			PlayerOne.GetComponent<NetworkPlayer>().ControllingPlayer = CurrentTrackedPlayer;
			CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().ControlledPlayer = PlayerOne;
			ButtonPlayerOne.interactable = false;
			CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne = true;

            // Create Camera and assign this player
            if (P1View != null)
            {

                foreach(NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                {
                    if(np.clientType == ClientChooser.ClientType.RenderClientWall)
                    {
                        np.RpcSetPlayerView(1, PlayerOne.GetComponent<NetworkPlayer>().netId.Value);
                    }
                }
            }
		}

	}
	public void ChoosePlayerTwo()
	{
		if (CurrentTrackedPlayer != null)
		{
			PlayerTwo.GetComponent<NetworkPlayer>().ControllingPlayer = CurrentTrackedPlayer;
			CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().ControlledPlayer = PlayerTwo;

			ButtonPlayerTwo.interactable = false;
			CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo = true;

            // Create Camera and assign this player
            if (P2View != null)
            {
                foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                {
                    if (np.clientType == ClientChooser.ClientType.RenderClientWall)
                    {
                        np.RpcSetPlayerView(2, PlayerTwo.GetComponent<NetworkPlayer>().netId.Value);
                    }
                }
            }
        }
    }

	public void RecalibratePlayer()
	{
		if (CurrentTrackedPlayer != null)
		{
			if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne)
			{
				PlayerOne.GetComponent<NetworkPlayer>().ResetOrientation();
			}
			if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo)
			{
				PlayerTwo.GetComponent<NetworkPlayer>().ResetOrientation();
			}
		}
	}
	public void ToggleChaperone()
	{
        if (CurrentTrackedPlayer != null)
		{
			if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne)
			{
				PlayerOne.GetComponent<NetworkPlayer>().ToggleChaperone();
			}
			if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo)
			{
				PlayerTwo.GetComponent<NetworkPlayer>().ToggleChaperone();
			}
		}
	}

    public void SetMovementLerpSpeed()
    {
        string s = InputFieldLS.text;
        float val;
        float.TryParse(s, out val);
        if (CurrentTrackedPlayer == null)
            return;

        if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne)
        {
            PlayerOne.GetComponent<NetworkPlayer>().SetMovementLerpSpeed(val);
        }
        if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo)
        {
            PlayerTwo.GetComponent<NetworkPlayer>().SetMovementLerpSpeed(val);
        }
    }
    public void SetMinMoveDistance()
    {
        string s = InputFieldMM.text;
        float val;
        float.TryParse(s, out val);
        if (CurrentTrackedPlayer == null)
            return;
        if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerOne)
        {
            PlayerOne.GetComponent<NetworkPlayer>().SetMinMoveDistance(val);
        }
        if (CurrentTrackedPlayer.GetComponent<TrackedPlayerNetworkBehaviour>().HasPlayerTwo)
        {
            PlayerTwo.GetComponent<NetworkPlayer>().SetMinMoveDistance(val);
        }
    }
    #endregion

}
