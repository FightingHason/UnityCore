//************************************************
//Brief: Delay Delete MonoBehaviour
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2016/12/05 Created by Liuhaixia
//************************************************
using System;
using System.Collections;
using UnityEngine;

namespace Gowild {
    public class DelayDelete : MonoBehaviour {

        public Single _delayTime = 0;

        Action _deleteCallback = null;

        // Use this for initialization
        void Start() {
            StartCoroutine(_Delete());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        IEnumerator _Delete() {
            yield return new WaitForSeconds(_delayTime);

            if (_deleteCallback != null) {
                _deleteCallback();
            }

            UnityEngine.Object.Destroy(this.gameObject);
        }

        /// <summary>
        /// 设置回调
        /// </summary>
        public void SetCallback(Action callback) {
            _deleteCallback = callback;
        }
    }
}