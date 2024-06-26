using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public TextMeshProUGUI villagerTypeText;

    public List<Villager> villagerList;

    public static Villager SelectedVillager { get; private set; }

    private void Update()
    {
        if(SelectedVillager == null)
        {
            villagerTypeText.text = "No Villager Selected";
        }
        else
        {
            villagerTypeText.text = SelectedVillager.GetType().Name;
        }
    }

    public static void SetSelectedVillager(Villager villager)
    {
        if(SelectedVillager != null)
        {
            SelectedVillager.Selected(false);
        }
        SelectedVillager = villager;
        SelectedVillager.Selected(true);
    }
    
    public void SelectVillager(int index)
    {
        SetSelectedVillager(villagerList[index]);
    }

    public void ScaleVillager(float scale)
    {
        SelectedVillager.transform.localScale = new Vector3(scale, scale, 1);
    }
}
