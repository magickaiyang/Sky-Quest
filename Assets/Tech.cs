using Assets;
using System.Collections;
using System.Collections.Generic;

public class Tech
{
    public enum LEVEL { L0, L1, L2, L3 };
    public static LEVEL[] techStatus = new LEVEL[] { LEVEL.L0, LEVEL.L0, LEVEL.L0 };  //0 collecting, 1 people, 2 spacecraft

    public static bool[,,] resTypeTech = new bool[3, 3, 5];  //[tech, level, res type]
    public static int[,,] resAmountTech = new int[3, 3, 5];  //[tech, level, res amount]

    public int DoResearch(int type, bool[] selection)  //0 for success, 1 for not enough res, -1 for wrong type
    {
        int currentLevel = -1;  //-1 for L0
        switch (techStatus[type])
        {
            case LEVEL.L0:
                currentLevel = -1;
                break;
            case LEVEL.L1:
                currentLevel = 0;
                break;
            case LEVEL.L2:
                currentLevel = 1;
                break;
            case LEVEL.L3:
                currentLevel = 2;
                break;
        }
        int toLevel = currentLevel + 1;
        for (int i = 0; i < 5; i++)  //compare res types
        {
            if (resTypeTech[type, toLevel, i] != selection[i])  //wrong
            {
                for (int j = 0; j < 5; j++)
                {
                    Res.numRes[j] -= Constants.Punishment[toLevel];  //deduction of all res by punishment amounts
                }
                return -1;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (resTypeTech[type, toLevel, i])  //do we need this kind of res?
            {
                if (Res.numRes[i] - resAmountTech[type, toLevel, i] < 0)
                    return 1;  //not enough res
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (resTypeTech[type, toLevel, i])  //need this kind of res
            {
                Res.numRes[i] -= resAmountTech[type, toLevel, i];
            }
        }

        techStatus[type] = techStatus[type] + 1;// next level
        return 0;  //success
    }

    // Use this for initialization
    public Tech()
    {
        System.Random r = new System.Random();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    resAmountTech[i, j, k] = r.Next(Constants.RES_MIN, Constants.RES_MAX);
                }
            }
        }

        //special spacecraft tech resAmount!
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 5; k++)
            {
                resAmountTech[2, j, k] = r.Next(5000, 10000);
            }
        }

        //for testing purpose
        //resTypeTech = new bool[3, 3, 5] { { {true,true,false,false,false },{true,false,false,false,false },{false,true,true,true,false } },{ {true,true,true,true,true },{true,false,false,true,true },{true,true,false,true,true } },{ {true,true,false,true,false },{true,false,true,false,true },{false,false,true,true,true } } };
        
        
          for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    int temp = r.Next(0, 2);
                    if (temp == 0)
                        resTypeTech[i, j, k] = false;
                    else
                        resTypeTech[i, j, k] = true;
                }
                if (resTypeTech[i, j, 0] == false && resTypeTech[i, j, 1] == false && resTypeTech[i, j, 2] == false && resTypeTech[i, j, 3] == false && resTypeTech[i, j, 4] == false)
                    resTypeTech[i, j, 1] = true;  //we need to require at least one type of res
            }
        }
    }
}
