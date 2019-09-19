using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoberDetailViewController : ViewController {

    [SerializeField] private NavigationViewController navigationView;
    [SerializeField] private Text nameLabel;
    [SerializeField] private Text specieLabel;
    [SerializeField] private Text LvLabel;
    [SerializeField] private Text AttackLabel;
    [SerializeField] private Text DefenceLabel;
    [SerializeField] private Text LifeLabel;
    [SerializeField] private Text DescriptionLabel;
    [SerializeField] private Text OccupationLabel;
    [SerializeField] private Image imageLabel;

    private Sober itemData;
    public override string Title
    {
        get
        {
            return (itemData != null) ? itemData.GetName() : "";
        }
    }

    /// <summary>
    /// 信息界面的数据更新
    /// </summary>
    /// <param name="itemData"></param>
    public void UpdateContent(Sober itemData)
    {
        this.itemData = itemData;

        nameLabel.text = itemData.GetName();
        specieLabel.text = itemData.GetSpecie();
        LvLabel.text = itemData.GetLvValue().ToString();
        DescriptionLabel.text = itemData.GetDescription();

        AttackLabel.text = itemData.GetAtkValue().ToString();
        DefenceLabel.text = itemData.GetDfcValue().ToString();
        LifeLabel.text = itemData.GetLifeValue().ToString();

        switch (itemData.GetOccRole())
        {
            case RoleOccupation.Warrige:
                OccupationLabel.text = "战士";
                break;
            case RoleOccupation.Mage:
                OccupationLabel.text = "法师";
                break;
            case RoleOccupation.Hunter:
                OccupationLabel.text = "猎人";
                break;
            default:
                break;
        }
        imageLabel.sprite = SpriteSheetManager.GetSpriteByName("IconAtlas", "hero" + itemData.GetIconIndex());
    }
    }
