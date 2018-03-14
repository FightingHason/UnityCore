//************************************************
//Brief: Reset Lost Shader
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2016/11/23 Created by Liuhaixia
//************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gowild {
    public class SetLostShader : MonoBehaviour {

        const String LOST_SHADER = "Unlit/Texture,Unlit/Transparent Cutout,Unlit/Transparent,UnityChan/Eye,UnityChan/Eye - Transparent,UnityChan/Eye - Transparent - Alpha,UnityChan/Eyelash - Transparent,UnityChan/Clothing,UnityChan/Clothing - Double-sided,UnityChan/Clothing - No Outline,UnityChan/Skin,UnityChan/Skin - Transparent,UnityChan/Skin - Item,UnityChan/Skin - Item - Alpha,UnityChan/Skin - No Outline,UnityChan/Hair,UnityChan/Hair - Double-sided,UnityChan/Hair - No Outline}";

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        Boolean _bInitFinish = false;

        List<String> _lostShaderList = new List<String>(LOST_SHADER.GetArrayBySplit());

        // Use this for initialization
        void Start() {
            if (_bInitFinish == false) {
                _SetShader(transform);
                _bInitFinish = true;
            }
        }

        /// <summary>
        /// 重设shader
        /// </summary>
        /// <param name="mTran"></param>
        void _SetShader(Transform mTran) {
            for (Int32 i = 0; i < mTran.childCount; i++) {
                Renderer render = mTran.GetChild(i).GetComponent<Renderer>();

                if (render != null) {
                    Material[] matArray = render.materials;
                    for (Int32 k = 0; k < matArray.Length; ++k) {
                        if (_lostShaderList.Contains(matArray[k].shader.name)) {
                            Shader shader = Shader.Find(matArray[k].shader.name);
                            if (shader != null) {
                                matArray[k].shader = shader;
                            }
                        }
                    }
                }

                _SetShader(mTran.GetChild(i));
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Excute() {
            if (_bInitFinish == false) {
                Start();
            } else {
                _SetShader(transform);
            }
        }

    } //end class
} //end namespace