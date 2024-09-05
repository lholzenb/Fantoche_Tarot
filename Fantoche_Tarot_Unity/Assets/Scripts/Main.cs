using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("Setup & General")]
    public GameObject Pair;
    public int pairNumber;
    public int currentOutcomePosNeg;
    [Space(20)]
    public GameObject CardLeftHolder;
    private UnityEngine.Vector2 cardLeftPos;
    public GameObject CardRightHolder;
    private UnityEngine.Vector2 cardRightPos;
    public GameObject CardTopHolder;
    private UnityEngine.Vector2 cardTopPos;
    public GameObject CardBottomHolder;
    private UnityEngine.Vector2 cardBottomPos;

    [Space(20)]
    [Header("Current Cards")]
    public GameObject Current_CardLeft;
    public GameObject Current_CardRight;
    public GameObject Current_CardTop;
    public GameObject Current_CardBottom;

    [Space(20)]
    [Header("Card Types")]
    private CardType red_is_fire;
    private CardType yellow_is_earth;
    private CardType green_is_air;
    private CardType blue_is_water;
    private CardType joker_card;

    void Start()
    {
        // game starts & checking if setup correct
        pairNumber = 1;
        if (CardLeftHolder == null || CardRightHolder == null || CardTopHolder == null || CardBottomHolder == null)
        {
            Debug.LogError("CARD SPOTS NOT ASSIGNED!");
            return;
        }

        // read positions of cards in graphical layout
        cardLeftPos = CardLeftHolder.transform.position;
        cardRightPos = CardRightHolder.transform.position;
        cardTopPos = CardTopHolder.transform.position;
        cardBottomPos = CardBottomHolder.transform.position;

        // deactivate holders after initialization of positions
        CardLeftHolder.SetActive(false);
        CardRightHolder.SetActive(false);
        CardTopHolder.SetActive(false);
        CardBottomHolder.SetActive(false);

        // current cards are set to null at start
        Current_CardLeft = null;
        Current_CardRight = null;
        Current_CardTop = null;
        Current_CardBottom = null;

        // defining card types - main color cards
        Debug.Log("All defined card types in-game ->");
        red_is_fire = new CardType("Red","Fire beats air.", "blue_is_water", "green_is_air");
        Debug.Log(red_is_fire.Name + ": " + red_is_fire.Description);
        yellow_is_earth = new CardType("Yellow","Earth beats water.", "green_is_air", "blue_is_water");
        Debug.Log(yellow_is_earth.Name + ": " + yellow_is_earth.Description);
        green_is_air = new CardType("Green","Air beats earth.", "red_is_fire", "yellow_is_earth");
        Debug.Log(green_is_air.Name + ": " + green_is_air.Description);
        blue_is_water = new CardType("Blue","Water beats fire.","yellow_is_earth", "red_is_fire");
        Debug.Log(blue_is_water.Name + ": " + blue_is_water.Description);

        // defining card types - other cards
        joker_card = new CardType("Joker","Joker card resolves a truce by being laid upside down (negative) or in the correct position (positive).","none","none");
    }

    // Update is called once per frame
    void Update()
    {
        // something something happens
        // LayCards(); -> Only triggering this when we want to trigger an action

        // HIER NOCH GENAUER DEFINIEREN

        // we calculate the outcome for the cards present on the board
        CardRelationshipLogic();
        FinalPhase();
    }

    void RadomizeCard(string requestedSpot)
    {
        List<CardType> possibleCards = new List<CardType>()
        {
        red_is_fire,
        yellow_is_earth,
        green_is_air,
        blue_is_water,
        };

        CardType randomCard = possibleCards[Random.Range(0, possibleCards.Count)];
        LayCards(randomCard, requestedSpot);
    }

    void LayCards(CardType cardType, string requestedSpot)
    {
        // at the beginning all spots should be open (interaction true)
        // -> set all interactibles to "active"


        // assigning card type to slots
        if (Current_CardLeft == null && requestedSpot == "left")
        {
            // -> set the respective interactible to "disabled"

            // we assign the type of the card chosen
            Current_CardLeft = new GameObject();
            Current_CardLeft.AddComponent<CardObject>().SetCardType(cardType);
            Debug.Log("Card placed on the left: " + Current_CardLeft.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardRight == null && requestedSpot == "right")
        {
            // -> set the respective interactible to "disabled"

            // we assign the type of the card chosen
            Current_CardRight = new GameObject();
            Current_CardRight.AddComponent<CardObject>().SetCardType(cardType);
            Debug.Log("Card placed on the right: " + Current_CardRight.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardTop == null && requestedSpot == "top")
        {
            // -> set the respective interactible to "disabled"

            // we assign the type of the card chosen
            Current_CardTop = new GameObject();
            Current_CardTop.AddComponent<CardObject>().SetCardType(cardType);
            Debug.Log("Card placed on the top: " + Current_CardTop.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardBottom == null && requestedSpot == "bottom")
        {
            // -> set the respective interactible to "disabled"

            // we assign the type of the card chosen
            Current_CardBottom = new GameObject();
            Current_CardBottom.AddComponent<CardObject>().SetCardType(cardType);
            Debug.Log("Card placed on the bottom: " + Current_CardBottom.GetComponent<CardObject>().cardType.Name);
        }
        else
        {
            // safe is safe - even tho we control it over deactivating interactibles
            Debug.LogWarning("This slot is already assigned! How did you even trigger this interaction?");
        }
    }

    void CardRelationshipLogic()
    {
        // here we determine the outcome for each new card presented - basically changing value when something changes
        // establishing value during this operation which resets every time it has gone through
        var currentOutcomeTemp = 0;

        if (Current_CardLeft != null)
        {
            if (CardMathematicalSummary("left") >= 1)
            {
                currentOutcomeTemp += 1;
            }
            else if (CardMathematicalSummary("left") <= -1)
            {
                currentOutcomeTemp -= 1;
            }
            else 
            {
                // needing some sort of visual representation of neutral value
            }
        }
        if (Current_CardRight != null)
        {
            if (CardMathematicalSummary("right") >= 1)
            {
                currentOutcomeTemp += 1;
            }
            else if (CardMathematicalSummary("right") <= -1)
            {
                currentOutcomeTemp -= 1;
            }
            else 
            {
                // needing some sort of visual representation of neutral value
            }
        }
        if (Current_CardTop != null)
        {
            if (CardMathematicalSummary("top") >= 1)
            {
                currentOutcomeTemp += 1;
            }
            else if (CardMathematicalSummary("top") <= -1)
            {
                currentOutcomeTemp -= 1;
            }
            else 
            {
                // needing some sort of visual representation of neutral value
            }
        }
        if (Current_CardBottom != null)
        {
            if (CardMathematicalSummary("bottom") >= 1)
            {
                currentOutcomeTemp += 1;
            }
            else if (CardMathematicalSummary("bottom") <= -1)
            {
                currentOutcomeTemp -= 1;
            }
            else 
            {
                // needing some sort of visual representation of neutral value
            }
        }      
        currentOutcomePosNeg = currentOutcomeTemp;
    }
    int CardMathematicalSummary(string position)
    {
        if (position == "left")
        {   
            var temp = 0;
            // top, bottom and right influence
            temp += CardCompareWith(Current_CardTop, Current_CardLeft);
            temp += CardCompareWith(Current_CardBottom, Current_CardLeft);
            temp += CardCompareWith(Current_CardRight, Current_CardLeft);
            return temp;
        }
        else if (position == "right")
        {   
            var temp = 0;
            // top, bottom and left influence this
            temp += CardCompareWith(Current_CardTop, Current_CardRight);
            temp += CardCompareWith(Current_CardBottom, Current_CardRight);
            temp += CardCompareWith(Current_CardLeft, Current_CardRight);
            return temp;
        }
        else if (position == "top")
        {
            var temp = 0;
            // bottom, left and right influence this
            temp += CardCompareWith(Current_CardBottom, Current_CardTop);
            temp += CardCompareWith(Current_CardLeft, Current_CardTop);
            temp += CardCompareWith(Current_CardRight, Current_CardTop);
            return temp;
        }
        else if (position == "bottom")
        {
            var temp = 0;
            // top, left and right influence this
            temp += CardCompareWith(Current_CardTop, Current_CardBottom);
            temp += CardCompareWith(Current_CardLeft, Current_CardBottom);
            temp += CardCompareWith(Current_CardRight, Current_CardBottom);
            return temp;
        }
        else 
        {
            Debug.LogError("Something in MATHEMATICAL SUMMARY went horribly wrong!!!");
            return 0;
        }
    }
    int CardCompareWith(GameObject ToCompareWith, GameObject myCard)
    {
        var myCardObject = myCard.GetComponent<CardObject>();
        string myWeakness = myCardObject.cardType.WeaknessAgainst;
        string myStrenght = myCardObject.cardType.StrenghtAgainst;
        if (myWeakness == ToCompareWith.GetComponent<CardObject>().cardType.StrenghtAgainst)
        {
            return -1;
        }
        else if (myStrenght == ToCompareWith.GetComponent<CardObject>().cardType.WeaknessAgainst)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void FinalPhase()
    {
        // we have an outcome, what do we do with it? - And when do we reset it for the next couple?
    }
}

// second class to easify card types
public class CardType
{
    public string Name {get; set;}
    public string Description {get; set;}
    public string WeaknessAgainst {get; set;}
    public string StrenghtAgainst {get; set;}

    // setting values and getting values (constructor)
    public CardType(string name, string description, string weaknessAgainst, string strenghAgainst)
    {
        Name = name;
        Description = description;
        WeaknessAgainst = weaknessAgainst;
        StrenghtAgainst = strenghAgainst;
    }
}
