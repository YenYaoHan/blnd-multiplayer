using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    private PhotonView photonView;
    private Animator animator;

    private CharacterController controller;
    
    private float inputX;
    private float inputZ;
    private Vector3 v_movement = Vector3.zero;
    private Vector3 v_velocity = Vector3.zero;
    public float moveSpeed = 6.0F;
    public float gravity = 20;

    // Start is called before the first frame update
    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
      //  if (!photonView.IsMine)
      //      return;

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        if (inputX!=0 || inputZ>0)
            animator.SetBool("isRun", true);
        else
            animator.SetBool("isRun", false);
        
        if (controller.isGrounded)
        {
            v_velocity.y = 0f;
        }
        else
        {
            v_velocity.y -= gravity * Time.deltaTime;
        }

        v_movement = controller.transform.forward * inputZ;

        controller.transform.Rotate(Vector3.up * inputX * (100f * Time.deltaTime));
        if(inputZ>0)
           controller.Move(v_movement * moveSpeed * Time.deltaTime);
        controller.Move(v_velocity);
    }
}
