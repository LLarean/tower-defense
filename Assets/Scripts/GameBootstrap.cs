using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Start()
    {
        PlayerModel playerModel = new PlayerModel
        {
            Health = 100,
            Gold = 100
        };
    }
}