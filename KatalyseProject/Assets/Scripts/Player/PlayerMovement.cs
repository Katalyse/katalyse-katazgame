using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public JavascriptHook jhJavascriptHook;
    public PanelEnd pePanelEnd;
    [SerializeField] private CharacterController ccCharacterController;
    [SerializeField] private Animator aPlayerAnimator;
    private LineRenderer lrTrail;
    private List<Vector3> Way;

    public float fPlayerSpeed, fRotationSpeed;
    public Vector3 fTimeBeforeDance;
    public bool bCanMove = true;
    private bool bShowWay = false, bisRotating = false, bCanGoForward;
    private float fMaxDistance = -1;
    private Vector3 StartPos;
    private Quaternion v3TargetRotation;

    private int index;

    private void Start()
    {
        bCanMove = true;
        Way = new List<Vector3>();
        lrTrail = GetComponentInChildren<LineRenderer>();
        lrTrail.enabled = false;
        fTimeBeforeDance.z = Random.Range(fTimeBeforeDance.x, fTimeBeforeDance.y);
        aPlayerAnimator.SetBool("IsWalking", false);
        StartPos = transform.position;
    }
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.distance <= 0.7f && bisRotating == false) setCanGoForward(false);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) IncreasePlayerSpeed();
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) DecreasePlayerSpeed();

        Rotate();
        if (bCanMove)
        {
            CheckGround();
            goForward();
            if (Input.GetKeyDown(KeyCode.UpArrow)) setCanGoForward(true);
            if (Input.GetKeyDown(KeyCode.DownArrow) && !bisRotating) RotateTo(180);
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !bisRotating) RotateTo(270);
            if (Input.GetKeyDown(KeyCode.RightArrow) && !bisRotating) RotateTo(90);
            
            AddPosToWay();
        }

        if (bShowWay)
        {
            StartCoroutine(ShowWay());
        }
    }
    public void SetMaxDistance(float f)
    {
        fMaxDistance = f;
    }
    public void RotateTo(float yRotation)
    {
        v3TargetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + yRotation, 0);
        bisRotating = true;
    }
    public void setCanGoForward(bool b)
    {
        bCanGoForward = b;
        if (bCanGoForward == false) StartPos = transform.position;
    }
    private void goForward()
    {
        aPlayerAnimator.SetBool("IsWalking", bCanGoForward);
        if (aPlayerAnimator.GetBool("IsWalking") == false)
        {
            fTimeBeforeDance.z -= Time.deltaTime;
            if (fTimeBeforeDance.z < 0)
            {
                aPlayerAnimator.SetBool("IsDancing", true);
            }
        }
        if (bCanGoForward)
        {
            if (Vector3.Distance(StartPos, transform.position) <= fMaxDistance || fMaxDistance == -1)
            {
                aPlayerAnimator.SetBool("IsDancing", false);
                fTimeBeforeDance.z = Random.Range(fTimeBeforeDance.x, fTimeBeforeDance.y);
                ccCharacterController.SimpleMove(transform.forward * fPlayerSpeed);
            }
            else
            {
                setCanGoForward(false);
            }
        }
    }
    void AddPosToWay()
    {
        if (Way.Count == 0)
        {
            Way.Add(transform.parent.position);
        }
        else if (Vector3.Distance(Way[Way.Count - 1], transform.parent.position) > 0.3)
        {
            Way.Add(transform.parent.position);
        }
    }

    public void RestartPlayer()
    {
        transform.parent.position = new Vector3(9, 0.5f, -9.5f);
        bCanMove = false;
        SetShowWay(false);
        lrTrail.positionCount = 0;
        Way.Clear();
    }

    public void SetShowWay(bool b)
    {
        index = 0;
        bShowWay = b;
        lrTrail.enabled = b;
    }
    
    private IEnumerator ShowWay()
    {
        yield return new WaitForSeconds(3f);
        if (index < Way.Count - 1)
        {
            index++;
            lrTrail.positionCount++;
            lrTrail.SetPosition(lrTrail.positionCount - 1, Way[index]);
            yield return new WaitForSeconds(.2f);
        }
        else
        {
            yield return new WaitForSeconds(5f);
            pePanelEnd.StartAnim();
            StopAllCoroutines();
        }
    }

    float fcurrentTime;
    void Rotate()
    { // call this in update
        if (bisRotating == true)
        {
            bCanGoForward = false;
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, v3TargetRotation, fcurrentTime / 5);
        
            if (fcurrentTime <= 1)
            {
                fcurrentTime += Time.deltaTime;
            }
            else
            {
                if (transform.parent.rotation != v3TargetRotation)
                {
                    transform.parent.rotation = v3TargetRotation;
                }
                bisRotating = false; // turns off rotating
                fcurrentTime = 0f; // resets this variable
                bCanGoForward = true;
            }
        }
    }

    public void StopMoving()
    {
        bCanMove = false;
        aPlayerAnimator.SetBool("IsWalking", false);
    }
    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, Vector3.down, out hit, Mathf.Infinity))
        {
            //print("Hit distance: " + hit.distance);
            if (hit.distance > 1.1)
            {
                ccCharacterController.SimpleMove(new Vector3(0, 0, 0));
            }
        }
    }

    public void IncreasePlayerSpeed()
    {
        if (fPlayerSpeed < 3)
        {
            fPlayerSpeed++;
            aPlayerAnimator.SetFloat("WalkSpeedAnimation", (fPlayerSpeed * 1.5f) / 3.5f);
        }
        else
        {
            print("[PLAYER]: Max Speed was reach -> " + fPlayerSpeed);
        }
    }
    public void DecreasePlayerSpeed()
    {
        if (fPlayerSpeed > 2)
        {
            fPlayerSpeed--;
            aPlayerAnimator.SetFloat("WalkSpeedAnimation", (fPlayerSpeed * 1.5f) / 3.5f);
        }
        else
        {
            print("[PLAYER]: Min Speed was reach -> " + fPlayerSpeed);
        }
    }
}
