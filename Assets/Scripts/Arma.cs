using UnityEngine;
using System.Collections;

public class Arma : Itens {
	public int danoBase;//coloquei público para visualizar o valor em tempo de execução
	private double chanceDeCritico;
    // Use this for initialization
    void Start () {
        this.buffPortador();
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
	}

	void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player" && portador.tag=="inimigo") {
            EnemyBehavior inimigo = portador.GetComponent("EnemyBehavior") as EnemyBehavior;
            PlayerBehavior player = coll.gameObject.GetComponent("PlayerBehavior") as PlayerBehavior;
            //O player só recebe dano se o inimigo estiver no estado de ataque e se não estiver defendendo(o dano é redirecionado pro escudo)
            if (inimigo.getEstado() == "attack" && player.getDefendendo()==false)
            {
                inimigo.incrementarNumGolpes();//conta o número de golpes que inimigo realiza
                player.tomarDano(getDanoBase());
            }
		}

		if (coll.gameObject.tag == "inimigo" && portador.tag=="Player") {
            Debug.Log("entrei aki");
            EnemyBehavior inimigo = coll.gameObject.GetComponent("EnemyBehavior") as EnemyBehavior;
            PlayerBehavior player = portador.GetComponent("PlayerBehavior") as PlayerBehavior;
            if (player.getEstado() == "attack") {
                Debug.Log("entrei na arma");
                inimigo.tomarDano(getDanoBase());
                player.setEstado(Estado_Do_Player.idle);
            }
		}
		
	}


}