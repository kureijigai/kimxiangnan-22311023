using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UImanager Instance;

    public Transform rowHolder;
    public Transform nameHolder;

    [Header("UI prefabs")]
    public GameObject rowPrefab;
    public GameObject namePrefab;

    [Header("First PlayerUI")]
    public RowUI defaultRowUI;
    public OnclickGeneric firstOnclick;

    public GameObject actionWindow;
    public GameObject abilityWindow;

    [Header("Ability Window")]
    public GameObject abilityUIprefab;
    public Transform abilityUIholder;
    public Text mananeededUI;


    public static int currentUicount=1;

  
    private void Awake()
    {
        Instance = this;

    }
    public void SpawnRow(out RowUI processedUI,UnitData passedData)
    {
        //init row
        GameObject tmpRow = Instantiate(rowPrefab);
        tmpRow.transform.SetParent(rowHolder,false);
        RowUI rowTmpInfo = tmpRow.GetComponent<RowUI>();
        //init name
        GameObject tmpname = Instantiate(namePrefab);
        tmpname.transform.SetParent(nameHolder,false);
        Text txtName=tmpname.GetComponent<Text>();
        OnclickGeneric onClickEvent=tmpname.GetComponent<OnclickGeneric>();


        tmpRow.name = "Character" + tmpRow.transform.childCount;

        rowTmpInfo.characterUI = txtName;
        onClickEvent.charHolder = passedData;

        processedUI = rowTmpInfo;
    }
    public void FillabillityWindow()
    {
        CleanabillityWindow();

        // 창에 내용을 채우는 용->스킬을 가져와서 띄우는 용
      var data = BattleManager.Instance.currentcharcter.CharacterData.characterAbilities;

      for (int i=0;i< data.Count;i++)
        {
            GameObject tmpAbilityPrefab = Instantiate(abilityUIprefab);
            tmpAbilityPrefab.transform.SetParent(abilityUIholder);

            AbilityUI tmpAbUI = tmpAbilityPrefab.GetComponent<AbilityUI>();
            tmpAbUI.abilityindex = i;

            tmpAbUI.Init(data[i].abilityName);
        }
    }

    public void SetmananeededUI(int abilityMana,int charCurMana)
    {
        mananeededUI.text = abilityMana + "/" + charCurMana;
    }
    void CleanabillityWindow()
    {
        foreach (Transform item in abilityUIholder)
        {
            Destroy(item.gameObject); 
        }
    }
}
