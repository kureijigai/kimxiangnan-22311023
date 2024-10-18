using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AbilityUI : MonoBehaviour, IPointerDownHandler
{
  
    public int abilityindex=0;

    public Text abilityText;

    bool isSelected;
    public void Init(string abilityName)
    {
        abilityText.text = abilityName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var charData = BattleManager.Instance.currentcharcter.CharacterData;

        for(int i=0;i<charData.characterAbilities. Count;i++)
        {
            if(abilityindex.Equals(i))
            {
                if(isSelected)
                {
                    BattleManager.Instance.currentcharcter.CharacterData.Attack(charData.characterAbilities[i]);
                    isSelected = false;
                }
                else
                {
                    UImanager.Instance.SetmananeededUI(charData.characterAbilities[i].mpCost, charData.currentmana);
                    isSelected = true;
                }
              
                break;
            }
        }
    }

   
}
