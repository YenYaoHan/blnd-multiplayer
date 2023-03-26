using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour {
    private PhotonView photonView;

    private void Start() {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    private void FixedUpdate() {

        if (photonView.IsMine && PhotonNetwork.IsConnected) {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch)) {
                photonView.RPC("ChangeFloorColor", RpcTarget.All, "r");
            }
        }
    }

    [PunRPC]
    private void ChangeFloorColor(string color, PhotonMessageInfo p) {

        if (color == "r")
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.red;
        if (color == "g")
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.green;
        if (color == "b")
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.blue;

    }
}
