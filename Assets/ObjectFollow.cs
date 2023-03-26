using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
public class ObjectFollow : MonoBehaviour {

    public PhotonView view;

    public string targetName;
    private Transform target;

    // Start is called before the first frame update
    void Start() {
        if (view.IsMine)
            target = GameObject.Find(targetName).transform;
    }

    // Update is called once per frame
    void Update() {
        if (view.IsMine) {
            gameObject.transform.position = target.position;
            gameObject.transform.rotation = target.rotation;
        }
    }
}
