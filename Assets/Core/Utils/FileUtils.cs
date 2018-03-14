//************************************************
//Brief: File Utils
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2017/04/17 Created by Liuhaixia
//************************************************
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using LitJson;
using UnityEngine;

namespace Gowild {
    public class FileUtils {

        const String DIR_TABLE = "Table/";
        static readonly Vector2 VECTOR2_CENTER = new Vector2(0.5F, 0.5F);

        #region Sprite & Texture2D

        /// <summary>
        /// 以IO方式进行加载图片
        /// </summary>
        public static Texture2D GetTexture2DByIO(String dirPath, String name) {
            return GetTexture2DByIO(Path.Combine(dirPath, name));
        }

        /// <summary>
        /// 以IO方式进行加载图片
        /// </summary>
        public static Texture2D GetTexture2DByIO(String path) {

            Texture2D texture = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            Byte[] bytes = _GetBytesByStream(path);
            if (bytes != null) {
                texture.LoadImage(bytes);
            }

            return texture;
        }

        /// <summary>
        /// 以IO方式进行加载
        /// </summary>
        public static Sprite GetSpriteByIO(String dirPath, String name) {
            return GetSpriteByIO(Path.Combine(dirPath, name));
        }

        /// <summary>
        /// 以IO方式进行加载
        /// </summary>
        public static Sprite GetSpriteByIO(String path) {

            Texture2D texture = new Texture2D(100, 100, TextureFormat.ARGB32, false);
            Byte[] bytes = _GetBytesByStream(path);
            if (bytes != null) {
                texture.LoadImage(bytes);
            }

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), VECTOR2_CENTER);
        }

        /// <summary>
        /// 以IO方式进行加载Sprite[]
        /// </summary>
        public static Sprite[] GetSpritesByIO(String dirPath) {

            Sprite[] dirImages = null;
            if (Directory.Exists(dirPath)) {
                String[] allFiles = Directory.GetFiles(dirPath);
                dirImages = new Sprite[allFiles.Length];

                for (Int32 i = 0; i < allFiles.Length; ++i) {
                    Texture2D texture = new Texture2D(273, 114);
                    Byte[] bytes = _GetBytesByStream(allFiles[i]);
                    if (bytes != null) {
                        texture.LoadImage(bytes);
                    }

                    dirImages[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), VECTOR2_CENTER);
                }
            }
            return dirImages;
        }

        /// <summary>
        /// 通过WWW获取图片
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static IEnumerator GetSpriteByWWW(String srcPath, Sprite sprite) {
            WWW www = new WWW(srcPath);
            yield return www;
            if (www != null && www.error.IsNullOrEmpty()) {
                Texture2D texture = www.texture;
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), VECTOR2_CENTER);
            } else {
                Debug.LogError("Load Error Path: " + srcPath);
            }
            www.Dispose();
            www = null;
        }

        #endregion


        #region Try Read Text

        /// <summary>
        /// Read Text For String
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean TryReadText(String path, out String info) {
            info = "";
            if (File.Exists(path)) {
                info = ReadTextByStream(path);
                if (false == info.IsNullOrEmpty()) {
                    return true;
                } else {
                    DeleteFile(path);
                    Debug.LogError("File exist, but info is null, delete file! Path: " + path);
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// Try Read Text For LitJson.JsonData
        /// </summary>
        /// <param name="path"></param>
        /// <param name="totalJson"></param>
        /// <returns></returns>
        public static Boolean TryReadText(String path, out JsonData totalJson) {
            totalJson = null;
            String jsonStr = null;
            if (File.Exists(path)) {
                jsonStr = ReadTextByStream(path);

                if (false == jsonStr.IsNullOrEmpty() && jsonStr != "\r" && jsonStr != "\n" && jsonStr != "\r\n") {
                    try {
                        totalJson = JsonMapper.ToObject(jsonStr);
                        return true;
                    } catch (Exception ex) {
                        DeleteFile(path);
                        Debug.LogError("File convert to <LitJson.JsonData> error, delete file! Path: " + path + " || Exception: " + ex.Message);
                        return false;
                    }
                } else {
                    DeleteFile(path);
                    Debug.LogError("File exist, but info is null, delete file! Path: " + path);
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// 获取sdcard xml表根节点
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="nodeRoot"></param>
        /// <returns>是否获取成功</returns>
        public static Boolean TryReadText(String path, out XmlNode rootNode) {
            rootNode = null;
            StreamReader sr;
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(path)) {
                return false;
            } else if (CheckTxtIsNull(path)) {
                DeleteFile(path);
                Debug.LogError("File exist, but info is null, delete file! Path: " + path);
                return false;
            } else {
                sr = new StreamReader(path, Encoding.UTF8);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                //xmlFilePath:xml文件路径
                XmlReader reader = XmlReader.Create(sr, settings);
                xmlDoc.Load(reader);
            }

            rootNode = xmlDoc.SelectSingleNode("Data");
            if (rootNode == null) {
                sr.Close();
                xmlDoc = null;
                DeleteFile(path);
                Debug.LogError("XML root node <Data> is not exist, delete file! Path: " + path);
                return false;
            }

            if (rootNode.ChildNodes.Count == 0) {
                sr.Close();
                rootNode = null;
                xmlDoc = null;
                DeleteFile(path);
                Debug.LogError("XML root node <Data> info is null, delete file! Path: " + path);
                return false;
            }
            sr.Close();
            return true;
        }

        /// <summary>
        /// Read Text Info For String
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean TryReadTextByResource(String relativePath, out String info) {
            info = null;
            TextAsset textAsset = Resources.Load(relativePath) as TextAsset;
            if (textAsset != null) {
                info = textAsset.text;
            }

            if (false == info.IsNullOrEmpty()) {
                return true;
            } else {
                Debug.LogError("Resources info is null！Path: " + relativePath);
                return false;
            }
        }

        /// <summary>
        /// Read Text Info For LitJson.JsonData
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="totalJson"></param>
        /// <returns></returns>
        public static Boolean TryReadTextByResource(String relativePath, out JsonData totalJson) {
            totalJson = null;
            String jsonStr = null;
            TextAsset textAsset = Resources.Load(relativePath) as TextAsset;
            if (textAsset != null) {
                jsonStr = textAsset.text;

                if (false == jsonStr.IsNullOrEmpty()) {
                    try {
                        totalJson = JsonMapper.ToObject(jsonStr);
                        return true;
                    } catch (Exception ex) {
                        Debug.LogError("Resources info convert to <LitJson.JsonData> error, delete file! Path: " + relativePath + " || Exception: " + ex.Message);
                        return false;
                    }
                } else {
                    Debug.LogError("Resources info is null！Path: " + relativePath);
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// Read Text Info By All(First Cache, Second Resources), For String
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Boolean TryReadTextByAll(String path, out String info) {
            info = null;
            if (File.Exists(path)) {
                info = ReadTextByStream(path);
                if (false == info.IsNullOrEmpty()) {
                    return true;
                } else {
                    Debug.LogError("File exist, but info is null, delete file! Path: " + path);
                    return false;
                }
            } else {
                return TryReadTextByResource(DIR_TABLE + path.LastSlashToPoint(), out info);
            }
        }

        /// <summary>
        /// Read Text Info By All(First Cache, Second Resources), For LitJson.JsonData
        /// </summary>
        /// <param name="path"></param>
        /// <param name="totalJson"></param>
        /// <returns></returns>
        public static Boolean TryReadTextByAll(String path, out JsonData totalJson) {
            totalJson = null;
            if (File.Exists(path)) {
                Boolean isRet = false;
                String jsonStr = ReadTextByStream(path);
                if (false == jsonStr.IsNullOrEmpty() && jsonStr != "\r" && jsonStr != "\n" && jsonStr != "\r\n") {
                    try {
                        totalJson = JsonMapper.ToObject(jsonStr);
                        isRet = true;
                    } catch (Exception ex) {
                        Debug.LogError("Resources info convert to <LitJson.JsonData> error, delete file! Path: " + path + " || Exception: " + ex.Message);
                        isRet = false;
                    }
                } else {
                    Debug.LogError("File exist, but info is null, delete file! Path: " + path);
                    isRet = false;
                }

                if (totalJson == null) {
                    DeleteFile(path);
                    isRet = TryReadTextByResource(DIR_TABLE + path.LastSlashToPoint(), out totalJson);
                }
                return isRet;
            } else {
                return TryReadTextByResource(DIR_TABLE + path.LastSlashToPoint(), out totalJson);
            }
        }

        #endregion

        #region Save

        /// <summary>
        /// Save JsonData Table File
        /// </summary>
        /// <param name="itemJson">save JsonData</param>
        /// <param name="isSave">save or not</param>
        /// <param name="tableName">file name</param>
        public static void SaveTextByDelete(Boolean isSave, String path, String info) {
            if (isSave) {
                FileUtils.DeleteFile(path);
                FileUtils.WriteTextByStream(path, info);

                Debug.Log("Save " + path + " Success!");
            }
        }

        #endregion


        #region Text

        /// <summary>
        /// 通过StreamReader逐行读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static String ReadTextByStreamLine(String path) {
            String result = "";

            if (File.Exists(path)) {
                try {
                    StreamReader sr = new StreamReader(path, Encoding.UTF8);
                    String line;
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null) {
                        result += line;
                    }
                    sr.Dispose();
                    sr.Close();
                    sr = null;
                } catch (Exception e) {
                    // 向用户显示出错消息
                    Debug.LogError("The file could not be read! Path: " + path + " || Message: " + e.Message);
                }
            } else {
                Debug.LogError("No exist file! Path: " + path);
            }

            return result;
        }

        /// <summary>
        /// 通过FileStream以IO方式加载txt
        /// </summary>
        /// <param name="totalPath"></param>
        /// <returns></returns>
        public static String ReadTextByIO(String path) {
            if (File.Exists(path)) {
                Byte[] bytes = _GetBytesByStream(path);
                if (bytes != null) {
                    return Encoding.UTF8.GetString(bytes);
                } else {
                    Debug.LogError("The file could not be read! Path: " + path);
                }
            } else {
                Debug.LogError("No exist file! Path: " + path);
            }
            return "";
        }

        /// <summary>
        /// 通过FileStream写入信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public static void WriteTextByIO(String path, String info) {
            try {
                FileStream fs = new FileStream(path, FileMode.Create);
                Byte[] data = Encoding.UTF8.GetBytes(info);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Dispose();
                fs.Close();
                fs = null;
            } catch (Exception e) {
                // 向用户显示出错消息
                Debug.LogError("The file could not be write! Path: " + path + " || Message: " + e.Message);
            }
        }

        /// <summary>
        /// 通过StreamReader一次性读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String ReadTextByStream(String path) {
            String result = "";

            if (File.Exists(path)) {
                try {
                    // 创建一个 StreamReader 的实例来读取文件 
                    // using 语句也能关闭 StreamReader
                    //using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                    //{
                    //    result = sr.ReadToEnd();
                    //}

                    StreamReader sr = new StreamReader(path, Encoding.UTF8);
                    result = sr.ReadToEnd();
                    sr.Dispose();
                    sr.Close();
                    sr = null;
                } catch (Exception e) {
                    // 向用户显示出错消息
                    Debug.LogError("The file could not be read! Path: " + path + " || Message: " + e.Message);
                }
            } else {
                Debug.LogError("No exist file! Path: " + path);
            }

            return result;
        }

        /// <summary>
        /// 通过StreamWriter写入信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="info"></param>
        public static void WriteTextByStream(String path, String info) {
            try {
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(info);
                sw.Flush();
                sw.Dispose();
                sw.Close();
                sw = null;

                fs.Dispose();
                fs.Close();
                fs = null;
            } catch (Exception e) {
                // 向用户显示出错消息
                Debug.LogError("The file could not be write! Path: " + path + " || Message: " + e.Message);
            }
        }

        #endregion


        #region File or Directory Operation(Delete, Create...)

        /// <summary>
        /// 检测目标路径文件
        /// </summary>
        public static Boolean ExistFile(String path) {
            if (path.IsNullOrEmpty()) {
                Debug.LogError("ExistFile Path is empty!");
                return false;
            }

            return File.Exists(path);
        }

        /// <summary>
        /// 删除目标路径文件
        /// </summary>
        public static void DeleteFile(String path) {
            if (path.IsNullOrEmpty()) {
                Debug.LogError("DeleteFile Path is empty!");
                return;
            }

            if (File.Exists(path)) {
                File.Delete(path);
            }

            CheckDirectory(path);
        }

        /// <summary>
        /// 检测目标路径文件的文件夹是否存在
        /// </summary>
        /// <param name="path">目标文件路径</param>
        public static void CheckDirectory(String path) {
            if (path.IsNullOrEmpty()) {
                Debug.LogError("CheckDirectory Path is empty!");
                return;
            }

            String dirName = path.StartToLastSlash();

            if (dirName.IsNullOrEmpty()) {
                Debug.LogError("CreateDirectory Path is empty!");
                return;
            }
            if (!Directory.Exists(dirName)) {
                Directory.CreateDirectory(dirName);
            }
        }

        /// <summary>
        /// 删除路径文件夹下所有文件（包括子文件夹）
        /// </summary>
        /// <param name="path"></param>
        public static void ClearDirFiles(String path) {
            try {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();//返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileInfo) {
                    if (i is DirectoryInfo) {//判断是否文件夹
                        DirectoryInfo subDir = new DirectoryInfo(i.FullName);
                        subDir.Delete(true);//删除子目录和文件
                    } else {
                        File.Delete(i.FullName);//删除指定文件
                    }
                }
                Debug.LogError("Delete Path: " + path + " All Info Success!");
            } catch (Exception e) {
                Debug.LogError("FileClearDir: " + path + " Exception: " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// 检测文件是否为空
        /// </summary>
        /// <returns></returns>
        public static Boolean CheckTxtIsNull(String path) {
            String info = "";
            if (File.Exists(path)) {
                info = ReadTextByStream(path);
                info = info.Replace(" ", "");
                if (false == info.IsNullOrEmpty()) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return true;
            }
        }

        #endregion


        #region MD5加解密字符串

        const String ENCRYPT_KEY = "vksxkwlvkd@rlarudals%*&@lhx";

        /// <summary>
        /// Encrypt script
        /// </summary>
        /// <param name="normalString"></param>
        /// <returns></returns>
        public static String EnscriptionMD5(String normalString) {
            if (normalString == null) {
                return null;
            }
            if (normalString.Equals(String.Empty)) {
                return String.Empty;
            }

            Byte[] results;
            UTF8Encoding UTF8 = new UTF8Encoding();

            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            Byte[] TDESKey = hashProvider.ComputeHash(UTF8.GetBytes(ENCRYPT_KEY));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            try {
                Byte[] dataToEncrypt = UTF8.GetBytes(normalString);
                ICryptoTransform encryptor = TDESAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            } catch (Exception e) {
                return "Error : " + e.ToString() + " normalString : " + normalString;
            } finally {
                TDESAlgorithm.Clear();
                hashProvider.Clear();
                TDESAlgorithm.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }

        /// <summary>
        /// Decryption script
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <returns></returns>
        public static String DescriptionMD5(String encryptedString) {
            if (encryptedString == null) {
                return null;
            }
            if (encryptedString.Equals(String.Empty)) {
                return String.Empty;
            }

            Byte[] results;

            UTF8Encoding UTF8 = new UTF8Encoding();

            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();
            Byte[] TDESKey = hashProvider.ComputeHash(UTF8.GetBytes(ENCRYPT_KEY));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            try {
                encryptedString = encryptedString.Replace(" ", "+");
                Byte[] dataToDecrypt = Convert.FromBase64String(encryptedString);
                ICryptoTransform decryptor = TDESAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            } catch (Exception e) {
                throw new Exception(e.ToString() + " encryptedString : " + encryptedString);
                //return "Error";
            } finally {
                TDESAlgorithm.Clear();
                hashProvider.Clear();
                TDESAlgorithm.Clear();
                hashProvider.Clear();
            }
            return UTF8.GetString(results);
        }

        /// <summary>
        /// Create File MD5
        /// </summary>
        /// <param name="filePath">path</param>
        /// <param name="fileMD5">MD5</param>
        /// <returns></returns>
        public static Boolean TryParseFileMD5(String filePath, out String fileMD5) {
            fileMD5 = "";
            if (!File.Exists(filePath)) {
                return false;
            }
            MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Byte[] hash = md5Generator.ComputeHash(file);
            fileMD5 = BitConverter.ToString(hash).Replace("-", "");
            file.Close();
            return true;
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// 通过IO加载文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static Byte[] _GetBytesByStream(String path) {
            if (File.Exists(path)) {
                try {
                    //创建文件读取流
                    FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    fileStream.Seek(0, SeekOrigin.Begin);
                    //创建文件长度缓冲区
                    Byte[] bytes = new Byte[fileStream.Length];
                    //读取文件
                    fileStream.Read(bytes, 0, (Int32)fileStream.Length);
                    //释放文件读取流
                    fileStream.Dispose();
                    fileStream.Close();
                    fileStream = null;
                    return bytes;
                } catch (Exception e) {
                    // 向用户显示出错消息
                    Debug.LogError("The file could not be read! Path: " + path + " || Message: " + e.Message);
                    return null;
                }
            } else {
                Debug.LogError("No Exist File! Path: " + path);
                return null;
            }
        }

        #endregion

    }// end class
}//end namespace