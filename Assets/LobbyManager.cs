using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

using UnityEngine.Networking;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public string roomName = "Room1";

    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        //if (PhotonNetwork.IsConnected)
        //{
        //    PhotonNetwork.JoinRandomRoom();
        //}
        //else
        //{
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.PhotonServerSettings.DevRegion = "asia";
        PhotonNetwork.PhotonServerSettings.PunLogging = PunLogLevel.ErrorsOnly;
        PhotonNetwork.PhotonServerSettings.RunInBackground = true;
        PhotonNetwork.PhotonServerSettings.EnableSupportLogger = false;

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
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
        //}
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
            "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");

        StartCoroutine(DownloadAsset());

     
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion +
            "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion +
            "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        Debug.Log("We load the 'Room for 1' ");
        Debug.Log(PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout);



        // Load the Room Level. 
        PhotonNetwork.LoadLevel("Scene_Room");

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.UserId);
    }

    IEnumerator DownloadAsset()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        List<Players> players = new List<Players>();

        Players p1 = new Players();
        p1.name = "player_eric";
        p1.url = "https://firebasestorage.googleapis.com/v0/b/blnd-multiplayer.appspot.com/o/player_eric.abs?alt=media&token=ade36445-04cd-4c62-8ce8-c84a678c4c8b";
        players.Add(p1);

        Players p2 = new Players();
        p2.name = "player_h";
        p2.url = "https://firebasestorage.googleapis.com/v0/b/blnd-multiplayer.appspot.com/o/player_h.abs?alt=media&token=7274fc5b-130a-458e-8a56-b9246d58b67b";
        players.Add(p2);

        Players p3 = new Players();
        p3.name = "playerhank";
        p3.url = "https://firebasestorage.googleapis.com/v0/b/blnd-multiplayer.appspot.com/o/playerhank.abs?alt=media&token=9d41d893-b2e1-4820-98bc-4f62a38e5cc3";
        players.Add(p3);


        foreach (var i in players)
        {
            using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(i.url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    var prefab = bundle.LoadAsset(i.name);
                    GameObject obj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    obj.name = i.name;

                    obj.transform.position = new Vector3(1000, 1000, 1000);
                    pool.ResourceCache.Add(obj.name, GameObject.Find(obj.name));
                    DontDestroyOnLoad(obj);

                    foreach (var j in obj.gameObject.GetComponent<PhotonAnimatorView>().GetSynchronizedParameters())
                    {
                        j.SynchronizeType = PhotonAnimatorView.SynchronizeType.Discrete;
                    }
                        
                    Debug.Log("Download Complete: "+ obj.name);
                }
            }
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}