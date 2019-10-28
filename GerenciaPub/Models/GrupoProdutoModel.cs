using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class GrupoProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public static List<GrupoProdutoModel> RecuperarLista(string filtro = "")
        {
            var ret = new List<GrupoProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(gru_descricao) like '%{0}%'", filtro.ToLower());
                    }

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                        "select * from grupo" + filtroWhere + " order by gru_descricao");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel
                        {
                            Id = (int)reader["gru_id"],
                            Nome = (string)reader["gru_descricao"],
                            Ativo = (bool)reader["gru_ativo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static List<GrupoProdutoModel> RecuperarListaAtivos()
        {
            var ret = new List<GrupoProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from grupo where gru_ativo=1 order by gru_descricao");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel
                        {
                            Id = (int)reader["gru_id"],
                            Nome = (string)reader["gru_descricao"],
                            Ativo = (bool)reader["gru_ativo"]
                        });
                    }
                }
            }

            return ret;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from grupo where (gru_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new GrupoProdutoModel
                        {
                            Id = (int)reader["gru_id"],
                            Nome = (string)reader["gru_descricao"],
                            Ativo = (bool)reader["gru_ativo"]
                        };
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from grupo where (gru_id = @id)";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into grupo (gru_descricao, gru_ativo) values (@nome, @ativo); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update grupo set gru_descricao=@nome, gru_ativo=@ativo where gru_id = @id";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}