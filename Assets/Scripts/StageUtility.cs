using UnityEngine;

namespace Yuuta.VRMGo
{
    
    public static class StageUtility
    {
        public static Transform FindStart()
            => GameObject.Find("Start").transform;

        public static Transform FindFinish()
            => GameObject.Find("Finish").transform;
    }
}