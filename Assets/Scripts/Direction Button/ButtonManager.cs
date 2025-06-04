using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    // 런타임 중에 이 블록에서 선택된 버튼 이름을 저장하는 변수
    private string _currentlySelectedButtonName = "";

    // 이 블록(루트) 내부의 모든 ButtonBehavior 컴포넌트를 담을 리스트
    private List<ButtonBehavior> _allButtons = new List<ButtonBehavior>();

    private void Awake()
    {
        // 1) 이 오브젝트(블록 루트)와 그 자식들 중에서 ButtonBehavior 컴포넌트를 모두 찾아서 리스트에 추가
        //    (비활성화된 버튼도 포함하려면 true)
        ButtonBehavior[] buttons = GetComponentsInChildren<ButtonBehavior>(includeInactive: true);
        foreach (var btn in buttons)
        {
            _allButtons.Add(btn);
        }
    }

    private void Start()
    {
        // 런타임 최초 시작 시점에는 특별히 어떤 버튼도 선택되어 있지 않으므로
        // 각 버튼의 SetDeselected()를 호출하여 기본 상태(=비선택)로 세팅
        foreach (var btn in _allButtons)
        {
            btn.SetDeselected();
        }

        // _currentlySelectedButtonName은 빈 문자열("") 상태 유지
    }

    /// <summary>
    /// ButtonBehavior에서 클릭(Select) 이벤트가 들어오면 호출됩니다.
    /// clicked 인자로 전달된 버튼 하나만 SetSelected()를 호출하고,
    /// 나머지 버튼들은 모두 SetDeselected()를 호출합니다.
    /// 또한 _currentlySelectedButtonName을 갱신합니다.
    /// </summary>
    /// <param name="clicked">클릭된 ButtonBehavior 객체</param>
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

    /// <summary>
    /// 현재 이 블록에서 선택된 버튼의 이름을 반환합니다.
    /// 만약 아무 버튼도 선택되지 않았다면 빈 문자열("")을 반환합니다.
    /// </summary>
    public string GetSelectedButtonName()
    {
        return _currentlySelectedButtonName;
    }

    /// <summary>
    /// 외부에서 "강제로" 어떤 버튼을 선택된 상태로 만들고 싶을 때 사용합니다.
    /// 예: 다른 스크립트에서 blockManager.SetSelectedByName("X minus Prefab") 호출
    /// </summary>
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
        // 만약 해당 이름을 가진 버튼이 없으면 아무 작업도 안 함
    }
}
