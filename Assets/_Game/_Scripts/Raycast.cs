using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private Destroyer destroyer;
    [SerializeField] private ScreenSide screenSide;
    public Destroyer Destroyer {  get { return destroyer; } }
    public ScreenSide ScreenSide {  get { return screenSide; } }
    public string maskRaycastScrew = "", maskRaycastHole = "";
    private RaycastHit2D[] hits;
    private Vector3 mousePosition, oldMousePosition;
    private Screw curScrew = null;
    private Hole curHole = null;
    private bool isMoveScrew = false;
    private bool isChooseScrew = false;
    public bool isEnd = false;
    public bool isStart=false;
    private int check=0;
  
   
   
   
   
    void Update()
    {
        if (Destroyer.blockCount >= 12)
        {
            isEnd = true;
            return;
        }
        if (!ScreenSide.isStart)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) // check hole
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hits = Physics2D.RaycastAll(mousePosition, Vector3.zero, 50);
          
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && (hit.collider.gameObject.layer == LayerMask.NameToLayer(maskRaycastHole)))                  
                {
                    curHole = hit.transform.GetComponent<Hole>();
                   //
                    break;
                }
                else
                {
                    curHole = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !isChooseScrew)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hits = Physics2D.RaycastAll(mousePosition, Vector3.zero, 50);
            ChooseScrew();
            GameManager.instance.soundController.PlaySoundFX(TypeSound.ScrewOut);

        }

        if (Input.GetMouseButtonUp(0) && !isChooseScrew && curScrew != null)
            isChooseScrew = true;
        else if (isChooseScrew && Input.GetMouseButtonUp(0))
        {
            isChooseScrew = false;
         
        }
            

        if (Input.GetMouseButtonDown(0) && !isMoveScrew && isChooseScrew && curScrew != null)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hits = Physics2D.RaycastAll(mousePosition, Vector3.zero, 50);
            StopChooseScrew();
            ChooseScrew();
            GameManager.instance.soundController.PlaySoundFX(TypeSound.ScrewIn);
        }
    }

    private void StopChooseScrew()
    {
        if (curHole != null)
        {
            if (!curHole.isHasScrew) //&& !curHole.isLock)
            {
                GameManager.instance.handEnd.DisableTut();
                curScrew.ChangePosition(curHole.GetPosition(), true);
                curScrew.SetCurPos(curHole.transform.position);
                curHole.CheckPosListPoint();
                curScrew.ChangeConnectRigidHingeJoint(curHole.listPointObjectInHole);
                curScrew.curHole.UpdateIsHasScrew(false);
                curScrew.SetCurHole(curHole);
                curHole.UpdateIsHasScrew(true);
            }
            else
            {
                curScrew.EnableCollider();
                curScrew.ChangePosition(curScrew.curHole.GetPosition());
                EndChooseScrew();
            }
        }
        else
        {
            curScrew.EnableCollider();
            curScrew.ChangePosition(curScrew.curHole.GetPosition());
            EndChooseScrew();
        }
    }

    private void ChooseScrew()
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && (hit.collider.gameObject.layer == LayerMask.NameToLayer(maskRaycastScrew)))
            {
                if (check == 0)
                {
                    
                    check++;
                }
                isMoveScrew = false;
                if (curScrew != null && hit.collider.gameObject != curScrew)
                {
                    EndChooseScrew();
                }
                oldMousePosition = mousePosition;
                curScrew = hit.transform.GetComponent<Screw>();
                curScrew.DisableCollider();
                curScrew.SetAnim(true);
                curScrew.IsChoose();



                GameManager.instance.handStart.DisableTut();
            
                GameManager.instance.handEnd.ActiveTut();
               
                break;
            }
        }
    }
    public void EndChooseScrew()
    {
        curScrew.EndChoose();
        isChooseScrew = false;
        curScrew.EnableCollider();
        curScrew = null;
        curHole = null;
        isMoveScrew = false;
        oldMousePosition = Vector3.zero;
       
       
    }
   
}
