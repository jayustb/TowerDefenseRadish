﻿//游戏的常量类

using UnityEngine;

public static class Const
{
    //资源目录
    public static readonly string ConDir_Level
        = Application.dataPath + @"/CarrotFantasyMain/Res/Levels";

    public static readonly string ConDir_Map
        = Application.dataPath + @"/CarrotFantasyMain/Res/Maps";

    //存档目录

    //Model
    public const string M_GameModel = "M_GameModel";
    public const string M_RoundModel = "M_RoundModel";


    //View
    public const string V_Start = "V_Start";
    public const string V_Select = "V_Select";
    public const string V_Board = "V_Board";
    public const string V_CountDown = "V_CountDown";
    public const string V_Win = "V_Win";
    public const string V_Lost = "V_Lost";
    public const string V_Sytem = "V_Sytem";
    public const string V_Complete = "V_Complete";
    public const string V_Spanwner = "V_Spanwner";
    public const string V_TowerPopup = "V_TowerPopup";


    //Controller
    public const string E_StartUp = "E_StartUp";

    public const string E_EnterScene = "E_EnterScene"; //SceneArgs
    public const string E_ExitScene = "E_ExitScene"; //SceneArgs

    public const string E_StartLevel = "E_StartLevel"; //StartLevelArgs
    public const string E_EndLevel = "E_EndLevel"; //EndLevelArgs

    public const string E_CountDownComplete = "E_CountDownComplete";

    public const string E_StartRound = "E_StartRound"; //StartRoundArgs
    public const string E_SpawnMonster = "E_SpawnMonster"; //SpawnMonsterArgs
    public const string E_SpawnTower = "E_SpawnTower"; //SpawnTowerArgs
    public const string E_UpgradeTower = "E_UpgradeTower"; //UpgradeTowerArgs
    public const string E_SellTower = "E_SellTower"; //SellTowerArgs

    public const string E_ShowCreate = "E_ShowCreate"; //ShowCreatorArgs
    public const string E_ShowUpgrade = "E_ShowUpgrade"; //ShowUpgradeArgs
    public const string E_HidePopup = "E_HidePopup";
}