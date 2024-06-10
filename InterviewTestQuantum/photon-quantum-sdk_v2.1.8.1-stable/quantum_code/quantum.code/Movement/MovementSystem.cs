using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerDataSet, ISignalOnPlayerDisconnected
    {
        //Gameobjects that are being updated
        public struct Filter
        {
            public EntityRef Entity;
            public CharacterController3D* CharacterController;
            public Transform3D* Transform;
            public PlayerLink* Link;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            //Game session data
            GameSession* gameSession = f.Unsafe.GetPointerSingleton<GameSession>();

            if (gameSession == null)
                return;

            if (gameSession->State != GameState.Playing)
                return;

            //Input
            var input = f.GetPlayerInput(filter.Link->Player);
            var inputVector = new FPVector2((FP)input->DirectionX/10, (FP)input->DirectionY/10);

            //anti cheat for input
            if(inputVector.SqrMagnitude > 1)
                inputVector = inputVector.Normalized;

            //Move Character controller
            filter.CharacterController->Move(f, filter.Entity, new FPVector3(inputVector.X, 0, inputVector.Y));

            //Jump Character controller
            if (input->Jump.WasPressed)
                filter.CharacterController->Jump(f);

            //Rotate - movment direction
            if(inputVector.SqrMagnitude != default)
            {
                filter.Transform->Rotation = FPQuaternion.Lerp(filter.Transform->Rotation, 
                    FPQuaternion.LookRotation(inputVector.XOY), f.DeltaTime * 10); 
            }

            //Respawn player if falls from the level
            if(filter.Transform->Position.Y < -10)
            {
                filter.Transform->Position = GetSpawnPosition(filter.Link->Player, f.PlayerCount);
            }

            Physics3D.HitCollection3D hitCollection3D = f.Physics3D.OverlapShape(filter.Transform->Position, FPQuaternion.Identity, Shape3D.CreateSphere(1));

            for(int i = 0; i < hitCollection3D.Count; i++)
            {
                if (hitCollection3D[i].IsTrigger)
                {
                    gameSession->State = GameState.GameOver;
                }
            }
        }

        public void OnPlayerDataSet(Frame f, PlayerRef player)
        {
            var data = f.GetPlayerData(player);

            var prototypeEntity = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
            var createEntity = f.Create(prototypeEntity);

            if(f.Unsafe.TryGetPointer<PlayerLink>(createEntity, out var playerLink))
            {
                playerLink->Player = player;
            }

            if (f.Unsafe.TryGetPointer<Transform3D>(createEntity, out var transform))
            {
                transform->Position = GetSpawnPosition(player, f.PlayerCount);
            }
        }

        FPVector3 GetSpawnPosition(int playerId, int playerCount)
        {
            int widthOfAllPlayers = playerCount * 2;

            return new FPVector3((playerId * 2) + 1 - widthOfAllPlayers/2, 3, 0);
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            foreach(var playerLink in f.GetComponentIterator<PlayerLink>())
            {
                if(playerLink.Component.Player != player)
                    continue;

                f.Destroy(playerLink.Entity);
            }
        }
    }  
}
