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
        PhotonNetwork.ConnectUsingSettings();       // Photon Server Setting ���Ͽ� �Էµ� ������� Photon�� ������
    }

    public override void OnConnectedToMaster()      // �÷��̾ Photon Server ������ ����Ǿ� �ְ� �ٸ� �۾��� ������ �� ������ ȣ��
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();      // Master Server�� ������� room�� �����ϰ� Lobby�� ����
    }

    public override void OnJoinedLobby()     // Master Server�� Lobby�� ���� �� ȣ��
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    void Update()
    {
        
    }
}
