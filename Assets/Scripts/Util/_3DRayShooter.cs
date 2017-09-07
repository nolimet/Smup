using UnityEngine;
using System.Collections;

namespace Util
{
    public class _3DRayShooter : MonoBehaviour
    {

        public int Range;

        [SerializeField]
        private Transform currentItem;

        private Vector3 ClickOffSet;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (currentItem != null)
                {
                    currentItem.position = getNewPos() + ClickOffSet;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                selectObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentItem != null)
                {
                    currentItem.gameObject.GetComponent<Renderer>().material.color = Color.white;
                    currentItem = null;
                }
            }
        }

        void selectObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            foreach (RaycastHit hit in hits)
            {
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

                        ClickOffSet = currentItem.position - hit.point;
                        ClickOffSet.z = -1f;
                        return;
                    }

                }
            }
        }

        Vector3 getNewPos()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Range);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.transform.gameObject.tag == "ground")
                    {
                        return hit.point;
                    }
                }
            }
            return Vector3.zero;
        }
    }
}