public class PlayerModel
{
    public Observable<int> Health;
    public Observable<int> Gold;

    public Observable<BuildModel> CurrentBuilding;
    public Observable<string> Notification;

    public PlayerModel(int health, int gold, BuildModel currentBuilding)
    {
        Health = health;
        Gold = gold;
        CurrentBuilding = currentBuilding;
        Notification = string.Empty;
    }
}