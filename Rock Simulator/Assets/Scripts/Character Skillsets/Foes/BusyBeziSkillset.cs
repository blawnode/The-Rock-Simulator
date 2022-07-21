using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

public class BusyBeziSkillset : FoeSkillSet
{
    public static List<Skill> GenerateSkills()
    {
        Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>> dict = new Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>>()
        {
            {SkillName.BusyBeziBasicAttack, BasicAttack },
            //{SkillName.BusyBeziWiggle, Wiggle },
        };
        return GenerateSkills(dict);
    }

    public static Dictionary<Skill, float> GenerateWeightedSkills()
    {
        List<Skill> list = new List<Skill>()
        {
            new Skill(),
            ((Skill) ScriptableObject.CreateInstance(typeof(Skill))).Init()
    }
        Dictionary<SkillName, KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>> dict = new Dictionary<SkillName, KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>>()
        {
            {SkillName.BusyBeziBasicAttack, new KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>(BasicAttack, 1f) },
            //{SkillName.BusyBeziWiggle, new KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>(Wiggle, 1f) },
        };
        return GenerateWeightedSkills(dict);
    }

    public static void BasicAttack(Animator animator, List<BattleUnit> targets)
    {
        animator.GetComponent<BattleUnit>().StartCoroutine(BasicAttackAux(animator, targets));
    }

    public static IEnumerator BasicAttackAux(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();
        GameObject candyPfb = BattleUnit.GetEffect(BattleUnit.Effect.BeziCandy);
        const float TIME = 1.25f;
        const int CANDY_AMOUNT = 8;
        const int CANDY_DAMAGE = 50;

        print("*throws candies");
        for (int i = 0; i < CANDY_AMOUNT; i++)
        {
            GameObject candy = Instantiate(candyPfb, animator.transform.position, Quaternion.identity, null);
            candy.GetComponent<PSCandy>().Launch();
        }

        yield return new WaitForSeconds(TIME);
        foreach(BattleUnit target in targets)
        {
            target.GetDamaged(CANDY_DAMAGE);
        }
        
        //print("This attack wasn't implemented yet.");
        BattleManager.Main.EnemyFinishedTurn(unit);
    }

    public static void Wiggle(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        print("Woo! Oh yeah! WOOO!!!");
        print("This attack wasn't implemented yet.");
        BattleManager.Main.EnemyFinishedTurn(unit);
    }
}
