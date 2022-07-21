// More of a BattleChoiceManager

using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Main;
    //[InspectorName("GUI Prefab")] [SerializeField] protected GameObject _guiPfb;
    [InspectorName("Battle GUI Canvas")] [SerializeField] protected BattleUICanvas _currentGUI;
    //[InspectorName("Button Prefab")] [SerializeField] protected GameObject buttonPfb;
    //protected GameObject _buttonPfb;  // It's disabled. Found as the first child of guiPfb.

    //protected GameObject _currentGui;
    protected BattleUnit _currentAlly;
    protected Skill _selectedSkill;

    private void Start()
    {
        Main = this;
        _currentGUI.Init();
        //_buttonPfb = _guiPfb.GetFirstChild();
    }

    public void SetUnit(BattleUnit battleUnit)
    {
        _currentGUI.AppearOn(battleUnit);
        _currentAlly = battleUnit;
    }

    // Naive and hacky approach: Generate only 3 first skills, ignore the rest.
    /*public GameObject GenerateGUI(BattleUnit battleUnit)
    {
        List<Skill> skills = battleUnit.Skills;
        //_currentGui = Instantiate(_guiPfb);
        GameObject button;
        print("Generating for: " + battleUnit.name);

        // IDLE button should already exist.
        /*print("Generating IDLE button");
        //button = Instantiate(buttonPfb);
        //button.GetComponent<Button>().onClick.AddListener(OnNothingBtnPressed);
        //button.transform.SetParent(currentGui.transform);**

        for (int i = 0; i < Mathf.Min(skills.Count, 3); i++)
        {
            print("Generating " + skills[i].Name + " button -> " + skills[i].Action.Method.Name);
            button = Instantiate(_buttonPfb);
            // https://answers.unity.com/questions/1288510/buttononclickaddlistener-how-to-pass-parameter-or.html
            //button.GetComponent<Button>().onClick.AddListener(delegate { skills[i].Action(battleUnit.GetComponent<Animator>(), null); });
            button.GetComponent<Button>().onClick.AddListener(delegate { SelectSkill(skills[i], battleUnit); });
            button.transform.SetParent(_currentGui.transform);
        }

        return _currentGui;
    }*/
    
    public void SetCurrentGUI(BattleUICanvas currentGui)
    {
        _currentGUI = currentGui;
    }

    public void SelectSkill(Skill skill, BattleUnit battleUnit)
    {
        // Now, you have to select an enemy, or cancel.
        print("]]] " + skill.ToString() + ", " + battleUnit);
        _selectedSkill = skill;
        _currentGUI.ShowTargets();
        // Don't tell BattleManager.cs yet.
    }

    public void SelectSkill(int id, BattleUnit battleUnit)
    {
        // Now, you have to select an enemy, or cancel.
        if (id >= battleUnit.Skills.Count) throw new System.Exception("Cannot select id #" + id + ". There's " + battleUnit.Skills.Count + " skills.");
        // Hehe, I'm not gonna do the StringBuilder shtick, muhahaha.
        SelectSkill(battleUnit.Skills[id], battleUnit);
    }

    public void SelectNothingness(BattleUnit battleUnit)
    {
        print("zzz....");
        BattleManager.Main.AllyFinishedTurn(_currentAlly);
    }

    public void SelectTargets(List<BattleUnit> targets)
    {
        _currentAlly.UseSkill(_selectedSkill.Name, targets);
    }

    public new static void print(object str)
    {
        BattleManager.Main.print(str);
    }
}
