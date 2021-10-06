using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAction : MonoBehaviour
{
    public float fTimeToDo;
    public bool bDoingAction;
    public string sInfoAction;
    protected float fTimer = 0;

    //public DoAction(float TimeToDo, bool DoingAction, string InfoAction) { fTimeToDo = TimeToDo; bDoingAction = DoingAction; sInfoAction = InfoAction; }

    private void Update()
    {
        if (bDoingAction)
        {
            DoingAction();
        }
    }
    public virtual void DoingAction() {}

    public void StartAction()
    {
        //GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerMovement>().StopMoving();
        bDoingAction = true;
    }
    public void EndAction()
    {
        GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerMovement>().bCanMove = true;
        bDoingAction = false;
    }
    public virtual void OnTriggerEnter(Collider other)
    {

    }
    public virtual void OnTriggerExit(Collider other)
    {
        sInfoAction = "";
        GameManager.getInstance().tiTextInformation.SetText(sInfoAction);
        //GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>().ueAction.RemoveListener(StartAction);
    }
}
