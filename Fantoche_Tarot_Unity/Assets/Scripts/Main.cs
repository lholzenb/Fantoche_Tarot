using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("Setup & General")]
    public GameObject Pair;
    public int pairNumber;
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
        red_is_fire = new CardType("red_is_fire","Fire beats air.", "blue_is_water", "green_is_air");
        Debug.Log(red_is_fire.Name + ": " + red_is_fire.Description);
        yellow_is_earth = new CardType("yellow_is_earth","Earth beats water.", "green_is_air", "blue_is_water");
        Debug.Log(yellow_is_earth.Name + ": " + yellow_is_earth.Description);
        green_is_air = new CardType("green_is_air","Air beats earth.", "red_is_fire", "yellow_is_earth");
        Debug.Log(green_is_air.Name + ": " + green_is_air.Description);
        blue_is_water = new CardType("blue_is_water","Water beats fire.","yellow_is_earth", "red_is_fire");
        Debug.Log(blue_is_water.Name + ": " + blue_is_water.Description);

        // defining card types - other cards
        joker_card = new CardType("joker_card","Joker card resolves a truce by being laid upside down (negative) or in the correct position (positive).","none","none");
    }

    // Update is called once per frame
    void Update()
    {
        // something something happens
        
        // we calculate the outcome for the cards present on the board
        CardRelationshipLogic();
        CardMathematicalSummary();
        FinalPhase();
    }

    void CardRelationshipLogic()
    {   
        var temp1 = false;
        // here we determine the outcome for each new card presented
        if (Current_CardLeft != null)
        {

        }

    }
    void CardMathematicalSummary()
    {
        //
    }
    void FinalPhase()
    {
        //
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
