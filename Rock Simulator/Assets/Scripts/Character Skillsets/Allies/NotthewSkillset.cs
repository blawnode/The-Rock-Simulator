using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

public class NotthewSkillset : SkillSet
{
    public static List<Skill> GenerateSkills()
    {
        // https://stackoverflow.com/questions/6856356/how-to-get-a-methodbase-object-for-a-method
        //Dictionary<SkillName, UnityAction> dict = new Dictionary<SkillName, UnityAction>();
        Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>> dict = new Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>>()
        {
            {SkillName.NotthewBasicAttack, BasicAttack },
            //{SkillName.NotthewAttackAll, BasicAttackUltra },
        };
        return GenerateSkills(dict);
    }


    public static void BasicAttack(Animator animator, List<BattleUnit> targets)
    {
        const int TARGET_COUNT = 1;
        if (targets.Count != TARGET_COUNT) throw new System.Exception("Expected: " + TARGET_COUNT + " targets.");
        BasicAttack(animator, targets[0]);
    }

    public static void BasicAttack(Animator animator, BattleUnit target)
    {
        animator.GetComponent<BattleUnit>().StartCoroutine(BasicAttackAux(animator, target));
    }

    public static IEnumerator BasicAttackAux(Animator animator, BattleUnit target)
    {
        const float TIME1 = 0.666f, TIME2 = 1.08333f;
        const int DAMAGE = 80;
        Vector3 meSwordOffset = new Vector3(-2, 0, 0);  // Used to "strike" the correct place.

        Transform targetTransform = target.transform;
        BattleUnit unit = animator.GetComponent<BattleUnit>();
        animator.Play("Basic Attack");
        yield return new WaitForEndOfFrame();
        //float time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        unit.StartCoroutine(LerpMovementForthBack(unit, target, meSwordOffset, TIME1, TIME2));
        yield return new WaitForSeconds(TIME1);
        print("Hyah!");
        // Hit(Target); or something.
        target.GetDamaged(DAMAGE);

        yield return new WaitForSeconds(TIME2 - TIME1);
        // Tell BattleManager you're done? Maybe I should do this some other way..?
        BattleManager.Main.AllyFinishedTurn(unit);
    }

    public static void BasicAttackUltra(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        print("HYYYAAAAAAAAAH!!!");
        print("Yeah lmao that's it, this attack wasn't implemented yet");
        BattleManager.Main.AllyFinishedTurn(unit);
    }
}
