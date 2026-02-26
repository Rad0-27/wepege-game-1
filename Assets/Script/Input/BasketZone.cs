using System.Collections.Generic;
using UnityEngine;

public class BasketZone : MonoBehaviour
{
    private HashSet<BreadItem> breadsInside = new HashSet<BreadItem>();

    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BreadItem item = other.GetComponent<BreadItem>();

        if (item != null)
        {
            breadsInside.Add(item);
            gameManager.CheckAllCollected();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BreadItem item = other.GetComponent<BreadItem>();

        if (item != null)
        {
            breadsInside.Remove(item);
        }
    }

    public int CountInside()
    {
        return breadsInside.Count;
    }
}