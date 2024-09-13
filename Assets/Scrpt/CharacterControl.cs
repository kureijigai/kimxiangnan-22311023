using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public UnitData CharacterData;
    public CharacterControl targetData;

    private void Start()
    {
        CharacterData.Init();


        CharacterData._target = targetData.CharacterData;

        StartCoroutine(CharacterData.CharacterLoop());

      /*  if(true)

        if(CharacterData.charUI.physicUI.Limitbar==null)
        {
            print(gameObject.name);
        }
        if(CharacterData.charUI.physicUI.Timebar==null)
        {
            print(gameObject.name);
        }
      */
    }
    private void Update()
    {//캐릭속도 증가
    
        if(CharacterData.Isreadyforaction)
        {
            Debug.Log("is ready to attack");
            if(CharacterData.charcterteam==CharcterTeam.FRIEND && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("player do a action");
                if(CharacterData._target.CanBeAttacked)
                {
                    CharacterData.Attack();
                }
                
            }
        }

    }
}
