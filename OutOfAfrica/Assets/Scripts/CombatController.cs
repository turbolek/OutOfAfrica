using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPoints;
    [SerializeField] private Transform[] _foeSpawnPoints;

    private PlayerUnitController _unitsGroup;
    private Predator _foe;

    private Dictionary<Man, CombatAvatar> _playerUnitsAvatars = new();
    private Dictionary<Predator, CombatAvatar> _foeAvatars = new();

    public async Task HandleCombat(PlayerUnitController unitsGroup, Predator foe)
    {
        _unitsGroup = unitsGroup;
        _foe = foe;

        if (TrySetupCombat())
        {
            await PlayoutCombat();
        }
    }

    private bool TrySetupCombat()
    {
        for (int i = 0; i < _unitsGroup.Members.Count; i++)
        {
            if (i >= _playerSpawnPoints.Length)
            {
                Debug.LogError("Not enough spawn points for player units");
                return false;
            }
            var spawnPoint = _playerSpawnPoints[i];

            var unit = _unitsGroup.Members[i];
            var unitAvatar = Instantiate(unit.CombatAvatarPrefab, spawnPoint);
            unitAvatar.Init(unit.Unit);
            _playerUnitsAvatars.Add(unit, unitAvatar);

        }

        if (_foeSpawnPoints.Length < 1)
        {
            Debug.LogError("Not enough spawn points for foes");
            return false;
        }

        var foeAvatar = Instantiate(_foe.CombatAvatarPrefab, _foeSpawnPoints[0]);
        foeAvatar.Init(_foe.Unit);
        _foeAvatars.Add(_foe, foeAvatar);

        return true;
    }

    private async Task PlayoutCombat()
    {
        while (!CheckCombatFinish())
        {
            SeekTargets();

            await Task.Yield();
        }
    }

    private bool CheckCombatFinish()
    {
        return CheckAllPlayerUnitsDead() || CheckAllFoesDead();
    }

    private bool CheckAllPlayerUnitsDead()
    {
        return _unitsGroup.Members.TrueForAll(member => member.Unit.IsDead);
    }

    private bool CheckAllFoesDead()
    {
        return _foe.Unit.IsDead;
    }

    private void SeekTargets()
    {
        foreach (var groupMember in _unitsGroup.Members)
        {
            if (!groupMember.Unit.IsFighting)
            {
                groupMember.Unit.SetTarget(_foe.Unit);
            }
        }

        if (!_foe.Unit.IsFighting)
        {
            var targetMan = _unitsGroup.Members.FirstOrDefault(member => !member.Unit.IsDead);
            var targetUnit = targetMan != null ? targetMan.Unit : null;
            _foe.Unit.SetTarget(targetUnit);
        }
    }
}
