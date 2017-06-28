using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoBehaviour {

    public CanvasGroup NodeMainPanel;
    public CameraManager CameraManager;

    public delegate void ActiveGUI(); // declare delegate type

    protected ActiveGUI callbackFct; // to store the function

    private bool isGuiOpened;

    private NodeManager nodeManager;
    private NodeUIConfigurator nodeMainConfigurator;

    [SerializeField]
    private GameObject currentNodeMenuOpened;

    public void Start()
    {
        IsGuiOpened = false;
        nodeMainConfigurator = NodeMainPanel.GetComponent<NodeUIConfigurator>();
    }


    public void OpenMainNodeMenu(GameObject currentNode)
    {

        CurrentNodeMenuOpened = currentNode;
        nodeManager = CurrentNodeMenuOpened.GetComponent<NodeManager>();
        NodeMainPanel.gameObject.SetActive(true);
        NodeMainPanel.DOFade(1f, 0.5f);
        CameraManager.MoveCameraToTarget(CurrentNodeMenuOpened.transform, ActiveGUIMode);
        nodeMainConfigurator.nodeName.text = nodeManager.nodeName;


    }

    public void CloseMainNodeMenu()
    {
        CameraManager.ReturnCameraToWorld(CurrentNodeMenuOpened.transform, ActiveGUIMode);
        NodeMainPanel.DOFade(0f, 0f).OnComplete(() => NodeMainPanel.gameObject.SetActive(false));
        nodeManager.Deselect();
        CurrentNodeMenuOpened = null;
    }

     void ActiveGUIMode()
    {
        IsGuiOpened = !IsGuiOpened;
        Debug.Log(IsGuiOpened);
    }

    public GameObject CurrentNodeMenuOpened
    {
        get
        {
            return currentNodeMenuOpened;
        }

        set
        {
            currentNodeMenuOpened = value;
        }
    }

    public bool IsGuiOpened
    {
        get
        {
            return isGuiOpened;
        }

        set
        {
            isGuiOpened = value;
        }
    }
}
