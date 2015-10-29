using UnityEngine;
using System.Collections;

public class Arma : Itens {
	private int danoBase;
	public GameObject portadorArma;//serve para ter acesso aos statos do portador(quem segura a arma)
	private double chanceDeCritico;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public int getDanoBase(){
		return danoBase;
	}
	public void setPortador(GameObject portador){
		Status estatusPortador = portador.GetComponent("Status") as Status;
		danoBase = status.ataque * estatusPortador.forca;
		//chanceDeCritico = (estatusPortador.inteligencia * status.velocidade);
	}
	/*public int danoReal(){
		int critico = 1;
		if (isCritico ()) {
			critico = 2;
		}
		int danoReal = critico*danoBase;

		return danoReal;
}
	private bool isCritico(){
		bool isCritico = false;
		int qtVetor = 100 / (int)chanceDeCritico;
		int[] vetor = new int[qtVetor];
		for(int i=0; i<qtVetor;i++){
			vetor[i] = 0;
			if(i== qtVetor-1){
				vetor[i]=1;
			}
		}
		int escolha  = (int)Random.Range(0, qtVetor-1);
		if(vetor[escolha]==1){
			isCritico=true;
		}

		return isCritico;

	}*/

	void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player" && portadorArma.tag=="inimigo") {
            EnemyBehavior inimigo = portadorArma.GetComponent("EnemyBehavior") as EnemyBehavior;
            if(inimigo.getEstado() == "attack")//O player só recebe dano se o inimigo estiver no estado de ataque
            {
                PlayerBehavior player = coll.gameObject.GetComponent("PlayerBehavior") as PlayerBehavior;
                inimigo.numDeGolpes = inimigo.numDeGolpes+1;//conta o número de golpes que inimigo realiza
                player.tomarDano(getDanoBase());
            }
		}

		if (coll.gameObject.tag == "inimigo" && portadorArma.tag=="Player") {
            EnemyBehavior inimigo = coll.gameObject.GetComponent("EnemyBehavior") as EnemyBehavior;
			inimigo.tomarDano(getDanoBase());
		}
		
	}



}