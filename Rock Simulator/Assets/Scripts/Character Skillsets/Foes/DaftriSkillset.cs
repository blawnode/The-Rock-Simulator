using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Enums;

public class DaftriSkillset : FoeSkillSet
{
    public static List<Skill> GenerateSkills()
    {
        Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>> dict = new Dictionary<SkillName, UnityAction<Animator, List<BattleUnit>>>()
        {
            {SkillName.DaftriBasicAttack, BasicAttack},
            //{SkillName.DaftriFart, Fart},
            {SkillName.DaftriFuckingDie, FuckinDie},
        };
        return GenerateSkills(dict);
    }

    public static Dictionary<Skill, float> GenerateWeightedSkills()
    {
        Dictionary<SkillName, KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>> dict = new Dictionary<SkillName, KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>>()
        {
            {SkillName.DaftriBasicAttack, new KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>(BasicAttack, 1f) },
            //{SkillName.DaftriFart, new KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>(Fart, 1f) },
            {SkillName.DaftriFuckingDie, new KeyValuePair<UnityAction<Animator, List<BattleUnit>>, float>(FuckinDie, 0.2f) },
        };
        return GenerateWeightedSkills(dict);
    }

    public static void BasicAttack(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();
        unit.StartCoroutine(BasicAttackAux(animator, targets));
    }

    public static IEnumerator BasicAttackAux(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();
        const float TIME_SLIP_INTRO = 1f;  // Animation plays for 1 second, the turn is passed, and the animation still continues for a lot of time.

        print("*eye slips further down Daftri's face");
        
        animator.Play("Eye Slip");
        yield return new WaitForSeconds(TIME_SLIP_INTRO);
        
        BattleManager.Main.EnemyFinishedTurn(unit);
    }

    public static void Fart(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        print("*shart.wav");
        print("This attack wasn't implemented yet.");
        BattleManager.Main.EnemyFinishedTurn(unit);
    }

    public static void FuckinDie(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();

        unit.StartCoroutine(FuckinDieAux(animator, targets));
    }

    private static IEnumerator FuckinDieAux(Animator animator, List<BattleUnit> targets)
    {
        BattleUnit unit = animator.GetComponent<BattleUnit>();
        GameObject mcDeath = BattleUnit.GetEffect(BattleUnit.Effect.DaftriMcDeath);
        const float TIME1_ANTICIPATION = 1.5f, TIME2_DEATH = 1f;

        animator.Play("Fuckin' Die");
        yield return new WaitForSeconds(TIME1_ANTICIPATION);
        print("*lego_yoda_death.mp4");  // DEBUG
        yield return new WaitForSeconds(TIME2_DEATH);
        Instantiate(mcDeath, animator.transform.position, Quaternion.identity, null);
        //Destroy(animator.gameObject);  // NEVER call Destroy like this, unless you're using BattleUnit.Die().
        BattleManager.Main.EnemyFinishedTurn(unit);
        unit.FinallyDie();
    }
}
