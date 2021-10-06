using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovePaint : DoAction
{
    private GameObject goPaint;
    public Transform TargetInPlace, TargetMovePlace;

    bool isInPlace = false;

    // Start is called before the first frame update
    void Start()
    {
        fTimer = 0;
        goPaint = gameObject;
    }
    private void Update()
    {
        if (bDoingAction)
        {
            print("DoAction = " + bDoingAction);
            DoingAction();
        }
    }
    public override void DoingAction()
    {
        if (!isInPlace) //Move Paint
        {
            if (fTimer < fTimeToDo)
            {
                //Interpolate Position
                goPaint.transform.position = Vector3.Lerp(TargetInPlace.position, TargetMovePlace.position, fTimer / fTimeToDo);
                goPaint.transform.rotation = Quaternion.Lerp(TargetInPlace.rotation, TargetMovePlace.rotation, fTimer / fTimeToDo);
                fTimer += Time.deltaTime;
            }
            else
            {
                fTimer = 0;
                isInPlace = !isInPlace;
                EndAction();
            }
        }
        else //Replace Paint
        {
            if (fTimer < fTimeToDo)
            {
                //Interpolate Position
                goPaint.transform.position = Vector3.Lerp(TargetMovePlace.position, TargetInPlace.position, fTimer / fTimeToDo);
                goPaint.transform.rotation = Quaternion.Lerp(TargetMovePlace.rotation, TargetInPlace.rotation, fTimer / fTimeToDo);
                fTimer += Time.deltaTime;
            }
            else
            {
                fTimer = 0;
                isInPlace = !isInPlace;
                EndAction();
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        GameManager.getInstance().tiTextInformation.SetText(sInfoAction);
        GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>().AddListenerAndKey(GameManager.Objects.tableau, this);
        print($"[MOVEPAINT]: {other.name} in collision");
    }

    public override void OnTriggerExit(Collider other)
    {
        sInfoAction = "";
        GameManager.getInstance().tiTextInformation.SetText(sInfoAction);
        GameManager.getInstance().goPlayer.GetComponentInChildren<PlayerActions>().RemoveListenerAndKey(GameManager.Objects.tableau, this);
    }

}
