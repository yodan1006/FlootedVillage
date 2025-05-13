using UnityEngine;

namespace TheTab.Runtime
{
    public class CellClick : MonoBehaviour
    {
        public int x;
        public int y;
        public CreatTable parentTable;

        private void OnMouseDown()
        {
            if(parentTable != null)
            {
                var renderer = GetComponent<SpriteRenderer>();
                parentTable.ToggleSandEmpty(x, y, renderer);
            }
        }
    }
}