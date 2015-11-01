using UnityEngine;
using System;
using System.Collections;
public enum Estado_Do_Player {
    idle,
    attack,
    morto,
}

public class PlayerBehavior : MonoBehaviour {

	public GameObject player;
	public GameObject ObjStatus;//Recebe o game object que guarda os status do player
	public GameObject barraHP;
	public Animator playerAnimator;
	public float velocidade;
	private Status status;//guarda todos os status do player(hp,hpAtual, forca e etc)
	private bool olharParaDir = true;
	private Vector3 posicaoDir;
	private Vector3 posicaoEsq;
	public TextMesh teste;
    private Estado_Do_Player estadoAtual;
    private int level = 1;//Determina o nível que o personagem se encontra
    private float qtXpTotal;//Determina quanto de xp o personagem precisa para upar
    private float qtXpAtual=0;//determina quanto de xp o personagem possui atualmente


	// Use this for initialization
	void Start () {
		this.status = ObjStatus.GetComponent ("Status") as Status;
		posicaoDir = playerAnimator.transform.localScale;
		posicaoEsq = posicaoDir;
		posicaoEsq.x = -1 * posicaoDir.x;
        this.qtXpTotal = this.determinarXp();
        this.setEstado(Estado_Do_Player.idle);
	
	}
	
	// Update is called once per frame
	void Update () {
		movimentacao ();
		if (Input.GetKey(KeyCode.Space)) {
            this.setEstado(Estado_Do_Player.attack);
            attack ();
		}

		if (status.hpAtual == 0) {
			morrer();
		}


	}

	private void movimentacao(){

		//Recebe as teclas padrao para o movimento do personagem
		float direcaoX = Input.GetAxis("Horizontal")*velocidade*Time.deltaTime;//teclas:A e D ou <- e ->
		float direcaoY = Input.GetAxis("Vertical")*velocidade*Time.deltaTime;//teclas: W e S ou setaCima e setaBaixo
		player.transform.Translate (direcaoX,direcaoY,0);//Move o personagem

		if (direcaoX>0){
			olharParaDir = true;
		}
		if (direcaoX<0){
			olharParaDir = false;
		}

		if(olharParaDir){
			playerAnimator.transform.localScale = posicaoDir;
		}
		else{
			playerAnimator.transform.localScale = posicaoEsq;
		}

		playerAnimator.SetFloat ("velocidade", Mathf.Abs(direcaoX));//Chama a animaçao de andar
	}

	private void attack(){
		playerAnimator.SetTrigger ("attack");

	}

	public void tomarDano(int dano){
        dano = dano - status.defesa;
        if (dano <= 0)
        {
            dano = 1;
        }
		HP_Bar hpBar = barraHP.GetComponent ("HP_Bar") as HP_Bar;
		if (dano >= status.hpAtual) {
			status.hpAtual = 0;
		} else {
			status.hpAtual = status.hpAtual - dano;
		}
		hpBar.alterarHP ();
	}

	private void morrer(){
		playerAnimator.SetBool ("morto", true);
	}

    private float determinarXp()
    {
       return this.qtXpTotal = 50 * (this.level * Mathf.Log10(this.level)) + 25;
    }

    public void receberXp(float xp)
    {
        this.qtXpAtual = qtXpAtual + xp;
        while (this.qtXpAtual >= this.qtXpTotal) {
            this.levelUp();
        }
    }

    private void levelUp()
    {
        this.level = this.level + 1;
        this.qtXpTotal = this.determinarXp();
    }

    public void setEstado(Estado_Do_Player novoEstado)
    {
        this.estadoAtual = novoEstado;
    }

    public string getEstado()
    {
        return this.estadoAtual.ToString();
    }

    public float getXpAtual()
    {
        return this.qtXpAtual;
    }

    public float getXpTotal()
    {
        return this.qtXpTotal;
    }
    public int getLevel()
    {
        return this.level;
    }
    public Status getStatus()
    {
        return this.status;
    }
}