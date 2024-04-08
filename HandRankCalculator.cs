class HandRankCalculator
{
    private Dictionary<string, int> v;
    private List<int> b;
    public HandRankCalculator()
    {
        v = new Dictionary<string, int>();
        v.Add("2", 2);
        v.Add("3", 3);
        v.Add("4", 4);
        v.Add("5", 5);
        v.Add("6", 6);
        v.Add("7", 7);
        v.Add("8", 8);
        v.Add("9", 9);
        v.Add("10",10);
        v.Add("J", 11);
        v.Add("Q", 12);
        v.Add("K", 13);
        v.Add("A", 14);
        b = new List<int>() { 1, 15, 225, 3375, 50625, 759375 };
    }

    private void handSort(List<Tuple<string, string>> hand)
    {
        hand.Sort((x, y) => v[y.Item1].CompareTo(v[x.Item1]));
    }
    private bool isRoyalFlush(List<Tuple<string, string>> hand)
    {
        if (isFlush(hand))
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (v[hand[i].Item1] != 14 - i)
                    return false;
            }
            return true;
        }
        return false;
    }
    private int hashRoyalFlush(List<Tuple<string, string>> hand)
    {
        return b[0];
    }

    private bool isStraightFlush(List<Tuple<string, string>> hand)
    {
        if (isStraight(hand) && isFlush(hand))
            return true;
        return false;
    }

    private int hashStraightFlush(List<Tuple<string, string>> hand)
    {
        if (hand[0].Item1 == "A" && hand[1].Item1 == "5")
            return v[hand[1].Item1] * b[0];
        else
            return v[hand[0].Item1] * b[0];
    }
    
    private bool isFourOfAKind(List<Tuple<string, string>> hand)
    {
        Dictionary <string, int> freq = new Dictionary <string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        if (freq.Count != 2) 
            return false;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value != 1 && pair.Value != 4) 
                return false;
        }
        return true;
    }

    private int hashFourOfAKind(List <Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        int result = 0;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == 1) result += v[pair.Key] * b[0];
            else result += v[pair.Key] * b[1];
        }
        return result;
    }

    private bool isFullHouse(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        if (freq.Count != 2)
            return false;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value != 2 && pair.Value != 3)
                return false;
        }
        return true;
    }

    private int hashFullHouse(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        int result = 0;
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == 2) result += v[pair.Key] * b[0];
            else result += v[pair.Key] * b[1];
        }
        return result;
    }
    private bool isFlush(List<Tuple<string, string>> hand)
    {
        HashSet<string> suits = new HashSet<string>();
        foreach (Tuple<string, string> card in hand)
            suits.Add(card.Item2);
        return suits.Count == 1;
    }

    private int hashFlush(List<Tuple<string, string>> hand)
    {
        int result = 0, freebit = 4;
        foreach (Tuple<string, string> card in hand)
        {
            result += v[card.Item1] * b[freebit--];
        }
        return result;
    }

    private bool isStraight(List<Tuple<string, string>> hand)
    {
        bool isStraight = true;
        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (v[hand[i].Item1] - 1 != v[hand[i + 1].Item1] && (v[hand[i].Item1] != 14 || v[hand[i + 1].Item1] != 5))
                isStraight = false;
        }
        return isStraight;
    }
    
    private int hashStraight(List<Tuple<string, string>> hand)
    {
        if (hand[0].Item1 == "A" && hand[1].Item1 == "5")
            return v[hand[1].Item1] * b[0];
        else
            return v[hand[0].Item1] * b[0];
    }

    private bool isThreeOfAKind(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        
        foreach (KeyValuePair<string, int> pair in freq)
        {
            if (pair.Value == 3)
                return true;
        }
        return false;
    }

    private int hashThreeOfAKind(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 1, result = 0;
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        foreach (Tuple<string, string> card in hand)
        {
            if (freq[card.Item1] == 3)
            {
                result += v[card.Item1] * b[2];
                freq[card.Item1] = 0;
            }
            else if (freq[card.Item1] != 0)
            {
                result += v[card.Item1] * b[freebit--];
            }
        }
        return result;
    }

    private bool isTwoPairs(List<Tuple<string, string>> hand) 
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        return freq.Count == 3;
    }

    private int hashTwoPairs(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 2, result = 0;
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        foreach (Tuple<string, string> card in hand)
        {
            if (freq[card.Item1] == 2)
            {
                result += v[card.Item1] * b[freebit--];
                freq[card.Item1] = 0;
            }
            else if (freq[card.Item1] != 0)
            {
                result += v[card.Item1] * b[0];
            }
        }
        return result;
    }

    private bool isOnePair(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        return freq.Count == 4;
    }

    private int hashOnePair(List<Tuple<string, string>> hand)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        int freebit = 2, result = 0;
        foreach (Tuple<string, string> card in hand)
        {
            if (freq.ContainsKey(card.Item1))
                freq[card.Item1]++;
            else freq[card.Item1] = 1;
        }
        foreach (Tuple<string, string> card in hand)
        {
            if (freq[card.Item1] == 2)
            {
                result += v[card.Item1] * b[3];
                freq[card.Item1] = 0;
            }
            else if (freq[card.Item1] != 0)
            {
                result += v[card.Item1] * b[freebit--];
            }
        }
        return result;
    }

    private int hashHighCard(List<Tuple<string, string>> hand)
    {
        int freebit = 4, result = 0;
        foreach (Tuple<string, string> card in hand)
        {
            result += v[card.Item1] * b[freebit--];
        }
        return result;
    }

    public Tuple<int, int> getValue(List < Tuple < string, string > > hand)
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