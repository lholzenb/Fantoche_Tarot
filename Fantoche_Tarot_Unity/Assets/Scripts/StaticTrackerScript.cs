using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTracker : MonoBehaviour
{
    public static StaticTracker Instance {get; set;}
    public int s_allowedCards = 3;

    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void changeAllowedCards(int newValue)
    {
        s_allowedCards = newValue;
        Debug.Log("new value for allowed cards: " + s_allowedCards);
    }
}
