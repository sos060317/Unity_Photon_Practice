using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

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

    public void CreateRoom()     // room�� �����ϰ� �ڵ����� room�� ��
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))       // InputField�� null���� �ƴ����� �� �� null�̸� true�� ��ȯ
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);      // room�� �̸��� ���ڷ� �����鼭 room����
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()     // room�� ������ ȣ��
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;     // ���� ���� room�� �̸��� �޾ƿ�
    }

    public override void OnCreateRoomFailed(short returnCode, string message)       // room�� ���� ���� �������� �� ȣ��
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();      // ���� room�� ����
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()       // room�� ������(LeaveRoom()�Լ��� ȣ��Ǹ�) ȣ��Ǵ� �Լ�
    {
        MenuManager.Instance.OpenMenu("title");
    }
}