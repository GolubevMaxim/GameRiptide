using Enemies;
using RiptideNetworking;

public static class RiptideExtensions
{
    public static void AddEnemy(this Message message, Enemy enemy)
    {
        message.AddUShort(enemy.Id);
        message.AddVector2(enemy.transform.localPosition);
    }
    
    public static void AddPlayer(this Message message, Player.Player player)
    {
        message.AddUShort(player.NetworkId);
        message.AddString(player.NickName);
        message.AddVector2(player.transform.localPosition);
        message.AddInt(player.playerHealth.HealthMax);
        message.AddInt(player.playerHealth.Health);
    }
}