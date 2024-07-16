using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{

    public PhotonView pv;

    public float moveSpeed = 6;
    public float jumpforce = 800;

    private Vector3 smoothMove;

    private GameObject sceneCamera;
    public GameObject playerCamera;


    void Start()
    {
        if (photonView.IsMine)
        {
            playerCamera = GameObject.Find("Main Camera");

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
        }
        else
        {
            smoothMovement();
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }

    }
}
