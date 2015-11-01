using UnityEngine;
using System.Collections;

public class HP_Bar : MonoBehaviour {
    public GameObject portadorVida;
    public TextMesh HPMesh;//Guarda a quantidade de HP atual do player
    public SpriteRenderer vida;//variavel para guarda a sprite do HP
    public TextMesh levelText;
    private Status status;
    private float HPTotal;
    private EnemyBehavior inimigo = null;
    private PlayerBehavior player = null;

    // Use this for initialization
    void Start()
    {
        if (portadorVida.tag == "Player") {
            this.player = portadorVida.GetComponent("PlayerBehavior") as PlayerBehavior;
            this.status = this.player.getStatus();
        }
        else if (portadorVida.tag == "inimigo") {
            this.inimigo = portadorVida.GetComponent("EnemyBehavior") as EnemyBehavior;
            this.status = this.inimigo.getStatus();
        }
        this.HPTotal = this.status.hp;

    }

    void Update()
    {
        this.exibirHPtext();
        this.exibirLevel();
    }

    private void exibirHPtext()
    {
        this.HPMesh.text = status.hpAtual + "";

    }

    private void exibirLevel()
    {
        if (portadorVida.tag == "Player") {
            if (this.player.getLevel().ToString().Length == 1) {
                this.levelText.text =0+this.player.getLevel().ToString();
            }
            else {
                this.levelText.text = this.player.getLevel().ToString();
            }
        }
        else if (portadorVida.tag == "inimigo") {
            this.levelText.text = this.inimigo.getLevel().ToString();
        }
    }
    //calcula o quanto o eixo x vai se desocar na sprite da vida
    //responsavel pela "animaçao" da barra de vida(faz ela descer) 
    float calcularX(float porcHP)
    {
        float x;//variavel para o eixo x
        x = -2 * (1 - porcHP);
        return x;
    }
    //Funçao responsavel por atualizar a GUI da vida
    public void alterarHP()
    {
        float porcHP = status.hpAtual / HPTotal;
        float x = calcularX(porcHP);
        Vector2 vec2 = new Vector2(porcHP, vida.transform.localScale.y);
        this.vida.transform.localPosition = new Vector3(x, 0, 0);
        this.vida.transform.localScale = vec2;//ajusta a escala da barra em funçao da porcentagem de HP

    }
}
