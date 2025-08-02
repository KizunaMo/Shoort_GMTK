using UnityEngine;

namespace Framework
{
    public static class Consts
    {
        /// <summary>
        /// Game Jam , what ever XD
        /// </summary>
        public class SceneGameObjectName
        {
            public const string CutBtn = "CutBtn";
            public const string NextCustomerBtn = "NextCustomerBtn";
            public const string CutomerList = "CutomeristViewModule";
            public const string RestCustomerBtn = "RestCustomerBtn";
            
            public const string ScoreText = "ScoreText";
            public const string GameOverPanel = "GameOver";
            
            public const string MainMenuPanel = "MainMenu";
            public const string StarGameBtn = "StartBtn";

            public const string AudioController = "AudioController";
        }


        public class AnimationName
        {
            public const string Jump = "Jump";
            public const float JumpDuration = 1f;
            public const string Enter = "Enter";
            public const float EnterDuration = 2f;
            public const string Exit = "Exit";
            public const float ExitDuration = 2f;
            public const string Angry = "Angry";
            public const float AngryDuration = 1.5f;
        }

        public class PrefabsPath
        {
            public const string CustomerItemPrefab = "Prefabs/Customer";
        }
        
        
        public static Vector3 spawnPosition = new Vector3(-80f, -20f, 0f);
        public static float GameOverShowTime = 2.0f;
    }
}