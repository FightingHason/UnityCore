//************************************************
//Brief: List Extension
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2018/02/01 Created by Liuhaixia
//************************************************
using System;
using System.Collections.Generic;

namespace Gowild {
    public static class ListExtension {

        /// <summary>
        /// List移除所有null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        //public static void RemoveNull<T>(this List<T> list)
        //{
        //    List<int> nullIndeList = new List<int>();
        //    int listCount = list.Count;
        //    for (int i = 0; i < listCount; i++)
        //    {
        //        if (list[i] == null)
        //            nullIndeList.Add(i);
        //    }

        //    if (nullIndeList.Count > 0)
        //    {
        //        for (int i = nullIndeList.Count - 1; i >= 0; --i)
        //            list.RemoveAt(nullIndeList[i]);
        //    }
        //}

        /// <summary>
        /// 删除List所有为null元素(优化版)
        /// </summary>
        public static void RemoveNull<T>(this List<T> list) {
            Int32 count = list.Count;
            for (Int32 i = 0; i < count; ++i) {
                if (list[i] == null) {
                    //Current Position
                    Int32 newCount = i++;
                    // 对每个非空元素，复制到当前位置
                    for (; i < count; ++i) {
                        if (list[i] != null) {
                            list[newCount++] = list[i];
                        }
                    }
                    //Remove
                    list.RemoveRange(newCount, count - newCount);
                    break;
                }
            }
        }

    }
}