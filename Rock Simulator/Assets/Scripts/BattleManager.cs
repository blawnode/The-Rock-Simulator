using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Main;

    public enum BattleState
    {
        START,  // Intro animations? Initialization?
        ALLIES_TURN,
        ENEMIES_TURN,
        WIN,  // Same as below, possibly with a victory/spoils screen
        LOSS,  // Outro animation, no one's turn, + loss/restart screen
    }

    [SerializeField] GameObject _candyPfb;  // Probably belongs somewhere else, if this project were serious. EffectManager.cs, EffectLibrary.cs
    [SerializeField] GameObject _timberDustPfb;  // Probably belongs somewhere else, if this project were serious. EffectManager.cs, EffectLibrary.cs

    [System.NonSerialized] public BattleState _battleState;
    [System.NonSerialized] private List<BattleUnit> _alliesThatCanPlay;  // Does the manager GIVE them the turns?
    [System.NonSerialized] private List<BattleUnit> _enemiesThatCanPlay;

    // TEMP/TESTS

    // Those should probably not be serialized, in a real, practical system.
    [InspectorName("Ally Units")][SerializeField] protected List<BattleUnit> _allies = null;
    [InspectorName("Enemy Units")] [SerializeField] protected List<BattleUnit> _enemies = null;
    [InspectorName("Debug Printing")] [SerializeField] protected bool _printingAllowed = true;

    private void Start()
    {
        Main = this;
        _battleState = BattleState.START;
        _alliesThatCanPlay = new List<BattleUnit>();
        _enemiesThatCanPlay = new List<BattleUnit>();

        if(_allies == null || _enemies == null || _allies.Count == 0 || _enemies.Count == 0)
        {
            throw new System.Exception("The Allies and Enemies lists must have at least one battle unit.");
        }
        foreach(BattleUnit battleUnit in _allies)
        {
            battleUnit.Init();
        }
        foreach(BattleUnit battleUnit in _enemies)
        {
            battleUnit.Init();
        }

        StartCoroutine(PostStart());  // Let the singletons' Main be initialized.
    }

    private IEnumerator PostStart()
    {
        yield return new WaitForEndOfFrame();
        StartAlliesTurn();
    }

    private bool AreAllAlliesDead()
    {
        return AreAllUnitsDead(_allies);
    }

    private bool AreAllEnemiesDead()
    {
        return AreAllUnitsDead(_enemies);
    }

    private bool AreAllUnitsDead(List<BattleUnit> units)
    {
        foreach(BattleUnit unit in units)
        {
            if(!unit.IsDead)
            {
                return false;
            }
        }
        return true;
    }

    private void StartAlliesTurn()
    {
        print("Allies' turn!");
        _battleState = BattleState.ALLIES_TURN;
        foreach (BattleUnit unit in _allies)
        {
            SetUnitReadiness(unit);
        }
        print(" Go, " + _alliesThatCanPlay[0] + "!");
        BattleUIManager.Main.SetUnit(_alliesThatCanPlay[0]);
    }

    private void StartEnemiesTurn()
    {
        print("Enemies' turn!");
        _battleState = BattleState.ENEMIES_TURN;
        foreach (BattleUnit unit in _enemies)
        {
            SetUnitReadiness(unit);
        }
        ActivateReadyEnemies();
    }

    // Activates the first one, which will fire off the next ones using EnemyFinishedTurn.
    private void ActivateReadyEnemies()
    {
        print("CHAAAAARGE!");
        /*foreach (BattleUnit unit in _enemiesThatCanPlay)
        {
            unit.UseRandomWeightedSkill();
        }

        _enemiesThatCanPlay.Clear();*/
        _enemiesThatCanPlay[0].UseRandomWeightedSkill();
    }

    public void AllyFinishedTurn(BattleUnit unit)
    {
        print("The ally " + unit + " finished their turn!");
        _alliesThatCanPlay.Remove(unit);

        if (AreAllEnemiesDead())
        {
            print("Tutututu, tu, tu, tu! Tu-tu!!!");
            return;
        }

        if (_alliesThatCanPlay.Count == 0)
        {
            StartEnemiesTurn();
        }
        else
        {
            print(_alliesThatCanPlay.Count + " ally turns left.");
            print(" Go, " + _alliesThatCanPlay[0] + "!");
            BattleUIManager.Main.SetUnit(_alliesThatCanPlay[0]);
        }
    }

    public void EnemyFinishedTurn(BattleUnit unit)
    {
        print("The enemy " + unit + " finished their turn!");
        _enemiesThatCanPlay.Remove(unit);

        if (AreAllAlliesDead())
        {
            print("Snake: \"AGHAGHHHHHHHHHHHGHHHHHHH\"");
            return;
        }

        if (_enemiesThatCanPlay.Count == 0)
        {
            StartAlliesTurn();
        }
        else
        {
            print(_enemiesThatCanPlay.Count + " enemy turns left.");
            print(" Go, " + _enemiesThatCanPlay[0] + "!");
            _enemiesThatCanPlay[0].UseRandomWeightedSkill();
        }
    }

    // If a unit is ready, it is able to perform a move.
    private void SetUnitReadiness(BattleUnit unit)
    {
        if (!unit.CanPlay) return;
        if(unit.IsFoe)
        {
            _enemiesThatCanPlay.Add(unit);
        }
        else
        {
            _alliesThatCanPlay.Add(unit);
        }
    }

    public BattleUnit ChooseRandomAlly()
    {
        return _allies.ChooseRandomObject();
    }

    public BattleUnit ChooseRandomEnemy()
    {
        return _enemies.ChooseRandomObject();
    }

    public void UnitDied(BattleUnit unit)
    {
        print("I've fallen, and I can't get up. x(");
        if(unit.IsFoe)
        {
            _enemiesThatCanPlay.Remove(unit);  // IF the unit exists.
            _enemies.Remove(unit);
        }
        else
        {
            _alliesThatCanPlay.Remove(unit);  // IF the unit exists.
            _allies.Remove(unit);
        }
    }

    public List<BattleUnit> GetAllies() => _allies;
    public List<BattleUnit> GetEnemies() => _enemies;

    public GameObject GetCandyPrefab()
    {
        return _candyPfb;
    }

    public GameObject GetTimberDustPrefab()
    {
        return _timberDustPfb;
    }

    public new void print(object str)
    {
        if (_printingAllowed) MonoBehaviour.print(str);
    }
}
