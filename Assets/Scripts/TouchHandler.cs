using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    # region Touch Parameter
    public bool[,] touchSensing;

    # endregion

    public TouchHandler touchHandler;

    // Start is called before the first frame update
    void Start()
    {
        touchSensing = new bool[GameParameter.stageCol * 3, GameParameter.stageRow * 3];
        for (int i = 0; i < GameParameter.stageCol * 3; i++)
        {
            for (int j = 0; j < GameParameter.stageRow * 3; j++)
            {
                touchSensing[i, j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            touchHandler.touchSensing[25, 1] = true;
        }
        if (Input.GetKeyUp("p"))
        {
            touchHandler.touchSensing[25, 1] = false;
        }
    }
}
