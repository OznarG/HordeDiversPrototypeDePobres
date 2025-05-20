using UnityEngine;

public class FarmingLand : MonoBehaviour
{
    [SerializeField] int _ID;
    [SerializeField] int health;
    [SerializeField] float timeToGrow;
    [SerializeField] int growPhase;
    [SerializeField] float watherReminds = 0;
    [SerializeField] bool readyToPlanted;
    public bool growingSomething;

    [SerializeField] Material preMaterial;
    [SerializeField] Material afterMaterial;
    [SerializeField] MeshRenderer original;

    [Header("--- Seed Phases ---")]
    public GameObject phaseOne;
    public GameObject phaseTwo;
    public GameObject phaseThree;
    public GameObject phaseFour;

    private void Start()
    {
        original = GetComponent<MeshRenderer>();
    }
    public bool GetItem(int ID)
    {
        if(growingSomething)
        {
            Debug.Log("is full");
            return false;
            
        }
        else
        {
            _ID = ID;
            Debug.Log("Successfull");
            readyToPlanted = true;
            return true;
        }
    }
    public void AddWater()
    {
        watherReminds = 2;
    }

    public void RemoveWater()
    {
        watherReminds--;
    }
    public void IncreaseGrowingPhase()
    {
        growPhase++;
    }

    public void EndOfDayUpdate()
    {
        if(growingSomething)
        {
            if(watherReminds <= 0)
            {
                health--;
            }
            watherReminds--;
            growPhase++;
            
        }
        UpdatePlant();
        Debug.Log("End of day called");
    }

    private void UpdatePlant()
    {
        switch (growPhase)
        {
            case 0:               
                if(phaseTwo != null)
                {
                    Instantiate(phaseOne, transform.position, Quaternion.identity);
                }
                break;
            case 1:               
                if(phaseThree != null)
                {
                    Instantiate(phaseTwo, transform.position, Quaternion.identity);
                }
                break;
            case 2:
                if(phaseThree != null)
                {
                    Instantiate(phaseThree, transform.position, Quaternion.identity);
                }               
                break;
            case 3:
                if (phaseFour != null)
                {
                    Instantiate(phaseFour, transform.position, Quaternion.identity);
                }                
                break;
            default:
                break;

        }
    }

    public void GetLandReady()
    {
        original.material = afterMaterial;
        readyToPlanted = true;
    }

    public void RemoveLandReady()
    {
        original.material = preMaterial;
        readyToPlanted = false;
    }

    public bool IsReadyToPlant()
    {
        return readyToPlanted;
    }

}
