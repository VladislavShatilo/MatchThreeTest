using UnityEngine;

public class TreeView : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int maxEnergy = 10000;

    public Renderer TargetRenderer => targetRenderer;
    public int MaxEnergy => maxEnergy;
}
