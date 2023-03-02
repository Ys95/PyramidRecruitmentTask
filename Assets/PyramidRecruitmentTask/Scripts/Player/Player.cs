using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class Player : MonoBehaviour
    {
        public int OwnedKeys { get; private set; }

        public void AddKey() => OwnedKeys++;

        public void UseKey() => OwnedKeys--;
    }
}