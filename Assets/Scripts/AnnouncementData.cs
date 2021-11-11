using System;

[Serializable]
public struct AnnouncementData 
{
    public AnnouncementData(Item item, int count, User user)
    {
        itemName = item.Name;
        itemCoast = item.Coast;
        this.count = count;

        userName = user.Name;
        userLevel = user.Level;
        userAvatarURL = user.ImageURL;
        itemIconName = item.IconName;
    }

    public string itemName;
    public int itemCoast;
    public int count;
    public string itemIconName;

    public string userName;
    public int userLevel;
    public string userAvatarURL;
}
