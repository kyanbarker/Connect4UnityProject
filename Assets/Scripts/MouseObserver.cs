using UnityEngine;

public class MouseObserver : MonoBehaviour
{
    private void OnGameEnd()
    {
        // Setting enabled to false does not work for some reason
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        Main.Instance.Drop((int)transform.localPosition.x);
    }

    private void OnMouseOver()
    {
        Main.Instance.PreviewTokenX = (int)transform.localPosition.x;
    }
}