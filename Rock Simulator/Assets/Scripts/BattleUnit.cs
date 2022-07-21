using System;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

[RequireComponent(typeof(Animator))]
public class BattleUnit : MonoBehaviour
{
    [SerializeField] protected CharacterName _name = CharacterName.fof;
    [SerializeField] protected int _hp = 200;
    [SerializeField] protected bool _isFoe = true;
    [SerializeField] protected Transform _bodyCenter;
    public bool IsFoe => _isFoe;
    public bool IsDead => _hp == 0;
    public bool CanPlay => !IsDead;  // Potential: If stuns were to be implemented, they would go in this condition.
    public Vector3 BodyCenter => _bodyCenter.position;
    //[SerializeField] protected List<Skill> _skills;  // Includes attacks. Using an Action, each attack plays an animation, moves the character (lerps?), *attacks*, etc.
    protected List<Skill> _skills;  // Includes attacks. Using an Action, each attack plays an animation, moves the character (lerps?), *attacks*, etc.
    public List<Skill> Skills
    {
        get
        {
            if (_skills == null) throw new Exception("BruuuuuH the skills are empty, you can't gettem");
            return _skills;
        }
        private set
        {
            _skills = value;
        }
    }
    // Uhh... ?The list will be attached onto this character's game object hierarchy?

    protected List<float> _skillWeights;  // Only for foes.
    public List<float> Weights
    {
        get
        {
            if (_skillWeights == null) throw new Exception("BruuuuuH the weights are empty, you can't gettem");
            return _skillWeights;
        }
        private set
        {
            _skillWeights = value;
        }
    }

    private Animator _animator;

    [Header("Tests")]
    public SkillName someSkillName = SkillName.fof;  // TODO: Remove me

    /*private void Start()
    {
        Init();
    }*/

    public void Init()
    {
        GenerateSkills();
        _animator = GetComponent<Animator>();
    }

    private void GenerateSkills()
    {
        List<Skill> skillset;
        List<float> weights = null;
        Dictionary<Skill, float> weightedSkillset;
        switch(_name)
        {
            case CharacterName.Notthew:
                skillset = NotthewSkillset.GenerateSkills();
                break;
            case CharacterName.Notz:
                skillset = NotzSkillset.GenerateSkills();
                break;
            case CharacterName.Daftri:
                weightedSkillset = DaftriSkillset.GenerateWeightedSkills();
                skillset = new List<Skill>(weightedSkillset.Keys);
                weights = new List<float>(weightedSkillset.Values);
                break;
            case CharacterName.BusyBezi:
                weightedSkillset = BusyBeziSkillset.GenerateWeightedSkills();
                skillset = new List<Skill>(weightedSkillset.Keys);
                weights = new List<float>(weightedSkillset.Values);
                break;
            default:
                throw new Exception("Could not generate skills for " + _name);
        }

        Skills = skillset;
        if(_isFoe) Weights = weights;
    }
    
    bool useSkillLock = false;  // A hack to prevent the UseSkill from happening twice, with an odd bug.
    private void Update()
    {
        if(someSkillName != SkillName.fof && !useSkillLock)
        {
            useSkillLock = true;
            UseSkill(someSkillName, new List<BattleUnit>() { BattleManager.Main.ChooseRandomEnemy() });
            someSkillName = SkillName.fof;
            useSkillLock = false;
            //BattleUIManager.Main.GenerateGUI(this);
        }

        /*if (_swTest == null && _name == CharacterName.BusyBezi)
        {
            _swTest = new Stopwatch();
            _swTest.Start();
        }
        if (_name == CharacterName.BusyBezi && _swTest.Elapsed.TotalSeconds >= 0.2f)
        {
            Instantiate(_psCandy);
            _swTest.Restart();
        }*/
    }

    public void UseSkill(SkillName skillName, List<BattleUnit> targets)
    {
        Skill skill = null;
        Skills.ForEach((Skill aSkill) => { if (aSkill.Name == skillName) { skill = aSkill; return; } });
        if (skill == null)
        {
            print("Boi u serious? The attack " + skillName + "doesn't belong to " + _name);
            return;
        }

        print("dope. attacking...");
        print(skill.Name);
        skill.Action.Invoke(_animator, targets);
    }

    //public void GetDamaged(int damage, Element damageType)
    public void GetDamaged(int damage)
    {
        _hp = Mathf.Max(0, _hp - damage);
        if(_hp == 0)
        {
            print("D'auuuuuuuugggggh...");
            _animator.Play("Die");
        }
        else
        {
            print("OUch, wtf. I'm at " + _hp + ", why?!");
            _animator.Play("Ouch");
        }
    }

    public void UseRandomWeightedSkill()
    {
        Skill chosenSkill = null;

        float weightSum = 0;
        Skills.ForEach((skill) => weightSum += skill.Weight);
        print("WEIGHT: " + weightSum + ", godamit...");
        float rndNum = UnityEngine.Random.Range(0, weightSum);

        float currentWeight = 0;
        foreach (Skill skill in Skills)
        {
            /*currentWeight += skill.Weight;
            if (rndNum <= currentWeight)  // The random range is inclusive on both ends. That's why's the <=.
            {
                print("Hell yea, chosen " + skill);
                chosenSkill = skill;
            break;
            }*/

            if (rndNum <= currentWeight + skill.Weight)  // The random range is inclusive on both ends. That's why's the <=.
            {
                print("Hell yea, chosen " + skill);
                chosenSkill = skill;
                break;
            }
            currentWeight += skill.Weight;
        }

        if (chosenSkill == null) throw new Exception("Dat Whasent soopouzd tu haapen");
        List<BattleUnit> targets;
        if (chosenSkill.TargetType == Skill.ETargetType.All)
            targets = BattleManager.Main.GetAllies();
        else
            targets = new List<BattleUnit>() { BattleManager.Main.ChooseRandomAlly() };
        print(_name + " is attacking " + targets.ToStringBetter() + " with " + chosenSkill.Name);
        UseSkill(chosenSkill.Name, targets);
    }

    // NEVER let this be used by animations.
    public void FinallyDie()
    {
        print("Shit");
        BattleManager.Main.UnitDied(this);
        Destroy(gameObject);
    }

    public enum Effect
    {
        DaftriMcDeath,
        BeziCandy,
        BeziTimber
    }

    // Returns an effect game object.
    // Implementing a SpawnEffect might be a lil' problematic - each effect will probably have different paramete8s.
    public static GameObject GetEffect(Effect effect)
    {
        print("Here's an effect.");
        switch (effect)
        {
            case Effect.DaftriMcDeath:
                print("TODO McDeath, coming right up!");
                return BattleManager.Main.GetCandyPrefab();  // TODO
            case Effect.BeziCandy:
                print("The candy dealer.");
                return BattleManager.Main.GetCandyPrefab();
            case Effect.BeziTimber:
                print("Oh no, why am I falling?!.");
                throw new Exception("idk lol.");
                //return BattleManager.Main.GetCandyPrefab();
            default:
                throw new Exception("Literally impossible AGAIN, wtf.");
        }
    }
    
    // Used by animations.
    // Will only be able to spawn CERTAIN effects, probably.
    // Whatever can be spawned will be spawned generically, and might not be as flexible as wanted.
    public void SpawnEffect(Effect effect)
    {
        print("OOOH Shit, an effect!");
        switch (effect)
        {
            case Effect.DaftriMcDeath:
                print("TODO Yea, uuuuh, can I get that with a McDeath?");
                //BattleManager.Main.GetCandyPrefab();  // TODO
                break;
            case Effect.BeziCandy:
                print("Wheee, candy!!");
                GameObject candy = Instantiate(BattleManager.Main.GetCandyPrefab(), transform.position, Quaternion.identity, null);
                candy.GetComponent<PSCandy>().Launch();
                break;
            case Effect.BeziTimber:
                print("TODO UAAAAGHHHGHHHHHHHHH!!");
                Instantiate(BattleManager.Main.GetTimberDustPrefab(), transform.position, Quaternion.identity, null);
                break;
            default:
                throw new Exception("Literally impossible, wtf.");
        }
    }

    public new static void print(object str)
    {
        BattleManager.Main.print(str);
    }
}
