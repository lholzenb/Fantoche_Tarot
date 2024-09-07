using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    private GameObject mechanicsHolder;
    private bool isHovering = false;
    void OnMouseEnter()
    {
        isHovering = true;
        mechanicsHolder.GetComponent<Main>().StatusUpdate(gameObject.name, "holder", true);
    }
    void OnMouseExit()
    {
        isHovering = false;
        mechanicsHolder.GetComponent<Main>().StatusUpdate(gameObject.name, "holder", false);
    }
    void Start()
    {
        mechanicsHolder = GameObject.Find("Mechanics_Holder");
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
