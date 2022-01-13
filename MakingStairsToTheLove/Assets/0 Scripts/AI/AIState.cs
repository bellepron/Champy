// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using DG.Tweening;

// public class SoldierState
// {
//     public enum SOLDIERSTATE
//     {
//         IDLE, TAKEPOSITION, PURSUE, ATTACK, RUNAWAY, TAUNT, DEAD
//     };

//     public enum EVENT
//     {
//         ENTER, UPDATE, EXIT
//     };

//     public SOLDIERSTATE s_name;
//     protected EVENT stage;
//     protected GameObject npc;
//     protected NavMeshAgent agent;
//     protected Animator anim;
//     protected Transform player;
//     protected SoldierState nextState;

//     float visDist = 5.0f;
//     float visAngle = 30.0f;
//     float shootDist = 15.0f;



//     GameObject closestCoin;
//     public float fatPoint = 1;
//     public float normalSpeed = 1f, runAwaySpeed = 1.5f;
//     bool eatenByPlayer = false;
//     int i = 0;
//     public bool isTaunted;


//     public SoldierState(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
//     {
//         npc = _npc;
//         agent = _agent;
//         anim = _anim;
//         stage = EVENT.ENTER;
//         player = _player;
//     }

//     public virtual void Enter() { stage = EVENT.UPDATE; }
//     public virtual void Update() { stage = EVENT.UPDATE; }
//     public virtual void Exit() { stage = EVENT.EXIT; }

//     public SoldierState Process()
//     {
//         if (stage == EVENT.ENTER) Enter();
//         if (stage == EVENT.UPDATE) Update();
//         if (stage == EVENT.EXIT)
//         {
//             Exit();
//             return nextState;
//         }
//         return this;
//     }
//     // public bool CanSeePlayer()
//     // {
//     //     Vector3 direction = player.position - npc.transform.position;
//     //     float angle = Vector3.Angle(direction, npc.transform.forward);

//     //     if (direction.magnitude < visDist && angle < visAngle)
//     //         return true;

//     //     return false;
//     // }
//     // public bool CanShootPlayer()
//     // {
//     //     RaycastHit hit;
//     //     if (Physics.Raycast(npc.GetComponent<Soldier>().gunMuzzleTransform.position, npc.GetComponent<Soldier>().gunMuzzleTransform.TransformDirection(Vector3.down), out hit, 20))
//     //     {
//     //         if (hit.transform.gameObject.GetComponent<InputHandler>() != null)
//     //         {
//     //             Debug.DrawRay(npc.GetComponent<Soldier>().gunMuzzleTransform.position, npc.GetComponent<Soldier>().gunMuzzleTransform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
//     //             return true;
//     //         }
//     //         else
//     //         {
//     //             Debug.DrawRay(npc.GetComponent<Soldier>().gunMuzzleTransform.position, npc.GetComponent<Soldier>().gunMuzzleTransform.TransformDirection(Vector3.down) * hit.distance, Color.red);
//     //             return false;
//     //         }
//     //     }
//     //     else
//     //         return false;
//     // }
//     // public bool CanPlayerSeeMe()
//     // {
//     //     Vector3 direction = npc.transform.position - player.position;
//     //     float angle = Vector3.Angle(direction, player.forward);

//     //     if (direction.magnitude < 8 && angle < 70)
//     //         return true;

//     //     return false;
//     // }

//     // public bool IsGameStarted()
//     // {
//     //     if (Globals.isLevelStarted)
//     //         return true;
//     //     else
//     //         return false;
//     // }

//     // public bool CanAttackPlayer()
//     // {
//     //     Vector3 direction = player.position - npc.transform.position;

//     //     if (direction.magnitude < shootDist)
//     //     {
//     //         return true;
//     //     }
//     //     return false;
//     // }
//     // public bool IsPlayerBehind()
//     // {
//     //     Vector3 direction = npc.transform.position - player.position;
//     //     float angle = Vector3.Angle(direction, npc.transform.forward);

//     //     if (direction.magnitude < 3 && angle < 40)
//     //     {
//     //         return true;
//     //     }
//     //     return false;
//     // }

//     // public bool IsClosePlayer()
//     // {
//     //     if (Vector3.Distance(player.position, npc.transform.position) < 5)
//     //         return true;
//     //     else
//     //         return false;
//     // }

//     // public Vector3 RunAwayDirection()
//     // {
//     //     Vector3 direction = npc.transform.position - player.transform.position;
//     //     Vector3 nDirection = direction.normalized;

//     //     return nDirection;
//     // }

//     // public bool IsAwayFromPlayer()
//     // {
//     //     Vector3 direction = npc.transform.position - player.transform.position;

//     //     if (direction.magnitude > 7)
//     //         return true;
//     //     else
//     //         return false;
//     // }

//     // public void EatenByP()
//     // {
//     //     eatenByPlayer = true;
//     // }
//     // public void Dead()
//     // {
//     //     agent.enabled = false;
//     //     npc.GetComponent<CapsuleCollider>().enabled = false;
//     //     Rigidbody rb = npc.GetComponent<Rigidbody>();
//     //     rb.isKinematic = true;
//     //     rb.useGravity = false;
//     //     rb.detectCollisions = false;

//     //     CharacterPBoxDetector.Instance.ai_s.Remove(npc.GetComponent<AI>());
//     // }

//     // public GameObject WhichHand()
//     // {
//     //     GameObject handL = InputHandler.Instance.handLTransform.gameObject;
//     //     GameObject handR = InputHandler.Instance.handRTransform.gameObject;
//     //     Vector3 diffVector0 = npc.transform.position - handL.transform.position;
//     //     Vector3 diffVector1 = npc.transform.position - handR.transform.position;
//     //     float sqrLen0 = diffVector0.sqrMagnitude;
//     //     float sqrLen1 = diffVector1.sqrMagnitude;

//     //     if (sqrLen0 > sqrLen1)
//     //         return handR;
//     //     else
//     //         return handL;
//     // }


// }

// public class Idle : SoldierState
// {
//     public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
//                 : base(_npc, _agent, _anim, _player)
//     {
//         s_name = SOLDIERSTATE.IDLE;
//     }

//     public override void Enter()
//     {
//         // anim.SetFloat("speed", 0);
//         base.Enter();
//     }

//     public override void Update()
//     {
//         // if (isTaunted == true) return;
//         // // if (IsGameStarted())
//         // // {
//         // //     nextState = new TakePosition(npc, agent, anim, player);
//         // //     stage = EVENT.EXIT;
//         // // }
//         // if (CanShootPlayer())
//         // {
//         //     nextState = new Attack(npc, agent, anim, player);
//         //     stage = EVENT.EXIT;
//         // }
//         // if (CanSeePlayer())
//         // {
//         //     nextState = new TakePosition(npc, agent, anim, player);
//         //     stage = EVENT.EXIT;
//         // }
//     }

//     public override void Exit()
//     {
//         // anim.ResetTrigger("isIdle");
//         base.Exit();
//     }
// }
// // public class TakePosition : SoldierState
// // {
// //     public TakePosition(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
// //                 : base(_npc, _agent, _anim, _player)
// //     {
// //         s_name = SOLDIERSTATE.TAKEPOSITION;
// //     }

// //     public override void Enter()
// //     {
// //         anim.SetFloat("speed", 1);
// //         base.Enter();
// //     }

// //     public override void Update()
// //     {
// //         // TODO:
// //         agent.SetDestination(npc.GetComponent<Soldier>().hideTransform.position);
// //         npc.transform.LookAt(npc.GetComponent<Soldier>().hideTransform.position);

// //         if (Mathf.Abs(Vector3.Distance(npc.GetComponent<Soldier>().hideTransform.position, npc.transform.position)) < 1f)
// //         {
// //             // if (TODO: ray at görüyo mu playerı görmiyosa idlea geç.)

// //             nextState = new Attack(npc, agent, anim, player);
// //             stage = EVENT.EXIT;
// //         }
// //     }

// //     public override void Exit()
// //     {
// //         anim.SetFloat("speed", 0);
// //         base.Exit();
// //     }
// // }
// // public class Pursue : SoldierState
// // {
// //     public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
// //                 : base(_npc, _agent, _anim, _player)
// //     {
// //         // s_name = SOLDIERSTATE.PURSUE;
// //         agent.speed = 15;
// //         agent.isStopped = false;
// //     }

// //     public override void Enter()
// //     {
// //         // anim.SetTrigger("isRunning");
// //         base.Enter();
// //     }

// //     public override void Update()
// //     {
// //         agent.SetDestination(player.position);

// //         if (agent.hasPath)
// //         {
// //             if (CanAttackPlayer())
// //             {
// //                 nextState = new Attack(npc, agent, anim, player);
// //                 stage = EVENT.EXIT;
// //             }
// //             else if (!CanSeePlayer())
// //             {
// //                 // nextState = new Patrol(npc, agent, anim, player);
// //                 stage = EVENT.EXIT;
// //             }
// //         }
// //     }

// //     public override void Exit()
// //     {

// //         anim.ResetTrigger("isRunning");
// //         base.Exit();
// //     }

// //     // public void PlayerEatSomeone()
// //     // {
// //     //     // if (IsFatterThanPlayer() == false)
// //     //     // {
// //     //     //     nextState = new RunAway(npc, agent, anim, player);
// //     //     //     stage = EVENT.EXIT;
// //     //     // }
// //     // }
// // }

// // public class Attack : SoldierState
// // {
// //     float rotationSpeed = 2.0f;
// //     float time = 0;
// //     float cd = 2;
// //     int index = 0;
// //     public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
// //                 : base(_npc, _agent, _anim, _player)
// //     {

// //         s_name = SOLDIERSTATE.ATTACK;
// //     }

// //     public override void Enter()
// //     {
// //         // agent.isStopped = true;
// //         base.Enter();
// //     }

// //     public override void Update()
// //     {
// //         time += Time.deltaTime;
// //         if (time >= cd && CanShootPlayer())
// //         {
// //             Debug.Log("atttttttaaack");
// //             anim.SetTrigger("isShooting");

// //             index++;
// //             if (index % 4 == 0)
// //                 time = 0.0f;
// //             else
// //                 time = 1.95f;
// //         }

// //         Vector3 direction = player.position - npc.transform.position;
// //         float angle = Vector3.Angle(direction, npc.transform.forward);
// //         direction.y = 0;

// //         npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
// //                                             Quaternion.LookRotation(direction),
// //                                             Time.deltaTime * rotationSpeed);

// //         if (!CanAttackPlayer())
// //         {
// //             nextState = new Idle(npc, agent, anim, player);
// //             stage = EVENT.EXIT;
// //         }
// //     }

// //     public override void Exit()
// //     {
// //         anim.ResetTrigger("isShooting");
// //         base.Exit();
// //     }
// // }

// // public class RunAway : SoldierState
// // {
// //     public RunAway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
// //                 : base(_npc, _agent, _anim, _player)
// //     {
// //         s_name = SOLDIERSTATE.RUNAWAY;
// //     }

// //     public override void Enter()
// //     {
// //         anim.SetFloat("speed", 1);
// //         agent.isStopped = false;
// //         agent.speed = runAwaySpeed;
// //         base.Enter();
// //     }

// //     public override void Update()
// //     {
// //         agent.SetDestination(npc.transform.position + RunAwayDirection());

// //         // if (IsAwayFromPlayer())
// //         // {
// //         //     nextState = new Collect(npc, agent, anim, player);
// //         //     stage = EVENT.EXIT;
// //         // }
// //     }

// //     public override void Exit()
// //     {
// //         base.Exit();
// //     }
// // }

// // public class Taunt : SoldierState
// // {
// //     float time = 0;
// //     float tauntTime = 2.5f;
// //     public Taunt(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
// //                 : base(_npc, _agent, _anim, _player)
// //     {
// //         s_name = SOLDIERSTATE.IDLE;
// //     }

// //     public override void Enter()
// //     {
// //         anim.SetFloat("speed", 0);

// //         npc.GetComponent<Soldier>().stunParticle.SetActive(true);
// //         npc.GetComponent<IKLookAI>().CloseIKSlightly();

// //         base.Enter();
// //     }

// //     public override void Update()
// //     {
// //         time += Time.deltaTime;
// //         if (time >= tauntTime)
// //         {
// //             isTaunted = false;

// //             npc.GetComponent<Soldier>().stunParticle.SetActive(false);
// //             npc.GetComponent<IKLookAI>().OpenIKSlightly();

// //             nextState = new Attack(npc, agent, anim, player);
// //             stage = EVENT.EXIT;
// //         }
// //     }

// //     public override void Exit()
// //     {
// //         // anim.ResetTrigger("isIdle");
// //         base.Exit();
// //     }
// // }