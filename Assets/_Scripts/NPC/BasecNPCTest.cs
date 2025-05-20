using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BasecNPCTest : MonoBehaviour
{
    [SerializeField] float affectionLevel;
    [SerializeField] bool female;
    [SerializeField] Sprite characterProfile;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    [SerializeField] GameObject normalPathOne;
    [SerializeField] GameObject normalPathTwo;
    [SerializeField] GameObject normalPathThree;
    [SerializeField] GameObject specialPathOne;
    [SerializeField] GameObject specialPathTwo;
    [SerializeField] GameObject specialPathThree;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            WelcomeTextGenerator();
            PlayText();
        }
    }
    private void Update() //Need a better way for this
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        
    }
    public void GetPath()
    {
        if((int)gameManager.instance.timer.day < 4 && (int)gameManager.instance.timer.day > 0)
        {
            if(gameManager.instance.timer.roamerCalculator >= 0 && gameManager.instance.timer.roamerCalculator <= 19)
            {
                agent.SetDestination(normalPathOne.transform.position);
            }
            if (gameManager.instance.timer.roamerCalculator >= 20 && gameManager.instance.timer.roamerCalculator <= 39)
            {
                agent.SetDestination(normalPathTwo.transform.position);
            }
            if (gameManager.instance.timer.roamerCalculator >= 40 && gameManager.instance.timer.roamerCalculator <= 60)
            {
                agent.SetDestination(normalPathThree.transform.position);
            }
        }
        else
        {
            if (gameManager.instance.timer.roamerCalculator >= 0 && gameManager.instance.timer.roamerCalculator <= 19)
            {
                agent.SetDestination(specialPathOne.transform.position);
            }
            if (gameManager.instance.timer.roamerCalculator >= 20 && gameManager.instance.timer.roamerCalculator <= 39)
            {
                agent.SetDestination(specialPathTwo.transform.position);
            }
            if (gameManager.instance.timer.roamerCalculator >= 40 && gameManager.instance.timer.roamerCalculator <= 60)
            {
                agent.SetDestination(specialPathThree.transform.position);
            }
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        gameManager.instance.profilePicture.sprite = null;
    //        gameManager.instance.chatText.text = " ";
    //        gameManager.instance.charWindow.SetActive(false);
    //    }
    //}
    private void PlayText()
    {
        gameManager.instance.PauseGame();
        gameManager.instance.profilePicture.sprite = characterProfile;
        gameManager.instance.chatText.text = gameManager.instance.textsToPlay[gameManager.instance.textIndex];
        gameManager.instance.charWindow.SetActive(true);
    }
    private void WelcomeTextGenerator()
    {
        string[] temp = new string[4];
        temp[0] = "Hola, Me llamo cabeza de guebo. Porque? porque no tengo pelo soy un pelon ";
        temp[1] = "Pero eso no es lo importante, tel REY tambien es pelon. Vio a un tipo que se llamaba.. mmmmm creo que luis? con mucho pelo, abundante, demaciado";
        temp[2] = "Por eso el rey exigio ejecutarlo desde que se vea, le pagaremos 5 monedas de bronce. eso es lo que vale, cuidado, le gusta violar gente";
        temp[3] = " ";
        gameManager.instance.textsToPlay = temp;

    }
}
