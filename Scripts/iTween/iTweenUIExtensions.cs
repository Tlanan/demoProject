using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenEventHandler : MonoBehaviour {
    
    /// <summary>
    /// 更新移动的委托
    /// </summary>
    public System.Action<Vector2> OnUpdateMoveDelegate { get; set; }

    /// <summary>
    /// 更新移动方法
    /// </summary>
    /// <param name="value"></param>
    public void OnUpdateMove(Vector2 value)
    {
        if(OnUpdateMoveDelegate!=null)
        {
            OnUpdateMoveDelegate.Invoke(value);
        }
    }
    /// <summary>
    /// 更新完成的委托
    /// </summary>
    public System.Action OnCompleteDelegate { get; set; }
    /// <summary>
    /// 更新完成的方法
    /// </summary>
    public void OnCoplete()
    {
        if(OnCompleteDelegate!=null)
        {
            OnCompleteDelegate.Invoke();
        }
    }
}


public static class iTweenUIExtensions
{
    private static iTweenEventHandler SetUpEventHandler(GameObject targetObj)
    {
        iTweenEventHandler eventHandler = targetObj.GetComponent<iTweenEventHandler>();
        if(eventHandler==null)
        {
            eventHandler = targetObj.AddComponent<iTweenEventHandler>();
        }
        return eventHandler;
    }


    public static void MoveTo(this RectTransform target,Vector2 pos,float time,float delay,iTween.EaseType easeType,System.Action onCompleteDelegate=null)
    {
        iTweenEventHandler eventHandler = SetUpEventHandler(target.gameObject);

        eventHandler.OnUpdateMoveDelegate = (Vector2 value) => { target.anchoredPosition = value; };

        eventHandler.OnCompleteDelegate = onCompleteDelegate;

        iTween.ValueTo(target.gameObject, iTween.Hash(
            "from", target.anchoredPosition,
            "to", pos,
            "time", time,
            "delay", delay,
            "easetype", easeType,
            "onupdate", "OnUpdateMove",
            "onupdatetarget", eventHandler.gameObject,
            "oncomplete", "OnComplete",
            "oncompletetarget", eventHandler.gameObject));
    }
}
