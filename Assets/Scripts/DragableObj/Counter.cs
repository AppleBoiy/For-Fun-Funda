using UnityEngine;

namespace DragableObj
{
    public class Counter : MonoBehaviour
    {
        private int _counter = 0;

        [Header("Count to finish")] 
        [SerializeField] private int totalCount;

        public int Couter()
        {
            return _counter;
        }

        public void AddCounter()
        {
            _counter++;
        }
    }
}