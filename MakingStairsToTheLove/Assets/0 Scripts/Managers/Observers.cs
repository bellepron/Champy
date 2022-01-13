using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWinObserver
{
    void WinScenario();
}
public interface ILoseObserver
{
    void LoseScenario();
}
public interface ILevelEndObserver
{
    void LevelEnd();
}
public interface ILevelStartObserver
{
    void LevelStart();
}
public class Observers : MonoBehaviour
{
    public static Observers Instance;

    private void Awake()
    {
        Instance = this;

        levelStartObservers = new List<ILevelStartObserver>();
        winObservers = new List<IWinObserver>();
        loseObservers = new List<ILoseObserver>();
        levelEndObservers = new List<ILevelEndObserver>();
    }



    #region Observer Functions

    private List<IWinObserver> winObservers;
    private List<ILoseObserver> loseObservers;
    private List<ILevelEndObserver> levelEndObservers;
    private List<ILevelStartObserver> levelStartObservers;

    #region Level Start Observer
    public void Add_LevelStartObserver(ILevelStartObserver observer)
    {
        levelStartObservers.Add(observer);
    }
    public void Remove_LevelStartObserver(ILevelStartObserver observer)
    {
        levelStartObservers.Remove(observer);
    }
    public void Notify_LevelStartObservers()
    {
        foreach (ILevelStartObserver observer in levelStartObservers.ToArray())
        {
            if (levelStartObservers.Contains(observer))
                observer.LevelStart();
        }
    }
    #endregion

    #region Win Observer
    public void Add_WinObserver(IWinObserver observer)
    {
        winObservers.Add(observer);
    }
    public void Remove_WinObserver(IWinObserver observer)
    {
        winObservers.Remove(observer);
    }
    public void Notify_WinObservers()
    {
        foreach (IWinObserver observer in winObservers.ToArray())
        {
            if (winObservers.Contains(observer))
                observer.WinScenario();
        }
    }
    #endregion

    #region Lose Observer
    public void Add_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Add(observer);
    }
    public void Remove_LoseObserver(ILoseObserver observer)
    {
        loseObservers.Remove(observer);
    }
    public void Notify_LoseObservers()
    {
        foreach (ILoseObserver observer in loseObservers.ToArray())
        {
            if (loseObservers.Contains(observer))
                observer.LoseScenario();
        }
    }
    #endregion

    #region End Level Observer
    public void Add_LevelEndObserver(ILevelEndObserver observer)
    {
        levelEndObservers.Add(observer);
    }

    public void Remove_LevelEndObserver(ILevelEndObserver observer)
    {
        levelEndObservers.Remove(observer);
    }

    public void Notify_LevelEndObservers()
    {
        foreach (ILevelEndObserver observer in levelEndObservers.ToArray())
        {
            if (levelEndObservers.Contains(observer))
                observer.LevelEnd();
        }
    }
    #endregion

    #endregion
}