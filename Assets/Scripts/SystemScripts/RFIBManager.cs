using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RFIBManager : MonoBehaviour
{
    #region Block & Touch Setting
    // 允許甚麼編號被接受
    static string[] AllowBlockType = {
        "9999"  // 99 floor
	};

    #endregion

    #region RFID parameter
    RFIBricks_Cores RFIB;
    short[] EnableAntenna = {1, 2, 3, 4};       //reader port
    string ReaderIP = "192.168.1.96";           //到時再說
    double ReaderPower = 32, Sensitive = -70;   //功率, 敏感度
    bool Flag_ToConnectTheReade = false;        //false就不會連reader

    #endregion

    public CardHandler cardHandler;

    // 編號規則:
    // 系統編號 此欄空白 方塊種類 編號+上下 方向
    
    void Start()
    {
        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        RFIB.setShowSysMesg(true);
        RFIB.setShowReceiveTag(true);
        RFIB.setShowDebugMesg(true);

        RFIB.setSysTagBased("8940 0000");       // 允許的系統編號
        RFIB.setAllowBlockType(AllowBlockType);

        RFIB.setRefreshTime(600);               // clear beffer
        RFIB.setDisappearTime(400);             // id 消失多久才會的消失
        RFIB.setDelayForReceivingTime(200);     // 清空之後停多久才收id

        BoardMapping();                         // 開始接收ID前要將地板配對

        RFIB.startReceive();
        RFIB.startToBuild();
        RFIB.printNoiseIDs();
    }

    // Update is called once per frame
    void Update()
    {
        RFIB.statesUpdate();
        StackSensing();
        TouchSensing();
        KeyPressed();
    }

    // 在開始接收ID前，這邊要將接收到的地板ID進行配對編號。
    private void BoardMapping()
    {
        //  [04]   | 0004 0104  ..   ..   ..   ..   ..  0704 0804
        //  [03]   | 0003 0103  ..   ..   ..   ..   ..  0703 0803
        //  [02]   | 0002 0102  ..   ..   ..   ..   ..  0702 0802
        //  [01]   | 0001 0101  ..   ..   ..   ..   ..  0701 0801
        //  [00]   | 0000 0100  ..   ..   ..   ..   ..  0700 0800
        //-------／-----------------------------------------------
        //   y ／x | [00] [01] [02] [03] [04] [05] [06] [07] [08] 

        for (int i = 0; i < GameParameter.blockNum; i++)
        {
            string pos = "0" + (i % GameParameter.stageCol).ToString() + "0" + (i / GameParameter.stageCol).ToString();
            RFIB.setBoardBlockMappingArray(i, pos);
        }
    }

    private void StackSensing()
    {
        // 偵測每格地板上堆疊了幾個方塊，並把數值更新到相對應的stackSensing表格
        //for (int i = 0; i < GameParameter.blockNum; i++)
        //{
        //    string[] idStack = new string[3] { "0000", "0000", "0000" };
        //    //string[] idDirection = new string[3] { "000000", "000000", "000000" };

        //    idStack[0] = GetBlockInfoXYZ(i % GameParameter.stageCol, i / GameParameter.stageCol, 0, "BlockIDType");
        //    //idDirection[0] = GetBlockInfoXYZ(i % GameParameter.stageCol, i / GameParameter.stageCol, 0, "StackWay");
        //    if (idStack[0] != "0000")
        //    {

        //    }
        //}

        for (int i = 0; i < GameParameter.stageCol; i++)
        {
            for (int j = 0; j < GameParameter.stageRow; j++)
            {
                for (int k = 0; k < GameParameter.maxHight; k++)
                {
                    string idStack = "0000";
                    idStack = GetBlockInfoXYZ(i, j, k, "BlockIDType");

                    if (idStack != "0000")
                    {
                        cardHandler.stackSensing[i, j, k] = CardDictionary.SearchCard(idStack);
                    }
                    else
                    {
                        cardHandler.stackSensing[i, j, k] = -1;
                    }
                }
            }
        }
    }

    private void TouchSensing()
    {

    }

    public void KeyPressed()
    {
        if (Input.GetKeyUp("="))
            RFIB.StopReader();
        if (Input.GetKeyUp("["))
            RFIB.printStackedOrders3D();
        if (Input.GetKeyUp(";"))
        {
            RFIB.printStackedOrders();
            Debug.Log(GetBlockInfoXYZ(3, 1, 0, "BlockIDType"));
        }
        if (Input.GetKeyUp("."))
        {
            Debug.Log("====g====");
            Debug.Log("BlcokID: " + GetBlockInfoXYZ(0, 4, 2, "BlcokID"));
            Debug.Log("SurfaceID: " + GetBlockInfoXYZ(0, 1, 1, "SurfaceID"));
            Debug.Log("StackWay: " + GetBlockInfoXYZ(0, 1, 1, "StackWay"));
            Debug.Log("BlockIDType: " + GetBlockInfoXYZ(0, 1, 1, "BlockIDType"));
        }
                
        if (Input.GetKeyUp("1"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("8940 0000 9999 0301 0001", "8940 0000 7101 0101 0001");
        }
        if (Input.GetKeyUp("2"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("8940 0000 9999 0501 0001", "8940 0000 7201 0201 0001");
        }
    }

    public string GetBlockInfoXYZ(int X, int Y, int Z, string TARGET)
    {
        foreach (int tmpID in RFIB.StackedOrders3D.Keys)
        {
            if (RFIB.StackedOrders3D[tmpID][0] == X - 1 && RFIB.StackedOrders3D[tmpID][1] == Y - 1 && RFIB.StackedOrders3D[tmpID][2] == Z + 1)
            {
                if (TARGET.Equals("BlcokID"))                   // 目前跟BlockIDType一樣
                    return tmpID + "";
                if (TARGET.Equals("SurfaceID"))
                    return RFIB.StackedOrders3D[tmpID][3] + "";
                if (TARGET.Equals("BlockIDType"))               // 123456 下前上後右左
                    return RFIB.StackedOrders3D[tmpID][5] + "";
                if (TARGET.Equals("StackWay"))
                    return RFIB.StackedOrders3D[tmpID][4] + "";
                else
                    return "0000";
            }
        }
        return "0000";
    }
}
