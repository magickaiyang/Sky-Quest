using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets;

public class UIController : MonoBehaviour
{
    public Res resources;
    DateTime lastCall = DateTime.MinValue;

    public Text population;
    public Text populationRate;

    public static double populationNum = 1.0;
    public static double populationRateNum = 0.01;

    public Text[] resourceTracker = new Text[5];
    public Text[] rateTracker = new Text[5];

    public Text researchHistory;
    public Text researchResults;

    private static Tech tech;
    public Button[] techLevelButtons = new Button[9];

    public static int techTypeToResearch;

    public static DateTime techStart;
    public static int techTimeToWait;

    public string researchString;
    public bool[] selection = new bool[5];

    public GameObject tutorial;

    // Use this for initialization
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Universe", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        resources = new Res();
        tech = new Tech();
        researchQueue = new ResourceSequence(this, 5);

        disableDeleteButton();
        disableMakeButton();
        disableTechButtons();
        researchHistory.text = "";
        researchResults.text = "";
        population.text = "";
        populationRate.text = "";

        for (int i = 0; i < resourceTracker.Length; i++)
        {
            resourceTracker[i].text = "";
            rateTracker[i].text = "";
        }
    }

    public void disableTechButtons()
    {
        foreach (Button b in techLevelButtons)
            b.interactable = false;
    }

    public void hideTutorial()
    {
        tutorial.SetActiveRecursively(false);
    }

    public void tweakTechButton_Popu_Status()
    {
        int[] currentLevels = new int[3];  //col, pop, space
        for (int i = 0; i < 3; i++)
        {
            switch (Tech.techStatus[i])
            {
                case Tech.LEVEL.L0:
                    currentLevels[i] = -1;
                    break;
                case Tech.LEVEL.L1:
                    currentLevels[i] = 0;
                    break;
                case Tech.LEVEL.L2:
                    currentLevels[i] = 1;
                    break;
                case Tech.LEVEL.L3:
                    currentLevels[i] = 2;
                    break;
            }
        }

        for (int i=0;i<3;i++)  //col
        {
            if (currentLevels[0] >= i)
            {
                techLevelButtons[i].interactable = false;  //already achieved
            }
            else if (populationNum>i)
            {
                if (currentLevels[0] - i == -1)
                    techLevelButtons[i].interactable = true;  //popu allow
            }
            else
            {
                techLevelButtons[i].interactable = false;  //popu too low
            }
        }

        for (int i = 3; i < 6; i++)  //popu
        {
            if (currentLevels[1] >= (i-3))
            {
                techLevelButtons[i].interactable = false;  //already achieved
            }
            else if (populationNum > (i - 3))
            {
                if(currentLevels[1]-(i-3)==-1)
                    techLevelButtons[i].interactable = true;  //popu allow
            }
            else
            {
                techLevelButtons[i].interactable = false;  //popu too low
            }
        }

        for (int i = 6; i < 9; i++)  //space
        {
            if (currentLevels[2] >= (i - 6))
            {
                techLevelButtons[i].interactable = false;  //already achieved
            }
            else if (populationNum > (i - 6))
            {
                if (currentLevels[2] - (i - 6) == -1)
                    techLevelButtons[i].interactable = true;  //popu allow
            }
            else
            {
                techLevelButtons[i].interactable = false;  //popu too low
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //keyboardScan();  //no keyboard operation for now

        //This calls 1 time / second
        DateTime now = DateTime.Now;
        TimeSpan s = now - lastCall;
        if (s.Seconds < 1) return;

        lastCall = now;

        resources.UpdateRes();

        double temp = 1;
        switch (Tech.techStatus[1])  //population
        {
            case Tech.LEVEL.L1:
                temp = Constants.PopL1Factor;
                break;
            case Tech.LEVEL.L2:
                temp = Constants.PopL2Factor;
                break;
            case Tech.LEVEL.L3:
                temp = Constants.PopL3Factor;
                break;
        }
        populationRateNum = 0.01;
        populationRateNum *= temp;
        populationNum += populationRateNum;

        for (int i = 0; i < 5; i++)
        {
            resourceTracker[i].text = Res.numRes[i].ToString();

            double cRate = Constants.CollectBaseSpeed * Res.numCollector[i]- populationNum * Constants.P_consume_per_s;
            switch (Tech.techStatus[0])
            {
                case Tech.LEVEL.L0:
                    break;
                case Tech.LEVEL.L1:
                    cRate *= Constants.CollectL1Factor;
                    break;
                case Tech.LEVEL.L2:
                    cRate *= Constants.CollectL2Factor;
                    break;
                case Tech.LEVEL.L3:
                    cRate *= Constants.CollectL3Factor;
                    break;
            }

            string rateString = cRate+"/s";
            rateTracker[i].text = rateString;
        }


        population.text = System.String.Format("{0:F2}", populationNum) + "K people";  //2 digit only
        populationRate.text = populationRateNum.ToString() + "K people/s";

        tweakTechButton_Popu_Status();
        techButtonsColor();

        TimeSpan ts = now - techStart;
        if (techStart != DateTime.MinValue &&ts.Seconds>=techTimeToWait)
        {
            int flag = tech.DoResearch(techTypeToResearch, selection);
            string result;
            if (flag == 1)
                result = "Failed: Not Enough Resources";
            else if (flag == 0)
                result = "Succeeded";
            else
                result = "Failed: Wrong Types";
            researchHistory.text = researchString;
            researchResults.text = result;
            techStart = DateTime.MinValue;
            for(int i=0;i<5;i++)
            {
                selection[i] = false;
            }
        }
    }

    public void techButtonsColor()
    {
        switch (Tech.techStatus[0])
        {
            case Tech.LEVEL.L1:
                techLevelButtons[0].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L2:
                techLevelButtons[1].GetComponent<Image>().color = Color.green;
                techLevelButtons[0].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L3:
                techLevelButtons[2].GetComponent<Image>().color = Color.green;
                //a quick fix
                techLevelButtons[0].GetComponent<Image>().color = Color.green;
                techLevelButtons[1].GetComponent<Image>().color = Color.green;
                break;
        }

        switch (Tech.techStatus[1])
        {
            case Tech.LEVEL.L1:
                techLevelButtons[3].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L2:
                techLevelButtons[3].GetComponent<Image>().color = Color.green;
                techLevelButtons[4].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L3:
                techLevelButtons[5].GetComponent<Image>().color = Color.green;
                //a quick fix
                techLevelButtons[3].GetComponent<Image>().color = Color.green;
                techLevelButtons[4].GetComponent<Image>().color = Color.green;
                break;
        }

        switch (Tech.techStatus[2])
        {
            case Tech.LEVEL.L1:
                techLevelButtons[6].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L2:
                techLevelButtons[7].GetComponent<Image>().color = Color.green;
                techLevelButtons[6].GetComponent<Image>().color = Color.green;
                break;
            case Tech.LEVEL.L3:
                techLevelButtons[8].GetComponent<Image>().color = Color.green;
                //a quick fix
                techLevelButtons[6].GetComponent<Image>().color = Color.green;
                techLevelButtons[7].GetComponent<Image>().color = Color.green;
                break;
        }
    }

    public void buildCollector(string s)
    {
        if (s.Length != 1)
        {
            print("Error: string must be a char!");
        }

        char c = s.ToCharArray()[0];
        resources.AddCollector(c - 'A');
    }

    public void destroyCollector(string s)
    {
        if (s.Length != 1)
        {
            print("Error: string must be a char!");
        }

        char c = s.ToCharArray()[0];
        resources.RemoveCollector(c - 'A');
    }

    public void startResearching()
    {
        if (researchQueue.length() == 0) return;
        researchString = researchQueue.getResearchString();
        researchQueue.clear();
        if (researchString.Contains("A"))
            selection[0] = true;
        if (researchString.Contains("B"))
            selection[1] = true;
        if (researchString.Contains("C"))
            selection[2] = true;
        if (researchString.Contains("D"))
            selection[3] = true;
        if (researchString.Contains("E"))
            selection[4] = true;

        //wait for some time
        techStart = DateTime.Now;
        switch(Tech.techStatus[techTypeToResearch])
        {
            case Tech.LEVEL.L0:
                techTimeToWait = Constants.L1time;
                break;
            case Tech.LEVEL.L1:
                techTimeToWait = Constants.L2time;
                break;
            case Tech.LEVEL.L2:
                techTimeToWait = Constants.L3time;
                break;
        }
        researchResults.text = "Wait";
    }
    //-----------------------------

    void keyboardScan()
    {
        /*Keybindings:
        A       -> resource A
        S       -> resource B
        D       -> resource C
        F       -> resource D
        G       -> resource E
        space   -> Make
            + shift: Undo

        if shift is held as well, it will instead destroy a resource builder
        if alt   is held as well, it will instead build   a resource builder
        nothing happens if both are held down
        */
        if (!Input.anyKeyDown) return;

        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        if (shift && alt) return;

        bool A = Input.GetKeyDown(KeyCode.A);
        bool B = Input.GetKeyDown(KeyCode.S);
        bool C = Input.GetKeyDown(KeyCode.D);
        bool D = Input.GetKeyDown(KeyCode.F);
        bool E = Input.GetKeyDown(KeyCode.G);
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (!(A || B || C || D || E || space)) return;

        if (shift)
        {
            if (A) resources.RemoveCollector('A');
            if (B) resources.RemoveCollector('B');
            if (C) resources.RemoveCollector('C');
            if (D) resources.RemoveCollector('D');
            if (E) resources.RemoveCollector('E');
            if (space) researchQueue.popResource();
            return;
        }
        else if (alt)
        {
            if (A) resources.AddCollector('A');
            if (B) resources.AddCollector('B');
            if (C) resources.AddCollector('C');
            if (D) resources.AddCollector('D');
            if (E) resources.AddCollector('E');
            return;
        }
        else
        {
            if (A) researchQueue.useResource("A");
            if (B) researchQueue.useResource("B");
            if (C) researchQueue.useResource("C");
            if (D) researchQueue.useResource("D");
            if (E) researchQueue.useResource("E");
            if (space) startResearching();
            return;
        }
    }



    public Button deleteResourceButton;
    public Button makeButton;
    public Button[] resourcesButtons;

    public ResourceSequence researchQueue;

    public void disableResourceButtons()
    {
        foreach (Button b in resourcesButtons)
            b.interactable = false;
    }

    public void disableDeleteButton()
    {
        deleteResourceButton.interactable = false;
    }

    public void enableResourceButtons()
    {
        foreach (Button b in resourcesButtons)
            b.interactable = true;
    }

    public void enableDeleteButton()
    {
        deleteResourceButton.interactable = true;
    }

    public void enableMakeButton()
    {
        makeButton.interactable = true;
    }

    public void disableMakeButton()
    {
        makeButton.interactable = false;
    }

    public static void copy(char[] a, ref char[] b, int number)
    {
        for (int i = 0; ; i++)
            b[i] = a[i];
    }

    public void useResource(string s)
    {
        researchQueue.useResource(s);
    }

    public void popResource()
    {
        researchQueue.popResource();
    }

    public void updateResButtons()
    {
        string researchString = researchQueue.getResearchString();
        researchHistory.text = researchString;
    }

    public void clearTechResult()
    {
        researchResults.text = "";
    }
}









//For research
public class ResourceSequence
{
    UIController ui;
    public static int maxResources = 5;
    private char[] list;
    private int len = 0;

    public int researchCost = 1;

    public ResourceSequence(UIController u, int max)
    {
        ui = u;

        maxResources = max;
        list = new char[maxResources];

        for (int i = 0; i < maxResources; i++)
        {
            list[i] = '\0';
        }
    }

    public int length() { return len; }

    public void useResource(string s)
    {
        if (s.Length == 0)
            return;

        if (s.Length != 1)
        {
            MonoBehaviour.print("Error: useResource string should be 1 long");
            return;
        }

        char[] a = s.ToCharArray();
        useResource(a[0]);
    }

    public void popResource()
    {
        if (len == 0) return;
        else if (len == 1)
        {
            ui.disableDeleteButton();
            ui.disableMakeButton();
        }
        else if (len == maxResources)
            ui.enableResourceButtons();

        list[--len] = '\0';
        ui.updateResButtons();
    }


    void useResource(char c)
    {
        if (len == maxResources) return;

        //prevent repeated chars
        for (int i = 0; i < 5; i++)
        {
            if (list[i] == c)
                return;
        }

        if (len == maxResources - 1)
            ui.disableResourceButtons();
        else if (len == 0)
        {
            ui.enableDeleteButton();
            ui.enableMakeButton();
            ui.clearTechResult();
        }

        list[len] = c;
        len++;
        ui.updateResButtons();
    }

    public static char[] createSortedArray(char[] a, int len)
    {
        char[] ret = new char[len];
        for (int i = 0; i < len; i++)
            ret[i] = a[i];

        for (bool noChangeMade = false; !noChangeMade;)
        {
            noChangeMade = true;
            for (int j = 0; j < len - 1; j++)
            {
                if (ret[j] > ret[j + 1])
                {
                    swap(ref ret, j, j + 1);
                    noChangeMade = false;
                }
            }
            len--;
        }
        return ret;
    }

    public string getResearchString()
    {
        return new string(createSortedArray(list, len));
    }


    static void swap(ref char[] a, int i, int j)
    {
        char c = a[i];
        a[i] = a[j];
        a[j] = c;
    }

    public void clear()
    {
        for (int i = len - 1; i >= 0; i--)
            list[i] = '\0';
        len = 0;

        ui.disableDeleteButton();
        ui.disableMakeButton();
        ui.enableResourceButtons();
    }
}