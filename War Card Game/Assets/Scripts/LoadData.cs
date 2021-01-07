using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class getting tthe data needed to play the game
/// and generate two random lists for user and system.
/// </summary>
public class LoadData : MonoBehaviour
{
    public List<Sprite> cardsImg;                        
    private List<(ECards Card, Sprite Img)> allCards;
    public List<(ECards Card, Sprite Img)> userCardsList { private set; get; }
    public List<(ECards Card, Sprite Img)> systemCardsList { private set; get; }
    static public LoadData instance;

    //
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        FillAllCards();
        GenerateTwoRandomCardsLists();
    }

    //Getting all sprites with the proper card name
    public void FillAllCards()
    {
        allCards = new List<(ECards Card, Sprite Img)>();
        int eCount = 2;
        for (int i = 0; i < 52; i++)
        {
            if (i % 13 == 0)
            {
                allCards.Add((ECards.CA, cardsImg[i]));
                eCount = 2;
            }
            else
            {
                allCards.Add(((ECards)eCount, cardsImg[i]));
                eCount++;
            }

        }
    }

    //Generate two random lists for both player and system.
    private void GenerateTwoRandomCardsLists()
    {
        userCardsList = new List<(ECards Card, Sprite Img)>();
        for (int i = 0; i < 26; i++)
        {
            int randomIndex = Random.Range(0, allCards.Count);
            userCardsList.Add(allCards[randomIndex]);
            allCards.RemoveAt(randomIndex);
        }
        systemCardsList = allCards;
        //For Debug
        //systemCardsList = new List<(ECards Card, Sprite Img)>();
        //systemCardsList.Add(allCards[0]);
        //userCardsList.Add(allCards[1]);
        //systemCardsList.Add(allCards[3]);
        //userCardsList.Add(allCards[5]);
        //systemCardsList.Add(allCards[7]);
        //userCardsList.Add(allCards[9]);
        //systemCardsList.Add(allCards[26]);
        //userCardsList.Add(allCards[39]);
        //systemCardsList.Add(allCards[21]);
        //userCardsList.Add(allCards[22]);
        //systemCardsList.Add(allCards[23]);
        //userCardsList.Add(allCards[24]);
        //systemCardsList.Add(allCards[25]);
        //userCardsList.Add(allCards[26]);
        //systemCardsList.Add(allCards[30]);
        //userCardsList.Add(allCards[31]);

    }
}
