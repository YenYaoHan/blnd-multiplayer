using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks {
    public string roomName = "Room1";

    void Awake() {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start() {
        Connect();
    }

    public void Connect() {
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.PhotonServerSettings.DevRegion = "jp";
        PhotonNetwork.PhotonServerSettings.PunLogging = PunLogLevel.ErrorsOnly;
        PhotonNetwork.PhotonServerSettings.RunInBackground = true;
        PhotonNetwork.PhotonServerSettings.EnableSupportLogger = false;

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        //PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "xxx-xxx-xxx-xxx-xxx";
        //PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = "xxx-xxx-xxx-xxx-xxx";
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1";
        PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ConnectionProtocol.Udp;
        PhotonNetwork.PhotonServerSettings.AppSettings.NetworkLogging = DebugLevel.ERROR;

        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 30000;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.CrcEnabled = true;
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.MaximumTransferUnit = 520;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        StartCoroutine(DownloadAsset());


    }

    public override void OnJoinedLobby() {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion +
            "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion +
            "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
    }

    public override void OnJoinedRoom() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        Debug.Log("We load the 'Room for 1' ");
        Debug.Log(PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout);

        // Load the Room Level. 
        PhotonNetwork.LoadLevel("2.RoomScene");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.UserId);
    }

    IEnumerator DownloadAsset() {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        yield return null;
    }
}