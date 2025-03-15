using Godot;
using System;

public interface IEnemyState
{
    public void NoticePlayer() { }
    public void MoveToBase() { }
    public void MoveToPlayer() { }
    public void ShootingToPlayer() { }
    public void MoveToShelter() { }
    public void MoveFromPlayer() { }
}
