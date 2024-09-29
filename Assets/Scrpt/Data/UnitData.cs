using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using static Unity.Burst.Intrinsics.X86.Avx;

[System.Serializable]
public class UnitData
{
    public string charname="CharacterName";
    [Space(10)]
    public abilityData basicAttack;
    public List<abilityData> characterAbilities;

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
    public UnityEvent onJustReady;
    public bool playerJustAttacked;

    [Space(16)]

    public UnitData _target;

    [HideInInspector]
    public CharacterControl _charcont;
 //   bool wasJustReady;

    public void Init()
    {
        
        if(charcterteam==CharcterTeam.FRIEND)
        {
            charUI.charData = this;
            charUI.init(maxhp, currenthp, maxmana, currentmana, charname, speedlimit, limitburstpoint);
            onJustReady.AddListener(onReadyDefault);
        }

        onAttack.AddListener(characterAttackdefault);
        onWasAttacked.AddListener(characterAttackeddefault);


     

        charcterState = CharcterState.IDEL;
    }

    public void Attack(abilityData ability)
    {
        if (charcterState == CharcterState.DIED) return;

        onAttack.Invoke();

        //temp attack
        switch(ability.output)
        {
            case AbilityOutput.DAMAGE:
                _target.Damage(ability.abValue);
                break;
            case AbilityOutput.HEAL:
                _target.Heal(ability.abValue);
                break;
             
        }
       
        //   Debug.Log("target was attacked");

        if (charcterteam==CharcterTeam.FRIEND)
        {
            IncreaseLB(5);
        }
        charcterState = CharcterState.ATTACKING;
    }

    public void Heal(int healamount)
    {
       currenthp=Mathf.Clamp(currenthp+healamount,0,_target.maxhp);
        charUI.UpdateHealthBar(currenthp, maxhp);
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

void onReadyDefault()
    {
        SelectCharacter();
    }
    public void SelectCharacter()
    {
        if (!Isreadyforaction)
            return;

        UImanager.instance.actionWindow.SetActive(true);

        foreach (var item in GameObject.FindObjectsOfType<CharacterControl>())
        {
            if (item.CharacterData.charcterteam != CharcterTeam.ENEMY)
                item.CharacterData.ResetUINameText();
        }

        charUI.physicUI.characterUI.color = Color.cyan;
        BattleManager.Instance.currentcharcter = _charcont;
    }

   public void ResetUINameText()
    { 
        charUI.physicUI.characterUI.color = Color.white;

    }
 public IEnumerator CharacterLoop()
   {
         while(charcterState !=CharcterState.DIED)
       {
         while(currentspeed<speedlimit)
            {
                currentspeed += Time.deltaTime;
                if (charcterteam == CharcterTeam.FRIEND)
                {
                    charUI.UpdateTimeBar(currentspeed);
                }
                
                if(charcterState==CharcterState.ATTACKED)
                {
                    charcterState = CharcterState.IDEL;
                }
                yield return null;
            }
         //we are ready
            currentspeed = speedlimit;
            charcterState = CharcterState.READY;
            onJustReady.Invoke();

            yield return new WaitUntil(() => charcterState == CharcterState.ATTACKING);

            //TODO: :Change this to ues animate events
            charcterState = CharcterState.IDEL;

            yield return new WaitUntil(() => charcterState == CharcterState.IDEL);


        }
    }
}
[System.Serializable]
public class CharacterUIData
{
    public RowUI physicUI;
    public int placeInUI = 1;

    [HideInInspector]
    public UnitData charData;

    public void init(int maxHP,int curHP,int maxMP,int curMP,string charName,float speedLimit,float limitMax)
    { 
        placeInUI=UImanager.currentUicount;

        if (placeInUI == 1)
        {
            //Do not want to spawn another rouw and use the first one.
            physicUI = UImanager.instance.defaultRowUI;
            UImanager.instance.firstOnclick.charHolder = charData;
        }
        else
        {
            //generate spaeing of the row
            UImanager.instance.SpawnRow(out physicUI,charData);
        }
      
        //health setup
        physicUI.HPbar.maxValue = maxHP;
        UpdateHealthBar(curHP, maxHP);
        //mpsetup
        physicUI.MPbar.maxValue = maxMP;
        UpdateMpBar(curMP);

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

    public void UpdateMpBar(int currentAmount)
    {
        physicUI.MPbar.value=currentAmount;
        physicUI.MPUI.text = currentAmount.ToString();
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