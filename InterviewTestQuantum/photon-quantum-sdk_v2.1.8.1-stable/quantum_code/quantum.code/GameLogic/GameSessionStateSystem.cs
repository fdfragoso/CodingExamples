﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe class GameSessionStateSystem : SystemMainThreadFilter<GameSessionStateSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public GameSession* GameSession;
        }

        public override void OnInit(Frame f)
        {
            Log.Debug("Quantum GameSessionStateSystem OnInit");
        }

        public override void Update(Frame f, ref Filter filter)
        {
            //Get the game session data
            GameSession* gameSession = f.Unsafe.GetPointerSingleton<GameSession>();

            if (gameSession == null)
                return;

            gameSession->TimeUntilStart = gameSession->TimeUntilStart - f.DeltaTime;

            if (gameSession->TimeUntilStart < 1 && gameSession->State == GameState.Countdown)
            {
                gameSession->State = GameState.Playing;
                Log.Debug("Quantum GameSessionStateSystem Countdown completed - Game Session State: " + gameSession->State);
                
            }
        }
    }
}

