using UnityEngine;

public class Platform : MonoBehaviour
{
    public Renderer PlatformRenderer { get { return _platformRenderer; } }

    [SerializeField] private Renderer _platformRenderer;
}
