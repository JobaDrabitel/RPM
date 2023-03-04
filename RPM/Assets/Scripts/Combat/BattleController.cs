using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private BattleHUD _battleHUD;
    [SerializeField] private BattleContainer _battleContainer;
    private Unit _skillTarget;
    private Unit _currentUnit;
    private bool _isSkillUsed = false;
    private int _currentUnitNumber = 0;
    private Unit[] _sortedUnits = new Unit[8];
    private void Start()
    {
        SpawnUnit();
        SetTurnsOrder();
        ChangeTurn();
        _battleHUD.SetupBattleHUD(_battleContainer.units);
    }
    private void UseSkill(int index, Unit unit, Unit target)
    {
        Skill skill = unit.GetSkill(index);
        skill.Target = target;
        skill.AddEffect();
        skill.CauseEffect();
        _battleHUD.HPBarValueChange(_battleContainer.units);
        Debug.Log(target.CurrentHP);
        _skillTarget = null;
        _isSkillUsed = false;
        unit.State = Unit.StateMachine.WAIT;
        ChangeTurn();
        if (!_currentUnit.IsPlayable)
            StartCoroutine(EnemyAction());
    }

    public void OnSkill3ButtonClick()
    {
        if (_currentUnit.IsPlayable)
        {
            Skill skill = _currentUnit.GetSkill(3);
            _battleHUD.DisplaySkillDescriprion(skill);
            _isSkillUsed = true;
            StartCoroutine(PlayerAction(2));
        }
    }
    public void OnSkill2ButtonClick()
    {
        if (_currentUnit.IsPlayable)
        {
            Skill skill = _currentUnit.GetSkill(2);
            _battleHUD.DisplaySkillDescriprion(skill);
            _isSkillUsed = true;
            StartCoroutine(PlayerAction(1));
        }
    }
    public void OnSkill1ButtonClick()
    {
        if (_currentUnit.IsPlayable)
        {
            Skill skill = _currentUnit.GetSkill(0);
            _battleHUD.DisplaySkillDescriprion(skill);
            _isSkillUsed = true;
            StartCoroutine(PlayerAction(0));
        }
    }
    public void ChangeTurn()
    {
       _currentUnit = _sortedUnits[_currentUnitNumber];
        _sortedUnits[_currentUnitNumber].State = Unit.StateMachine.TURN;
        _currentUnitNumber++;
        string log = $"����� ���� ��� ������� {_currentUnitNumber}";
        Debug.Log(log);
        if (_currentUnitNumber >= _sortedUnits.Length)
            _currentUnitNumber = 0;
    }
    public IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(1f);
            UseSkill(0, _currentUnit, _battleContainer.units[3]);
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator PlayerAction(int index)
    {
        yield return new WaitUntil(() => _skillTarget != null);
        UseSkill(index, _currentUnit, _skillTarget);
        yield return new WaitForSeconds(1f);

    }
    public void SpawnUnit()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i <= 3)
            {
                GameObject playerClone = Instantiate(_battleContainer.prefabs[i], _battleContainer.spawnpoints[i]);
                playerClone.GetComponent<Unit>().IsPlayable = true;
                _battleContainer.units[i] = playerClone.GetComponent<Unit>();
            }
            else
            {
                GameObject enemyClone = Instantiate(_battleContainer.prefabs[i], _battleContainer.spawnpoints[i]);
                enemyClone.GetComponent<Unit>().IsPlayable = false;
                _battleContainer.units[i] = enemyClone.GetComponent<Unit>();
            }
        }
    }
    public void SetTurnsOrder()
    {
        for (int i = 0; i < 8; i++)
        _sortedUnits[i] = _battleContainer.units[i];
        Unit temp;
        for (int write = 0; write < _sortedUnits.Length; write++)
        {
            for (int sort = 0; sort < _sortedUnits.Length-1; sort++)
            {
                if (_sortedUnits[sort].SetRandomInitiative() < _sortedUnits[sort + 1].SetRandomInitiative())
                {
                    temp = _sortedUnits[sort + 1];
                    _sortedUnits[sort + 1] = _sortedUnits[sort];
                    _sortedUnits[sort] = temp;
                }
            }
        }
    }
    public void GetTarget(int index)
    {
        if (_isSkillUsed && _battleContainer.units[index].State!=Unit.StateMachine.DEAD)
            _skillTarget = _battleContainer.units[index];
            _battleHUD.DisplayUnitDescription(_battleContainer.units[index]);

    }
}

