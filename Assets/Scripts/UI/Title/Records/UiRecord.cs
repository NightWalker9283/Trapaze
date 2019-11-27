using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UiRecord : MonoBehaviour
{
    [SerializeField] ToggleGroup tggModes;
    [SerializeField] GameObject ModeRecords, GeneralRecords;
    int oldSelectedId;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnChangeValueInToggleGroup(Toggle change)
    {
        var selected = tggModes.ActiveToggles().First();
        var selectedId = (selected!=null?selected.GetComponent<ModeElementForRecords>().id:0);
        if (selectedId == 0)
        {
            
            GeneralRecords.SetActive(true);
            ModeRecords.SetActive(false);
        }
        else
        {
            GeneralRecords.SetActive(false);
            ModeRecords.SetActive(true);
        }




        oldSelectedId = selectedId;
    }

   
}
