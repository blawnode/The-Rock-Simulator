using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;
using static Enums;
using UnityEngine;
using static Skill;
using System;

// TODO: Redundant? If so, remove.
public class FoeSkillSet : SkillSet
{
    public List<float> Weights { get; set; }

    public static Dictionary<Skill, float> GenerateWeightedSkills(Dictionary<SkillName, Tuple<UnityAction<Animator, List<BattleUnit>>, Texture2D, float, ETargetType>> dict)
    {
        Dictionary<Skill, float> generatedSkills = new Dictionary<Skill, float>();

        Skill newSkill;
        UnityAction<Animator, List<BattleUnit>> newAction;
        SkillName newSkillName;
        Texture2D newIcon;
        float newWeight;
        ETargetType newTargetType;

        foreach (KeyValuePair<SkillName, Tuple<UnityAction<Animator, List<BattleUnit>>, Texture2D, float, ETargetType>> item in dict)
        {
            newSkillName = item.Key;
            (newAction, newIcon, newWeight, newTargetType) = item.Value.ToValueTuple();
            newSkill = (Skill) ScriptableObject.CreateInstance(typeof(Skill));
            newSkill.Init(newSkillName, newAction, weight: newWeight);
            generatedSkills.Add(newSkill, newWeight);
        }
        return generatedSkills;
    }
}
