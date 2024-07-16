using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject connectedScreen;
    public GameObject DisconnectedScreen;
    public void OnClick_ConnectButton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectedScreen.SetActive(true);
    }
    public override void OnJoinedLobby()
    {
        if(DisconnectedScreen.activeSelf)
            DisconnectedScreen.SetActive(false);
        connectedScreen.SetActive(true);
    }
    
}
