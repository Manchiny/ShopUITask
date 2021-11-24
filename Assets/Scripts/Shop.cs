using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }
    public List<AnnouncementData> AnnouncementsData { get; private set; }
    private string _path;

    private UsersTestDatabase _usersDatabase;
    private ItemsTestDatabase _itemsDatabase;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            AnnouncementsData = new List<AnnouncementData>();
            _path = Path.Combine(Application.persistentDataPath, "Announcements.json");

            return;
        }

        Destroy(gameObject);
    }
    private void Start()
    {
        LoadTestData(100); // создаю случайные объ€влени€ дл€ тестировани€.
    }

    private void LoadTestData(int countOfAnnouncements)
    {
        _usersDatabase = FindObjectOfType<UsersTestDatabase>();
        _itemsDatabase = FindObjectOfType<ItemsTestDatabase>();

        //if (File.Exists(_path))
        //{
        //    LoadDataFromJSON();
        //}
        //else
        //{
            for (int i = 0; i < countOfAnnouncements; i++)
            {
                Item item = _itemsDatabase.GetRandomItem();
                User user = _usersDatabase.GetRandomUser();
                int count = Random.Range(1, 51);
                CreateAnnouncement(item, count, user);
            }

            SaveDataToJSON();
        //}
    }

    private void CreateAnnouncement(Item item, int count, User user)
    {
        AnnouncementsData.Add(new AnnouncementData(item, count, user));
    }

    private void SaveDataToJSON()
    {
        FileHandler.SaveListToJSON<AnnouncementData>(AnnouncementsData, _path);
    }

    private void LoadDataFromJSON()
    {
        AnnouncementsData = FileHandler.ReadListFromJSON<AnnouncementData>(_path);
    }
}
