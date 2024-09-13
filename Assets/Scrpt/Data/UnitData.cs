using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class UnitData
{
    public string charname="CharacterName";
    [Space(10)]
    public CharacterUIData charUI;
    [Space(10)]
    public int maxhp=100;
    public int currenthp=100;

    [Space(10)]
    public int maxmana=100;
    public int currentmana=100;
    [Space(10)]
    public float limitburstpoint=100;
    public float currentBpoint=100;
    [Space(10)]
    public float speedlimit=10;
    public float currentspeed=10;

    [Space(10)]
    public CharcterState charcterState;
    public CharcterTeam charcterteam;

    [Space(10)]
    public UnityEvent onAttack;
    public UnityEvent onWasAttacked;
    public bool playerJustAttacked;

    [Space(16)]

    public UnitData _target;

    public void Init()
    {
        
        if(charcterteam==CharcterTeam.FRIEND)
        charUI.init(maxhp,currenthp,maxmana,currentmana,charname,speedlimit,limitburstpoint);

        onAttack.AddListener(characterAttackdefault);
        onWasAttacked.AddListener(characterAttackeddefault);

        charcterState = CharcterState.IDEL;
    }

    public void Attack()
    {
        onAttack.Invoke();

        //temp attack
        _target.Damage(10);
     //   Debug.Log("target was attacked");

        if(charcterteam==CharcterTeam.FRIEND)
        {
            IncreaseLB(5);
        }
    }


    public void Damage(int damageAmount)
    {
        currenthp -= damageAmount;

      

        if (currenthp<=0)
        {
            currenthp = 0;
            charcterState = CharcterState.DIED;
        }
        //handlingLB
        if(charcterteam==CharcterTeam.FRIEND)
        {
            IncreaseLB(5);
            charUI.UpdateHealthBar(currenthp,maxhp);
        }
     
        onWasAttacked.Invoke();
    }
    public bool CanBeAttacked
    {
        get
        {
            return charcterState == CharcterState.IDEL || charcterState == CharcterState.READY; ;
        }
    }
    public bool CanattackTarget
    {
        get
        {
            return _target.charcterState == CharcterState.IDEL || _target.charcterState == CharcterState.READY;
        }
    }
    public bool Isreadyforaction
    {
        get{
            
            return currentspeed >= speedlimit;
            }
       
    }
    void IncreaseLB(int amount)
    {
        currentBpoint += amount;
        currentBpoint = Mathf.Clamp(currentBpoint, 0, limitburstpoint);

        charUI.UpdateLimitBar(currentBpoint);
    }
    void characterAttackdefault()
    {
        playerJustAttacked = false;
        currentspeed = 0;
    }

    void characterAttackeddefault()
    {
        Debug.Log(charname + "was attacked"); 
    }
 /*   public IEnumerator CharacterBehaviour()
   {
       //베이스캐릭 타겟



       yield return new WaitUntil(() =>CanattackTarget);

       //캐릭터가 액션을 취할때까지 기다림

       yield return new WaitUntil(() => playerJustAttacked);

       onAttack.Invoke();
   }*/

 public IEnumerator CharacterLoop()
   {
       while(charcterState !=CharcterState.DIED)
       {
           if (currentspeed >= speedlimit)
           {
              currentspeed = speedlimit;
                charcterState = CharcterState.READY;
           }
           else
           {
              currentspeed += Time.deltaTime;

                if(charcterteam==CharcterTeam.FRIEND)
                {
                    charUI.UpdateTimeBar(currentspeed);
                }

                charUI.UpdateTimeBar(currentspeed);
                charcterState = CharcterState.IDEL;
           }
           yield return null;
       }

   }
}
[System.Serializable]
public class CharacterUIData
{
    public RowUI physicUI;
    public int placeInUI = 1;

    

    public void init(int maxHP,int curHP,int maxMP,int curMP,string charName,float speedLimit,float limitMax)
    { 
        placeInUI=UImanager.currentUicount;

        if (placeInUI == 1)
        {
            //Do not want to spawn another rouw and use the first one.
            physicUI = UImanager.instance.defaultRowUI;
        }
        else
        {
            //generate spaeing of the row
            UImanager.instance.SpawnRow(out physicUI);
        }
      
        //health setup
        physicUI.HPbar.maxValue = maxHP;
        UpdateHealthBar(curHP, maxHP);
        //mpsetup
        physicUI.MPbar.maxValue = maxMP;
        physicUI.MPbar.value = curMP;
        physicUI.MPUI.text = curMP.ToString() + "/" + maxMP.ToString();

        //charinfo setup
        physicUI.characterUI.text = charName;

        //limit & speed bar Setup
        physicUI.Limitbar.maxValue = limitMax;
        physicUI.Limitbar.value = 0;

        physicUI.Timebar.maxValue = speedLimit;
        physicUI.Timebar.value = 0;

        UImanager.currentUicount++;
    }

    public void UpdateTimeBar(float currentProg)
    {
        physicUI.Timebar.value=currentProg;
    }
    public void UpdateLimitBar(float currentProg)
    {
    physicUI.Limitbar.value=currentProg;
    }

    public void UpdateHealthBar(int currentAmount,int maxAmount)
    {
        physicUI.HPbar.value=currentAmount;
        physicUI.HPUI.text= currentAmount.ToString() + "/" + maxAmount.ToString();
    }

    public void UpdateMpBar(int currentAmout,int maxAmount)
    {
        physicUI.MPbar.value=currentAmout;

    }
}
public enum CharcterTeam
{
    FRIEND,
    ENEMY,
}
public enum CharcterState
{    LOADING,
    IDEL,
    READY,
    ATTACKED,
    ATTACKING,
    DIED,
}