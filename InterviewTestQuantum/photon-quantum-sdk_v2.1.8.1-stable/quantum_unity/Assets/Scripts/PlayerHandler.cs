using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Quantum;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] EntityView _entityView;

    public void OnEntityInstantiated()
    {
        Debug.Log("Player OnEntityInstantiated");

        QuantumGame quantumGame = QuantumRunner.Default.Game;

        Frame frame = quantumGame.Frames.Verified;

        if(frame.TryGet(_entityView.EntityRef, out PlayerLink playerLink))
        {
            //check if player is local
            if(quantumGame.PlayerIsLocal(playerLink.Player))
            {
                CinemachineVirtualCamera virtualCam = FindAnyObjectByType<CinemachineVirtualCamera>();
                virtualCam.Follow = transform;
            }
        }
    }
}
