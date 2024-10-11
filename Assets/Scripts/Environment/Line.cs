using UnityEngine;
using UnityEngine.Events;

public enum LineType
{
    Start,
    Finish,
    DeadZone
}

public class Line : MonoBehaviour
{
    public UnityAction<LineType> LineCrossed;

    [SerializeField] private LineType _lineType;
    [SerializeField] private LayerMask _targetLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) != 0)
        {
            LineCrossed?.Invoke(_lineType);
        }
    }
}
