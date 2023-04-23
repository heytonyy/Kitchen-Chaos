//------------------------------------------------------------------------------
// Selected Counter Visual Script (Highlight Selected Counter):
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // selected counter to watch.
    [SerializeField] private BaseCounter baseCounter;

    // selected counter visual game object.
    [SerializeField] private GameObject[] visualGameObject;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Subscribe to the OnSelectedCounterChanged event.
    private void Player_OnSelectedCounterChanged(object sender, Player.SelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObject)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObject)
        {
            visualGameObject.SetActive(false);
        }
    }
}
