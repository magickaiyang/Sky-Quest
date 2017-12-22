using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechButtons : MonoBehaviour {

    public static GameObject lastObj;

    public Text researchhistory;
    public Text timer;
    public Image timerbox;
    public GameObject blocker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick_Col()
    {
        if (lastObj != null)
            lastObj.GetComponent<Image>().color = Color.white;
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.yellow;
        UIController.techTypeToResearch = 0;
        lastObj = EventSystem.current.currentSelectedGameObject;

        int l1 = 0, l2 = 0, l3 = 0;
        for(int i=0;i<5;i++)
        {
            if (Tech.resTypeTech[0, 0, i])
                l1++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[0, 1, i])
                l2++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[0, 2, i])
                l3++;
        }
        researchhistory.text = "Number of types: L1:"+l1+" L2:"+l2+" L3:"+l3;
    }

    public void OnClick_Popu()
    {
        if (lastObj != null)
            lastObj.GetComponent<Image>().color = Color.white;
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.yellow;
        UIController.techTypeToResearch = 1;
        lastObj = EventSystem.current.currentSelectedGameObject;

        int l1 = 0, l2 = 0, l3 = 0;
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[1, 0, i])
                l1++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[1, 1, i])
                l2++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[1, 2, i])
                l3++;
        }
        researchhistory.text = "Number of types: L1:" + l1 + " L2:" + l2 + " L3:" + l3;
    }

    public void OnClick_Space()
    {
        if (lastObj != null)
            lastObj.GetComponent<Image>().color = Color.white;
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.yellow;
        UIController.techTypeToResearch = 2;
        lastObj = EventSystem.current.currentSelectedGameObject;

        int l1 = 0, l2 = 0, l3 = 0;
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[2, 0, i])
                l1++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[2, 1, i])
                l2++;
        }
        for (int i = 0; i < 5; i++)
        {
            if (Tech.resTypeTech[2, 2, i])
                l3++;
        }

        researchhistory.text = "Number of types: L1:" + l1 + " L2:" + l2 + " L3:" + l3;
    }

    public void OnClick_Blocker()
    {
        blocker.SetActiveRecursively(false);
        timer.enabled = true;
        timerbox.enabled = true;
    }
}
