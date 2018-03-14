//************************************************
//Brief: Transform Extension
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2018/03/07 Created by Liuhaixia
//************************************************
using System;
using UnityEngine;

namespace Gowild {
    public static class TransformExtension {

        /// <summary>
        /// 查找父节点下所有子匹配的子对象
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindChildByName(this Transform trans, String childName) {
            int count = trans.childCount;
            while (count > 0) {
                count--;
                if (trans.GetChild(count).name.Equals(childName)) {
                    return trans.GetChild(count);
                } else {
                    Transform temp = FindChildByName(trans.GetChild(count), childName);
                    if (temp != null) {
                        return temp;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Reset for standard
        /// </summary>
        public static void ResetStandard(this Transform trans) {
            if (trans) {
                trans.localPosition = Vector3.zero;
                trans.localRotation = Quaternion.identity;
                trans.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// Reset for standard by parent transform
        /// </summary>
        public static void ResetStandard(this Transform trans, Transform parentTrans) {
            if (trans) {
                trans.SetParent(parentTrans);
                trans.ResetStandard();
            }
        }

        /// <summary>
        /// Delete all childrens
        /// </summary>
        /// <param name="trans"></param>
        public static void DestroyChildrens(this Transform trans) {
            if (trans != null) {
                int childCount = trans.childCount;
                for (int i = childCount - 1; i >= 0; i--) {
                    GameObject.DestroyImmediate(trans.GetChild(i).gameObject);
                }
            }
        }
    }
}