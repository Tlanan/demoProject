using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class NavigationViewController : ViewController {


    private Stack<ViewController> stackedViews = new Stack<ViewController>();

    private ViewController currentView = null;

    [SerializeField] private Text titileLabel;
    [SerializeField] private  Button backButton;
    [SerializeField] private Text backButtonLabel;
    [SerializeField] private GameObject target;

    private void Awake()
    {
        backButton.onClick.AddListener(OnPressBackButton);
        backButton.gameObject.SetActive(false);
    }

    public void OnPressBackButton()
    {
        Pop();
    }

    private void EnableInteraction(bool isEnable)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = isEnable;
    }


    public void Push(ViewController newView)
    {
        if(currentView==null)
        {
            newView.gameObject.SetActive(true);
            currentView = newView;
            return;
        }

        //EnableInteraction(false);

        ViewController lastView = currentView;
        stackedViews.Push(lastView);

        Vector2 lastViewPos = lastView.CachedRectTransform.anchoredPosition;
        lastViewPos.x = -this.CachedRectTransform.rect.width*2;
        lastView.CachedRectTransform.MoveTo(
            lastViewPos, 0.3f, 0.0f, iTween.EaseType.easeOutSine, () => { lastView.gameObject.SetActive(false); });

        newView.gameObject.SetActive(true);
        Vector2 newViewPos = newView.CachedRectTransform.anchoredPosition;
        newView.CachedRectTransform.anchoredPosition =
            new Vector2(this.CachedRectTransform.rect.width*2, newViewPos.y);
        newViewPos.x = 0.0f;
        newView.CachedRectTransform.MoveTo(
            newViewPos, 0.3f, 0.0f, iTween.EaseType.easeOutSine, () => { EnableInteraction(true); });

        currentView = newView;
        titileLabel.text = newView.Title;
        backButtonLabel.text = lastView.Title;
        backButton.gameObject.SetActive(true);
        target.gameObject.SetActive(false);
    }


    public void Pop()
    {
        if (stackedViews.Count < 1) return;
        //EnableInteraction(false);

        ViewController lastView = currentView;
        Vector2 lastViewPos = lastView.CachedRectTransform.anchoredPosition;
        lastViewPos.x = this.CachedRectTransform.rect.width*2;
        lastView.CachedRectTransform.MoveTo(
            lastViewPos, 0.3f, 0.0f, iTween.EaseType.easeOutSine, () => { lastView.gameObject.SetActive(false); });

        ViewController poppedView = stackedViews.Pop();
        poppedView.gameObject.SetActive(true);
        Vector2 poppedViewPos = poppedView.CachedRectTransform.anchoredPosition;
        poppedViewPos.x = 0.0f;
        poppedView.CachedRectTransform.MoveTo(
            poppedViewPos, 0.3f, 0.0f, iTween.EaseType.easeOutSine, () => { EnableInteraction(true); });

        currentView = poppedView;

        titileLabel.text = poppedView.Title;
        target.gameObject.SetActive(true);

        //三重页面才需要
        if(stackedViews.Count>=1)
        {
            backButtonLabel.text = stackedViews.Peek().Title;
            backButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(false);
            target.gameObject.SetActive(true);
        }
    }
}
