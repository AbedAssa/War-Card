using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// class holds all the custom events needed in the game.
/// </summary>
public class CustomEvents : MonoBehaviour
{
    //This event get called each time a card is distributed for User
    static public event Action<object> OnCardDistributedForUser;


    //Trigger for OnCardDistributionDoneForUser,This method must be called whenever
    //A card is distributed for User
    static public void OnCardDistributedForUserTigger(object obj)
    {
        OnCardDistributedForUser?.Invoke(obj);
    }


    //This event get called each time a card is distributed for System
    static public event Action<object> OnCardDistributedForSystem;


    //Trigger for OnCardDistributionDoneForUser,This method must be called whenever
    //A card is distributed for System
    static public void OnCardDistributedForSystemTigger(object obj)
    {
        OnCardDistributedForSystem?.Invoke(obj);
        
    }


    //Get tiggered when played card is playing but still not showing the card face
    static public event Action<object> OnExitFirstFlipUserState;
    static public void OnExitFirstFlipUserStateTrigger(object obj)
    {
        OnExitFirstFlipUserState?.Invoke(obj);

    }

    //Get tiggered when played card is playing and showing card face.
    static public event Action<object> OnExitSecondFlipUserState;
    static public void OnExitSecondFlipUserStateTigger(object obj)
    {
        OnExitSecondFlipUserState?.Invoke(obj);

    }


    /// Get triggered when first card out of the 3 cards reach the center.
    static public event Action<object> OnExitMovingCardCenterState;
    static public void OnExitMovingCardCenterStateTigger(object obj)
    {
        OnExitMovingCardCenterState?.Invoke(obj);

    }



}
