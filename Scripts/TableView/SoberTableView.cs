using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoberTableView : TableViewCell<Sober>
{

    [SerializeField] private Text nameLabel;
    [SerializeField] private Text specieLabel;
    [SerializeField] private Text LvLabel;
    //[SerializeField] private Text AttrLabel;

    public override void UpdateContent(Sober itemData)
    {
        nameLabel.text = itemData.GetName();
        specieLabel.text = itemData.GetSpecie();
        LvLabel.text = itemData.GetLvValue().ToString();
        
    }

}
