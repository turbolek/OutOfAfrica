using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private CameraVariable _combatCameraVariable;

    private CombatAvatar _combatAvatar;

    public void Init(CombatAvatar combatAvatar)
    {
        _combatAvatar = combatAvatar;
        _combatAvatar.HealthChanged += OnHealthChanged;
        _combatCameraVariable.Modified += OnCombatCameraModified;

        UpdateHealthbar(_combatAvatar.Unit.CurrentHP, _combatAvatar.Unit.HP);
        HandlePosition(_combatCameraVariable.Value);
    }

    public void Clear()
    {
        _combatAvatar.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        UpdateHealthbar(_combatAvatar.Unit.CurrentHP, _combatAvatar.Unit.HP);
    }

    private void OnCombatCameraModified((Camera newCamera, Camera previousCamera) args)
    {
        if (args.newCamera != null)
        {
            HandlePosition(args.newCamera);
        }
    }


    private void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        _fillImage.fillAmount = currentHealth / maxHealth;
    }

    private void HandlePosition(Camera camera)
    {
        transform.position = camera.WorldToScreenPoint(_combatAvatar.transform.position) + (Vector3)_offset;
    }
}
