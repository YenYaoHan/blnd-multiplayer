using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RoomManager : MonoBehaviourPunCallbacks {

    public string name = "";
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start() {



        playerPrefab = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        Debug.Log("<Color=lime>" + PhotonNetwork.LocalPlayer.UserId + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CloudRegion + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.Name + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.Players.Count + "</color>");
    }
    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.Log("<color=red>Crash Cause:" + cause + "</color>");
        Debug.Log("<color=red>Crash Time:" + System.DateTime.Now.ToString() + "</color>");

        PhotonNetwork.LoadLevel("1.LobbyScene");
        PhotonNetwork.Destroy(playerPrefab);
    }
}