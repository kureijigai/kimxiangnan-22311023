using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    public UnitData CharacterData;
    public CharacterControl targetData;

    private void Awake()
    {
        CharacterData._charcont = this;
    }

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
 /*   private void Update()
    {//캐릭속도 증가
    
    

    }
 */
}
