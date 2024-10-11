using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour, IRestartable
{
    [SerializeField] private List<GameObject> _obstacles = new List<GameObject>();

    public void Restart()
    {
        for (int i = 0; i < _obstacles.Count; i++)
        {
            if (_obstacles[i].TryGetComponent(out IRestartable restartable))
            {
                restartable.Restart();
            }
        }
    }
}
