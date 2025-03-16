using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    public class _3DRayShooter : MonoBehaviour
    {
        [FormerlySerializedAs("Range")] public int range;

        [SerializeField] private Transform currentItem;

        private Vector3 _clickOffSet;

        private void Update()
        {
            if (Input.GetMouseButton(0))
                if (currentItem != null)
                    currentItem.position = GetNewPos() + _clickOffSet;

            if (Input.GetMouseButtonDown(0)) SelectObject();

            if (Input.GetMouseButtonUp(0))
                if (currentItem != null)
                {
                    currentItem.gameObject.GetComponent<Renderer>().material.color = Color.white;
                    currentItem = null;
                }
        }

        private void SelectObject()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray, 100);
            foreach (var hit in hits)
                if (hit.collider != null && hit.transform.gameObject.tag != "ground" && hit.transform.gameObject.tag != "NotDragable")
                {
                    hit.transform.gameObject.SendMessage("3dHitray", SendMessageOptions.DontRequireReceiver);
                    if (currentItem == hit.transform && currentItem != null)
                    {
                        currentItem.gameObject.GetComponent<Renderer>().material.color = Color.white;
                        currentItem = null;
                    }
                    else
                    {
                        if (currentItem != null)
                            currentItem.gameObject.GetComponent<Renderer>().material.color = Color.white;

                        hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.gray;
                        currentItem = hit.transform;

                        _clickOffSet = currentItem.position - hit.point;
                        _clickOffSet.z = -1f;
                        return;
                    }
                }
        }

        private Vector3 GetNewPos()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(ray, range);

            foreach (var hit in hits)
                if (hit.collider != null)
                    if (hit.transform.gameObject.tag == "ground")
                        return hit.point;

            return Vector3.zero;
        }
    }
}
