using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

using UnityEngine.Networking;

[System.Serializable]
public class Players
{
    public string name;
    public string url;
}

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //yield return (DownloadAsset());

        playerPrefab = PhotonNetwork.Instantiate("player_h", Vector3.zero, Quaternion.identity);
        playerPrefab.name = "player_h";

        //foreach (var i in pool.ResourceCache)
        //    Destroy(i.Value.gameObject);
        //pool.ResourceCache.Clear();

        Debug.Log("<Color=lime>" + PhotonNetwork.LocalPlayer.UserId + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CloudRegion + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.Name + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.PlayerCount + "</color>");
        Debug.Log("<Color=lime>" + PhotonNetwork.CurrentRoom.Players.Count + "</color>");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("<color=red>Crash Cause:" + cause + "</color>");
        Debug.Log("<color=red>Crash Time:" + System.DateTime.Now.ToString() + "</color>");

        PhotonNetwork.LoadLevel("Scene_Lobby");
        PhotonNetwork.Destroy(playerPrefab);
    }
}