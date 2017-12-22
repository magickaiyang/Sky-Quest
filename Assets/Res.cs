using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class Res
{
    public static bool[,] resTypeCollector = new bool[5, 5];  //[collector type, res type] 
    public static int[,] resAmountCollector = new int[5, 5];  //[collector type, res amount]

    public static int[] numCollector = new int[5];  //0 A, 1 B,...

    public static int[] numRes = new int[5]; //0 A, 1 B,...

    // Use this for initialization
    public Res()  //generate data
    {
        System.Random r = new System.Random();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                resAmountCollector[i, j] = r.Next(Constants.RES_MIN, Constants.RES_MAX);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int temp = r.Next(0, 2);
                if (temp == 0)
                    resTypeCollector[i, j] = false;
                else
                    resTypeCollector[i, j] = true;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (resTypeCollector[i, 0] == false && resTypeCollector[i, 1] == false && resTypeCollector[i, 2] == false && resTypeCollector[i, 3] == false && resTypeCollector[i, 4] == false)//just in case every res type is false!!
                resTypeCollector[i, 3] = true;  //we set res type D to be true 
        }

        //set initial numcollector to 1
        for (int i = 0; i < 5; i++)
        {
            numCollector[i] = 1;
        }
    }

    public bool AddCollector(int type) //build one new ("type" of) collector 
    {
        for (int i = 0; i < 5; i++)
        {
            if (resTypeCollector[type, i])
                if (numRes[i] - resAmountCollector[type, i] < 0)
                    return false;  //not enough res amount
        }
        for (int i = 0; i < 5; i++)
        {
            if (resTypeCollector[type, i])
                numRes[i] -= resAmountCollector[type, i];
        }
        numCollector[type]++;
        return true;

    }

    public bool RemoveCollector(int type)  //num may be zero
    {
        if (numCollector[type] <= 0)
            return false;
        for (int i = 0; i < 5; i++)
        {
            if (resTypeCollector[type, i])
            {
                numRes[i] += (int)(resAmountCollector[type, i]*0.9);
            }
        }
        numCollector[type]--;

        return true;
    }

    public void UpdateRes()  //called once per second
    {
        for (int i = 0; i < 5; i++)
        {
            double temp = Constants.CollectBaseSpeed * numCollector[i];
            switch (Tech.techStatus[0])
            {
                case Tech.LEVEL.L0:
                    break;
                case Tech.LEVEL.L1:
                    temp *= Constants.CollectL1Factor;
                    break;
                case Tech.LEVEL.L2:
                    temp *= Constants.CollectL2Factor;
                    break;
                case Tech.LEVEL.L3:
                    temp *= Constants.CollectL3Factor;
                    break;
            }
            //!!population consume here!!======================================
            temp -= UIController.populationNum * Constants.P_consume_per_s;

            numRes[i] += (int)temp;

            if(numRes[i]<-1)  //starve to death!!!
            {
                Timer.endGame(1);
            }
        }
    }
}