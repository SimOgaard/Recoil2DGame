using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveTime(PlayerController playerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/time.timer";
        FileStream stream = new FileStream(path, FileMode.Create);

        TimerData timerData = new TimerData(playerController);

        formatter.Serialize(stream, timerData);
        stream.Close();
    }

    public static TimerData LoadTime()
    {
        string path = Application.persistentDataPath + "/time.timer";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TimerData timerData = formatter.Deserialize(stream) as TimerData;
            stream.Close();

            return timerData;
        }
        else
        {
            Debug.Log("error file not found");
            return null;
        }
    }
}
