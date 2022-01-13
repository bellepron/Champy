// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using DG.Tweening;

// public class Soldier : MonoBehaviour, ILevelStartObserver
// {
//     public NavMeshAgent agent;
//     Animator anim;
//     SoldierState currentState;

//     // Player
//     Transform player;
//     Vector3 playerPos;
//     float playerFatPoint;

//     int experiencePoint = 5;

//     public Transform hideTransform;

//     void Start()
//     {
//         agent = this.GetComponent<NavMeshAgent>();
//         anim = this.GetComponent<Animator>();
//         currentState = new Idle(this.gameObject, agent, anim, player);
//         Observers.Instance.Add_LevelStartObserver(this);
//     }
//     public void LevelStart()
//     {
//         StartCoroutine(MyUpdate());
//     }

//     IEnumerator MyUpdate()
//     {
//         while (true)
//         {
//             currentState = currentState.Process();
//             yield return null;
//         }
//     }
// }