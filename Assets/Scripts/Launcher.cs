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

    public void CreateRoom()     // room을 생성하고 자동으로 room에 들어감
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))       // InputField가 null인지 아닌지를 비교 후 null이면 true를 반환
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);      // room의 이름을 인자로 받으면서 room생성
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()     // room에 들어갔으면 호출
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;     // 현재 들어온 room의 이름을 받아옴
    }

    public override void OnCreateRoomFailed(short returnCode, string message)       // room에 들어가는 것을 실패했을 때 호출
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();      // 현재 room을 나감
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()       // room을 떠나면(LeaveRoom()함수가 호출되면) 호출되는 함수
    {
        MenuManager.Instance.OpenMenu("title");
    }
}