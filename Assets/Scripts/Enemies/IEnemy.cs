namespace Enemies
{
    public delegate void OnDeath(IEnemy enemy);
    public interface IEnemy
    {
        public event OnDeath OnDeath;
    }
}