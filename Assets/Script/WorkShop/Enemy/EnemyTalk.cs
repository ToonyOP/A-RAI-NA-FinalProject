using TMPro;
using UnityEngine;

public class EnemyTalk : Enemy, IInteractable
{
    public bool canTalk = true;
    public bool isInteractable { get => canTalk; set => canTalk = value; }
    public TMP_Text interactionTextUI;
    public TMP_Text WordTextUI;
    public override void SetUP()
    {
        base.SetUP();
        if (interactionTextUI == null)
        {
            interactionTextUI = GetComponentInChildren<TMP_Text>();
        }
    }
    public void Update()
    {
        if (player == null) return;

        if (interactionTextUI != null)
        {
            if (GetDistanPlayer() >= 2f || !canTalk)
            {
                interactionTextUI.gameObject.SetActive(false);
            }
            else
            {
                interactionTextUI.gameObject.SetActive(true);
            }
        }
        Turn(player.transform.position - transform.position);
    }
    public void Interact(Player player)
    {
        if (WordTextUI != null)
        {
            WordTextUI.gameObject.SetActive(true);
            CancelInvoke("CloseWord");
            Invoke("CloseWord", 3);
        }
    }
    void CloseWord()
    {
        if (WordTextUI != null)
        {
            WordTextUI.gameObject.SetActive(false);
        }
    }
}