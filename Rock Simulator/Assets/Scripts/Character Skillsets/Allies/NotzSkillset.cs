using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

public class NotzSkillset : SkillSet
{
    public static List<Skill> GenerateSkills()
    {
        Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>> dict = new Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>>()
        {
            {SkillName.NotzBasicAttack, BasicAttack },
            //{SkillName.NotzHealAll, HealAll },
        };
        return GenerateSkills(dict);
    }

    public static void BasicAttack(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        unit.StartCoroutine(IBasicAttack(animator, targets));
    }

    private static IEnumerator IBasicAttack(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        print("Uuuuugggghhhh....");
        animator.Play("Ugh");
        yield return new WaitForSeconds(1.5f);

        //print("This attack wasn't implemented yet.");
        BattleManager.Main.AllyFinishedTurn(unit);
    }

    public static void HealAll(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        print("*hits every ally with the staff");
        print("This attack wasn't implemented yet.");
        BattleManager.Main.AllyFinishedTurn(unit);
    }
}
