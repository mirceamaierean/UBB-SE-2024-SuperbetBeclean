
using SuperbetBeclean.Model;
using System;
using System.Collections.Generic;

class HandRankCalculator
{
    private Dictionary<string, int> cardValues;
    /// Since our cards range in value from 2 to 14, we will use base 15 to hash a certain hand
    private List<int> pows;
    const int NULL = 0;
    const int INIT = 1, ONE = 1;
    const int PAIR = 2;
    const int TRIPS = 3;
    const int QUADS = 4;

    public HandRankCalculator()
    {
        cardValues = new Dictionary<string, int>();
        cardValues.Add("2", 2);
        cardValues.Add("3", 3);
        cardValues.Add("4", 4);
        cardValues.Add("5", 5);
        cardValues.Add("6", 6);
        cardValues.Add("7", 7);
        cardValues.Add("8", 8);
        cardValues.Add("9", 9);
        cardValues.Add("10",10);
        cardValues.Add("J", 11);
        cardValues.Add("Q", 12);
        cardValues.Add("K", 13);
        cardValues.Add("A", 14);
        pows = new List<int>() { 1, 15, 225, 3375, 50625, 759375 }; /// Powers of 15
    }

    private void handSort(List<Card> hand)
    {
        hand.Sort((x, y) => cardValues[y.Value].CompareTo(cardValues[x.Value]));
    }
    private bool isRoyalFlush(List<Card> hand)
    {
        if (isFlush(hand))
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (cardValues[hand[i].Value] != 14 - i)
                    return false;
            }
            return true;
        }
        return false;
    }
    private int hashRoyalFlush(List<Card> hand)
    {
        return pows[0];
    }

    private bool isStraightFlush(List<Card> hand)
    {
        if (isStraight(hand) && isFlush(hand))
            return true;
        return false;
    }

    private int hashStraightFlush(List<Card> hand)
    {
        if (hand[0].Value == "A" && hand[1].Value == "5")
            return cardValues[hand[1].Value] * pows[0];
        else
            return cardValues[hand[0].Value] * pows[0];
    }
    
    private bool isFourOfAKind(List<Card> hand)
    {
        Dictionary <string, int> freq = new Dictionary <string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        if (freq.Count != 2) 
            return false;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value != ONE && pair.Value != QUADS) 
                return false;
        }
        return true;
    }

    private int hashFourOfAKind(List <Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        int result = 0;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == ONE) result += cardValues[pair.Key] * pows[0];
            else result += cardValues[pair.Key] * pows[1];
        }
        return result;
    }

    private bool isFullHouse(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        if (freq.Count != 2)
            return false;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value != PAIR && pair.Value != TRIPS)
                return false;
        }
        return true;
    }

    private int hashFullHouse(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        int result = 0;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == PAIR) result += cardValues[pair.Key] * pows[0];
            else result += cardValues[pair.Key] * pows[1];
        }
        return result;
    }
    private bool isFlush(List<Card> hand)
    {
        HashSet<string> suits = new HashSet<string>();
        foreach (Card card in hand)
            suits.Add(card.Suit);
        return suits.Count == 1;
    }

    private int hashFlush(List<Card> hand)
    {
        int result = 0, freebit = 4;
        foreach (Card card in hand)
        {
            result += cardValues[card.Value] * pows[freebit--];
        }
        return result;
    }

    private bool isStraight(List<Card> hand)
    {
        bool isStraight = true;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (cardValues[hand[i].Value] - 1 != cardValues[hand[i + 1].Value] && (hand[i].Value != "A" || hand[i + 1].Value != "5"))
                isStraight = false;
        }
        return isStraight;
    }
    
    private int hashStraight(List<Card> hand)
    {
        if (hand[0].Value == "A" && hand[1].Value == "5")
            return cardValues[hand[1].Value] * pows[0];
        else
            return cardValues[hand[0].Value] * pows[0];
    }

    private bool isThreeOfAKind(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == TRIPS)
                return true;
        }
        return false;
    }

    private int hashThreeOfAKind(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 1, result = 0;
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        foreach (Card card in hand)
        {
            if (freq[card.Value] == TRIPS)
            {
                result += cardValues[card.Value] * pows[2];
                freq[card.Value] = NULL;
            }
            else if (freq[card.Value] != NULL)
            {
                result += cardValues[card.Value] * pows[freebit--];
            }
        }
        return result;
    }

    private bool isTwoPairs(List<Card> hand) 
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        return freq.Count == 3;
    }

    private int hashTwoPairs(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 2, result = 0;
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        foreach (Card card in hand)
        {
            if (freq[card.Value] == PAIR)
            {
                result += cardValues[card.Value] * pows[freebit--];
                freq[card.Value] = NULL;
            }
            else if (freq[card.Value] != NULL)
            {
                result += cardValues[card.Value] * pows[0];
            }
        }
        return result;
    }

    private bool isOnePair(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        return freq.Count == 4;
    }

    private int hashOnePair(List<Card> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 2, result = 0;
        foreach (Card card in hand)
        {
            if (freq.ContainsKey(card.Value))
                freq[card.Value]++;
            else freq[card.Value] = INIT;
        }
        foreach (Card card in hand)
        {
            if (freq[card.Value] == PAIR)
            {
                result += cardValues[card.Value] * pows[3];
                freq[card.Value] = NULL;
            }
            else if (freq[card.Value] != NULL)
            {
                result += cardValues[card.Value] * pows[freebit--];
            }
        }
        return result;
    }

    private int hashHighCard(List<Card> hand)
    {
        int freebit = 4, result = 0;
        foreach (Card card in hand)
        {
            result += cardValues[card.Value] * pows[freebit--];
        }
        return result;
    }

    public Tuple<int, int> getValue(List < Card > hand)
    {
        handSort(hand);
        if (isRoyalFlush(hand))
            return new Tuple<int, int>(10, hashRoyalFlush(hand));
        if (isStraightFlush(hand))
            return new Tuple<int, int>(9, hashStraightFlush(hand));
        if (isFourOfAKind(hand))
            return new Tuple<int, int>(8, hashFourOfAKind(hand));
        if (isFullHouse(hand))
            return new Tuple<int, int>(7, hashFullHouse(hand));
        if (isFlush(hand))
            return new Tuple<int, int>(6, hashFlush(hand));
        if (isStraight(hand))
            return new Tuple<int, int>(5, hashStraight(hand));
        if (isThreeOfAKind(hand))
            return new Tuple<int, int>(4, hashThreeOfAKind(hand));
        if (isTwoPairs(hand))
            return new Tuple<int, int>(3, hashTwoPairs(hand));
        if (isOnePair(hand))
            return new Tuple<int, int>(2, hashOnePair(hand));
        return new Tuple<int, int>(1, hashHighCard(hand));
    }
}