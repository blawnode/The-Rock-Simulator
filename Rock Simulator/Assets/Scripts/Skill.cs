using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

// Concept I will not implement: Have some attacks only be useable on certain characters, like "Pet", to pet doggos.
[CreateAssetMenu(fileName = "Skill", menuName = "RockSim/Skill", order = 1)]
public class Skill : ScriptableObject
{
    [SerializeField] UnityAction<Animator, List<BattleUnit>> _action;
    [SerializeField] SkillName _name;
    [SerializeField] float _weight = 0;
    [SerializeField] Texture2D _icon;
    [SerializeField] ETargetType _targetType = ETargetType.Single;
    public UnityAction<Animator, List<BattleUnit>> Action { get => _action; }
    public SkillName Name { get => _name; }
    public float Weight { get => _weight; }
    public ETargetType TargetType { get => _targetType; }

    // TODO: Worth considering: none. For abilities like Daftri's "Fuckin Die"
    public enum ETargetType
    {
        Single,
        All
    }

    /*public void Init(SkillName name, UnityAction<Animator, List<BattleUnit>> action)
    {
        _name = name;
        _action = action;
    }

    public void Init(SkillName name, UnityAction<Animator, List<BattleUnit>> action, Texture2D icon)
    {
        _name = name;
        _action = action;
        _icon = icon;
    }

    public void Init(SkillName name, UnityAction<Animator, List<BattleUnit>> action, float weight)
    {
        _name = name;
        _action = action;
        _weight = weight;
    }

    public void Init(SkillName name, UnityAction<Animator, List<BattleUnit>> action, Texture2D icon, float weight)
    {
        _name = name;
        _action = action;
        _icon = icon;
        _weight = weight;
    }*/

    public void Init(SkillName name, UnityAction<Animator, List<BattleUnit>> action, Texture2D icon = null, float weight = 1, ETargetType targetType = ETargetType.Single)
    {
        _name = name;
        _action = action;
        _icon = icon;
        _weight = weight;
        _targetType = targetType;
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}

// Put these in the GameManager.cs or SkillManager.cs or smth.

public class test {
    public Dictionary<SkillName, List<CharacterName>> skillToAllowedCharacters = new Dictionary<SkillName, List<CharacterName>>()
    {
        { SkillName.BusyBeziBasicAttack, new List<CharacterName>(){CharacterName.BusyBezi} },
        { SkillName.BusyBeziWiggle, new List<CharacterName>(){CharacterName.BusyBezi} },
        { SkillName.DaftriBasicAttack, new List<CharacterName>(){CharacterName.Daftri} },
        { SkillName.DaftriFart, new List<CharacterName>(){CharacterName.Daftri} },
        { SkillName.DaftriFuckingDie, new List<CharacterName>(){CharacterName.Daftri} },
        { SkillName.NotthewBasicAttack, new List<CharacterName>(){CharacterName.Notthew} },
        { SkillName.NotzBasicAttack, new List<CharacterName>(){CharacterName.Notz} },
    };
}

public class SkillProperties : List<object>
{
    
}