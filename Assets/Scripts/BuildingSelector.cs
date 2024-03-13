using TMPro;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    // _title.text = hit.collider.gameObject.name;
                }
            }
        }
    }
}
