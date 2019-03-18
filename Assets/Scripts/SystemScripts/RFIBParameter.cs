using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class RFIBParameter
{
    public static readonly int stageRow = 5;
    public static readonly int stageCol = 9;
    public static readonly int maxHight = 3;

    public static readonly int blockNum = stageRow * stageCol;

    // 允許甚麼編號被接受
    public static readonly string[] AllowBlockType = {       
        "9999",     // 99 floor
        "7101",     // 71 cut
        "7201"      // 72 cook
	};

    // RFIB_ID對應的instance_ID
    public static int SearchCard(string idStr)
    {
        switch (idStr)
        {
            case "7101": return 0;      // 71 cut
            case "7201": return 1;      // 72 cook

            case "0000": return -1;
        }
        return -1;
    }
}
