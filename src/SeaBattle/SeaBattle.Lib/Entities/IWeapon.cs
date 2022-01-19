namespace SeaBattle.Lib.Entities
{
    public interface IWeapon : IEntity
    {
        ushort Damage { get; }

        ushort AttackRange { get; }
    }
}