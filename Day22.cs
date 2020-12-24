using System;
using System.Collections.Generic;

class Day22
{
    List<string> data;
    Queue<int> deck1 = new Queue<int>();
    Queue<int> deck2 = new Queue<int>();
    List<(Queue<int>, Queue<int>)> activeGameHistory = new List<(Queue<int>, Queue<int>)>();
    Stack<(Queue<int> priorDeck1, Queue<int> priorDeck2)> gameStack = new Stack<(Queue<int> priorDeck1, Queue<int> priorDeck2)>();
    Stack<(int lastCard1, int lastCard2)> lastCardsOnTableStack = new Stack<(int lastCard1, int lastCard2)>();
    // Stack<int> lastCard1Stack = new Stack<int>();
    // Stack<int> lastCard2Stack = new Stack<int>();
    Stack<List<(Queue<int>, Queue<int>)>> historyStack = new Stack<List<(Queue<int>, Queue<int>)>>();

    Stack<List<Queue<int>>> player1HistoryStack = new Stack<List<Queue<int>>>();

    // long NUM_ROUNDS = 0;

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
        //  start off by making copies of the decks
        Queue<int> activeDeck1 = new Queue<int>(deck1);
        Queue<int> activeDeck2 = new Queue<int>(deck2);

        while (activeDeck1.Count > 0 && activeDeck2.Count > 0)
        {
            int card1 = activeDeck1.Dequeue();
            int card2 = activeDeck2.Dequeue();
            playARegularRound(activeDeck1, activeDeck2, card1, card2);
        }
        if (activeDeck1.Count > 0)
        {
            scoreTheGame(activeDeck1);
        }
        else
        {
            scoreTheGame(activeDeck2);
        }
    }

    private void playTheSecondGame()
    {

        //  start off by making copies of the decks
        Queue<int> activeDeck1 = new Queue<int>(deck1);
        Queue<int> activeDeck2 = new Queue<int>(deck2);

        int counter = 1;
        int winner = -1;

        List<Queue<int>> currentHistory = new List<Queue<int>>();

        //  set while limit to arbitrary high number
        while (counter < 500000)
        {

            //  check history
            bool weveBeenHereBefore = false;
            foreach (var pastHand in currentHistory)
            {
                if (queuesAreTheSame(pastHand, activeDeck1))
                {
                    weveBeenHereBefore = true;
                    break;
                }
            }
            if (weveBeenHereBefore)
            {
                //  how to indicate player 1 won
                if (gameStack.Count == 0)
                {
                    Console.WriteLine("This was the top-level game; won through infinite default");
                    winner = 1;
                    break;
                }
                (Queue<int> deck1, Queue<int> deck2) gameToResume = gameStack.Pop();
                (int lastCard1, int lastCard2) = lastCardsOnTableStack.Pop();
                currentHistory = player1HistoryStack.Pop();
                activeDeck1 = gameToResume.deck1;
                activeDeck2 = gameToResume.deck2;
                //  player 1 always wins in case of infinite loop
                activeDeck1.Enqueue(lastCard1);
                activeDeck1.Enqueue(lastCard2);
            }
            else
            {
                currentHistory.Add(new Queue<int>(activeDeck1));
            }

            currentHistory.Add(new Queue<int>(activeDeck1));
            int card1 = activeDeck1.Peek();
            int card2 = activeDeck2.Peek();

            bool roundComplete = playRecursiveRound(activeDeck1, activeDeck2);
            //  need to create a subgame
            if (!roundComplete)
            {
                Queue<int> savedDeck1ForLater = new Queue<int>(activeDeck1);
                Queue<int> savedDeck2ForLater = new Queue<int>(activeDeck2);
                List<Queue<int>> savedHistory = new List<Queue<int>>(currentHistory);
                Queue<int> deck1ForSubGame = getDeckForSubgame(card1, activeDeck1);
                Queue<int> deck2ForSubGame = getDeckForSubgame(card2, activeDeck2);
                gameStack.Push((savedDeck1ForLater, savedDeck2ForLater));
                lastCardsOnTableStack.Push((card1, card2));
                player1HistoryStack.Push(savedHistory);
                activeDeck1 = deck1ForSubGame;
                activeDeck2 = deck2ForSubGame;
            }
            //  normal round
            else
            {
                if (activeDeck1.Count < 1 || activeDeck2.Count < 1)
                {
                    winner = activeDeck1.Count > 0 ? 1 : 2;
                    if (gameStack.Count == 0)
                    {
                        break;
                    }
                    (Queue<int> deck1, Queue<int> deck2) gameToResume = gameStack.Pop();
                    (int lastCard1, int lastCard2) = lastCardsOnTableStack.Pop();
                    currentHistory = player1HistoryStack.Pop();
                    activeDeck1 = gameToResume.deck1;
                    activeDeck2 = gameToResume.deck2;
                    if (winner == 1)
                    {
                        activeDeck1.Enqueue(lastCard1);
                        activeDeck1.Enqueue(lastCard2);
                    }
                    else
                    {
                        activeDeck2.Enqueue(lastCard2);
                        activeDeck2.Enqueue(lastCard1);
                    }
                }
            }
            counter++;
        }

        Console.WriteLine($"Took this many turns: {counter}");
        //  print the results
        Console.WriteLine($"Winner is {winner}; final decks:");
        printDeck("1", activeDeck1);
        printDeck("2", activeDeck2);
        if (winner == 1)
        {
            scoreTheGame(activeDeck1);
        }
        else if (winner == 2)
        {
            scoreTheGame(activeDeck2);
        }
    }

    private bool playRecursiveRound(Queue<int> activeDeck1, Queue<int> activeDeck2)
    {
        int card1 = activeDeck1.Dequeue();
        int card2 = activeDeck2.Dequeue();

        //  we have to create a subgame
        if (activeDeck1.Count >= card1 && activeDeck2.Count >= card2)
        {
            return false;
        }

        //  otherwise, just keep playing
        playARegularRound(activeDeck1, activeDeck2, card1, card2);
        return true;
    }

    private Queue<int> getDeckForSubgame(int lastCard, Queue<int> deck)
    {
        Queue<int> copy = new Queue<int>(deck);
        Queue<int> newDeck = new Queue<int>();
        for (int i = 0; i < lastCard; i++)
        {
            newDeck.Enqueue(copy.Dequeue());
        }
        return newDeck;
    }

    private void playARegularRound(Queue<int> deck1, Queue<int> deck2, int card1, int card2)
    {
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
    }


    private void scoreTheGame(Queue<int> winningDeck)
    {
        int score = 0;
        int numCards = winningDeck.Count;
        while (winningDeck.Count > 0)
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

    private bool queuesAreTheSame(Queue<int> queue1, Queue<int> queue2)
    {
        if (queue1.Count != queue2.Count) return false;
        Queue<int> queue1Copy = new Queue<int>(queue1);
        Queue<int> queue2Copy = new Queue<int>(queue2);
        while (queue1Copy.Count > 0 && queue2Copy.Count > 0)
        {
            if (queue1Copy.Dequeue() != queue2Copy.Dequeue()) return false;
        }
        return true;
    }
    private void printDeck(string name, Queue<int> deck)
    {
        Console.Write($"Deck {name}: ");
        Queue<int> deckCopy = new Queue<int>(deck);
        while (deckCopy.Count > 0)
        {
            Console.Write($"{deckCopy.Dequeue()}, ");
        }
        Console.WriteLine();
    }
}