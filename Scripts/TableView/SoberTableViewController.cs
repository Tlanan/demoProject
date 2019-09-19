using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoberTableViewController : TableViewController<Sober> {

    [SerializeField] private NavigationViewController navigationView;
    [SerializeField] private SoberDetailViewController detailView;

    public void TableReset()
    {
        tableData.Clear();
    }
        
    public void AddTableData(Sober sober)
    {
        tableData.Add(sober);
    }

    public void RemoveTableData(Sober sober)
    {
        tableData.Remove(sober);
    }

    protected override float CellHeightAtIndex(int index)
    {
        return 34.0f;
    }

    protected override void Awake()
    {
        base.Awake();
        SpriteSheetManager.Load("IconAtlas");
        
    }
    protected override void Start()
    {
        base.Start();
        

        if(navigationView!=null)
        {
            navigationView.Push(this);
        }
    }
    public void Update()
    {
        if (tableData.Count > 0)
        {
            UpdateContents();
        }
        else
        {
            if (GetCellsCount() != 0)
            {
                int k = GetCellsCount();
                for (int i = 0; i < k; i++)
                {
                    SetFalseCell();
                }
                Relset();
            }
        }
    }


    public override string Title
    {
        get
        {
            return "队伍";
        }
    }

    /// <summary>
    /// 添加cell的点击功能，当某个cell被按下时
    /// 将当前导航页面压入
    /// </summary>
    /// <param name="cell"></param>
    public void OnPressCell(SoberTableView cell)
    {

        if(navigationView!=null)
        {
            //导航栏的更新内容方法
            detailView.UpdateContent(tableData[cell.DataIndex]);
            navigationView.Push(detailView);
        }
    }
}
