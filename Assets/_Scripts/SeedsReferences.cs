using Unity.VisualScripting;
using UnityEngine;

public class SeedsReferences : MonoBehaviour
{
    public static SeedsReferences instance;
    [Header("--- Apple Seed ---")]
    public GameObject phaseOneApple;
    public GameObject phaseTwoApple;
    public GameObject phaseThreeApple;
    public GameObject phaseFourApple;

    [Header("--- Tomatoe Seed ---")]
    public GameObject phaseOneTomatoe;
    public GameObject phaseTwoTomatoe;
    public GameObject phaseThreeTomatoe;
    public GameObject phaseFourTomatoe;

    [Header("--- Punkin Seed ---")]
    public GameObject phaseOnePunkim;
    public GameObject phaseTwoPunkim;
    public GameObject phaseThreePunkim;
    public GameObject phaseFourPunkim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public bool AddSeed(int id, FarmingLand land)
    {
        
        if(id == 200)
        {
            if(land.growingSomething)
            {
                return false;
            }
            else
            {
                Debug.Log("Seed Added");

                land.phaseOne = phaseOneApple;
                land.phaseTwo = phaseTwoApple;
                land.phaseThree = phaseThreeApple;
                land.phaseFour = phaseFourApple;
                land.EndOfDayUpdate();
                land.growingSomething = true;
                return true;
            }
            
            
        }
        else if(id == 250)
        {
            if (land.growingSomething)
            {
                return false;
            }
            else
            {
                Debug.Log("Seed Added");

                land.phaseOne =   phaseOneTomatoe;
                land.phaseTwo =   phaseTwoTomatoe;
                land.phaseThree = phaseThreeTomatoe;
                land.phaseFour = phaseFourTomatoe;
                land.EndOfDayUpdate();
                land.growingSomething = true;
                return true;
            }
        }
        else if(id == 300)
        {
            if (land.growingSomething)
            {
                return false;
            }
            else
            {
                Debug.Log("Seed Added");

                land.phaseOne =    phaseOnePunkim;
                land.phaseTwo =    phaseTwoPunkim;
                land.phaseThree =  phaseThreePunkim;
                land.phaseFour = phaseFourPunkim;
                land.EndOfDayUpdate();
                land.growingSomething = true;
                return true;
            }
        }
        return false;
    }
}
