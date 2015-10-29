using UnityEngine;
using System.Collections;

public enum Estado_Do_Inimigo{
	idle,
	attack,
	morrer,
}


public class EnemyBehavior : MonoBehaviour
{
    public GameObject objBarraHP;//guarda o gameObject da barra de hp do inimigo
    public GameObject objStatus;// '' Status do inimigo
    public GameObject objArma;
    public int delayAtaque;//Determina o tempo em que o inimigo fica parado após atingir um determinado número de ataques
    private float timer = 0;
    public short numDeAtaquesMax;//Determina quantos ataques o inimigo pode realizar antes de se cansar 
    public float distanciaMinimaDoPlayer;
    public Animator enemyAnimator;
    public float velocidade;
    private Status status;
    private HP_Bar barraHP;
    private PlayerBehavior player;//variavel que guarda o jogador.
    private Estado_Do_Inimigo estadoAtual;
    private float posicaoDir;
    public int numDeGolpes = 0;//responsavel por indicar quantos golpes foram realizados pelo inimigo.
    private bool atrasarAtaque = false;
    public TextMesh teste;
    float distanciaDoPlayer;//pega a distancia do inimigo para o jogador
                            // Use this for initialization
    void Start()
    {
        status = objStatus.GetComponent("Status") as Status;
        barraHP = objBarraHP.GetComponent("HP_Bar") as HP_Bar;
        //player = FindObjectOfType (typeof(PlayerBehavior)) as PlayerBehavior;
        Arma armaInimigo = objArma.GetComponent("Arma") as Arma;
        armaInimigo.setPortador(objStatus);
        //teste.text = armaInimigo.getDanoBase () + "";
        distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);//pega a distancia do inimigo para o jogador
        setEstado(Estado_Do_Inimigo.idle);
    }

    // Update is called once per frame
    void Update()
    {
        comportamento();
        contarTempo();
    }

    public void tomarDano(int dano)
    {
        dano = dano - status.defesa;
        if (dano <= 0)
        {
            dano = 1;//Dano mínimo 
        }
        if (dano >= status.hpAtual)
        {
            status.hpAtual = 0;
        }
        else
        {
            status.hpAtual = status.hpAtual - dano;
        }
        barraHP.alterarHP();
    }

    void comportamento()
    {
        distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);//pega a distancia do inimigo para o jogador
        switch (estadoAtual)
        {
            case Estado_Do_Inimigo.idle:
                {
                    distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);//pega a distancia do inimigo para o jogador
                    if (distanciaDoPlayer < 100 && distanciaDoPlayer > 0)
                    {
                        if (distanciaDoPlayer < distanciaMinimaDoPlayer)
                        {
                            setEstado(Estado_Do_Inimigo.attack);
                        }
                        else
                        {
                            seguirPlayer(true);
                        }

                    }
                    else
                    {
                        seguirPlayer(false);
                    }
                    if (status.hpAtual <= 0)
                    {
                        setEstado(Estado_Do_Inimigo.morrer);
                    }
                }
                break;
            case Estado_Do_Inimigo.attack:
                {
                    if (distanciaDoPlayer >= distanciaMinimaDoPlayer || player.getStatus().hpAtual == 0)
                    {//caso o player se afaste d+ durante um ataque o mob vai voltar a segui-lo
                        enemyAnimator.SetBool("attack", false);
                        setEstado(Estado_Do_Inimigo.idle);
                    }
                    else if (numDeGolpes >= numDeAtaquesMax)
                    {//para os ataques quando numero de golpes deferidos for igual ao número máximo de golpes
                        enemyAnimator.SetBool("attack", false);
                        atrasarAtaque = true;
                    }
                    else
                    {
                        enemyAnimator.SetBool("attack", true);//inicia a animação de ataque
                    }
                    if (status.hpAtual <= 0)
                    {
                        setEstado(Estado_Do_Inimigo.morrer);
                    }
                }
                break;

            case Estado_Do_Inimigo.morrer:
                {
                    enemyAnimator.SetBool("morrer", true);
                }
                break;
        }
    }

    void setEstado(Estado_Do_Inimigo novoEstado)
    {
        estadoAtual = novoEstado;
    }

    void seguirPlayer(bool seguir)
    {
        if (player.gameObject.transform.position.x < transform.position.x)
        {
            transform.Translate(Vector3.left * velocidade * Time.deltaTime);
            transform.localScale = new Vector3(2, 2, 2);//faz o inimigo olhar na direçao do player
        }
        else if (player.gameObject.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
            transform.Translate(Vector3.right * velocidade * Time.deltaTime);
        }


    }

    public string getEstado()
    {
        return estadoAtual.ToString();
    }
    //serve como um "timer" para indicar o tempo que o inimigo deve ficar sem atacar
    private void contarTempo()
    {
        if (atrasarAtaque)
        {
            timer += Time.deltaTime;
            if (timer >= delayAtaque)
            {
                numDeGolpes = 0;
                timer = 0;
                atrasarAtaque = false;
            }
        }
    }
    //Esse método é responsavel pela visão do inimigo
    //quando o player "colide"(ou seja entra) no campo de visão
    //do inimigo o inimigo iniciará a perseguição ao player
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            player = coll.gameObject.GetComponent("PlayerBehavior") as PlayerBehavior;
        }
    }
}
