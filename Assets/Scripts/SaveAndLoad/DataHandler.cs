using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class DataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public DataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    #region Data action
    /// <summary>
    /// Handles to save data to file.
    /// </summary>
    /// <param name="_gameData"></param>
    public void SaveData(GameData _gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            string dataToStore = JsonConvert.SerializeObject(_gameData, Formatting.Indented);
            byte[] key = GenerateKey();
            byte[] iv = GenerateIV();
            string encryptData = EncryptData(dataToStore, key, iv);

            string directory = Path.GetDirectoryName(fullPath);
            if (!File.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (FileStream stream = new(fullPath, FileMode.Create))
            using (StreamWriter writer = new(stream))
            {
                writer.Write(Convert.ToBase64String(key) + "\n" + Convert.ToBase64String(iv) + "\n" + encryptData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when trying to save data: {fullPath}\n{ex}");
        }
    }

    /// <summary>
    /// Handles to load data from file.
    /// </summary>
    /// <returns></returns>
    public GameData LoadData()
    {
        GameData gameData = null;
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            try
            {
                string[] lines = File.ReadAllLines(fullPath);
                byte[] key = Convert.FromBase64String(lines[0]);
                byte[] iv = Convert.FromBase64String(lines[1]);
                string encryptData = lines[2];
                string dataToLoad = DecryptData(encryptData, key, iv);

                gameData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error when trying to load data: {fullPath}\n{ex}");
            }
        }

        return gameData;
    }

    /// <summary>
    /// Handles to delete data file.
    /// </summary>
    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
    #endregion

    #region Encrypt and Decrypt file
    /// <summary>
    /// Handles to generate key.
    /// </summary>
    /// <returns></returns>
    private byte[] GenerateKey()
    {
        using (RNGCryptoServiceProvider rng = new())
        {
            byte[] key = new byte[32];
            rng.GetBytes(key);

            return key;
        }
    }

    /// <summary>
    /// Handles to generate initialization vector.
    /// </summary>
    /// <returns></returns>
    private byte[] GenerateIV()
    {
        using (RNGCryptoServiceProvider rng = new())
        {
            byte[] iv = new byte[16];
            rng.GetBytes(iv);

            return iv;
        }
    }

    /// <summary>
    /// Handles to encrypt data.
    /// </summary>
    /// <param name="_dataToEncrypt"></param>
    /// <param name="_key"></param>
    /// <param name="_iv"></param>
    /// <returns></returns>
    private string EncryptData(string _dataToEncrypt, byte[] _key, byte[] _iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream msEncrypt = new())
            {
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(_dataToEncrypt);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    /// <summary>
    /// Handles to decrypt data.
    /// </summary>
    /// <param name="_dataToDecrypt"></param>
    /// <param name="_key"></param>
    /// <param name="_iv"></param>
    /// <returns></returns>
    private string DecryptData(string _dataToDecrypt, byte[] _key, byte[] _iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream msDecrypt = new(Convert.FromBase64String(_dataToDecrypt)))
            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
    #endregion
}
