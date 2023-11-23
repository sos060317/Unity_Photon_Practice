using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

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
    [SerializeField] GameObject startGameButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();       // Photon Server Setting 파일에 입력된 설정대로 Photon에 연결함
    }

    public override void OnConnectedToMaster()      // 플레이어가 Photon Server 서버에 연결되어 있고 다른 작업을 진행할 수 있으면 호출
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();      // Master Server에 사용중인 room을 나열하고 Lobby에 진입
        PhotonNetwork.AutomaticallySyncScene = true;     // 마스터 클라이언트와 일반 클라이언트들이 레벨을 동기화할지 결정함
                                                         //true로 설정하면 마스터 클라이언트에서 LoadLevel()로 레벨을 변경하면 모든 클라이언트들이 자동으로 동일한 레벨을 로드함
    }

    public override void OnJoinedLobby()     // Master Server의 Lobby에 들어갔을 때 호출
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);    // 마스터 클라이언트에게만 보이게 해줌 
    }

    public override void OnMasterClientSwitched(Player newMasterClient)     // 마스터 클라이언트가 바뀌면
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)       // room에 들어가는 것을 실패했을 때 호출
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);     // Build Setting의 1번째 인덱스 씬을 로드함
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();      // 현재 room을 나감
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);      // info.Name(이름? ID?)로 room에 참가
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()       // room을 떠나면(LeaveRoom()함수가 호출되면) 호출되는 함수
    {
        MenuManager.Instance.OpenMenu("title");
    }

    #region 아래 함수 설명
    // room을 갱신하기 위해서 사용
    // 실행되는 경우
    // -로비에 접속 시
    // -새로운 룸이 만들어질 경우
    // -룸이 삭제되는 경우
    // -룸의 IsOpen 값이 변화할 경우(아예 RoomInfo 내 데이터가 바뀌는 경우 전체일 수도 있습니다)
    // -매개변수로 넘어오는 roomList는 "현재 존재하는 방 전부"가 아니라 "변동사항이 있는 방"만 넘어옴
    #endregion
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)    // 해당 room이 제거되었는지 확인
                continue;       // 제거됨
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);       // 제거되지 않음
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}