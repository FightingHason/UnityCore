//************************************************
//Brief: StopWatch Utils
//
//Author: Liuhaixia
//E-Mail: 609043941@qq.com
//
//History: 2016/08/23 Created by Liuhaixia
//************************************************
using System.Diagnostics;
namespace Gowild {
    public class StopWatchUtils {

        #region Singleton

        static StopWatchUtils _instance = null;
        public static StopWatchUtils Inst {
            get {
                if (_instance == null) {
                    _instance = new StopWatchUtils();
                }
                return _instance;
            }
        }

        #endregion

        Stopwatch _stopwatch = null;

        StopWatchUtils() {
            _stopwatch = new Stopwatch();
        }

        public void Start() {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        /// <summary>
        /// Stop By Tag
        /// </summary>
        /// <param name="tag"></param>
        public void Stop(string tag) {
            _stopwatch.Stop();
            if (string.IsNullOrEmpty(tag)) {
                UnityEngine.Debug.Log("StopWatch Print ElapsedMillseconds: " + _stopwatch.ElapsedMilliseconds);
            } else {
                UnityEngine.Debug.Log("Tag: " + tag + " | StopWatch Print ElapsedMillseconds: " + _stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// First Stop, Second Start
        /// </summary>
        /// <param name="tag"></param>
        public void StopAndStart(string tag) {
            Stop(tag);
            Start();
        }

        //public long ElapseTicks()
        //{
        //    return _stopwatch.ElapsedTicks;
        //}

        /// <summary>
        /// Destroy Instance
        /// </summary>
        public void Destroy() {
            _stopwatch = null;
            _instance = null;
        }
    }//end class
}//end namespace