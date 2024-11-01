using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WRITTEN WITH GPT
public class MenuCollider : MonoBehaviour
{
    public GameObject menu; // Das Menü, das überwacht werden soll

    private bool isMenuOpen = false; // Status, ob das Menü gerade offen ist oder die Maus drüber ist

    void OnMouseEnter()
    {
        // Die Maus ist über dem Menü
        isMenuOpen = true;
        Debug.Log("Menu Hovered: Card hover disabled.");
        DisableCardHover(); // Karten Hover deaktivieren
    }

    void OnMouseExit()
    {
        // Die Maus hat das Menü verlassen
        isMenuOpen = false;
        Debug.Log("Menu not hovered: Card hover enabled.");
        EnableCardHover(); // Karten Hover wieder aktivieren
    }

    // Karten Hover deaktivieren
    void DisableCardHover()
    {
        foreach (var card in FindObjectsOfType<CardObject>()) // Alle Kartenobjekte im Spiel finden
        {
            if (card.GetComponent<Collider2D>() != null)
            {
                card.GetComponent<Collider2D>().enabled = false; // Collider deaktivieren
            }
        }
    }

    // Karten Hover aktivieren
    void EnableCardHover()
    {
        foreach (var card in FindObjectsOfType<CardObject>()) // Alle Kartenobjekte im Spiel finden
        {
            if (card.GetComponent<Collider2D>() != null)
            {
                card.GetComponent<Collider2D>().enabled = true; // Collider aktivieren
            }
        }
    }
}
