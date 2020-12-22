using System;
using System.Collections.Generic;

class Day22
{
    List<string> data;
    Queue<int> deck1 = new Queue<int>();
    Queue<int> deck2 = new Queue<int>();

    public Day22(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        dealTheCards();
        playTheFirstGame();
        playTheSecondGame();

    }
    private void playTheFirstGame()
    {
        while (deck1.Count > 0 && deck2.Count > 0)
        {
            playARegularRound(deck1, deck2);
        }
        scoreTheGame();
    }

    private void playTheSecondGame()
    {

    }

    private void playARecursiveRound(Queue<int> activeDeck1, Queue<int> activeDeck2, Stack<(Queue<int> priorDeck1, Queue<int> priorDeck2)> gameStack)
    {

    }


    private void playARegularRound(Queue<int> deck1, Queue<int> deck2)
    {
        int card1 = deck1.Dequeue();
        int card2 = deck2.Dequeue();
        if (card1 > card2)
        {
            deck1.Enqueue(card1);
            deck1.Enqueue(card2);
        }
        else if (card2 > card1)
        {
            deck2.Enqueue(card2);
            deck2.Enqueue(card1);
        }
        else
        {
            Console.WriteLine("Tie");
        }
    }

    private void scoreTheGame()
    {
        Queue<int> winningDeck = deck1.Count > 0 ? deck1 : deck2;
        int score = 0;
        int numCards = winningDeck.Count;
        for (int i = 0; i < numCards; i++)
        {
            score += winningDeck.Count * winningDeck.Dequeue();
        }
        Console.WriteLine($"Score is {score}");
    }

    private void dealTheCards()
    {
        //  deal the cards
        Queue<int> activeDeck = deck1;
        foreach (string line in data)
        {
            if (line.StartsWith("Player"))
            {
                if (line.Contains("1"))
                {
                    activeDeck = deck1;
                    continue;
                }
                else if (line.Contains("2"))
                {
                    activeDeck = deck2;
                    continue;
                }
            }
            else if (line == "") { continue; }
            else
            {
                activeDeck.Enqueue(int.Parse(line));
            }
        }
    }

}