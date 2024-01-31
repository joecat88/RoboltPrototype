using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class ButtonClickPerimeter : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
        }
        
    }
}
