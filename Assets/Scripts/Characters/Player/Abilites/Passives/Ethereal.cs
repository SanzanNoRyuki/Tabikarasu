using Characters.LifeCycle;
using Characters.Player.Skills.Elemental.Passives;

namespace Characters.Player.Abilites.Passives
{
    /// <summary>
    /// If enabled, disables <see cref="LifeCycleManager.Ticking">ticking</see>.
    /// </summary>
    public class Ethereal : Passive
    {
        private void OnEnable()
        {
            LifeCycleManager.Ticking = false;
        }

        private void OnDisable()
        {
            LifeCycleManager.Ticking = true;
        }
    }
}