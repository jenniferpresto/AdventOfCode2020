using System;
using System.Collections.Generic;

class Day99
{
    string data = "952316487"; // my data
    // string data = "389125467"; // test data
    CircularLinkedList cupCircle = new CircularLinkedList();
    const int NUM_CUPS = 1000000;
    const int NUM_MOVES = 10000000;

    Dictionary<int, Node> allCupsDictionary = new Dictionary<int, Node>();

    public void calculate()
    {
        foreach (char c in data)
        {
            int value = int.Parse(c.ToString());
            Node n = cupCircle.addNewNodeAfterCurrentAndMoveCurrent(value);
            allCupsDictionary.Add(value, n);
        }

        if (NUM_CUPS > data.Length)
        {
            for (int i = data.Length; i < NUM_CUPS; i++)
            {
                Node n = cupCircle.addNewNodeAfterCurrentAndMoveCurrent(i + 1);
                allCupsDictionary.Add(i + 1, n);
            }
        }

        int firstCup = int.Parse(data[0].ToString());
        cupCircle.current = allCupsDictionary[firstCup];
        cupCircle.printFirstTwentyNodes();

        Console.WriteLine(DateTime.Now);
        for (int i = 0; i < NUM_MOVES; i++)
        {
            doAMove();
        }
        cupCircle.current = allCupsDictionary[1];
        Console.WriteLine("First twenty Final cups");
        cupCircle.printFirstTwentyNodes();
        long firstCupValue = allCupsDictionary[1].next.value;
        long secondCupValue = allCupsDictionary[Convert.ToInt32(firstCupValue)].next.value;
        Console.WriteLine($"Product of first two cups: {firstCupValue * secondCupValue}");
        Console.WriteLine(DateTime.Now);
    }

    public void doAMove()
    {
        //  next three cups
        Node cup1 = cupCircle.current.next;
        Node cup2 = cup1.next;
        Node cup3 = cup2.next;

        //  get destination value
        int destValue = cupCircle.current.value - 1;

        //  get value of what will be the next current cup
        //  then link current cup to that one
        int nextCurrentCupValue = cup3.next.value;
        cupCircle.current.next = cup3.next;

        for (int i = 0; i < 3; i++)
        {
            bool foundCup = true;
            if (destValue < 1)
            {
                destValue += NUM_CUPS;
            }

            if (destValue == cup1.value || destValue == cup2.value || destValue == cup3.value)
            {
                destValue--;
                foundCup = false;
            }
            if (foundCup) { break; }
        }

        //  insert the cups
        //  setting the current value here prevents iteration through potentially 999,999 cups
        cupCircle.current = allCupsDictionary[destValue];
        cupCircle.insertThreeCupsAfterDestinationCup(cup1, cup2, cup3, destValue);

        //  then use dictionary to set current one back to value saved at beginning of method
        cupCircle.current = allCupsDictionary[nextCurrentCupValue];
    }
}

class CircularLinkedList
{
    public Node current;
    public CircularLinkedList()
    {
        current = null;
    }

    public Node addNewNodeAfterCurrentAndMoveCurrent(int value)
    {
        Node n = new Node(value);
        //  empty list
        if (current == null)
        {
            current = n;
        }
        //  one node in list
        else if (current.next == null)
        {
            current.next = n;
            n.next = current;
            current = n;
        }
        //  more items in list
        else
        {
            n.next = current.next;
            current.next = n;
            current = n;
        }

        return n;
    }

    public void iterateCurrentByOne()
    {
        this.current = this.current.next;
    }

    public void setCurrentNodeToValue(int v)
    {
        if (this.current.value == v)
        {
            Console.WriteLine("We're already there");
            return;
        }

        //  NB: Potentially very lengthy process
        while (this.current.value != v)
        {
            current = current.next;
        }
    }

    public void insertThreeCupsAfterDestinationCup(Node cup1, Node cup2, Node cup3, int value)
    {
        Node n = this.current;
        //  NB: if current value not set beforehand, this is potentially lengthy process
        while (true)
        {
            if (n.value == value)
            {
                cup3.next = n.next;
                n.next = cup1;
                break;

            }
            n = n.next;
        }
    }
    public void printFullList()
    {
        if (current == null)
        {
            Console.WriteLine("The list is empty");
            return;
        }
        Node n = current;
        Console.Write($"{current.value},");
        current = current.next;
        while (current != n)
        {
            Console.Write($"{current.value},");
            current = current.next;
        }
        Console.WriteLine();
    }

    public void printFirstTwentyNodes()
    {
        Node n = current;
        for (int i = 0; i < 20; i++)
        {
            if (current == null)
            {
                Console.WriteLine("Empty list");
                return;
            }
            Console.Write($"{n.value},");
            n = n.next;
            if (n == current) break;
        }
        Console.WriteLine();
    }
}

public class Node
{
    public readonly int value;
    public Node next;
    public Node(int v)
    {
        this.value = v;
        this.next = null;
    }
}
