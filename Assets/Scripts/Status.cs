using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {

	private int id;
    //status base
	public int forca;
	public int vitalidade;
    public int inteligencia;
    public int defesa;
    /*escaladores de status
    public int eForca=1;
    public int eVitalidade = 1;
    public int eInteligencia = 1;
    */
    //Status derivados
    public int magia;
    public int ataque;
    public int hp;
    public int hpAtual;
    public int mp;
    public int mpAtual;
    //Atributos que realizão a conexão com o banco
    private ConexaoBanco banco;
    private ArrayList listaStatus;
    private string personagem;

    public void distribuirPontos(int forca, int vitalidade, int inteligencia)
    {
        this.forca = forca;
        this.inteligencia = inteligencia;
        this.vitalidade = vitalidade;

        this.defesa = forca*1 + vitalidade*2;
        this.magia = inteligencia * 2;
        this.ataque = forca * 2;
        this.hp = vitalidade * 10;
        this.hpAtual = hp;
        this.mp = inteligencia*10;
        this.mpAtual = mp;
    }


	// Use this for initialization
	void Start () {
        if (gameObject.tag == "Player") {
            banco = new ConexaoBanco();
            personagem = PlayerPrefs.GetString("Personagem");
            listaStatus = banco.recuperarStatus(personagem);
            forca = (int)listaStatus[0];
            defesa = (int)listaStatus[1];
            vitalidade = (int)listaStatus[2];
            inteligencia = (int)listaStatus[3];
            magia = (int)listaStatus[4];
            ataque = (int)listaStatus[5];
            hp = (int)listaStatus[6];
            hpAtual = (int)listaStatus[7];
            mp = (int)listaStatus[8];
            mpAtual = (int)listaStatus[9];
        } else {
//            GameObject mob = gameObject.GetComponentInParent<GameObject>();
           // print(mob.name);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
