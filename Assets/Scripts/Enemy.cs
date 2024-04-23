public abstract class Enemy : Entity
{
    public double DistanceToPlayer { get; set; }
    protected abstract void Run();

}
