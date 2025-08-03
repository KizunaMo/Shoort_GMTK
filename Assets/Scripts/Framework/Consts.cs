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

            public const string TimerUI = "Timer";
            public const string FinalResultCheckPanelUI = "FinalResultPanel";

            public const string OpeningController = "Opening";
            public const string RulerController = "Ruler";
        }


        public class AnimationName
        {
            public const string Jump = "Jump";
            public const float JumpDuration = 1f;
            public const string Enter = "Enter";
            public const float EnterDuration = 0.5f;
            public const string Exit = "Exit";
            public const float ExitDuration = 1f;
            public const string Angry = "Angry";
            public const float AngryDuration = 1.5f;

            public const string DuringCutting = "DuringCutting";
            public const float DuringCuttingDuration = 2f;

            //失敗扭來扭去
            public const float FailedAnimationDuration = 5f;
        }

        public class PrefabsPath
        {
            public const string CustomerItemPrefab = "Prefabs/Customer";
        }


        public class CustomKeywords
        {
            public const string HairRoot = "HairRoot";
            public const string BlinkRoot = "BlinkRoot";
            public const string EyesRoot = "EyesRoot";
            public const string MouthRoot = "MouthRoot";
            public const string MakeupRoot = "MakeupRoot";
            public const string FaceRoot = "FaceRoot";
            public const string HeadRoot = "HeadRoot";
            public const string DecorationRoot = "DecorationRoot";
            public const string HipRoot = "HipsRoot";
            public const string BodyRoot = "BodyRoot";
            public const string FootRoot = "FootRoot";
            public const string HandRoot = "HandRoot";

            public const string HairStyleRoot = "HairStyleRoot";

            public const string SuccessCutHairIndex = "hair1";

            public const float Hidth = 15;
            public const float Height = 15;
        }

        public class HairColor
        {
            // hair fornt total 1-20  (20)  :: hair 1-8 (8)
            public const int TotalFrontHairCount = 20;
            // public const int TotalHairCount = 8;

            public const string HairFornt_1_Hex = "#b615a25";
            public const string HairFornt_2_Hex = "#ebce84";
            public const string HairFornt_3_Hex = "#759166";
            public const string HairFornt_4_Hex = "#573d3d";
            public const string HairFornt_5_Hex = "#de7942";
            public const string HairFornt_6_Hex = "#8bd4bd";
            public const string HairFornt_7_Hex = "#a6a9ca";
            public const string HairFornt_8_Hex = "#646fe6";
            
            public const string HairFornt_9_Hex = "#e187e6";
            public const string HairFornt_10_Hex = "#674a66";
            public const string HairFornt_11_Hex = "#707d7d";
            public const string HairFornt_12_Hex = "#b5f482";
            public const string HairFornt_13_Hex = "#e2806a";
            public const string HairFornt_14_Hex = "#baa83c";
            public const string HairFornt_16_Hex = "#e26296";
            public const string HairFornt_17_Hex = "#309198";
            public const string HairFornt_18_Hex = "#4755bd";
            public const string HairFornt_19_Hex = "#e3c636";
            public const string HairFornt_20_Hex = "#4b5b5b";


            public const string Head_1_Hex = "@e3c636";
        }

        public static string[] AllHairFrontsHex = new string[]
        {
            Consts.HairColor.HairFornt_1_Hex,
            Consts.HairColor.HairFornt_2_Hex,
            Consts.HairColor.HairFornt_3_Hex,
            Consts.HairColor.HairFornt_4_Hex,
            Consts.HairColor.HairFornt_5_Hex,
            Consts.HairColor.HairFornt_6_Hex,
            Consts.HairColor.HairFornt_7_Hex,
            Consts.HairColor.HairFornt_8_Hex,
            Consts.HairColor.HairFornt_9_Hex,
            Consts.HairColor.HairFornt_10_Hex,
            Consts.HairColor.HairFornt_11_Hex,
            Consts.HairColor.HairFornt_12_Hex,
            Consts.HairColor.HairFornt_13_Hex,
            Consts.HairColor.HairFornt_14_Hex,
            Consts.HairColor.HairFornt_16_Hex,
            Consts.HairColor.HairFornt_17_Hex,
            Consts.HairColor.HairFornt_18_Hex,
            Consts.HairColor.HairFornt_19_Hex,
            Consts.HairColor.HairFornt_20_Hex
        };
        
        // public static string[] AllHairsHex = new string[]
        // {
        //     Consts.HairColor.HairFornt_1_Hex,
        //     Consts.HairColor.HairFornt_2_Hex,
        //     Consts.HairColor.HairFornt_3_Hex,
        //     Consts.HairColor.HairFornt_4_Hex,
        //     Consts.HairColor.HairFornt_5_Hex,
        //     Consts.HairColor.HairFornt_6_Hex,
        //     Consts.HairColor.HairFornt_7_Hex,
        //     Consts.HairColor.HairFornt_8_Hex,
        // };

        public static Vector3 spawnPosition = new Vector3(-80f, -20f, 0f);
        public static float GameOverShowTime = 3.0f;
        public static float FinalResultShowTime = 5.0f;

        public static float CamFarway = 100;
        public static float CamNearby = 50;
    }
}