using UnityEngine;
using System;
using System.IO;

public static class JsonDataSaver // static クラスとして定義すると、インスタンス化せずに直接呼び出せる
{
    /// <summary>
    /// 指定されたデータをJSONファイルとして保存します。
    /// </summary>
    /// <typeparam name="T">保存するデータの型</typeparam>
    /// <param name="data">保存するデータオブジェクト</param>
    /// <param name="fileName">ファイル名 (例: "ranking.json")</param>
    /// <returns>保存が成功した場合はtrue、失敗した場合はfalse</returns>
    public static bool SaveData<T>(T data, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName); // Path.Combineでパスを結合
        try
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
            Debug.Log($"データを保存しました: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"データの保存中にエラーが発生しました ({fileName}): {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// 指定されたJSONファイルからデータをロードします。
    /// </summary>
    /// <typeparam name="T">ロードするデータの型</typeparam>
    /// <param name="fileName">ファイル名 (例: "ranking.json")</param>
    /// <returns>ロードしたデータオブジェクト、失敗した場合はデフォルト値</returns>
    public static T LoadData<T>(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log($"データをロードしました: {filePath}");
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"データのロード中にエラーが発生しました ({fileName}): {e.Message}");
                return default(T); // ロード失敗時は型のデフォルト値を返す (nullなど)
            }
        }
        else
        {
            Debug.LogWarning($"ファイルが見つかりません ({fileName}): {filePath}");
            return default(T); // ファイルがない場合はnullまたはデフォルト値を返す
        }
    }

    /// <summary>
    /// 指定されたファイルを削除します。
    /// </summary>
    /// <param name="fileName">削除するファイル名</param>
    public static void DeleteData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"ファイルを削除しました: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"ファイルの削除中にエラーが発生しました ({fileName}): {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"削除対象のファイルが見つかりませんでした: {filePath}");
        }
    }
}
