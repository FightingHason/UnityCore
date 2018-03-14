//************************************************
//Brief: String Extension
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2018/02/01 Created by Liuhaixia
//************************************************
using System;
using System.Collections.Generic;

namespace Gowild {
    public static class StringExtension {

        const String REPLACE_CLONE = "(Clone)";

        /// <summary>
        /// String是否为null,empty,""
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean IsNullOrEmpty(this String source) {
            return String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Whether or not number
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Boolean IsNumber(this String source) {
            if (source.IsNullOrEmpty()) {
                return false;
            }
            for (int i = 0; i < source.Length; ++i) {
                if (source[i] >= '0' && source[i] <= '9') {
                    continue;
                } else {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Delete String
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        public static String Delete(this String source, String delete) {
            if (source.IsNullOrEmpty() || delete.IsNullOrEmpty()) {
                return source;
            }
            return source.Replace(delete, CharConst.STR_EMPTY);
        }

        /// <summary>
        /// Delete "(Clone)"
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String DeleteCloneName(this String source) {
            if (source.IsNullOrEmpty()) {
                return source;
            }
            return source.Replace(REPLACE_CLONE, CharConst.STR_EMPTY);
        }

        /// <summary>
        /// 标准化路径分割符
        /// </summary>
        public static String StandardSplitForPath(this String path) {
            return path.Replace(CharConst.STR_BACK_SLASH, CharConst.STR_SLASH);
        }

        /// <summary>
        /// 剔除不需要的数据
        /// </summary>
        /// <param name="sourceList">从sourceList里面剔除数据</param>
        /// <param name="deleteList">从deleteList获取需要剔除的数据</param>
        public static void DeleteList(this List<String> sourceList, List<String> deleteList) {
            for (int j = sourceList.Count - 1; j >= 0; j--) {
                if (deleteList.Contains(sourceList[j])) {
                    sourceList.RemoveAt(j);
                }
            }
        }

        #region Split

        /// <summary>
        /// 获取Text分割后字符串数组
        /// </summary>
        /// <param name="text">需要分割的字符串</param>
        /// <param name="splitChar">分割符</param>
        /// <returns>字符串数组</returns>
        public static String[] GetArrayBySplit(this String source, Char splitChar=CharConst.CHAR_COMMA) {
            if (source.IsNullOrEmpty()) {
                return null;
            }
            return source.Split(splitChar);
        }

        /// <summary>
        /// 获取Text分割后字符串数组
        /// </summary>
        /// <param name="text">需要分割的字符串</param>
        /// <param name="splitStr">分割符</param>
        /// <returns>字符串数组</returns>
        public static String[] GetArrayBySplit(this String source, String splitStr) {
            if (source.IsNullOrEmpty()) {
                return null;
            }
            return source.Split(new String[] { splitStr }, StringSplitOptions.None);
        }

        /// <summary>
        /// 字符串数组组成字符串
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static String GetStringByArray(this String[] sourceArray, String splitStr = CharConst.STR_COMMA) {
            if (sourceArray == null) {
                return null;
            }

            String retStr = CharConst.STR_EMPTY;
            for (int i = 0; i < sourceArray.Length; i++) {
                retStr += sourceArray[i];
                if (splitStr != null && i != (sourceArray.Length - 1)) {
                    retStr += splitStr.ToString();
                }
            }
            return retStr;
        }

#endregion


        #region Substring

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="source">原始字符串</param>
        /// <param name="key">字符串key</param>
        /// <param name="isContainKey">是否包含key</param>
        /// <returns>截取成功字符串</returns>
        public static String Substring(this String source, String key, Boolean isContainKey = false) {
            Int32 startPosition = source.IndexOf(key);
            if (isContainKey) {
                return source.Substring(startPosition);
            } else {
                return source.Substring(startPosition + key.Length);
            }
        }

        /// <summary>
        /// 切割字符串，开始至第一个"/"
        /// </summary>
        public static String StartToFirstSlash(this String source) {
            Int32 length = source.IndexOf(CharConst.CHAR_SLASH);
            if (length == -1 || length == 0) {
                length = source.Length;
            }
            return source.Substring(0, length);
        }

        /// <summary>
        /// 切割字符串，开始至最后一个"/"
        /// </summary>
        public static String StartToLastSlash(this String source) {
            Int32 length = source.LastIndexOf(CharConst.CHAR_SLASH);
            if (length == -1) {
                length = source.Length;
            }
            return source.Substring(0, length);
        }

        /// <summary>
        /// 切割字符串，开始至第一个"."
        /// </summary>
        public static String StartToFirstPoint(this String source) {
            Int32 length = source.IndexOf(CharConst.CHAR_DOT);
            if (length == -1 || length == 0) {
                length = source.Length;
            }
            return source.Substring(0, length);
        }

        /// <summary>
        /// 切割字符串，开始至最后一个"."
        /// </summary>
        public static String StartToLastPoint(this String source) {
            Int32 length = source.LastIndexOf(CharConst.CHAR_DOT);
            if (length == -1) {
                length = source.Length;
            }
            return source.Substring(0, length);
        }

        /// <summary>
        /// 切割字符串，从最后一个"/"开始到字符串结束
        /// </summary>
        public static String LastSlashToEnd(this String source) {
            return source.Substring(source.LastIndexOf(CharConst.CHAR_SLASH) + 1);
        }

        /// <summary>
        /// 切割字符串，从最后一个"/"开始到第一个"."结束
        /// </summary>
        public static String LastSlashToPoint(this String source) {
            String temp = source.LastSlashToEnd();
            temp = temp.StartToFirstPoint();
            return temp;
        }

        /// <summary>
        /// 检测字符串，是否以"/"开始，如果是删除
        /// </summary>
        public static String StartWithSlash(this String source) {
            String temp = source;
            while (true) {
                if (temp.StartsWith(CharConst.STR_SLASH)) {
                    temp = temp.Substring(1, temp.Length - 1);
                } else {
                    break;
                }
            }
            return temp;
        }

        #endregion


        #region Convert To Base ValueType

        /// <summary>
        /// 字符串转Boolean
        /// </summary>
        /// <param name="text">待处理字符串</param>
        /// <param name="defaultValue">默认Char值</param>
        /// <returns>转化后Char</returns>
        public static Boolean ToBoolean(this String source, Boolean defaultValue = false) {
            if (source.IsNullOrEmpty()) {
                return defaultValue;
            } else {
                try {
                    return Convert.ToBoolean(source);
                } catch (Exception) {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 字符串转Single
        /// </summary>
        /// <param name="text">待处理字符串</param>
        /// <param name="defaultValue">默认Single值</param>
        /// <returns>转化后Single</returns>
        public static Single ToSingle(this String source, Single defaultValue = 0) {
            if (source.IsNullOrEmpty()) {
                return defaultValue;
            } else {
                try {
                    return Convert.ToSingle(source);
                } catch (Exception) {
                    return defaultValue;
                } 
            }
        }

        /// <summary>
        /// 字符串转Double
        /// </summary>
        /// <param name="text">待处理字符串</param>
        /// <param name="defaultValue">默认Double值</param>
        /// <returns>转化后Double</returns>
        public static Double ToDouble(this String source, Double defaultValue = 0) {
            if (source.IsNullOrEmpty()) {
                return defaultValue;
            } else {
                try {
                    return Convert.ToDouble(source);
                } catch (Exception) {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 字符串转Int32
        /// </summary>
        /// <param name="text">待处理字符串</param>
        /// <param name="defaultValue">默认Int32值</param>
        /// <returns>转化后Int32</returns>
        public static Int32 ToInt32(this String source, Int32 defaultValue = 0) {
            if (source.IsNullOrEmpty()) {
                return defaultValue;
            } else {
                try {
                    return Convert.ToInt32(source);
                } catch (Exception) {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="text">待处理字符串</param>
        /// <param name="defaultValue">默认Int64值</param>
        /// <returns>转化后Int64</returns>
        public static Int64 ToInt64(this String source, Int64 defaultValue = 0) {
            if (source.IsNullOrEmpty()) {
                return defaultValue;
            } else {
                try {
                    return Convert.ToInt64(source);
                } catch (Exception) {
                    return defaultValue;
                }
            }
        }

        #endregion

    }
}