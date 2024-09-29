using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnclickGeneric : MonoBehaviour, IPointerDownHandler{

    public UnitData charHolder;

    public void OnPointerDown(PointerEventData eventData)
    {
        BattleManager.Instance.SelectCharacter(charHolder);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
