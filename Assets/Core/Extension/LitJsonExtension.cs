//************************************************
//Brief: Json Extension
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2017/04/17 Created by Liuhaixia
//************************************************
using LitJson;
using System;
using System.Collections;

namespace Gowild {
    public static class LitJsonExtension {

        /// <summary>
        /// 获取Json数据String
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean ContainKey(this JsonData jsonData, String key) {
            if (jsonData != null) {
                if (((IDictionary)jsonData).Contains(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取Json数据String
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetString(this JsonData jsonData, String key, String defaultValue = "") {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToString(data);
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据Single
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Single GetFloat(this JsonData jsonData, String key, Single defaultValue = 0f) {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToSingle(data.ToString());
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据Double
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Double GetDouble(this JsonData jsonData, String key, Double defaultValue = 0f) {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToDouble(data.ToString());
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据Int32
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int32 GetInt(this JsonData jsonData, String key, Int32 defaultValue = 0) {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToInt32(data.ToString());
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据Int64
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Int64 GetLong(this JsonData jsonData, String key, Int64 defaultValue = 0) {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToInt64(data.ToString());
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据Boolean
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Boolean GetBool(this JsonData jsonData, String key, Boolean defaultValue = false) {
            if (jsonData.ContainKey(key)) {
                Object data = jsonData[key];
                if (data != null) {
                    try {
                        return Convert.ToBoolean(data.ToString());
                    } catch (Exception) {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取Json数据String
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetJsonArrayString(this JsonData jsonData, String key, String defaultValue = "") {
            if (jsonData.ContainKey(key)) {
                return jsonData[key].ToJson();
            } else {
                return defaultValue;
            }
        }

    }// end class
}//end namespace

