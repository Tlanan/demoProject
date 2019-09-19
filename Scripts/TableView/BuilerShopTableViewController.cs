using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilerShopTableViewController : TableViewController<BuilderShop> {

    [SerializeField] private NavigationViewController navigationView;
    [SerializeField] private BuilerShopDetailViewController detailView;
    private void LoadData()
    {
        tableData = new List<BuilderShop>()
        {
            new BuilderShop{costPrice=300,description="每升级一次人口增长5，当前最大人口：",index=0,ItemAble=5},
            new BuilderShop{costPrice=200,description="每升级一次金币储存加20%，当前最多金币数：",index=1,ItemAble=2},
            new BuilderShop{costPrice=500,description="提高部落人口的恢复力，当前城墙等级：",index=2,ItemAble=10},
        };
    }


    public void Release()
    {
       if(tableData.Count!=0)
        {
            tableData[0].costPrice = 300;
            tableData[1].costPrice = 200;
            tableData[2].costPrice = 500;
            detailView.ShowUI();
        }
    }


    protected override void Start()
    {
        LoadData();
        if (navigationView != null)
        {
            navigationView.Push(this);
        }
    }

    public void OnPressCell(int index)
    {

        if (navigationView != null)
        {
            //导航栏的更新内容方法
            detailView.UpdateContent(tableData[index]);
            navigationView.Push(detailView);
        }
    }

    public override string Title
    {
        get
        {
            return "建筑";
        }

    }
}
