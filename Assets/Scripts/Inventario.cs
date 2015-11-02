using UnityEngine;
using System.Collections;



public class Inventario : MonoBehaviour {
	private GameObject [,] inventario = new GameObject[5,5];//matriz com os itens do heroi
	public GameObject armaAtual;
	public GameObject posicaoArma;
	public GameObject player;
	public GameObject objEstatus;//pega os status do player
	private bool estaComArma=false;
	private float lastClick = 0;//conta o intervalo entre os clicks do mouse
	//variaveis da GUI do inventario
	private Rect janelaInventario;
	private string statusText;
	private int tamJanelaX = 400;
	private int tamJanlelaY = 220;
	private int espacoBotaoX=20;
	private int espacoBotaoY=0;
	private bool gerarInv = false;//variavel de controle para exibir a inventario.

	void OnGUI() {
		if (gerarInv) {
			janelaInventario = GUI.Window (0, new Rect (300, 150, tamJanelaX, tamJanlelaY), GUIinventario, "Inventario");
		}
		}

	// Use this for initialization
	void Start () {
		//Acredito que aqui ficara a parte do banco, os itens serao carregados para o invetario do personagem
		inventario[0,0] = (GameObject)Resources.Load ("Prefabs/Armas/EspadaBasica", typeof(GameObject));
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			if(gerarInv){
				gerarInv=false;
			}else{
				gerarInv=true;
			}
			/*Vector3 posicao = player.transform.position+(player.transform.position-posicaoArma.transform.position);
			armaAtual = Instantiate(armaAtual,posicao,armaAtual.transform.rotation) as GameObject;
			armaAtual.transform.parent=posicaoArma.transform;
			Arma arma = armaAtual.GetComponent("Arma") as Arma;
			texto.text= arma.status.ataque.ToString();*/
		}
	
	}

	void GUIinventario(int windowID){//funçao que cria a GUI do inventario
		GUI.BeginGroup (new Rect(0,20, 200, 200));
		GUI.Box(new Rect(0, 0, 200, 200), "");
		for (int i=0; i<5; i++) {
			for(int j=0;j<5;j++){
				if(inventario[i,j]!=null){
					//captura a imagem do item
					SpriteRenderer render = inventario[i,j].GetComponent("SpriteRenderer") as SpriteRenderer;//captura o render do prefab
					Texture2D imagemItem = render.sprite.texture;//apartir do render ele obtem a imagem do prefab(Sprite) 
					if(GUI.Button (new Rect (40*j, 40*i, 40, 40),imagemItem)){
						//aqui ficara o codido resposanvel por equipar os itens 
						//ao personagem
						if(Time.time-lastClick<0.3){//soh equipa arma ao se dar um "double click"
							equiparArma(inventario[i,j]);

						}
						else{//um click exibe o status do item
							Arma armaSelecionada = inventario[i,j].GetComponent("Arma") as Arma;
							Arma arma = armaSelecionada.GetComponent ("Arma") as Arma;
							arma.setPortador (objEstatus);
							statusText = "Dano:"+armaSelecionada.getDanoBase();
						}
						lastClick = Time.time;
					}
				}else{
					if(GUI.Button (new Rect (40*j, 40*i, 40, 40),""+i+""+j)){
						//caso nao tenha itens o slot ficara default
					}
				}
			}
		}
		GUI.EndGroup();
		GUI.Box(new Rect(200, 20, 200, 200), statusText);//box de informaçao dos itens

	}

	void equiparArma(GameObject item){
		//aqui ficara o cogido resposanvel por equipar os itens 
		//ao personagem
		if (armaAtual != null) {//caso ja tenha uma arma equipada ela sera destruida, para que outra a substitua
			Destroy(armaAtual);
		}
		Vector3 posicao = posicaoArma.transform.position;//posicao da arma(precisa ser ajustada)
		//Coloca o player como portador da arma
		armaAtual = Instantiate(item,posicao,posicaoArma.transform.rotation) as GameObject;//instancia a arma na mao do player
		Arma arma = armaAtual.GetComponent ("Arma") as Arma;
        arma.portadorArma = player;
		arma.setPortador (objEstatus);
		armaAtual.transform.parent=posicaoArma.transform;//transforma a arma em "filha" do player(assim ela se movera junto com ele)
		armaAtual.transform.localPosition = new Vector3 (0, 0, 0);
		armaAtual.transform.localScale = new Vector3 (1, 1, 1);

	}

}
