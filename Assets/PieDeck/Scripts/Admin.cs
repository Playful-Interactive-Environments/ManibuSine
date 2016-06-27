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
    public GameObject PlayerOne;
	public GameObject PlayerTwo;

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

    public void SetMovementLerpSpeed(string s)
    {
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
    public void SetMinMoveDistance(string s)
    {
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
