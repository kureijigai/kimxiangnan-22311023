using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public CharacterControl currentcharcter;

    public static BattleManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void SelectCharacter(UnitData newChar)
    {
        newChar.SelectCharacter();
    }

    public void DoBasicAttackOnTarget()
    {
        if (currentcharcter.CharacterData.Isreadyforaction)
        {
            Debug.Log("is ready to attack");
            if (currentcharcter.CharacterData.charcterteam == CharcterTeam.FRIEND)
            {
                Debug.Log("player do a action");
               if (currentcharcter.CharacterData._target.CanBeAttacked)
                {
                    currentcharcter.CharacterData.Attack(currentcharcter.CharacterData.basicAttack);
                }

            }
        }
       
    }
}
