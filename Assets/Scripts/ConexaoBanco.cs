using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;

public class ConexaoBanco : MonoBehaviour {

    private MySqlConnection con;
    private string stConexao = "Server=localhost;Database=rpgbd;Uid=root;Pwd=root;";
    private MySqlCommand cmd;
    private MySqlDataReader reader;


    public ConexaoBanco() {

        con = new MySqlConnection(stConexao);
        cmd = new MySqlCommand();
        cmd.Connection = con;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void inserirJogador(string _login, string _senha) {
        cmd.CommandText = "INSERT INTO rpgbd.jogador(jog_login,jog_senha) VALUES('" + _login + "','" + _senha + "');";
        cmd.Connection = con;
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void inserirPersonagem(string nome, string jogador, Status status) {//insere na tabela status o status do player, em seguida insere o personagem, 
                                                                               //a funcao LAST_INSERT_ID()(funcao do mysql) pega o id do ultimo INSERT executado no caso pega o ID do status
        cmd.CommandText = "INSERT INTO status(sta_forca,sta_defesa,sta_vitalidade,sta_inteligencia," +
                    "sta_magia,sta_ataque,sta_mp,sta_mp_atual,sta_hp,sta_hp_atual)" +
                    "VALUES(@forca, @defesa, @vitalidade, @inteligencia, @magia, @ataque, @mp, @mp_atual, @hp, @hp_atual);" +

                    "INSERT INTO personagem(per_nome,per_lvl,per_status,per_jogador)" +
                    "VALUES(@nome,0,LAST_INSERT_ID(),@jogador);";

        cmd.Parameters.AddWithValue("@forca", status.forca);
        cmd.Parameters.AddWithValue("@defesa", status.defesa);
        cmd.Parameters.AddWithValue("@vitalidade", status.vitalidade);
        cmd.Parameters.AddWithValue("@inteligencia", status.inteligencia);
        cmd.Parameters.AddWithValue("@magia", status.magia);
        cmd.Parameters.AddWithValue("@ataque", status.ataque);
        cmd.Parameters.AddWithValue("@mp", status.mp);
        cmd.Parameters.AddWithValue("@mp_atual", status.mpAtual);
        cmd.Parameters.AddWithValue("@hp", status.hp);
        cmd.Parameters.AddWithValue("@hp_atual", status.hpAtual);

        cmd.Parameters.AddWithValue("@nome", nome);
        cmd.Parameters.AddWithValue("@jogador", jogador);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void deletarPersonagem(string nomePersonagem) {
        cmd.CommandText = "DELETE FROM status WHERE sta_id = (SELECT per_status FROM personagem WHERE per_nome = '" + nomePersonagem + "');";
        cmd.Connection = con;
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public bool Logar(string _login, string _senha) {
        bool valor = false;
        cmd.CommandText = "SELECT jog_login,jog_senha FROM jogador WHERE jog_login='" + _login + "' AND jog_senha='" + _senha + "';";
        cmd.Connection = con;
        con.Open();
        reader = cmd.ExecuteReader();
        if (reader.HasRows) {
            valor = true;
        }
        reader.Close();
        con.Close();
        return valor;

    }

    public ArrayList recuperarPersonagem(string jogador) {
        ArrayList lista = new ArrayList();
        cmd.CommandText = "SELECT per_nome, per_lvl FROM personagem WHERE per_jogador ='" + jogador + "';";
        con.Open();
        reader = cmd.ExecuteReader();
        if (reader.HasRows) {
            while (reader.Read()) {
                lista.Add(reader["per_nome"].ToString());
                lista.Add(reader["per_lvl"].ToString());
            }
            reader.Close();
            con.Close();
            return lista;
        } else {
            reader.Close();
            con.Close();
            return null;
        }
    }

    public ArrayList recuperarStatus(string NomePersonagem) {
        ArrayList lista = new ArrayList();
        cmd.CommandText = "SELECT * FROM status WHERE sta_id = (SELECT per_status FROM personagem WHERE per_nome = '" + NomePersonagem + "')";
        con.Open();
        reader = cmd.ExecuteReader();

        if (reader.HasRows) {
            while (reader.Read()) {
                lista.Add(reader["sta_forca"]);
                lista.Add(reader["sta_defesa"]);
                lista.Add(reader["sta_vitalidade"]);
                lista.Add(reader["sta_inteligencia"]);
                lista.Add(reader["sta_magia"]);
                lista.Add(reader["sta_ataque"]);
                lista.Add(reader["sta_hp"]);
                lista.Add(reader["sta_hp_atual"]);
                lista.Add(reader["sta_mp"]);
                lista.Add(reader["sta_mp_atual"]);
            }
            reader.Close();
            con.Close();
        }

        return lista;
    }

}
