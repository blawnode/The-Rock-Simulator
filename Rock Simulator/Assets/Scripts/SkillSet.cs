using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using static Enums;
using static Skill;

public class SkillSet : MonoBehaviour
{
    public List<Skill> Skills { get; set; }

    public static List<Skill> GenerateSkills(ICollection<SkillName> skillNames, ICollection<Tuple<UnityAction<Animator, List<BattleUnit>>, Texture2D, float, ETargetType>> skillProperties)
    {
        if (skillProperties.Count != skillNames.Count) throw new Exception("Couldn't generate skills: The amount of skill properties doesn't match the amoutn of skill names.");
        List<Skill> generatedSkills = new List<Skill>();
        Skill newSkill;
        UnityAction<Animator, List<BattleUnit>> newAction;
        SkillName newSkillName;
        Texture2D newIcon;
        float newWeight;
        ETargetType newTargetType;

        // https://stackoverflow.com/questions/1955766/iterate-two-lists-or-arrays-with-one-foreach-statement-in-c-sharp
        foreach (var item in skillProperties.Zip(skillNames, Tuple.Create))
        {
            (newAction, newIcon, newWeight, newTargetType) = item.Item1.ToValueTuple();
            newSkillName = item.Item2;
            newSkill = (Skill)ScriptableObject.CreateInstance(typeof(Skill));
            newSkill.Init(newSkillName, newAction, newIcon, newWeight, newTargetType);  // ISSUE: Need to set many things. Surely, not from scratch..?
            generatedSkills.Add(newSkill);
        }

        return generatedSkills;
    }
    /*
    public static List<Skill> GenerateSkills(ICollection<SkillName> skillNames, ICollection<UnityAction<Animator, List<BattleUnit>>> actions)
    {
        if (actions.Count != skillNames.Count) throw new Exception("Couldn't generate skills: The amount of actions doesn't match the amoutn of skill names.");
        List<Skill> generatedSkills = new List<Skill>();
        Skill newSkill;
        UnityAction<Animator, List<BattleUnit>> newAction;
        SkillName newSkillName;
        Texture2D newIcon;
        float newWeight;
        ETargetType newTargetType;

        // https://stackoverflow.com/questions/1955766/iterate-two-lists-or-arrays-with-one-foreach-statement-in-c-sharp
        foreach (var item in actions.Zip(skillNames, Tuple.Create))
        {
            newAction = item.Item1;
            newSkillName = item.Item2;
            newSkill = (Skill)ScriptableObject.CreateInstance(typeof(Skill));
            newSkill.Init(newSkillName, newAction);  // ISSUE: Need to set many things. Surely, not from scratch..?
            generatedSkills.Add(newSkill);
        }

        return generatedSkills;
    }
    */
    public static List<Skill> GenerateSkills(IDictionary<SkillName, Tuple<UnityAction<Animator, List<BattleUnit>>, Texture2D, float, ETargetType>> actionsNSkillNames)
    {
        ICollection<SkillName> skillNames = actionsNSkillNames.Keys;
        //ICollection<UnityAction<Animator, List<BattleUnit>>> actions = actionsNSkillNames.Values;
        ICollection<Tuple<UnityAction<Animator, List<BattleUnit>>, Texture2D, float, ETargetType>> actions = actionsNSkillNames.Values;
        return GenerateSkills(skillNames, actions);
    }

    public static IEnumerator LerpMovementForthBack(BattleUnit me, BattleUnit target, Vector3 meAttackOffset, float time1 = 1, float time2 = 1)
    {
        Transform meTransform = me.transform;
        Vector3 meStartPosition = me.transform.position;
        Vector3 targetTransform = target.transform.position + meAttackOffset;
        float startTime = Time.time;
        float endTime = startTime + time1;
        float t;
        float x1 = meTransform.position.x, x2 = targetTransform.x;
        float y1 = meTransform.position.y, y2 = targetTransform.y;
        while (Time.time <= endTime)
        {
            t = (Time.time - startTime) / time1;
            meTransform.position = new Vector3(Mathf.Lerp(x1, x2, t), Mathf.Lerp(y1, y2, t), meTransform.position.z);
            yield return new WaitForEndOfFrame();
        }

        startTime = Time.time;
        float timeDiff = time2 - time1;
        endTime = startTime + timeDiff;
        while (Time.time <= endTime)
        {
            t = (Time.time - startTime) / timeDiff;
            meTransform.position = new Vector3(Mathf.Lerp(x2, x1, t), Mathf.Lerp(y2, y1, t), meTransform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }

    public static void print(string str)
    {
        BattleManager.Main.print(str);
    }
}
