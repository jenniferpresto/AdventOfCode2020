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

    long NUM_ROUNDS = 0;

    public Day22(List<string> dataList)
    {
        data = dataList;
    }

    public void calculate()
    {
        dealTheCards();
        // playTheFirstGame();
        playTheSecondGame();

        // List<int> test1 = new List<int>();
        // List<int> test2 = new List<int>();
        // List<int> test3 = new List<int>();
        // HashSet<List<int>> testHashSet = new HashSet<List<int>>();
        // test1.Add(1);
        // test1.Add(2);
        // test1.Add(3);
        // test2.Add(3);
        // test2.Add(2);
        // test2.Add(1);
        // test3.Add(1);
        // test3.Add(2);
        // test3.Add(3);
        // testHashSet.Add(test1);
        // testHashSet.Add(test2);
        // testHashSet.Add(test3);
        // Console.WriteLine("Number of items in HashSet: {testHashSet.Count}");
    }
    private void playTheFirstGame()
    {
        while (deck1.Count > 0 && deck2.Count > 0)
        {
            int card1 = deck1.Dequeue();
            int card2 = deck2.Dequeue();
            playARegularRound(deck1, deck2, card1, card2);
        }
        // printDeck("1", deck1);
        // printDeck("2", deck2);
        scoreTheGame();
    }

    private void playTheSecondGame()
    {

        //  start off by making copies of the decks
        Queue<int> activeDeck1 = new Queue<int>(deck1);
        Queue<int> activeDeck2 = new Queue<int>(deck2);

        // int winner = playARecursiveGame(deck1, deck2);

        // Console.WriteLine($"Winner is {winner}");
        // printDeck("1", deck1);
        // printDeck("2", deck2);
        // if (winner == 1)
        // {
        //     scoreTheGame(deck1);
        // }
        // else if (winner == 2)
        // {
        //     scoreTheGame(deck2);
        // }
        // return;

        int counter = 1;
        Console.WriteLine($"Round {counter} through loop");
        printDeck("1", activeDeck1);
        printDeck("2", activeDeck2);
        int winner = -1;
        while (counter < 5000)
        {


            int card1 = activeDeck1.Peek();
            int card2 = activeDeck2.Peek();

            bool roundComplete = playRecursiveRound(activeDeck1, activeDeck2);
            if (!roundComplete)
            {
                Console.WriteLine("Creating a subgame");
                Queue<int> savedDeck1ForLater = new Queue<int>(activeDeck1);
                Queue<int> savedDeck2ForLater = new Queue<int>(activeDeck2);
                Queue<int> deck1ForSubGame = getDeckForSubgame(card1, activeDeck1);
                Queue<int> deck2ForSubGame = getDeckForSubgame(card2, activeDeck2);
                gameStack.Push((savedDeck1ForLater, savedDeck2ForLater));
                lastCardsOnTableStack.Push((card1, card2));

                activeDeck1.Clear();
                activeDeck2.Clear();
                activeDeck1 = null;
                activeDeck2 = null;

                activeDeck1 = deck1ForSubGame;
                activeDeck2 = deck2ForSubGame;
            }
            //  normal round
            else
            {
                if (activeDeck1.Count < 1 || activeDeck2.Count < 1)
                {
                    Console.WriteLine("We have a winner of a game");
                    winner = activeDeck1.Count > 0 ? 1 : 2;
                    if (gameStack.Count == 0)
                    {
                        Console.WriteLine("This was the top-level game");
                        printDeck("1", activeDeck1);
                        printDeck("2", activeDeck2);
                        break;
                    }
                    activeDeck1.Clear();
                    activeDeck2.Clear();
                    activeDeck1 = null;
                    activeDeck2 = null;
                    (Queue<int> deck1, Queue<int> deck2) gameToResume = gameStack.Pop();
                    (int lastCard1, int lastCard2) = lastCardsOnTableStack.Pop();
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
            Console.WriteLine($"Round {counter} through loop");
            printDeck("1", activeDeck1);
            printDeck("2", activeDeck2);
        }


        //  print the results
        Console.WriteLine($"Winner is {winner}");
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
            //  make a copy of the game, drawn cards, history
            // Queue<int> deck1ForSubGame = getDeckForSubgame(card1, activeDeck1);
            // Queue<int> deck2ForSubGame = getDeckForSubgame(card2, activeDeck2);
            // gameStack.Push((activeDeck1, activeDeck2));
            // lastCardsOnTableStack.Push((card1, card2));
            // lastCard1Stack.Push(card1);
            // lastCard2Stack.Push(card2);
            // List<(Queue<int>, Queue<int>)> savedHistory = new List<(Queue<int>, Queue<int>)>(activeGameHistory);
            // historyStack.Push(savedHistory);
            // activeGameHistory.Clear();
            return false;
        }

        //  otherwise, just keep playing
        playARegularRound(activeDeck1, activeDeck2, card1, card2);
        return true;
    }

    //  returns the winner of the game
    private int playARecursiveGame(Queue<int> activeDeck1, Queue<int> activeDeck2)
    {
        Console.WriteLine(NUM_ROUNDS);
        NUM_ROUNDS++;
        //  check for infinite loop
        bool foundSame = false;
        for (int i = 0; i < activeGameHistory.Count; i++)
        {
            (Queue<int>, Queue<int>) tuple = activeGameHistory[i];
            if (queuesAreTheSame(activeDeck1, tuple.Item1) && queuesAreTheSame(activeDeck2, tuple.Item2))
            {
                // // Console.WriteLine("Match!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! (the super-long way)");
                foundSame = true;
                break;
            }
        }
        if (foundSame)
        {
            // // Console.WriteLine("Player 1 wins this game");
            if (gameStack.Count > 0)
            {
                // // Console.WriteLine("Going one game up after near-brush with infinity");
                (Queue<int> deck1, Queue<int> deck2) priorGame = gameStack.Pop();
                (int lastCard1, int lastCard2) lastCards = lastCardsOnTableStack.Pop();
                // int lastCard1 = lastCard1Stack.Pop();
                // int lastCard2 = lastCard2Stack.Pop();
                activeGameHistory.Clear();
                activeGameHistory = null;
                activeGameHistory = historyStack.Pop();
                // // Console.WriteLine($"Prior game count: {priorGame.deck1.Count} cards vs {priorGame.deck2.Count}; after a normal round, stacks are now: game {gameStack.Count}, last cards {lastCardsOnTableStack.Count}, history {historyStack.Count}; new history list has {activeGameHistory.Count} entries");
                // printDeck("Prior 1", priorGame.deck1);
                // printDeck("Prior 2", priorGame.deck2);
                //  winner is always player 1 if infinite loop
                priorGame.deck1.Enqueue(lastCards.lastCard1);
                priorGame.deck1.Enqueue(lastCards.lastCard2);
                // // Console.WriteLine("Adding new cards");
                // printDeck("Prior 1", priorGame.deck1);
                // printDeck("Prior 2", priorGame.deck2);
                // // Console.ReadLine();

                // // // Console.ReadLine();
                return playARecursiveGame(priorGame.deck1, priorGame.deck2);
            }
            else
            {
                // Console.WriteLine("Returning 1");
                return 1;
            }
        }
        else
        {
            Queue<int> activeDeck1Snapshot = new Queue<int>(activeDeck1);
            Queue<int> activeDeck2Snapshot = new Queue<int>(activeDeck2);
            // printDeck("active 1 snapshot: ", activeDeck1Snapshot);
            // printDeck("active 2 snapshot: ", activeDeck2Snapshot);

            activeGameHistory.Add((activeDeck1Snapshot, activeDeck2Snapshot));

            // // Console.WriteLine($"Length of current history: {activeGameHistory.Count}");

        }

        int card1 = activeDeck1.Dequeue();
        int card2 = activeDeck2.Dequeue();
        if (activeDeck1.Count >= card1 && activeDeck2.Count >= card2)
        {
            //  make a copy of the game, drawn cards, history
            Queue<int> deck1ForSubGame = getDeckForSubgame(card1, activeDeck1);
            Queue<int> deck2ForSubGame = getDeckForSubgame(card2, activeDeck2);
            gameStack.Push((activeDeck1, activeDeck2));
            lastCardsOnTableStack.Push((card1, card2));
            // lastCard1Stack.Push(card1);
            // lastCard2Stack.Push(card2);
            List<(Queue<int>, Queue<int>)> savedHistory = new List<(Queue<int>, Queue<int>)>(activeGameHistory);
            historyStack.Push(savedHistory);
            activeGameHistory.Clear();
            // // Console.WriteLine($"Saving a history of {savedHistory.Count} snapshots");
            // // Console.WriteLine($"Going to subgame with {card1} and {card2}; stacks are now: game {gameStack.Count}, last cards {lastCardsOnTableStack.Count}, history {historyStack.Count}; new history list has {activeGameHistory.Count} entries");
            // printDeck("1 pushed to stack", activeDeck1);
            // printDeck("2 pushed to stack", activeDeck2);
            // // Console.ReadLine();
            return playARecursiveGame(deck1ForSubGame, deck2ForSubGame);
        }
        else
        {
            // // Console.WriteLine($"Playing a regular round with {card1} and {card2}; there are {gameStack.Count} games in the stack");
            playARegularRound(activeDeck1, activeDeck2, card1, card2);
            //  see if that regular round won the game
            if (activeDeck1.Count == 0 || activeDeck2.Count == 0)
            {
                //  who won?
                int winner = activeDeck1.Count > 0 ? 1 : 2;
                if (gameStack.Count > 0)
                {
                    (Queue<int> deck1, Queue<int> deck2) priorGame = gameStack.Pop();
                    (int lastCard1, int lastCard2) lastCards = lastCardsOnTableStack.Pop();
                    // int lastCard1 = lastCard1Stack.Pop();
                    // int lastCard2 = lastCard2Stack.Pop();
                    activeGameHistory.Clear();
                    activeGameHistory = null;
                    activeGameHistory = historyStack.Pop();

                    // // Console.WriteLine("Winner from regular round! Going one game up...");
                    // // Console.WriteLine($"Prior game count: {priorGame.deck1.Count} cards vs {priorGame.deck2.Count}; after a normal round, stacks are now: game {gameStack.Count}, last cards {lastCardsOnTableStack.Count}, history {historyStack.Count}; new history list has {activeGameHistory.Count} entries");
                    // printDeck("Prior 1", priorGame.deck1);
                    // printDeck("Prior 2", priorGame.deck2);
                    if (winner == 1)
                    {
                        priorGame.deck1.Enqueue(lastCards.lastCard1);
                        priorGame.deck1.Enqueue(lastCards.lastCard2);
                    }
                    else
                    {
                        priorGame.deck2.Enqueue(lastCards.lastCard2);
                        priorGame.deck2.Enqueue(lastCards.lastCard1);
                    }
                    // // Console.WriteLine("Adding new cards");
                    // printDeck("Prior 1", priorGame.deck1);
                    // printDeck("Prior 2", priorGame.deck2);
                    // // Console.ReadLine();

                    // // // Console.ReadLine();
                    return playARecursiveGame(priorGame.deck1, priorGame.deck2);
                }
                else
                {
                    return winner;
                }
            }
            else
            {
                return playARecursiveGame(activeDeck1, activeDeck2);
            }
        }
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

    private void scoreTheGame()
    {
        // printDeck("deck1", deck1);
        // printDeck("deck2", deck2);
        Queue<int> winningDeck = deck1.Count > 0 ? deck1 : deck2;
        int score = 0;
        int numCards = winningDeck.Count;
        for (int i = 0; i < numCards; i++)
        {
            // // Console.WriteLine($"score before {score}");
            score += winningDeck.Count * winningDeck.Dequeue();
            // // Console.WriteLine($"score after {score}");
        }
        Console.WriteLine($"Score is {score}");
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