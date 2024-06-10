using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Quantum;
using Quantum.Demo;

public class MainMenuUIHandler : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback, ILobbyCallbacks
{
    [Header("Connection handler")]

    [SerializeField]
    ConnectionHandler connectionHandler;

    [Header("Panels")]

    [SerializeField]
    Canvas mainMenuCanvas;

    [SerializeField]
    CanvasGroup mainPanel;

    [SerializeField]
    CanvasGroup playerPanel;

    [SerializeField]
    CanvasGroup sessionPanel;

    [SerializeField]
    CanvasGroup roomPanel;

    [Header("Player details")]
    [SerializeField]
    TMP_InputField playerNameInputfield;

    [Header("Room details")]
    [SerializeField]
    VerticalLayoutGroup roomMemberListLayoutGroup;

    [SerializeField]
    GameObject roomMemberNamePrefab;

    [Header("Session details")]
    [SerializeField]
    VerticalLayoutGroup sessionListLayoutGroup;

    [SerializeField]
    GameObject sessionListPrefab;

    //Quantum client
    QuantumLoadBalancingClient client;

    [SerializeField]
    public RoomData roomData;

    enum PhotonEventCode : byte
    {
        StartGame = 110
    }

    //Player details
    string playerName = "";

    //Map
    long mapGuid = 0L;

    //Modes
    bool isQuickPlayAutoJoinEnabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        client?.Service();
    }

    #region Multiplayer connect and join code

    bool ConnectToMaster()
    {
        var appSettings = PhotonServerSettings.CloneAppSettings(PhotonServerSettings.Instance.AppSettings);

        client = new QuantumLoadBalancingClient(PhotonServerSettings.Instance.AppSettings.Protocol);

        //Get connection callback events etc
        client.AddCallbackTarget(this);

        if (string.IsNullOrEmpty(appSettings.AppIdRealtime.Trim()))
        {
            Utils.DebugLogError("Missing Quantum AppID");
            return false;
        }

        //If none was provided give a random one
        if (playerName == "")
            playerName = Utils.GetRandomName();

        //Connect to the Photon Cloud
        if (!client.ConnectUsingSettings(appSettings, playerName))
        {
            Utils.DebugLogError("Unable to issue Connect to Master command");
            return false;
        }

        Utils.DebugLog($"Attempting to connect to region {appSettings.FixedRegion}");

        return true;
    }

    EnterRoomParams CreateEnterRoomParams(string roomName)
    {

        //Setup room properties
        EnterRoomParams enterRoomParams = new EnterRoomParams();

        enterRoomParams.RoomOptions = new RoomOptions();

        if(roomName == null ||  roomName == "") 
            enterRoomParams.RoomName = "Room#" + Random.Range(1f, 2048f);
        else
            enterRoomParams.RoomName = roomName;

        enterRoomParams.RoomOptions.IsVisible = true;
        enterRoomParams.RoomOptions.MaxPlayers = 0;

        enterRoomParams.RoomOptions.Plugins = new string[] { "QuantumPlugin" };
        enterRoomParams.RoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "MAP-GUID", mapGuid },
        };

        enterRoomParams.RoomOptions.CustomRoomPropertiesForLobby = new string[] { "MAP-GUID" };

        enterRoomParams.RoomOptions.PlayerTtl = PhotonServerSettings.Instance.PlayerTtlInSeconds * 1000;
        enterRoomParams.RoomOptions.EmptyRoomTtl = PhotonServerSettings.Instance.EmptyRoomTtlInSeconds * 1000;

        return enterRoomParams;
    }

    void JoinRandomOrCreateRoom()
    {
        //Prevent disconnects during long scene loads
        connectionHandler.Client = client;
        connectionHandler.StartFallbackSendAckThread();

        // Pick the first map we can find
        var allMapsInResources = UnityEngine.Resources.LoadAll<MapAsset>(QuantumEditorSettings.Instance.DatabasePathInResources);
        mapGuid = allMapsInResources[0].AssetObject.Guid.Value;

        Utils.DebugLog($"Using MAP long GUID {mapGuid}. GUID {allMapsInResources[0].AssetObject.Guid}");

        //Setup room properties
        EnterRoomParams enterRoomParams = CreateEnterRoomParams("");

        OpJoinRandomRoomParams joinRandomParams = new OpJoinRandomRoomParams();

        //Find a random room or create one if needed.
        if (!client.OpJoinRandomOrCreateRoom(joinRandomParams, enterRoomParams))
        {
            Utils.DebugLogError("Unable to join random room or create room");
            return;
        }


        Utils.DebugLog($"Joining random room or creating new room");
    }

    void JoinRoom(string roomName)
    {
        //Prevent disconnects during long scene loads
        connectionHandler.Client = client;
        connectionHandler.StartFallbackSendAckThread();

        // Pick the first map we can find
        var allMapsInResources = UnityEngine.Resources.LoadAll<MapAsset>(QuantumEditorSettings.Instance.DatabasePathInResources);
        mapGuid = allMapsInResources[0].AssetObject.Guid.Value;

        Utils.DebugLog($"Using MAP long GUID {mapGuid}. GUID {allMapsInResources[0].AssetObject.Guid}");

        //Setup room properties
        EnterRoomParams enterRoomParams = CreateEnterRoomParams(roomName);

        //Join specific room
        if (!client.OpJoinRoom(enterRoomParams))
        {
            Utils.DebugLogError("Unable to join room");
            return;
        }
    }

    void JoinLobby()
    {
        if (!client.OpJoinLobby(TypedLobby.Default))
        {
            Utils.DebugLogError("Unable join lobby");

            return;
        }
    }

    void StartGame()
    {
        if (!client.OpRaiseEvent((byte)PhotonEventCode.StartGame, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable))
        {
            Utils.DebugLogError("Unable to start game");
            return;
        }

        roomData.roomName = client.CurrentRoom.Name;
        roomData.maxPlayers = client.CurrentRoom.MaxPlayers;
        roomData.numberPlayers = client.CurrentRoom.PlayerCount;

        Utils.DebugLog("Starting game");
    }

    void StartQuantumGame()
    {
        if (QuantumRunner.Default != null)
        {
            //Check for other runners. Only 1 runner is allowed
            Utils.DebugLogWarning($"Another QuantumRunner '{QuantumRunner.Default.name}' has prevented starting the game");
            return;
        }

        RuntimeConfig runtimeConfig = new RuntimeConfig();

        runtimeConfig.Map.Id = mapGuid;

        var param = new QuantumRunner.StartParameters
        {
            RuntimeConfig = runtimeConfig,
            DeterministicConfig = DeterministicSessionConfigAsset.Instance.Config,
            GameMode = Photon.Deterministic.DeterministicGameMode.Multiplayer,
            FrameData = null,
            InitialFrame = 0,
            PlayerCount = client.CurrentRoom.MaxPlayers,
            LocalPlayerCount = 1,
            RecordingFlags = RecordingFlags.None,
            NetworkClient = client,
            StartGameTimeoutInSeconds = 10.0f
        };

        // Get the clientID which needs to be the same for reconnect to work.
        var clientId = ClientIdProvider.CreateClientId(ClientIdProvider.Type.PhotonUserId, client);

        Utils.DebugLog($"Starting QuantumRunner with clientID {clientId} and map guid {mapGuid}. Local player count {param.LocalPlayerCount}");

        mainMenuCanvas.gameObject.SetActive(false);

        QuantumRunner.StartGame(clientId, param);

    }


    #endregion

    #region UI Code
    void HideAllPanels()
    {
        mainPanel.gameObject.SetActive(false);
        roomPanel.gameObject.SetActive(false);
        playerPanel.gameObject.SetActive(false);
        sessionPanel.gameObject.SetActive(false);
    }

    void UpdateRoomDetails()
    {
        if (!client.InRoom)
        {
            Utils.DebugLogError("Client no longer in room, cannot update room details");

            ClearChildrenLayoutGroup(roomMemberListLayoutGroup.transform);

            return;
        }

        ClearChildrenLayoutGroup(roomMemberListLayoutGroup.transform);

        //Loop through the list of players and add them to the UI 
        foreach (KeyValuePair<int, Player> player in client.CurrentRoom.Players)
        {
            GameObject instantiatedObject = Instantiate(roomMemberNamePrefab, roomMemberListLayoutGroup.transform);
            instantiatedObject.GetComponent<TextMeshProUGUI>().text = player.Value.NickName;

        }
    }

    void ClearChildrenLayoutGroup(Transform layoutGroup)
    {
        //Clear the old UI in reversed order
        for (int i = layoutGroup.childCount - 1; i >= 0; i--)
        {
            Destroy(layoutGroup.GetChild(i).gameObject);
        }
    }

    IEnumerator UpdateRoomDetailsCO()
    {
        while (roomPanel.gameObject.activeInHierarchy)
        {
            UpdateRoomDetails();
            yield return new WaitForSeconds(1);
        }
    }

    #endregion

    #region UI Events

    public void OnPlayerDetailsSetClicked()
    {
        //Set the players name
        playerName = playerNameInputfield.text;

        HideAllPanels();

        mainPanel.gameObject.SetActive(true);
    }
    public void OnQuickPlayClicked()
    {
        HideAllPanels();
        roomPanel.gameObject.SetActive(true);

        isQuickPlayAutoJoinEnabled = true;

        if (client != null && client.IsConnected)
        {
            JoinRandomOrCreateRoom();
        }
        else
            ConnectToMaster();
    }

    public void OnStartGameClicked()
    {
        StartGame();
    }

    public void OnSessionBrowserClicked()
    {
        isQuickPlayAutoJoinEnabled = false;

        HideAllPanels();
        sessionPanel.gameObject.SetActive(true);

        if (client != null && client.IsConnected)
        {
            JoinLobby();
        }
        else
            ConnectToMaster();
    }

    public void OnRoomJoinedClicked(string roomName)
    {
        HideAllPanels();
        roomPanel.gameObject.SetActive(true);

        JoinRoom(roomName);
    }

    public void OnLeftSessionList()
    {
        HideAllPanels();
        mainPanel.gameObject.SetActive(true);
    }

    #endregion

    #region Unity events

    void OnDestroy()
    {

    }

    #endregion

    #region Connection events

    public void OnConnected()
    {
        Utils.DebugLog($"OnConnected UserId: {client.UserId}");
    }

    public void OnConnectedToMaster()
    {
        Utils.DebugLog($"Connected to master server in region {client.CloudRegion}");

        if (isQuickPlayAutoJoinEnabled)
            JoinRandomOrCreateRoom();
        else JoinLobby();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Utils.DebugLog($"OnDisconnected cause {cause}");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Utils.DebugLog($"OnRegionListReceived");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Utils.DebugLog($"OnCustomAuthenticationResponse");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Utils.DebugLog($"OnCustomAuthenticationFailed");
    }

    #endregion

    #region Room events
    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Utils.DebugLog($"OnFriendListUpdate");
    }

    public void OnCreatedRoom()
    {
        Utils.DebugLog($"OnCreatedRoom");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        Utils.DebugLog($"OnCreateRoomFailed");
    }

    public void OnJoinedRoom()
    {
        Utils.DebugLog($"OnJoinedRoom");

        StartCoroutine(UpdateRoomDetailsCO());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Utils.DebugLogError($"OnJoinRoomFailed return code {returnCode} message {message}");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Utils.DebugLogError($"OnJoinRandomFailed return code {returnCode} message {message}");
    }

    public void OnLeftRoom()
    {
        Utils.DebugLog($"OnLeftRoom");
    }

    #endregion

    #region Photon Events
    public void OnEvent(EventData photonEvent)
    {
        Utils.DebugLog($"photonEvent received code {photonEvent.Code}. ");

        switch (photonEvent.Code)
        {
            case (byte)PhotonEventCode.StartGame:
                client.CurrentRoom.CustomProperties.TryGetValue("MAP-GUID", out object mapGuidValue);

                if (mapGuidValue == null)
                {
                    Utils.DebugLogError("Failed to get map GUID, disconnecting");
                    client.Disconnect();

                    return;
                }

                //Check if we are the master cleint
                if (client.LocalPlayer.IsMasterClient)
                {
                    client.CurrentRoom.IsVisible = false;
                    client.CurrentRoom.IsOpen = false;
                }

                StartQuantumGame();


                break;

        }

    }

    #endregion

    #region Lobby events

    public void OnJoinedLobby()
    {
        Utils.DebugLog("OnJoinedLobby");
    }

    public void OnLeftLobby()
    {
        Utils.DebugLog("OnLeftLobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Utils.DebugLog($"OnRoomListUpdate. Rooms {roomList.Count}");

        ClearChildrenLayoutGroup(sessionListLayoutGroup.transform);

        foreach (RoomInfo roomInfo in roomList)
        {
            string mapName = "";

            if (roomInfo.CustomProperties.TryGetValue("MAP-GUID", out var mapID))
            {
                var allMapsInResources = UnityEngine.Resources.LoadAll<MapAsset>(QuantumEditorSettings.Instance.DatabasePathInResources);

                foreach (var map in allMapsInResources)
                {
                    if (map.AssetObject.Guid.Value == (long)mapID)
                    {
                        mapName = map.name;
                        break;
                    }
                }
            }

            Utils.DebugLog($"Room name {roomInfo.Name} map {mapName} player count {roomInfo.PlayerCount} max {roomInfo.MaxPlayers} open {roomInfo.IsOpen} visble {roomInfo.IsVisible}");

            GameObject instantiatedObject = Instantiate(sessionListPrefab, sessionListLayoutGroup.transform);
            instantiatedObject.GetComponent<SessionItemUIHandler>().SetTexts(roomInfo.Name, mapName, $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}");
        }

    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Utils.DebugLog("OnLobbyStatisticsUpdate");
    }

    #endregion
}
