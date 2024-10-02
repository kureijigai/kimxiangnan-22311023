using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UImanager instance;

    public Transform rowHolder;
    public Transform nameHolder;

    [Header("UI prefabs")]
    public GameObject rowPrefab;
    public GameObject namePrefab;

    [Header("First PlayerUI")]
    public RowUI defaultRowUI;
    public OnclickGeneric firstOnclick;

    public GameObject actionWindow;
    [Header("Ability Window")]
    public GameObject abilityUIholder;
    public GameObject abilityUIprefab;
    public Text manaNeededUI;


    public static int currentUicount=1;

    private void Awake()
    {
        instance = this;
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

    }
    void CleanabillityWindow()
    {

    }
}
