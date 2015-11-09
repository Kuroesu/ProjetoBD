using UnityEngine;
using System.Collections;

public class Escudo : Itens {
    private int defesaAtual;
    public int defesaBase;//guarda a defesa original do escudo
    private PlayerBehavior player=null;
    private EnemyBehavior inimigo=null;
    private float timer = 0;
    public int recuperacaoDeDefesa=10;//indica quanto de "defesa" o escudo repura por segundo após receber dano

	// Use this for initialization
	void Start () {
        if (portador.tag == "Player") {
            this.player = portador.GetComponent("PlayerBehavior") as PlayerBehavior;
            this.defesaAtual = this.status.defesa+this.player.getStatus().defesa;
        }
        else if (portador.tag == "inimigo") {
            this.inimigo = portador.GetComponent("EnemyBehavior") as EnemyBehavior;
            this.defesaAtual = this.status.defesa + this.inimigo.getStatus().defesa;
        }
        this.defesaBase = this.defesaAtual;
        this.buffPortador();

    }

    void Update()
    {
        this.recuperarEscudo();
    }

    public void receberDano(int dano)
    {
        if (this.portador.tag == "Player") {
            if (this.player.getDefendendo()) {// o escudo só funciona se o portador estiver defendendo
                if (dano >= this.defesaAtual) {//se o dano for maior que a defesa, "quebra" a defesa 
                    int danoRecebido = dano - this.defesaAtual;//o portador recebe a diferença de dano
                    this.player.tomarDano(danoRecebido);
                    this.baixarEscudo();//perde a defesa
                }
                else {
                    this.defesaAtual = this.defesaAtual - dano;//a cada o golpe a defesa vai diminuindo 
                }
            }
        }
        if (this.portador.tag == "inimigo") {
            if (this.inimigo.getDefendendo()) {
                if (dano >= this.defesaAtual) {
                    int danoRecebido = dano - this.defesaAtual;//o portador recebe a diferença de dano
                    this.inimigo.tomarDano(danoRecebido);
                }
                else {
                    this.defesaAtual = this.defesaAtual - dano;//a cada o golpe a defesa vai diminuindo 
                }
            }
        }
    }

    public void baixarEscudo()
    {
        this.defesaAtual = this.defesaBase;
        if (this.player != null) {
            this.player.setDefendendo(false);
        }
        else if (this.inimigo != null) {
            this.inimigo.setDefendendo(false);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "arma") {
            Arma arma = coll.gameObject.GetComponent("Arma") as Arma;
            if (arma.portador.tag != "Player") {
                EnemyBehavior inimigo = arma.portador.GetComponent("EnemyBehavior") as EnemyBehavior;
                if (inimigo.getEstado() == "attack") {
                    this.receberDano(arma.getDanoBase());
                }
            }
        }

    }

    //recupera uma quantidade de defesa do escudo por segundo
    //a defesa só é recuperada se o portador não estiver defendendo
    private void recuperarEscudo()
    {
        if (this.defesaAtual < this.defesaBase) {
            if ((this.player != null && this.player.getDefendendo()==false) || (this.inimigo!=null && this.inimigo.getDefendendo()==false)) {
                timer += Time.deltaTime;
                if (timer >= 1) {
                    this.defesaAtual = this.defesaAtual + this.recuperacaoDeDefesa;
                    timer = 0;
                }

            }
        }
    }

    public int getDefesaAtual()
    {
        return this.defesaAtual;
    }


}
