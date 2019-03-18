using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    # region Game Parameter
    public TouchHandler touchHandler;
    public CardHandler cardHandler;
    public GameObject levelController;

    public bool playing;

    public int playBtnX;
    public int playBtnY;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        ifPlay();
    }

    private void ifPlay()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (touchHandler.touchSensing[playBtnX * 3 + i, playBtnY * 3 + j])
                {
                    if (!playing)
                    {
                        levelController.SendMessage("StartCooking");
                        playing = true;
                        //cardHandler.setCardTrans = true;
                    }
                }
            }
        }
    }
}
