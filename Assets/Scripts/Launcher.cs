using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.ComponentModel.Design;

public class Launcher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();       // Photon Server Setting 파일에 입력된 설정대로 Photon에 연결함
    }

    public override void OnConnectedToMaster()      // 플레이어가 Photon Server 서버에 연결되어 있고 다른 작업을 진행할 수 있으면 호출
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();      // Master Server에 사용중인 room을 나열하고 Lobby에 진입
    }

    public override void OnJoinedLobby()     // Master Server의 Lobby에 들어갔을 때 호출
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    void Update()
    {
        
    }
}
