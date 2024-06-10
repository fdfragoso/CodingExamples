using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    public string roomName;
    public int numberPlayers;
    public int maxPlayers;
}
