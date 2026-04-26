using UnityEngine;

public class LoadingScreenView : MonoBehaviour, ILoadingScreenView
{
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
}