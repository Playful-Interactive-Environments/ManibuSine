using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : ATuioPlayerManager
{

    public List<TrackedPlayer> GetPlayerList()
    {
        List<TrackedPlayer> pList = new List<TrackedPlayer>();
        foreach (ATuioPlayer tuioPlayer in m_playerList)
        {
            pList.Add(tuioPlayer as TrackedPlayer);
        }
        return pList;
    }
}
