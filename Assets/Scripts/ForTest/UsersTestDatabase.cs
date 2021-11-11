using System.Collections.Generic;
using UnityEngine;

public class UsersTestDatabase : MonoBehaviour
{
    public List<User> Users { get; private set; }

    private List<string> _userNames;

    private Dictionary<int, string> avatars = new Dictionary<int, string>();
    private void Awake()
    {
        Users = new List<User>();
        _userNames = new List<string>();
        _userNames.Add("Алексадр");
        _userNames.Add("Андрей");
        _userNames.Add("Адель");
        _userNames.Add("Алёна");
        _userNames.Add("Алексей");
        _userNames.Add("Анастасия");
        _userNames.Add("Владислав");
        _userNames.Add("Виктория");
        _userNames.Add("Владимир");
        _userNames.Add("Вероника");
        _userNames.Add("Дмитрий");
        _userNames.Add("Денис");
        _userNames.Add("Диана");
        _userNames.Add("Кирилл");
        _userNames.Add("Кристина");
        _userNames.Add("Настя");
        _userNames.Add("Ольга");
        _userNames.Add("Олег");
        _userNames.Add("Оксана");
        _userNames.Add("Никита");
        _userNames.Add("Николай");
        _userNames.Add("Наталья");
        _userNames.Add("Наоми");
        _userNames.Add("Максим");
        _userNames.Add("Марина");
        _userNames.Add("Магомед");


        avatars.Add(0, "https://sun1-97.userapi.com/impf/c849524/v849524816/716ba/y87TknmSrmo.jpg?size=400x0&quality=90&crop=29,29,841,841&sign=3517618049aca07ce704dcf6f0140673&c_uniq_tag=-ngwI6bWY8NxGiUsJ_kLxDrnF-ec8SOzUXiotXmSvbo&ava=1");
        avatars.Add(1, "https://i.ytimg.com/vi/oaNXLibK-88/hqdefault.jpg");
        avatars.Add(2, "https://icon2.cleanpng.com/20190220/hw/kisspng-eric-cartman-stan-marsh-computer-icons-portable-ne-5c6d738d259338.0326132415506768771539.jpg");
        avatars.Add(3, "https://c0.klipartz.com/pngpicture/520/434/gratis-png-eric-cartman-south-park-el-palo-de-la-verdad-kyle-broflovski-stan-marsh-kenny-mccormick-hitler-thumbnail.png");
        avatars.Add(4, "https://i.ytimg.com/vi/_iBU_hdhiAg/hqdefault.jpg");
        avatars.Add(5, "https://sun9-16.userapi.com/c623320/v623320537/59659/1bKW9IxR7vc.jpg");
        avatars.Add(6, "https://money-h.com/picture/fin-dou/insurance-lingo-101-what-is-personal-injury-protection-pip-2.jpg");
        avatars.Add(7, "https://e7.pngegg.com/pngimages/519/735/png-clipart-eric-cartman-kyle-broflovski-kenny-mccormick-south-park-the-stick-of-truth-butters-stotch-park-child-face.png");
        avatars.Add(8, "https://yt3.ggpht.com/a/AATXAJxiqB9mtnoZdghPN9Z8Ql2E-Ce_rQ5l5E1rTg=s900-c-k-c0xffffffff-no-rj-mo");
        avatars.Add(9, "https://south-park.cz/wp-content/uploads/2018/09/unamed-3rd-4th-graders-isla.png");
        avatars.Add(10, "http://clipart-library.com/newhp/53-533310_slavery-clipart-clipart-the-south-jenny-simons-south.png");
        avatars.Add(11, "https://www.linux.org.ru/photos/131424:-227574271.png");
        avatars.Add(12, "https://yt3.ggpht.com/ytc/AAUvwniEiO0tToEmU4zOVns7SjGpBqkJ9t6Q03_CHFvzLw=s900-c-k-c0x00ffffff-no-rj");
        avatars.Add(13, "https://yt3.ggpht.com/a/AATXAJz8pLyKEKCzYmvJgJ2PZMdQQ-FgwA8QmnLb7thR=s900-c-k-c0xffffffff-no-rj-mo");
        avatars.Add(14, "https://p.kindpng.com/picc/s/388-3883518_transparent-kennys-png-kenny-south-park-png-download.png");
        avatars.Add(15, "https://w7.pngwing.com/pngs/321/499/png-transparent-kenny-mccormick-chef-kyle-broflovski-stan-marsh-black-friday-kenny-mccormick-television-orange-computer-wallpaper.png");



        CreateUsersDatabase(30);
    }
    private void CreateUsersDatabase(int usersCount)
    {
        for (int i = 0; i < usersCount; i++)
        {
            CreatRandomUser();
        }
    }
    private void CreatRandomUser()
    {
        TestUser user = new TestUser();

        int count = avatars.Count;
        int random = Random.Range(0, count);

        string randomURL = "";
        avatars.TryGetValue(random, out randomURL);

        if (string.IsNullOrEmpty(randomURL))
            return;

        user.CreateRandomInfo(_userNames[Random.Range(0, _userNames.Count)], randomURL); 
        Users.Add(user);
    }

    public User GetRandomUser()
    {
        return Users[Random.Range(0, Users.Count)];
    }
}
