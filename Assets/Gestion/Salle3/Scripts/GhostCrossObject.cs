using UnityEngine;
using UnityEngine.UI; 

public class GhostCroosObject : MonoBehaviour
{
    public Slider gaugeSlider;
    
    [SerializeField]
    private  float gaugeMax = 100;
    [SerializeField]
    private  float currentValue;
    [SerializeField]
    private float speedIncrease = 20f;
    [SerializeField]
    private float speedDecrease = 20f;
    private bool isCroosing = false;
    [SerializeField]
    private string layerTraversable = "WallPhase";
    private LayerMask layerDefault;
    private LayerMask layerIgnore;
    
    

    void Start()
    {
        currentValue = gaugeMax;
        UpdateGaugeUI();

        layerDefault = gameObject.layer;
        layerIgnore = LayerMask.NameToLayer(layerTraversable);

        

        Debug.Log("Layer joueur: " + layerDefault + ", Layer à traverser: " + layerIgnore);
        Physics.IgnoreLayerCollision(layerDefault, layerIgnore, false);

        
    }

    

    void Update()
    {
        if (Input.GetKey(KeyCode.G) && currentValue > 0) 
        {
            ActivatePower();
            DecreaseGauge();
        }
        else
        {
            DesactivatePower();
            RechargeGauge();
        }
        UpdateGaugeUI();
    }

    private void ActivatePower()
    {
        if (!isCroosing) 
        {
            isCroosing = true;
            Physics.IgnoreLayerCollision(layerDefault, layerIgnore, true);
        }
    }

    private void DesactivatePower()
    {
        if (isCroosing) 
        {
            isCroosing = false;
            Physics.IgnoreLayerCollision(layerDefault, layerIgnore, false);
        }
    }

    private void DecreaseGauge() 
    {
        currentValue -= speedDecrease * Time.deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0, gaugeMax);
    }

    public void RechargeGauge() 
    {
        if (currentValue < gaugeMax)
        {
            currentValue += speedIncrease * Time.deltaTime;
            currentValue = Mathf.Clamp(currentValue, 0, gaugeMax);
        }
    }

    private void OnGaugeEmpty()
    {
        Debug.Log("Jauge vide");
    }

    private void UpdateGaugeUI()
    {
        if (gaugeSlider != null)
        {
            gaugeSlider.value = currentValue;
            gaugeSlider.maxValue = gaugeMax; 
        }
    }

 

   
}
