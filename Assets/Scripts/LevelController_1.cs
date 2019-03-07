using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController_1 : MonoBehaviour
{
    public CardHandler cardHandler;

    private bool gameFinish = false;

    public GameObject[] levelCards;
    public int[] availablePlace;

    public int startX;
    public int startY;

    public GameObject parentDish;
    public GameObject dishPrefab;
    public GameObject[] ingredientsPrefab;

    private GameObject dish;
    private GameObject ingredients;

    // Start is called before the first frame update
    void Start()
    {
        cardHandler.setCanPlaceCard(availablePlace, true);
        cardHandler.setCards(levelCards);

        dish = Instantiate(dishPrefab, parentDish.transform);
        dish.transform.localPosition = new Vector3(
            startX * GameParameter.stageGap,
            startY * GameParameter.stageGap,
            0);

        for (int i = 0; i < ingredientsPrefab.Length; i++)
        {
            ingredients = Instantiate(ingredientsPrefab[i], dish.transform);
            ingredients.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            dish.transform.localPosition += new Vector3(0, GameParameter.dishSpeed, 0);
        }
        if (Input.GetKey("a"))
        {
            dish.transform.localPosition += new Vector3(-GameParameter.dishSpeed, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            dish.transform.localPosition += new Vector3(0, -GameParameter.dishSpeed, 0);
        }
        if (Input.GetKey("d"))
        {
            dish.transform.localPosition += new Vector3(GameParameter.dishSpeed, 0, 0);
        }
    }
}
