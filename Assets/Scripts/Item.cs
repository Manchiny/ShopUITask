using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _coast;
    [SerializeField] private string _iconName;

    public string Name
    {
        get => _name;
    }

    public int Coast
    {
        get => _coast;
    }

    public string IconName
    {
        get => _iconName;
    }
}
