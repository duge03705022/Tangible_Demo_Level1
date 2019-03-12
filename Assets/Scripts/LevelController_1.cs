using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController_1 : MonoBehaviour
{
    # region Level Parameter
    public GameController gameController;
    public CardHandler cardHandler;

    public int startX;
    public int startY;

    public GameObject[] chef;
    public GameObject[] ingredientStep;

    public GameObject dish;
    public GameObject basket;

    public GameObject[] levelCards;
    public GameObject[] hints;
    public int[] availablePlace;
    public int[] answer;

    private bool gameFail;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        cardHandler.setCanPlaceCard(availablePlace, true);
        cardHandler.setCards(levelCards);

        ResetDish();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartCooking()
    {
        gameFail = false;
        SetHints(false);
        Debug.Log("Game Start!");
        StartCoroutine(CookingProcess());
    }

    IEnumerator CookingProcess()
    {
        if (!gameFail)
        {
            var step = StartCoroutine(DishMove("Right", 3));
            yield return step;
        }

        if (!gameFail)
        {
            var step = StartCoroutine(CheckProcess(0));
            yield return step;
        }

        if (!gameFail)
        {
            var step = StartCoroutine(DishMove("Right", 2));
            yield return step;
        }

        if (!gameFail)
        {
            var step = StartCoroutine(CheckProcess(1));
            yield return step;
        }

        if (!gameFail)
        {
            var step = StartCoroutine(DishMove("Right", 3));
            yield return step;
        }

        if (!gameFail)
        {
            FinishCooking();
        }
    }

    IEnumerator DishMove(string direction, int step)
    {
        for (int i = 0; i < step * 75; i++)
        {
            yield return new WaitForSeconds(0.01f);
            switch (direction)
            {
                case "Up":
                    dish.transform.localPosition += new Vector3(0f, 0.1f, 0f);
                    break;
                case "Left":
                    dish.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
                    break;
                case "Down":
                    dish.transform.localPosition += new Vector3(0f, -0.1f, 0f);
                    break;
                case "Right":
                    dish.transform.localPosition += new Vector3(0.1f, 0f, 0f);
                    break;
                default:
                    Debug.Log("Move direction error");
                    break;
            }
        }
    }

    IEnumerator CheckProcess(int chefNum)
    {
        if (cardHandler.stackSensing[availablePlace[chefNum]] == answer[chefNum])
        {
            ingredientStep[chefNum].SetActive(false);
            chef[chefNum].SendMessage("StartAct");
            yield return new WaitForSeconds(2f);
            ingredientStep[chefNum + 1].SetActive(true);
        }
        else
        {
            FailCooking();
        }
    }

    private void FinishCooking()
    {
        basket.SetActive(false);
        Debug.Log("Game Finish!!!");
        gameController.playing = false;
    }

    private void FailCooking()
    {
        gameFail = true;
        gameController.playing = false;
        cardHandler.setCardTrans = true;
        SetHints(true);
        ResetDish();
        Debug.Log("Game Over...");
    }

    private void ResetDish()
    {
        dish.transform.localPosition = new Vector3(
            startX * GameParameter.stageGap,
            startY * GameParameter.stageGap,
            0);
    }

    private void SetHints(bool onOrOff)
    {
        for (int i = 0; i < hints.Length; i++)
        {
            hints[i].SetActive(onOrOff);
        }
    }
}
