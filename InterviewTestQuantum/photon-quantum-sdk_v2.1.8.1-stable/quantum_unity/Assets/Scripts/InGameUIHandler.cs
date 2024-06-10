using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using TMPro;
using UnityEngine.Purchasing;
using Quantum.Demo;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI playerCount;

    [SerializeField] RoomData roomData;

    private void Start()
    {
        roomName.text = "Room Name: " + roomData.roomName;

    }

    private void Update()
    {
        if(Utils.TryGetQuantumFrame(out Frame frame))
        {
            if(frame.TryGetSingletonEntityRef<GameSession>(out var entity) == false)
            {
                countDownText.text = "Game Session singleton not found";
                return;
            }

            var gameSession = frame.GetSingleton<GameSession>();
            

            int countDown = (int)gameSession.TimeUntilStart;

            switch(gameSession.State) 
            {
                case GameState.Countdown:
                    countDownText.text = $"{countDown}";
                    break;
                case GameState.Playing:
                    if (countDown == 0)
                        countDownText.text = "Go!";

                    if (countDown < 0)
                        countDownText.text = "";

                    break;
                case GameState.GameOver:
                    countDownText.text = "Game Over!";
                    break;
            }
        }

        if (QuantumRunner.Default)
        {
            var gameInstance = QuantumRunner.Default.Game;

            if (gameInstance?.Session?.Game == null)
                return;

            playerCount.text = "Number of Players: " + gameInstance.Session.PlayerCount;
        }
    }
}
