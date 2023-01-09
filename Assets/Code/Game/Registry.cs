using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Registry : Singleton<Registry>
{
    public List<Enemy> Enemies = new ();
    public List<Apple> Apples = new ();
    private List<Arrow> Arrows = new ();
    public List<TreeBranch> Branches = new ();

    public void Add(Apple apple)
    {
        Apples.Add(apple);
    }

    public void Remove(Apple apple)
    {
        Apples.Remove(apple);
    }

    public void Add(Arrow arrow)
    {
        Arrows.Add(arrow);
    }

    public void Remove(Arrow arrow)
    {
        Arrows.Remove(arrow);
    }

    public void Add(Enemy obj)
    {
        Enemies.Add(obj);
    }
    
    public void Remove(Enemy obj)
    {
        Enemies.Remove(obj);
    }

    public void Add(TreeBranch obj)
    {
        Branches.Add(obj);
    }
    
    public void Remove(TreeBranch obj)
    {
        Branches.Remove(obj);
    }
    
    public IEnumerable<Enemy> GetEnemies(Vector3 pos, float radius)
    {
        return Enemies.Where(e => Vector3.Distance(e.transform.position, pos) <= radius);
    }
    
    public IEnumerable<Arrow> GetArrows(Vector3 pos, float radius)
    {
        return Arrows.Where(e => Vector3.Distance(e.transform.position, pos) <= radius);
    }
    
    public IEnumerable<Apple> GetApples(Vector3 pos, float radius)
    {
        return Apples.Where(e => Vector3.Distance(e.transform.position, pos) <= radius);
    }

    public IEnumerable<TreeBranch> GetBranches(int room)
    {
        return Branches.Where(b => b.RoomIndex == room);
    }
}
