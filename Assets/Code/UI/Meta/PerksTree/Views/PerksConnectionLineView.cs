using UnityEngine;

namespace Code.UI.Meta.PerksTree.Views
{
    public class PerksConnectionLineView : MonoBehaviour
    {
        [SerializeField] private float lineWidth;
        [SerializeField] private RectTransform lineTransform;

        public void Draw(Code.Data.Vector2Int positionA, Code.Data.Vector2Int positionB)
        {
            Draw(positionA.X, positionA.Y, positionB.X, positionB.Y);
        }

        private void Draw(float ax, float ay, float bx, float by)
        {
            var a = new Vector3(ax, ay, 0);
            var b = new Vector3(bx, by, 0);
            var dif = a - b;
        
            lineTransform.localScale = Vector3.one;
            lineTransform.localPosition = (a + b) / 2;
            lineTransform.sizeDelta = new Vector3(dif.magnitude, lineWidth);
            lineTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
    }
}
