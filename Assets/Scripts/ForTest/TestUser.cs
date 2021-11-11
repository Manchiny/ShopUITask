using UnityEngine;

public class TestUser : User
{
    public void CreateRandomInfo(string name, string url)
    {
        Name = name;
        Level = Random.Range(0, 100);
        ImageURL = url;
    }
}

