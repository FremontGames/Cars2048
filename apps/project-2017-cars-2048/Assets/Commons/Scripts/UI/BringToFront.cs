using UnityEngine;
using System.Collections;

namespace Commons.UI
{
    public class BringToFront : MonoBehaviour
    {

        void OnEnable()
        {
            transform.SetAsLastSibling();
        }
    }
}