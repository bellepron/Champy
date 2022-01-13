using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : Singleton<Player>, ILevelStartObserver, IWinObserver, ILoseObserver, ILevelEndObserver
{
    Animator anim;
    Dreamteck.Splines.SplineFollower splineFollower;
    public float speed = 5;
    bool updating;

    [SerializeField] Transform brickBasketTr;
    [SerializeField] List<GameObject> bricks = new List<GameObject>();
    float pushTimer;
    [SerializeField] float pushCd = 0.05f;
    [SerializeField] LayerMask layerMask;

    bool pressing;
    int index;

    private void Start()
    {
        anim = GetComponent<Animator>();
        splineFollower = FindObjectOfType<Dreamteck.Splines.SplineFollower>();
        splineFollower.followSpeed = 0;

        Observers.Instance.Add_LevelStartObserver(this);
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
        Observers.Instance.Add_LevelEndObserver(this);
    }

    void ILevelStartObserver.LevelStart()
    {
        splineFollower.followSpeed = speed;
        anim.SetBool("isWalking", true);

        StartCoroutine(MyUpdate());
    }

    IEnumerator MyUpdate()
    {
        updating = true;
        while (updating)
        {
            UseBrick();

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() == null) return;
        else
        {
            other.GetComponent<IInteractable>().Interact(this);
        }
    }

    public void InteractWithBrick(GameObject brick)
    {
        index++;
        Vector3 offset = new Vector3(0, Mathf.FloorToInt((index - 1) / 3) * 0.3f, ((index - 1) % 3) * -0.82f);
        brick.transform.parent = brickBasketTr;
        brick.transform.DOLocalJump(brickBasketTr.localPosition + offset, 2, 1, 0.4f);
        StartCoroutine(DelayedAddToBricks(brick));
    }
    IEnumerator DelayedAddToBricks(GameObject brick)
    {
        yield return new WaitForSeconds(0.5f);
        bricks.Add(brick);
    }
    void UseBrick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pushTimer = pushCd;
            pressing = true;
        }
        if (Input.GetMouseButton(0))
        {
            pushTimer += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pushTimer = 0;
            pressing = false;
        }

        if (pushTimer < pushCd) return;

        pushTimer = 0;

        if (bricks.Count >= 1)
        {
            index--;

            Transform lastBrickTr = bricks[bricks.Count - 1].transform;
            bricks.RemoveAt(bricks.Count - 1);
            lastBrickTr.parent = null;

            Vector3 stairScale = new Vector3(2, 0.2f, 0.4f);
            lastBrickTr.DOScale(stairScale, 0.1f);
            lastBrickTr.DOMove(transform.position + new Vector3(0, 0.35f, 0), 0.1f);
            // lastBrickTr.localPosition = transform.position + new Vector3(0, 0.35f, 0);
            Debug.Log(transform.position);

            transform.DOLocalMoveY(transform.localPosition.y + 0.35f, 0.09f);
        }
        else
            Debug.Log("Ups");
    }

    private void FixedUpdate()
    {
        if (IsOnGround() == true)
        {
            anim.SetBool("isFalling", false);
        }
        else
        {
            if (pressing == false)
                anim.SetBool("isFalling", true);
            else
                anim.SetBool("isFalling", false);
        }
    }

    bool IsOnGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void IWinObserver.WinScenario()
    {
        updating = false;
        splineFollower.followSpeed = 0;
        anim.SetBool("isWalking", false);
    }


    void ILoseObserver.LoseScenario()
    {
        updating = false;
        splineFollower.followSpeed = 0;
    }

    void ILevelEndObserver.LevelEnd()
    {

    }
}