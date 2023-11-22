using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEditor;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    private void Awake()
    {
        Instance = this;
    }

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
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
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

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);      // info.Name(�̸�? ID?)�� room�� ����
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()       // room�� ������(LeaveRoom()�Լ��� ȣ��Ǹ�) ȣ��Ǵ� �Լ�
    {
        MenuManager.Instance.OpenMenu("title");
    }

    #region �Ʒ� �Լ� ����
    // room�� �����ϱ� ���ؼ� ���
    // ����Ǵ� ���
    // -�κ� ���� ��
    // -���ο� ���� ������� ���
    // -���� �����Ǵ� ���
    // -���� IsOpen ���� ��ȭ�� ���(�ƿ� RoomInfo �� �����Ͱ� �ٲ�� ��� ��ü�� ���� �ֽ��ϴ�)
    // -�Ű������� �Ѿ���� roomList�� "���� �����ϴ� �� ����"�� �ƴ϶� "���������� �ִ� ��"�� �Ѿ��
    #endregion
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}