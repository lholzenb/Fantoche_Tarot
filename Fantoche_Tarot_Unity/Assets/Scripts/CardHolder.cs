using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    private GameObject mechanicsHolder;
    private SpriteRenderer spriteRenderer;
    private bool isHovering = false;
    private bool isHoldingCard = false;
     private bool deactivateInteractions = false;
    UnityEngine.Vector3 baseScale;
    
    
    void OnMouseEnter()
    {
        if (deactivateInteractions)
        {
            HighlightCard(false);
            return;
        }
        isHovering = true;
        isHoldingCard = mechanicsHolder.GetComponent<Main>().StatusUpdate(gameObject.name, "holder", true);
        if (!isHoldingCard)
        {
            HighlightCard(true);
        }
    }
    void OnMouseExit()
    {
        if (deactivateInteractions)
        {
            HighlightCard(false);
            return;
        }
        isHovering = false;
        isHoldingCard = mechanicsHolder.GetComponent<Main>().StatusUpdate(gameObject.name, "holder", false);
        HighlightCard(false);
    }
    void OnMouseUp()
    {
        if (deactivateInteractions)
        {
            HighlightCard(false);
            return;
        }
        if (isHovering == true)
        {
            mechanicsHolder.GetComponent<Main>().LayPrep("lay");
        }
    }
    void Start()
    {
        mechanicsHolder = GameObject.Find("Mechanics_Holder");
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void HighlightCard(bool highlight)
    {
        if (highlight)
        {
            // Zum Beispiel die Karte etwas vergrößern
            transform.localScale = new Vector3(baseScale.x + 0.2f, baseScale.y + 0.2f, baseScale.z + 0.2f);
            spriteRenderer.sortingOrder = 15;
        }
        else
        {
            transform.localScale = baseScale;
            spriteRenderer.sortingOrder -= 1;
        }
    }

    public void DeactivateInteractions()
    {
        deactivateInteractions = true;
    }
}
