using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AbilityUI : MonoBehaviour, IPointerDownHandler
{
  
    public int abilityindex=0;

    public Text abilityText;

    
    public void Init(string abilityName)
    {
        abilityText.text = abilityName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var charData = BattleManager.Instance.currentcharcter.CharacterData.characterAbilities;
        for(int i=0;i<charData.Count;i++)
        {
            if(abilityindex.Equals(i))
            {
                BattleManager.Instance.currentcharcter.CharacterData.Attack(charData[i]);
                break;
            }
        }
    }
}
