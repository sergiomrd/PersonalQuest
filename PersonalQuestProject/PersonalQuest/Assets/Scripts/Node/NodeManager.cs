using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using DG.Tweening;

public class NodeManager : LeanSelectableBehaviour
{
    public LeanTapSelect leanTap;
    public GameController GameController;
    public CameraManager CameraManager;
    public CanvasManager Canvas;
    private Outline outline;
    private Transform nodePosition;
    private CanvasGroup nodeMainMenuCanvasGroup;
    public string nodeName;

    public virtual void Start()
    {
        nodePosition = gameObject.transform;
        outline = gameObject.GetComponent<Outline>();
    }

    protected override void OnSelect(LeanFinger finger)
    {

        outline.ShowHide_Outline(true);
        Canvas.OpenMainNodeMenu(gameObject);

    }

    public void Deselect()
    {
        outline.ShowHide_Outline(false);
        leanTap.CurrentSelectable = null;
    }

    

    

    
}
