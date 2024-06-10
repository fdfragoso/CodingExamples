using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionItemUIHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI sessionNameText;

    [SerializeField]
    TextMeshProUGUI sessionMapText;

    [SerializeField]
    TextMeshProUGUI numberOfPlayersText;

    public void SetTexts(string sessionName, string mapName, string numberOfPlayers)
    {
        sessionNameText.text = sessionName;
        sessionMapText.text = mapName;
        numberOfPlayersText.text = numberOfPlayers;
    }

    public void OnJoinButtonClicked()
    {
        FindObjectOfType<MainMenuUIHandler>().OnRoomJoinedClicked(sessionNameText.text);
    }


}
