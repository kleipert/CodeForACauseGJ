using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthBar;
        public void SetNewHealthByPercent(int percent)
        {
            _healthBar.value = percent;
        }

        public void SetMaxHealth(int value)
        {
            _healthBar.maxValue = value;
        }
    }
}
