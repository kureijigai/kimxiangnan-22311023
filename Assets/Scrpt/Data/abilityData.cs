 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class abilityData
{
    public string abilityName="AbilityOne";

    public int abValue=10;
    public int mpCost=1;

    public AbilityType type=AbilityType.MELEE;
    public AbilityOutput output=AbilityOutput.DAMAGE;


    //Todo:patical
    
}
public enum AbilityType
{
    RANGED,
    MELEE
}
public enum AbilityOutput
{
    DAMAGE,
    HEAL
}
