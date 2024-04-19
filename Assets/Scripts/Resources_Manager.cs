using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Animations;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Resource_Manager", fileName = "Resource_Manager")]
public class Resources_Manager : ScriptableObject
{
    private static Resources_Manager _instance;
    public static Resources_Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                // تحميل مورد الإدارة من مجلد الموارد
                _instance = Resources.Load<Resources_Manager>("Resource_Manager");
            }

            return _instance;
        }
    }

    [Header("Player Manager")]
    public GamePlayManager _Game_Play_Manager;

    [Header("Builiding Resources")]
    public AnimatorController Screw_Button_Animator;

    [Header("Cards")]
    public Cards cards;
    

    #region  Cards Resources ==========================================>
    [System.Serializable] 
    public class Cards
    {
        public CardController Card_1, Card_2, Card_3, Card_4, Card_5, Card_6, Card_7, Card_8, Card_9, Card_10;
        public CardController Take_And_Give,Look_All_cards,Remove_Cart,Card_20,Card_Myness_1,Card_25,Card_0;
    }
    
    
    public List<CardController> All_Cards
    {
        get
       {
        List<CardController> allCards = new List<CardController>();

        // Add all cards to the list
        allCards.Add(cards.Card_1);
        allCards.Add(cards.Card_2);
        allCards.Add(cards.Card_3);
        allCards.Add(cards.Card_4);
        allCards.Add(cards.Card_5);
        allCards.Add(cards.Card_6);
        allCards.Add(cards.Card_7);
        allCards.Add(cards.Card_8);
        allCards.Add(cards.Card_9);
        allCards.Add(cards.Card_10);
        allCards.Add(cards.Take_And_Give);
        allCards.Add(cards.Look_All_cards);
        allCards.Add(cards.Remove_Cart);
        allCards.Add(cards.Card_20);
        allCards.Add(cards.Card_Myness_1);
        allCards.Add(cards.Card_25);
        allCards.Add(cards.Card_0);

        return allCards;
      }
    }
    #endregion
}
