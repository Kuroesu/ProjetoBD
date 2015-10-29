using UnityEngine;
using System.Collections;

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


	// Use this for initialization
	void Start () {
		status = ObjStatus.GetComponent ("Status") as Status;
		posicaoDir = playerAnimator.transform.localScale;
		posicaoEsq = posicaoDir;
		posicaoEsq.x = -1 * posicaoDir.x;
	
	}
	
	// Update is called once per frame
	void Update () {
		movimentacao ();
		if (Input.GetKey(KeyCode.Space)) {
			attack ();
		}

		if (Input.GetKeyDown (KeyCode.B)) {//so para testar o dano
			int dano=10;
			tomarDano(dano);
		}

		if (status.hpAtual == 0) {
			morrer();
		}


	}

	public Status getStatus(){
		return status;
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
}