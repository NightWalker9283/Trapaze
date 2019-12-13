using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WndTitles : MonoBehaviour
{
    [SerializeField] ToggleGroup tggTitles;
    [SerializeField] Text txtNameTitle, txtConditionTitle, txtDescriptionTitle;

    Titles titles;
    // Start is called before the first frame update
    void Start()
    {
        titles = GameMaster.gameMaster.titles;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateTitleInfo(bool isOn)
    {
        if (isOn)
        {
           
            var id = tggTitles.ActiveToggles().FirstOrDefault().GetComponent<ListElementTitle>().id;
            var titleObject = titles.allTitles.Find(dt => dt.id == id);
            if (titleObject != null)
            {
                txtNameTitle.text = titleObject.name;
                txtConditionTitle.text = titleObject.condition;
                txtDescriptionTitle.text = titleObject.description;
            }
        }
    }
}
