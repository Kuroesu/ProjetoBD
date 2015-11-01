using UnityEngine;
using System.Collections;

public class XpBar : MonoBehaviour {
    public GameObject player;
    public TextMesh xPtext;
    public SpriteRenderer qtXp;
    private PlayerBehavior playerBehavior;

	// Use this for initialization
	void Start () {
        this.playerBehavior = player.GetComponent("PlayerBehavior") as PlayerBehavior;
	}
	
	// Update is called once per frame
	void Update () {
        this.atualizarBarra();
	}

    private void atualizarBarra()
    {
        float porcent = this.playerBehavior.getXpAtual() / this.playerBehavior.getXpTotal();
        this.xPtext.text = this.playerBehavior.getXpAtual().ToString("0.00") + "/" + this.playerBehavior.getXpTotal().ToString("0.00") + "("+ (porcent*100).ToString("0.0")+"%)";
        this.qtXp.transform.localPosition = new Vector2(this.porcToX(porcent), this.qtXp.transform.localPosition.y);
        this.qtXp.transform.localScale = new Vector2(porcent, this.qtXp.transform.localScale.y);
    }
    //converte a porcentagem em um número no eixo X
    //Faz com que a barra de xp se mova corretamente em função de quanto falta para preenchê-la
    private float porcToX(float porc)
    {
        float resultado = -5 * (1 - porc);
        return resultado;
    }
}
