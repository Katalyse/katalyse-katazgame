using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : DoAction
{
    public GameObject goDoor;
    public Transform TargetOpen, TargetClose;

    bool isOpen = false;
    bool bIsPlayerOut = true;

    // Start is called before the first frame update
    void Start()
    {
        fTimer = 0;
    }
    private void Update()
    {
        if (bDoingAction)
        {
            DoingAction();
        }
        if (bIsPlayerOut && isOpen)
        {
            if (fTimer < fTimeToDo)
            {
                //Interpolate Position
                goDoor.transform.position = Vector3.Lerp(TargetOpen.position, TargetClose.position, fTimer / fTimeToDo);
                fTimer += Time.deltaTime;
            }
            else
            {
                fTimer = 0;
                isOpen = !isOpen;
            }
        }
    }
    public override void DoingAction()
    {
        if (!isOpen) //Close Door
        {
            if (fTimer < fTimeToDo)
            {
                //Interpolate Position
                goDoor.transform.position = Vector3.Lerp(TargetClose.position, TargetOpen.position, fTimer / fTimeToDo);
                fTimer += Time.deltaTime;
            }
            else
            {
                fTimer = 0;
                isOpen = !isOpen;
                EndAction();
            }
        }
        else //Open Door
        {
            if (fTimer < fTimeToDo)
            {
                //Interpolate Position
                goDoor.transform.position = Vector3.Lerp(TargetOpen.position, TargetClose.position, fTimer / fTimeToDo);
                fTimer += Time.deltaTime;
            }
            else
            {
                fTimer = 0;
                isOpen = !isOpen;
                EndAction();
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        bIsPlayerOut = false;
        GameManager.getInstance().tiTextInformation.SetText(sInfoAction);

        GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>().AddListenerAndKey(GameManager.Objects.porte, this);
        print($"[OPENDOOR]: {other.name} in collision");
    }

    public override void OnTriggerExit(Collider other)
    {
        bIsPlayerOut = true;
        GameManager.getInstance().tiTextInformation.SetText("");
        GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>().RemoveListenerAndKey(GameManager.Objects.porte, this);
    }

}
