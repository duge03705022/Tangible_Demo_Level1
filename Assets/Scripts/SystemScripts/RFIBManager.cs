﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// 編號規則:
// 系統編號 此欄空白 方塊種類 編號+上下 方向

public class RFIBManager : MonoBehaviour
{
    RFIBricks_Cores RFIB;
    public CardHandler cardHandler;
    public BlockController blockController;

    #region RFIB parameter
    short[] EnableAntenna = {1, 2, 3, 4};       // reader port
    string ReaderIP = "192.168.1.96";           // 到時再說
    double ReaderPower = 32, Sensitive = -70;   // 功率, 敏感度
    bool Flag_ToConnectTheReade = false;        // false就不會連reader

    bool showSysMesg = true;
    bool showReceiveTag = true;
    bool showDebugMesg = true;

    string sysTagBased = "8940 0000";           // 允許的系統編號

    int refreshTime = 600;                      // clear beffer
    int disappearTime = 400;                    // id 消失多久才會的消失
    int delayForReceivingTime = 200;            // 清空之後停多久才收id

    #endregion

    public string[,,] blockId;

    void Start()
    {
        #region Set RFIB Parameter
        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        RFIB.setShowSysMesg(showSysMesg);
        RFIB.setShowReceiveTag(showReceiveTag);
        RFIB.setShowDebugMesg(showDebugMesg);

        RFIB.setSysTagBased(sysTagBased);
        RFIB.setAllowBlockType(RFIBParameter.AllowBlockType);

        RFIB.setRefreshTime(refreshTime);
        RFIB.setDisappearTime(disappearTime);
        RFIB.setDelayForReceivingTime(delayForReceivingTime);

        // 開始接收ID前要將地板配對
        BoardMapping();

        RFIB.startReceive();
        RFIB.startToBuild();
        RFIB.printNoiseIDs();

        #endregion

        blockId = new string[RFIBParameter.stageCol, RFIBParameter.stageRow, RFIBParameter.maxHight];
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

        for (int i = 0; i < RFIBParameter.blockNum; i++)
        {
            string pos = "0" + (i % RFIBParameter.stageCol).ToString() + "0" + (i / RFIBParameter.stageCol).ToString();
            RFIB.setBoardBlockMappingArray(i, pos);
        }
    }

    private void StackSensing()
    {
        // 偵測每格地板上堆疊了幾個方塊，並把數值更新到相對應的stackSensing表格
        for (int i = 0; i < RFIBParameter.stageCol; i++)
        {
            for (int j = 0; j < RFIBParameter.stageRow; j++)
            {
                for (int k = 0; k < RFIBParameter.maxHight; k++)
                {
                    string idStack = "0000";
                    idStack = GetBlockInfoXYZ(i, j, k, "BlockIDType");

                    if (idStack != "0000")
                    {
                        blockId[i, j, k] = idStack;
                    }
                    else
                    {
                        blockId[i, j, k] = "0000";
                    }
                }
            }
        }
    }

    private void TouchSensing()
    {
        Stack<string> touchShow = Arduino.touchShowID;
        Stack<string> touchShow2 = Arduino2.touchShowID;
        while (touchShow.Count() > 0)
        {
            blockController.touchStr = touchShow.Pop();
        }
        while (touchShow2.Count() > 0)
        {
            blockController.touchStr2 = touchShow2.Pop();
        }
    }

    public void KeyPressed()
    {
        if (Input.GetKeyUp("1"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("8940 0000 9999 0301 0001", "8940 0000 7101 0101 0001");
        }
        if (Input.GetKeyUp("2"))
        {
            RFIB._Testing_AddTestingTemporarilyTag("8940 0000 9999 0501 0001", "8940 0000 7201 0201 0001");
        }

        #region Information
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

        #endregion
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
