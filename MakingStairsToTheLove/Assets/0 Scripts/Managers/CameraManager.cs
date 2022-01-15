using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour, ILevelEndObserver, IWinObserver, ILoseObserver
{
    public static CameraManager Instance;

    [SerializeField] CinemachineVirtualCamera cam_1;
    [SerializeField] CinemachineVirtualCamera cam_2;
    [SerializeField] CinemachineVirtualCamera cam_3;

    bool levelEnd = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam_1.Priority = 20;
        cam_2.Priority = 10;
        cam_3.Priority = 10;

        Observers.Instance.Add_LevelEndObserver(this);
        Observers.Instance.Add_WinObserver(this);
        Observers.Instance.Add_LoseObserver(this);
    }
    void WinCam()
    {
        cam_1.Priority = 10;
        cam_2.Priority = 20;
    }

    void EndCam()
    {
        cam_2.Priority = 10;
        cam_3.Priority = 20;
        cam_3.Follow = null;
        cam_3.LookAt = null;
    }

    void IWinObserver.WinScenario()
    {
        WinCam();
    }

    void ILoseObserver.LoseScenario()
    {

    }

    void ILevelEndObserver.LevelEnd()
    {
        EndCam();
    }
}