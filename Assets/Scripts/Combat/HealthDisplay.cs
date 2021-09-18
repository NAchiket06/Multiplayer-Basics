using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject healthBarParent = null;
    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;   
    }

    private void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }
    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        //healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        healthBarImage.GetComponent<RectTransform>().localScale =  new Vector3((float)currentHealth / maxHealth,1,1);
    }
}
