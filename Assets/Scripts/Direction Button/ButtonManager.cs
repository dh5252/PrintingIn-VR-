using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    private string _currentlySelectedButtonName = "";

    private List<ButtonBehavior> _allButtons = new List<ButtonBehavior>();

    private void Awake()
    {

        ButtonBehavior[] buttons = GetComponentsInChildren<ButtonBehavior>(includeInactive: true);
        foreach (var btn in buttons)
        {
            _allButtons.Add(btn);
        }
    }

    private void Start()
    {
        foreach (var btn in _allButtons)
        {
            btn.SetDeselected();
        }

    }

    public void OnButtonClicked(ButtonBehavior clicked)
    {
        foreach (var btn in _allButtons)
        {
            if (btn == clicked)
            {
                btn.SetSelected();
                _currentlySelectedButtonName = btn.name;
            }
            else
            {
                btn.SetDeselected();
            }
        }
    }

    public string GetSelectedButtonName()
    {
        if (_currentlySelectedButtonName == "X minus Prefab")
            return "x minus";
        else if (_currentlySelectedButtonName == "X plus Prefab")
            return "x plus";
        else if (_currentlySelectedButtonName == "Z minus Prefab")
            return "z minus";
        else if (_currentlySelectedButtonName == "Z plus Prefab")
            return "z plus";
        return _currentlySelectedButtonName;
    }

    public void SetSelectedByName(string buttonName)
    {
        foreach (var btn in _allButtons)
        {
            if (btn.name == buttonName)
            {
                OnButtonClicked(btn);
                return;
            }
        }
    }
}
