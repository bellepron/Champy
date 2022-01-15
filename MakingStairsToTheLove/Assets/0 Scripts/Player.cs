using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using EZ_Pooling;

public class Player : Singleton<Player>, ILevelStartObserver, IWinObserver, ILoseObserver, ILevelEndObserver
{
    // [SerializeField] PlayerSettings p_Sets;
    private UnityAction behaviour;
    Animator anim;
    Dreamteck.Splines.SplineFollower splineFollower;
    bool updating;
    bool pressing;
    public float speed;

    [SerializeField] Transform brickBasketTr;
    [SerializeField] List<GameObject> bricks = new List<GameObject>();
    float pushTimer;
    [SerializeField] LayerMask layerMask;
    [SerializeField] RagdollToggle ragdollToggle;
    public Transform dancePoseHand;
    Vector3 brickScale;
    [SerializeField] Transform brickDestroyParticle;


    #region Changeable Values
    [Header("Brick Collect&Put")]
    float stairPutInterval = 0.05f;
    float stairPuttingTime = 0.1f;
    Vector3 puttedStairScale = new Vector3(2, 0.2f, 0.4f);
    float stairPutingHeigt = 0.35f;
    float distanceFromCollectedBricks = 0.01f;

    [Header("Brick Allign")]
    float alignDistX = 0.3f;
    float alignDistZ = 0.3f;
    float alignInterval = 0.05f;
    float alignTime = 0.5f;

    [Header("End Run")]
    float endRunSpeed = 5;

    [Header("Dance Pose")]
    float playerRotationSpeed = 180;
    float loverRotationSpeed = 180;

    [Header("Ragdoll")]
    float forceToRaggdoll = 100;
    #endregion

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
        brickScale = GameObject.FindGameObjectWithTag("Brick").transform.localScale;

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
        Vector3 offset = new Vector3(0, Mathf.FloorToInt((bricks.Count - 1) / 3) * (brickScale.y + distanceFromCollectedBricks), ((bricks.Count - 1) % 3) * -(brickScale.z + distanceFromCollectedBricks));
        brick.transform.parent = brickBasketTr;
        brick.transform.localPosition = offset;
        brick.transform.localEulerAngles = Vector3.zero;

        SoundManager.Instance.Pop();
    }

    void UseBrick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pushTimer = stairPutInterval;
            pressing = true;
        }
        if (Input.GetMouseButton(0))
        {
            pushTimer += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pushTimer = stairPutInterval;
            pressing = false;
        }

        if (pushTimer < stairPutInterval) return;

        pushTimer = 0;

        if (bricks.Count >= 1)
        {
            Transform lastBrickTr = bricks[bricks.Count - 1].transform;
            bricks.RemoveAt(bricks.Count - 1);
            lastBrickTr.parent = null;

            lastBrickTr.DOScale(puttedStairScale, stairPuttingTime);
            lastBrickTr.DOMove(transform.position + new Vector3(0, stairPutingHeigt, 0), stairPuttingTime);
            lastBrickTr.transform.eulerAngles = splineFollower.transform.eulerAngles;

            transform.DOLocalMoveY(transform.localPosition.y + stairPutingHeigt, stairPuttingTime);

            SoundManager.Instance.Pop();
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
        ragdollToggle.pelvisRb.AddForce(((transform.forward * -1).normalized + new Vector3(0, 1, 0)) * forceToRaggdoll, ForceMode.Impulse);

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
        Vector3 startPos = transform.position;
        Transform splineF_Tr = splineFollower.transform;
        Vector3 direction = splineF_Tr.forward;
        Vector3 splineF_Right = splineF_Tr.right * alignDistX;
        Vector3 playerDestination = transform.position;

        for (int i = bricks.Count - 1; i >= 0; i = i - 2)
        {
            if (i < 1)
            {
                StartCoroutine(DestroyUnusedBricks(i));
                break;
            }

            indexx++;

            if (indexx > 26)
            {
                StartCoroutine(DestroyUnusedBricks(i));
                break;
            }

            BrickAlingMove(bricks[i], -splineF_Right + startPos + indexx * direction * alignDistZ, splineF_Tr.eulerAngles);
            BrickAlingMove(bricks[i - 1], splineF_Right + startPos + indexx * direction * alignDistZ, splineF_Tr.eulerAngles);

            yield return new WaitForSeconds(alignInterval);
        }

        playerDestination = startPos + indexx * direction * 0.3f;
        StartCoroutine(LastRun(alignInterval * (indexx - 1), playerDestination));
    }
    IEnumerator DestroyUnusedBricks(int index)
    {
        for (int i = index; i >= 0; i = i - 1)
        {
            Transform _brickTr = bricks[i].transform;
            SpawnBrickParticle(_brickTr);
            Invoke(nameof(DespawnBrickParticle), 0.09f);
            SoundManager.Instance.Pop();
            bricks[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void SpawnBrickParticle(Transform _brickTr)
    {
        EZ_PoolManager.Spawn(brickDestroyParticle, _brickTr.position, _brickTr.rotation);
    }
    void DespawnBrickParticle()
    {
        EZ_PoolManager.Despawn(brickDestroyParticle);
    }
    void BrickAlingMove(GameObject brick, Vector3 pos, Vector3 eulerAng)
    {
        brick.transform.parent = null;
        brick.transform.DOMove(pos, alignTime);
        brick.transform.DORotate(eulerAng, alignTime, RotateMode.FastBeyond360);
    }
    IEnumerator LastRun(float t, Vector3 playerDestination)
    {
        yield return new WaitForSeconds(t);
        anim.SetBool("isWalking", true);
        GetComponent<Rigidbody>().isKinematic = true;

        bool a = true;
        while (a)
        {
            if (Vector3.Distance(transform.position, playerDestination) < 0.5f)
            {
                anim.SetBool("isWalking", false);

                Observers.Instance.Notify_LevelEndObservers();
                a = false;
            }

            transform.position += splineFollower.transform.forward * Time.deltaTime * endRunSpeed;

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
            dancePoseHand.transform.parent.transform.Rotate(Vector3.up * loverRotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.up * playerRotationSpeed * Time.deltaTime, Space.World);

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