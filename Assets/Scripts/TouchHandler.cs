using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    # region Touch Parameter
    public bool[,] touchSensing;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        touchSensing = new bool[RFIBParameter.stageCol * 3, RFIBParameter.stageRow * 3];
        for (int i = 0; i < RFIBParameter.stageCol * 3; i++)
        {
            for (int j = 0; j < RFIBParameter.stageRow * 3; j++)
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
            touchSensing[25, 1] = true;
        }
        if (Input.GetKeyUp("p"))
        {
            touchSensing[25, 1] = false;
        }
    }
}
