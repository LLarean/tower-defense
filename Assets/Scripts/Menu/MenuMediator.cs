using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Menu
{
    public class MenuMediator : MonoBehaviour
    {
        [Inject] private RoundStarter _roundStarter;

        [Button()] public void StartMatch() => _roundStarter.StartMatch();
    }
}
