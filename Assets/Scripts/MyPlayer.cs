using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public SpriteRenderer sr;
    public Text nameText;
    private Rigidbody2D rb;
    private bool IsGrounded;
    void Start()
    {

        if (photonView.IsMine)
        {
            nameText.text = PhotonNetwork.NickName;

            rb = GetComponent<Rigidbody2D>();
            sceneCamera = GameObject.Find("Main Camera");

            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else
        {
            nameText.text = pv.Owner.NickName;
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

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sr.flipX = false;
            pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }

    }

    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        sr.flipX = true;

    }

    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        sr.flipX = false;

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (photonView.IsMine)
        {
            if (col.gameObject.tag == "Ground")
            {
                IsGrounded = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (photonView.IsMine)
        {
            if (col.gameObject.tag == "Ground")
            {
                IsGrounded = false;
            }
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpforce);
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
