using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUICanvas : MonoBehaviour
{
    [InspectorName("Skill Panel")] [SerializeField] protected GameObject _skillPanel;
    [InspectorName("Cancel Panel")] [SerializeField] protected GameObject _cancelPanel;

    //[InspectorName("Button Prefab")] [SerializeField] protected GameObject _buttonPfb;  // A template
    [InspectorName("Skill #1")] [SerializeField] protected Button _buttonSkill1;
    [InspectorName("Skill #2")] [SerializeField] protected Button _buttonSkill2;
    [InspectorName("Skill #3")] [SerializeField] protected Button _buttonSkill3;
    protected Button[] _buttonSkills;
    //[InspectorName("Do the Idle")] [SerializeField] protected Button _buttonIdle;
    //[InspectorName("Cancel Button")] [SerializeField] protected GameObject _cancelButton;

    [InspectorName("Selection Arrow Prefab")] [SerializeField] protected GameObject _selectionArrowPfb;

    //public GameObject SkillPanel { get => _skillPanel; }
    //public GameObject CancelPanel { get => _cancelPanel; }

    public Button ButtonSkill1 { get => _buttonSkill1; }
    public Button ButtonSkill2 { get => _buttonSkill2; }
    public Button ButtonSkill3 { get => _buttonSkill3; }
    //public Button ButtonIdle { get => _buttonIdle; }
    //public GameObject CancelButton { get => _cancelButton; }

    private BattleUnit _battleUnit;
    private List<GameObject> _selectionArrows;

    public void Init()
    {
        _buttonSkills = new Button[] { _buttonSkill1, _buttonSkill2, _buttonSkill3 };
        _selectionArrows = new List<GameObject>();
    }

    public void AppearOn(BattleUnit battleUnit)
    {
        ShowSkillPanel();
        gameObject.SetActive(true);
        transform.position = battleUnit.BodyCenter + new Vector3(4, 0, 0);
        _battleUnit = battleUnit;
        for(int i = 0; i < Mathf.Min(battleUnit.Skills.Count, 3); i++)
        {
            _buttonSkills[i].gameObject.SetActive(true);
        }
        for(int i = Mathf.Min(battleUnit.Skills.Count, 3); i < 3 ; i++)
        {
            _buttonSkills[i].gameObject.SetActive(false);
        }
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }

    public void ShowSkillPanel()
    {
        _skillPanel.SetActive(true);
        _cancelPanel.SetActive(false);
    }

    private void ShowCancelPanel()
    {
        _skillPanel.SetActive(false);
        _cancelPanel.SetActive(true);
    }

    public void SelectSkillButton(int id)
    {
        ShowCancelPanel();
        BattleUIManager.Main.SelectSkill(id, _battleUnit);
    }

    public void SelectNothingButton()
    {
        Disappear();  // Crucial that this happens before the selection.
        BattleUIManager.Main.SelectNothingness(_battleUnit);
    }

    public void SelectTargets(List<BattleUnit> targets)
    {
        DeleteArrows();
        Disappear();  // Crucial that this happens before the selection.
        print(targets.ToStringBetter() + " selected!");
        BattleUIManager.Main.SelectTargets(targets);
    }

    // Assumption: All of the enemies that are alive are valid targets.
    public void ShowTargets()
    {
        Vector3 arrowOffset = new Vector3(-2, 0, 0);
        GameObject newSelectionArrow;
        Button btn;
        foreach (BattleUnit enemy in BattleManager.Main.GetEnemies())
        {
            if (enemy.IsDead) continue;
            newSelectionArrow = Instantiate(_selectionArrowPfb, transform);
            _selectionArrows.Add(newSelectionArrow);
            btn = newSelectionArrow.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate { SelectTargets(new List<BattleUnit>() { enemy }); });
            newSelectionArrow.transform.position = enemy.BodyCenter + arrowOffset;
        }
    }

    private void DeleteArrows()
    {
        foreach (GameObject selectionArrow in _selectionArrows)
        {
            Destroy(selectionArrow);
        }
    }

    public void CancelSkillSelection()
    {
        // Back to the OG state.
        ShowSkillPanel();
        DeleteArrows();
    }

    public new static void print(object str)
    {
        BattleManager.Main.print(str);
    }
}
