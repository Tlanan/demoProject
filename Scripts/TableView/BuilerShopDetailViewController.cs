using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuilerShopDetailViewController : ViewController {

    [SerializeField] private Text priceLabel;
    [SerializeField] private Text descriptionLabel;
    [SerializeField] private Button m_Btn;
    [SerializeField] private GameLoad m_gameLoad;
    private RoleInfoUI m_roleInfoUI;
    private BuilderShop itemData;
    private LogGameUI m_logGameUI;

    public void UpdateContent(BuilderShop itemData)
    {
        m_roleInfoUI = m_gameLoad.GetGameManager().GetRoleInfoUI();
        m_logGameUI = m_gameLoad.GetGameManager().GetLogGameUI();
        this.itemData = itemData;
        ShowUI();
        SetBtnFuncation(itemData.index);
    }
    /// <summary>
    /// 更新UI显示
    /// </summary>
    public void  ShowUI()
    {
        priceLabel.text = itemData.costPrice.ToString();
        descriptionLabel.text = itemData.description + GetItemDsc(itemData.index);
    }

    /// <summary>
    /// 警告框显示
    /// </summary>
    public void LogShow()
    {
        m_logGameUI.LogMessage(LogEnum.NoImprove);
        m_logGameUI.Show();
    }

    /// <summary>
    /// 通过index绑定btn对应的事件
    /// </summary>
    /// <param name="index"></param>
     public void SetBtnFuncation(int index)
    {
        switch (index)
        {
            case 0:
                m_Btn.onClick.RemoveAllListeners();
                m_Btn.onClick.AddListener(BtnBuilder);
                break;
            case 1:
                m_Btn.onClick.RemoveAllListeners();
                m_Btn.onClick.AddListener(BtnCoin);
                break;
            case 2:
                m_Btn.onClick.RemoveAllListeners();
                m_Btn.onClick.AddListener(BtnCover);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 得到对应索引的item的最大值
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetItemDsc(int index)
    {
        switch (index)
        {
            case 0:
                return m_roleInfoUI.MaxCount.ToString();
            case 1:
                return m_roleInfoUI.CoinMax.ToString();
            case 2:
                return m_roleInfoUI.CoverNumber.ToString();
            default:
                return "";
        }
    }
    /// <summary>
    /// 扩充最大人口
    /// </summary>
    public void BtnBuilder()
    {
        
        if (m_roleInfoUI.CoinCount-itemData.costPrice>=0)
        {
            m_roleInfoUI.MaxCount += itemData.ItemAble;
            m_roleInfoUI.CoinCount -= itemData.costPrice;
            itemData.costPrice += m_roleInfoUI.MaxCount * 25;
            ShowUI();
        }
        else
        {
            LogShow();
        }
    }
    /// <summary>
    /// 扩充最大金币
    /// </summary>
    public void BtnCoin()
    {
        
        if (m_roleInfoUI.CoinCount - itemData.costPrice >= 0)
        {
            m_roleInfoUI.CoinMax *= itemData.ItemAble;
            m_roleInfoUI.CoinCount -= itemData.costPrice;
            itemData.costPrice *= (m_roleInfoUI.CoinMax / (itemData.costPrice*10));
            ShowUI();
        }
        else
        {
            LogShow();
        }
    }
    /// <summary>
    /// 扩充最大城墙等级
    /// </summary>
    public void BtnCover()
    {
        
        if (m_roleInfoUI.CoinCount - itemData.costPrice >= 0)
        {
            m_roleInfoUI.CoverNumber += itemData.ItemAble;
            m_roleInfoUI.CoinCount -= itemData.costPrice;
            itemData.costPrice *= (m_roleInfoUI.CoverNumber/10);
            ShowUI();
        }
        else
        {
            LogShow();
        }
    }

}
