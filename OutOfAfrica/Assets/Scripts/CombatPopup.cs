using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatPopup : InteractionPopup
{
    [SerializeField] private TMP_Text _resultText;
    [Scene]
    [SerializeField] private string _combatScenePath;

    public string CombatSceneName { get; private set; }

    private PlayerUnitController _unitsGroup;
    private Predator _foe;

    private void OnDestroy()
    {
        SceneManager.UnloadSceneAsync(CombatSceneName);
    }

    protected override async void OnInit(PlayerUnitController unit, Targetable targetable)
    {
        CombatSceneName = _combatScenePath.Split("/").Last().Split(".")[0];
        _unitsGroup = unit;
        _foe = targetable.GetComponent<Predator>();

        await HandleCombat();
    }

    private async Task HandleCombat()
    {
        _resultText.gameObject.SetActive(false);
        _closeButton.gameObject.SetActive(false);

        var combatSceneLoadOperation = SceneManager.LoadSceneAsync(CombatSceneName, LoadSceneMode.Additive);

        while (combatSceneLoadOperation.isDone)
        {
            await Task.Yield();
        }

        for (int i = 0; i < 10; i++)
        {
            await Task.Yield();
        }

        var combatController = FindAnyObjectByType<CombatController>();
        if (combatController == null)
        {
            Debug.LogError("Could not find a combat controller");
            return;
        }

        await combatController.HandleCombat(_unitsGroup, _foe);

        _resultText.gameObject.SetActive(true);
        _closeButton.gameObject.SetActive(true);
    }

}
