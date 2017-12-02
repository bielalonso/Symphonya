﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Symphonya_RedeSocial.Models
{

    public class Bandas
    {
        public Int32 ID { get; set; }
        public String Nome { get; set; }
        public String Descricao { get; set; }
        public String Lider { get; set; }
        public String Imagem_Perfil_Banda { get; set; }
        public String Imagem_Capa_Banda { get; set; }
        public Int32 LiderID { get; set; }

        public Bandas() { }

        public Bandas(Int32 ID, Int32 IDUsuario)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Bandas WHERE ID=@ID;";
            Comando.Parameters.AddWithValue("@ID", ID);
            Comando.Parameters.AddWithValue("@IDUsuario", IDUsuario);

            SqlDataReader Leitor = Comando.ExecuteReader();

            Leitor.Read();

            this.ID = (Int32)Leitor["ID"];
            this.Nome = (String)Leitor["Nome"];
            this.Descricao = (String)Leitor["Descricao"];

            Conexao.Close();
        }

        public static Bandas MostrarBanda(Int32 ID)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT Bandas.ID, Bandas.Nome, Bandas.Descricao, Bandas.LiderID, Usuario.Nome AS NomeUsuario, Usuario.Sobrenome FROM Bandas,Usuario,Usuario_Has_Bandas WHERE Usuario.ID = Bandas.LiderID AND UsuarioID = @ID OR LiderID LIKE @ID;";
            Comando.Parameters.AddWithValue("@ID", ID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Bandas Banda = new Bandas();
            Leitor.Read();

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return null;
            }
            else
            {
                Banda.ID = (Int32)Leitor["ID"];
                Banda.Nome = (String)Leitor["Nome"];
                Banda.Descricao = (String)Leitor["Descricao"];
                Banda.Lider = (String)Leitor["NomeUsuario"] + (String)Leitor["Sobrenome"];
                Banda.LiderID = (Int32)Leitor["LiderID"];
            }

            Conexao.Close();

            return Banda;
        }
        public Boolean NovaBanda(Int32 IDLider)
        {

            SqlConnection Conexao = new SqlConnection("Server=ESN509VMSSQL;Database=Symphonya;User Id=Aluno;Password=Senai1234;");
            Conexao.Open();

            //CRIACAO DO COMANDO SQL
            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "INSERT INTO Bandas ( Nome , Descricao, LiderID)"
              + "VALUES ( @Nome , @Descricao, @Lider);";
            Comando.Parameters.AddWithValue("@Nome", this.Nome);
            Comando.Parameters.AddWithValue("@Descricao", this.Descricao);
            Comando.Parameters.AddWithValue("@Lider", IDLider);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return Resultado > 0 ? true : false;
        }
        public static Boolean excludeBand(Int32 ID)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "DELETE FROM Usuario_Has_Bandas WHERE BandasID = @ID;";
            Comando.Parameters.AddWithValue("@ID", ID);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();
            return Resultado > 0 ? true : false;

        }
        public static Bandas ListarBanda(Int32 BandaID)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Bandas WHERE ID = @ID";
            Comando.Parameters.AddWithValue("@ID", BandaID);

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return null;
            }

            //LISTA COM ID DAS BANDAS
            Bandas Bandas = new Bandas();
            Bandas.ID = (Int32)Leitor["ID"];
            Bandas.Nome = (String)Leitor["Nome"];
            Bandas.Descricao = (String)Leitor["Descricao"];
            Bandas.LiderID = (Int32)Leitor["LiderID"];
            Bandas.Imagem_Perfil_Banda = (String)Leitor["Imagem_Perfil_Banda"];
            Bandas.Imagem_Capa_Banda = (String)Leitor["Imagem_Capa_Banda"];

            Conexao.Close();

            return Bandas;
        }

        public static Bandas VisualizarBanda(Int32 LiderID)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Bandas WHERE LiderID = @ID";
            Comando.Parameters.AddWithValue("@ID", LiderID);


            SqlDataReader Leitor = Comando.ExecuteReader();

            //LISTA COM ID DAS BANDAS
            Bandas Bandas = new Bandas();
            Bandas.ID = (Int32)Leitor["ID"];
            Bandas.Nome = (String)Leitor["Nome"];
            Bandas.Descricao = (String)Leitor["Descricao"];
            Bandas.LiderID = (Int32)Leitor["LiderID"];
            Bandas.Imagem_Perfil_Banda = (String)Leitor["Imagem_Perfil_Banda"];
            Bandas.Imagem_Capa_Banda = (String)Leitor["Imagem_Capa_Banda"];

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return null;
            }

            Conexao.Close();

            return Bandas;
        }

        public static List<Bandas> ListarBandas(String pesquisa)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT * FROM Bandas WHERE Nome LIKE '" + @pesquisa + "%';";

            SqlDataReader Leitor = Comando.ExecuteReader();

            //LISTA COM ID DAS BANDAS
            List<Bandas> Bandas = new List<Bandas>();
            while (Leitor.Read())
            {
                Bandas B = new Bandas();
                B.ID = (Int32)Leitor["ID"];
                B.Nome = (String)Leitor["Nome"];
                B.Descricao = (String)Leitor["Descricao"];
                B.Imagem_Perfil_Banda = (String)Leitor["Imagem_Perfil_Banda"];
                Bandas.Add(B);
            }

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return null;
            }

            Conexao.Close();

            return Bandas;
        }

        public Boolean AlterarBanda(Int32 IDUsuario)
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "UPDATE Usuario_Has_Bandas SET ID = @ID, Nome = @Nome, Descricao = @Descricao WHERE IDBandas = @ID AND IDUsuario = @IDUsuario;";
            Comando.Parameters.AddWithValue("@ID", this.ID);
            Comando.Parameters.AddWithValue("@Nome", this.Nome);
            Comando.Parameters.AddWithValue("@Descricao", this.Descricao);
            Comando.Parameters.AddWithValue("@IDUsuario", IDUsuario);

            Int32 Resultado = Comando.ExecuteNonQuery();

            Conexao.Close();

            return Resultado > 0 ? true : false;
        }

        public static Int32 Contar()
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT COUNT (*) FROM Bandas;";

            SqlDataReader Leitor = Comando.ExecuteReader();
            Leitor.Read();

            Int32 Contador = (Int32)Leitor[""];

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return 0;
            }

            Conexao.Close();

            return Contador;
        }

        public static List<Bandas> Listar()
        {
            SqlConnection Conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["Symphonya"].ConnectionString);
            Conexao.Open();

            SqlCommand Comando = new SqlCommand();
            Comando.Connection = Conexao;
            Comando.CommandText = "SELECT Bandas.Nome, Bandas.Descricao, Bandas.ID, Usuario.Nome AS 'NomeUsuario', Usuario.Sobrenome, Bandas.Imagem_Perfil_Banda, Bandas.Imagem_Capa_Banda FROM Bandas, Usuario WHERE LiderID = Usuario.ID ORDER BY Nome;";

            SqlDataReader Leitor = Comando.ExecuteReader();

            List<Bandas> Bands = new List<Bandas>();
            while (Leitor.Read())
            {
                Bandas B = new Bandas();
                B.ID = (Int32)Leitor["ID"];
                B.Nome = ((String)Leitor["Nome"]);
                B.Descricao = (String)Leitor["Descricao"];
                B.Lider = (String)Leitor["NomeUsuario"] + " " + (String)Leitor["Sobrenome"];
                B.Imagem_Perfil_Banda = (String)Leitor["Imagem_Perfil_Banda"];
                B.Imagem_Capa_Banda = (String)Leitor["Imagem_Capa_Banda"];

                Bands.Add(B);
            }

            if (!Leitor.HasRows)
            {
                Conexao.Close();
                return null;
            }

            Conexao.Close();

            return Bands;
        }
    }
}