using UnityEngine;
using System.Collections;

public abstract class Itens : MonoBehaviour {
    protected int id;
    public Status status;
    public GameObject portador;//serve para ter acesso aos statos do portador(quem segura a arma)
                               // Use this for initialization
    protected int quantidade = 0; // armazena a quantidade de itens repetidos. 
    private string descricao; //armazena a descrição do item. "Essa espada foi forjada no coração de Mordor.Essas coisas"
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
    }

    //caso item de bonus de atributos ao portador
    //Essa função só não contempla os atributos de ataque e defesa
    //pois seu comportamento vai ser diferente para escudos e armas;
    protected void buffPortador()
    {
        Status statusPortador = portador.GetComponentInChildren<Status>() as Status;
        statusPortador.distribuirPontos(statusPortador.forca + this.status.forca, 
            statusPortador.vitalidade + this.status.vitalidade, statusPortador.inteligencia + this.status.inteligencia);
        statusPortador.hp = statusPortador.hp + this.status.hp;
        statusPortador.hpAtual = statusPortador.hp;
        statusPortador.mp = statusPortador.mp + this.status.mp;
        statusPortador.mpAtual = statusPortador.mp;
    }
    //Retira os atributos recebidos dos itens.
    public void removerBuffs()
    {
        Status statusPortador = portador.GetComponentInChildren<Status>() as Status;
        statusPortador.distribuirPontos(statusPortador.forca - this.status.forca,
            statusPortador.vitalidade - this.status.vitalidade, statusPortador.inteligencia - this.status.inteligencia);
        statusPortador.hp = statusPortador.hp - this.status.hp;
        statusPortador.hpAtual = statusPortador.hp;
        statusPortador.mp = statusPortador.mp - this.status.mp;
        statusPortador.mpAtual = statusPortador.mp;
    }


    public int getQuantidade()
    {
        return this.quantidade;
    }

    public void setQuantidade(int novaQuantidade)
    {
        this.quantidade = novaQuantidade;
    }

    public string getDescricao()
    {
        return this.descricao;
    }





}
