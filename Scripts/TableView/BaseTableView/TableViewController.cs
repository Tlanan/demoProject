using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ScrollRect))]
public class TableViewController<T> : ViewController {


    protected  List<T> tableData = new List<T>();
    [SerializeField] private RectOffset padding;
    [SerializeField] private float spacingHeight = 10.0f;

    [SerializeField] private GameObject cellBase;
    private LinkedList<TableViewCell<T>> cells = new LinkedList<TableViewCell<T>>();

    private Rect visibleRect;
    [SerializeField] private RectOffset visbleRectPadding;

    private Vector2 preScrollPos;

    private ScrollRect cachedScrollRect;
    public ScrollRect CachedScroellRect
    {
        get
        {
            if(cachedScrollRect==null)
            {
                cachedScrollRect = GetComponent<ScrollRect>();
            }
            return cachedScrollRect;
        }
    }
    protected virtual void Awake()
    {

    }



    protected virtual void Start()
    {
        cellBase.SetActive(false);
        CachedScroellRect.onValueChanged.AddListener(OnScorllPosChanged);
    }
    protected virtual float CellHeightAtIndex(int index)
    {
        return 0.0f;
    }

    protected void UpdateContentSize()
    {
       
        float contentHeight = 0.0f;//计算多少个cell的 宽度总和 加 间隔spacingHeight
        for(int i=0;i<tableData.Count;i++)
        {
            contentHeight += CellHeightAtIndex(i);
            if(i>0)
            {
                contentHeight += spacingHeight;
            }
        }

        Vector2 sizeDelta = CachedScroellRect.content.sizeDelta;
        sizeDelta.y = padding.top + contentHeight + padding.bottom;
        CachedScroellRect.content.sizeDelta = sizeDelta;
    }

    /// <summary>
    /// 根据索引index创建cell
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private TableViewCell<T> CreateCellForIndext(int index)
    {
        GameObject obj = Instantiate(cellBase) as GameObject;
        obj.SetActive(true);
        TableViewCell<T> cell = obj.GetComponent<TableViewCell<T>>();

        #region 再设置父物体之前，先取得位置信息
        Vector3 scale = cell.transform.localScale;
        Vector2 sizeDelta = cell.CachedRectTransform.sizeDelta;
        Vector2 offsetMin = cell.CachedRectTransform.offsetMin;
        Vector2 offsetMax = cell.CachedRectTransform.offsetMax;
        #endregion
        //设置父物体
        cell.transform.SetParent(cellBase.transform.parent);
        //重新将位置信息赋值给在父物体下的cell
        cell.transform.localScale = scale;
        cell.CachedRectTransform.sizeDelta = sizeDelta;
        cell.CachedRectTransform.offsetMax = offsetMax;
        cell.CachedRectTransform.offsetMin = offsetMin;

        UpdateCellForIndex(cell, index);
        cells.AddLast (cell);//链表的后进先出原则
        return cell;
    }
    /// <summary>
    /// 根据索引index更新对应的cell的内容
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="index"></param>
    private void UpdateCellForIndex(TableViewCell<T> cell,int index)
    {
        cell.DataIndex = index;//确定cell的索引
        if (cell.DataIndex >= 0 && cell.DataIndex <= tableData.Count - 1)
        {
            cell.gameObject.SetActive(true);
            cell.UpdateContent(tableData[cell.DataIndex]);
            cell.Height = CellHeightAtIndex(cell.DataIndex);
        }
        else
        {
            cell.gameObject.SetActive(false);
        }
    }

    private void UpdateVisibleRect()
    {
        //visibleRect的位置是距离滚动内容的基准点的相对位置
        visibleRect.x = CachedScroellRect.content.anchoredPosition.x + visbleRectPadding.left;
        visibleRect.y =- CachedScroellRect.content.anchoredPosition.y + visbleRectPadding.top;

        //visibleRect的尺寸是滚动视图尺寸加上填充内容的尺寸
        visibleRect.width = CachedRectTransform.rect.width + visbleRectPadding.left + visbleRectPadding.right;
        visibleRect.height = CachedRectTransform.rect.height + visbleRectPadding.top + visbleRectPadding.bottom;
    }

    /// <summary>
    /// 更新cell内容
    /// </summary>
    protected void UpdateContents()
    {
        UpdateContentSize();
        UpdateVisibleRect();

        if(cells.Count<1)
        {
            Vector2 cellTop = new Vector2(0.0f, -padding.top);
            for(int i=0;i<tableData.Count;i++) 
            {
                float cellHeight = CellHeightAtIndex(i);//确定每一个cell的高度
                Vector2 cellBottom = cellTop + new Vector2(0.0f, -cellHeight);
                if((cellTop.y<=visibleRect.y&&cellTop.y>=visibleRect.y-visibleRect.height)||
                    (cellBottom.y<=visibleRect.y&&cellBottom.y>=visibleRect.y-visibleRect.height))
                {
                    TableViewCell<T> cell = CreateCellForIndext(i);
                    cell.Top = cellTop;//确定第一个cell的位置
                    break;
                }
                //确定第二个cell 的位置
                cellTop = cellBottom + new Vector2(0.0f, spacingHeight);
            }
            //如果visibleRect范围内为空，则创建cell 
            FillVisibleRectWithCells();
        }
        else//如果cells已经有cell，则从最开始cell 依次设置对应项目的索引并修改 更新位置和内容
        {
            LinkedListNode<TableViewCell<T>> node = cells.First;//表示cells里的各个节点，从而用于修改cell 的索引
            UpdateCellForIndex(node.Value, node.Value.DataIndex); //更新第一个索引所对应的cell
            node = node.Next; //指向下一个节点

            while (node!=null)//直到最后一个节点
            {
                UpdateCellForIndex(node.Value, node.Previous.Value.DataIndex + 1);

                node.Value.Top = node.Previous.Value.Bottom + new Vector2(0.0f, -spacingHeight);
                node = node.Next;
            }

            //如果visibleRect范围为空则创建cell 
            FillVisibleRectWithCells();
        }
    }

    /// <summary>
    /// 创建visibleRect范围内可显示的cell
    /// </summary>
    private void FillVisibleRectWithCells()
    {
        if (cells.Count < 1)  return;

        TableViewCell<T> lastCell = cells.Last.Value; //取出cells链表的第一个（后进先出）
        int nextCellDataIndex = lastCell.DataIndex + 1; //下一个cell的索引
        //下一个cell 的位置   上一个lastCell的下边界Bottom加上各个cell 的间隔 spacingHeight
        Vector2 nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

        while (nextCellDataIndex < tableData.Count &&
            nextCellTop.y >= visibleRect.y - visibleRect.height)//直到list列表中的最后一个，且每个cell 的位置必须是在visibleRect可视的
        {
            TableViewCell<T> cell = CreateCellForIndext(nextCellDataIndex);//根据下一个索引创建cell
            cell.Top = nextCellTop;//赋予位置信息

            //做循环条件
            lastCell = cell;
            nextCellDataIndex = lastCell.DataIndex + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

        }
    }

    private void ReuseCells(int scrollDirection)
    {
        if(cells.Count<1)
        {
            return;
        }

        if(scrollDirection>0)
        {
            TableViewCell<T> firstCell = cells.First.Value;
            while(firstCell.Bottom.y>visibleRect.y)//直到超出visibleRect可视范围
            {
                TableViewCell<T> lastCell = cells.Last.Value;
                UpdateCellForIndex(firstCell, lastCell.DataIndex + 1);//将最后一个cell信息取出，用于显示

                firstCell.Top = lastCell.Bottom + new Vector2(0.0f, -spacingHeight);

                cells.AddLast(firstCell);//头变尾
                cells.RemoveFirst(); //第一个cell从链表中擦除
                firstCell = cells.First.Value;  //将下一个cell设置为要更新的cell
            }

            FillVisibleRectWithCells();
        }
        else if(scrollDirection<0)
        {
            TableViewCell<T> lastCell = cells.Last.Value;

            while(lastCell.Top.y<visibleRect.y-visibleRect.height)
            {
                TableViewCell<T> firstCell = cells.First.Value;
                UpdateCellForIndex(lastCell, firstCell.DataIndex - 1);
                lastCell.Bottom = firstCell.Top + new Vector2(0.0f, spacingHeight);

                cells.AddFirst(lastCell);
                cells.RemoveLast();
                lastCell = cells.Last.Value;
            }
        }
    }

    public void OnScorllPosChanged(Vector2 scrollPos)
    {
        UpdateVisibleRect();//更新滚动视图
        //判断滑动方向
        //1  向上滑 当前位置比之前位置小
        //-1 向下滑 当前位置比之前位置大
        ReuseCells(scrollPos.y < preScrollPos.y ? 1 : -1);
        preScrollPos = scrollPos;
    }

    public void Relset()
    {
        cells.Clear();
    }

    public int GetCellsCount()
    {
        return cells.Count;
    }

    public void SetFalseCell()
    {
        cells.First.Value.gameObject.SetActive(false);
        cells.RemoveFirst();
    }
}
