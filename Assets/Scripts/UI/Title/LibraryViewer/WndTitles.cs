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

            var le = tggTitles.ActiveToggles().FirstOrDefault().GetComponent<ListElementTitle>();
            var titleObject = titles.allTitles.Find(dt => dt.id == le.id);
            if (titleObject != null)
            {
                txtConditionTitle.text = titleObject.condition;
                if (le.enable)
                {
                    txtNameTitle.text = titleObject.name;
                    txtDescriptionTitle.text = titleObject.description;
                }
                else
                {
                    txtNameTitle.text = "?????";
                    txtDescriptionTitle.text = "?????";
                }
            }
        }
    }
}
