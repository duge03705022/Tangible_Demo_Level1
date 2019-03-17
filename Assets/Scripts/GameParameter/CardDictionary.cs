using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CardDictionary
{
    public static int SearchCard(string idStr)
    {
        switch (idStr)
        {
            case "7101": return 0;
            case "7201": return 1;
        }
        return -1;
    }
}
