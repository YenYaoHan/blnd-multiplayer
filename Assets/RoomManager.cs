using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerPrefab = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        playerPrefab.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

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
