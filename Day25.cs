using System;

class Day25
{

    //  my data
    const double doorPubKey = 5099500;
    const double cardPubKey = 7648211;

    //  test data
    // const double doorPubKey = 17807724;
    // const double cardPubKey = 5764801;

    double doorLoopSize = 0;
    double cardLoopSize = 0;


    public Day25() { }

    public void calculate()
    {

        double numLoops = 1;
        double testValue = 1;
        while (true)
        {
            testValue = (testValue * 7) % 20201227;
            if (testValue == doorPubKey)
            {
                doorLoopSize = numLoops;
            }
            else if (testValue == cardPubKey)
            {
                cardLoopSize = numLoops;
            }
            if (doorLoopSize > 0 && cardLoopSize > 0) { break; }
            numLoops++;
        }
        Console.WriteLine($"Door loop is {doorLoopSize} and card loop is {cardLoopSize}");
        double encryptionKey = 1;
        double subjNum = doorLoopSize < cardLoopSize ? cardPubKey : doorPubKey;
        double loopNum = Math.Min(doorLoopSize, cardLoopSize);
        for (double i = 0; i < loopNum; i++)
        {
            encryptionKey = (encryptionKey * subjNum) % 20201227;
        }
        Console.WriteLine($"Encryption key is {encryptionKey}");
    }
}