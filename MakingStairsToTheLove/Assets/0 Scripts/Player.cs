using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Player : Singleton<Player>, ILevelStartObserver, IWinObserver, ILoseObserver, ILevelEndObserver
{
    private UnityAction behaviour;
    Animator anim;
    Dreamteck.Splines.SplineFollower splineFollower;
    public float speed = 5;
    bool updating;

    [SerializeField] Transform brickBasketTr;
    [SerializeField] List<GameObject> bricks = new List<GameObject>();
    float pushTimer;
    [SerializeField] float pushCd = 0.05f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] RagdollToggle ragdollToggle;
    public Transform dancePoseHand;

    bool pressing;

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

        behaviour += UseBrick;
        StartCoroutine(MyUpdate());
        StartCoroutine(MyFixedUpdate());
    }

    IEnumerator MyUpdate()
    {
        updating = true;
        while (updating)
        {
            // UseBrick();
            behaviour?.Invoke();

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
        bricks.Add(brick);
        Vector3 offset = new Vector3(0, Mathf.FloorToInt((bricks.Count - 1) / 3) * 0.09f, ((bricks.Count - 1) % 3) * -0.19f);
        brick.transform.parent = brickBasketTr;
        brick.transform.localPosition = offset;
        brick.transform.localEulerAngles = Vector3.zero;
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
            Transform lastBrickTr = bricks[bricks.Count - 1].transform;
            bricks.RemoveAt(bricks.Count - 1);
            lastBrickTr.parent = null;

            Vector3 stairScale = new Vector3(2, 0.2f, 0.4f);
            lastBrickTr.DOScale(stairScale, 0.1f);
            lastBrickTr.DOMove(transform.position + new Vector3(0, 0.35f, 0), 0.1f);
            lastBrickTr.transform.eulerAngles = splineFollower.transform.eulerAngles;

            transform.DOLocalMoveY(transform.localPosition.y + 0.35f, 0.09f);
        }
        else
            Debug.Log("Ups");
    }

    #region Falling
    IEnumerator MyFixedUpdate()
    {
        while (updating)
        {
            FallAnim();

            yield return new WaitForFixedUpdate();
        }
    }

    private void FallAnim()
    {
        if (IsOnGround() == true)
        {
            anim.SetBool("isFalling", false);
        }
        else
        {
            if (pressing == false)
            {
                anim.SetBool("isFalling", true);
            }
            else
            {
                if (bricks.Count != 0)
                    anim.SetBool("isFalling", false);
                else
                    anim.SetBool("isFalling", true);
            }
        }
    }

    bool IsOnGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), Vector3.down, out hit, 0.6f, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Interacts
    public void InteractWithSpike()
    {
        Dead();
        ScatterBricks();
    }
    #endregion

    void Dead()
    {
        Stop();
        ragdollToggle.RagdollActivate(true);
        ragdollToggle.pelvisRb.AddForce(((transform.forward * -1).normalized + new Vector3(0, 1, 0)) * 100, ForceMode.Impulse);

        Observers.Instance.Notify_LoseObservers();
    }

    void ScatterBricks()
    {
        Vector3 explosionPos = brickBasketTr.position + brickBasketTr.up * 0.2f + brickBasketTr.forward * 0.5f;
        for (int i = 0; i < bricks.Count; i++)
        {
            Transform _brick = brickBasketTr.GetChild(0);
            _brick.transform.parent = null;
            _brick.GetComponent<BoxCollider>().isTrigger = false;

            Rigidbody _brickRb = _brick.gameObject.AddComponent<Rigidbody>();

            _brickRb.AddExplosionForce(2, explosionPos + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0), 3, 0, ForceMode.Impulse);
        }
    }

    #region Win
    void IWinObserver.WinScenario()
    {
        Stop();
        anim.SetBool("isWalking", false);

        StartCoroutine(Bricks_EndAlign());
    }

    IEnumerator Bricks_EndAlign()
    {
        int indexx = 0;
        float delay = 0.05f;
        Vector3 startPos = transform.position;
        Vector3 direction = splineFollower.transform.forward;

        for (int i = bricks.Count - 1; i >= 0; i = i - 2)
        {
            if (i < 1) break;

            indexx++;
            bricks[i].transform.parent = null;
            bricks[i].transform.DOMove(splineFollower.transform.right * -0.3f + startPos + indexx * direction * 0.3f, 0.5f);
            bricks[i].transform.DORotate(splineFollower.transform.eulerAngles, 0.5f, RotateMode.FastBeyond360);
            bricks[i - 1].transform.parent = null;
            bricks[i - 1].transform.DOMove(splineFollower.transform.right * 0.3f + startPos + indexx * direction * 0.3f, 0.5f);
            bricks[i - 1].transform.DORotate(splineFollower.transform.eulerAngles, 0.5f, RotateMode.FastBeyond360);

            yield return new WaitForSeconds(delay);
        }
        StartCoroutine(LastRun(delay * indexx));
    }
    IEnumerator LastRun(float t)
    {
        yield return new WaitForSeconds(t);

        Vector3 targetPos = transform.position;
        if (bricks.Count > 1)
        {
            targetPos = bricks[1].transform.position;
            anim.SetBool("isWalking", true);
        }
        GetComponent<Rigidbody>().isKinematic = true;

        bool a = true;
        while (a)
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                anim.SetBool("isWalking", false);

                Observers.Instance.Notify_LevelEndObservers();
                a = false;
            }

            transform.position += splineFollower.transform.forward * Time.deltaTime * 5;

            yield return null;
        }
    }
    #endregion

    void ILoseObserver.LoseScenario()
    {
        Stop();
    }

    void ILevelEndObserver.LevelEnd()
    {
        if (Globals.hasReachTOLover == true)
        {
            anim.SetTrigger("happy");
            anim.SetTrigger("dancePose");
        }
        else
        {
            anim.SetTrigger("sad");
        }
    }

    public IEnumerator RotateLover()
    {
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            dancePoseHand.transform.parent.transform.Rotate(Vector3.up * 180 * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.up * 180 * Time.deltaTime, Space.World);

            yield return null;
        }
    }

    void Stop()
    {
        updating = false;
        behaviour -= UseBrick;

        splineFollower.followSpeed = 0;
    }
    public void InteractWithDoNotUseBrick()
    {
        behaviour -= UseBrick;
    }
}