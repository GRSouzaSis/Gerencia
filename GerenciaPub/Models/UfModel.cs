using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class UfModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

        // public bool Ativo { get; set; }

        public static List<UfModel> RecuperarLista()
        {
            var ret = new List<UfModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from uf order by uf_id";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UfModel
                        {
                            Id = (int)reader["uf_id"],
                            Nome = (string)reader["uf_nome"]
                        });
                    }
                }
            }

            return ret;
        }


        /* public static List<UfModel> RecuperarListaA()
         {
             var ret = new List<UfModel>();

             using (var conexao = new SqlConnection())
             {
                 conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                 conexao.Open();
                 using (var comando = new SqlCommand())
                 {
                     comando.Connection = conexao;
                     comando.CommandText = "select * from uf_id order by uf_nome";
                     var reader = comando.ExecuteReader();
                     while (reader.Read())
                     {
                         ret.Add(new UfModel
                         {
                             Id = (int)reader["uf_id"],
                             Nome = (string)reader["uf_nome"]
                         });
                     }
                 }
             }

             return ret;
         }*/

        public static UfModel RecuperarPeloId(int id)
        {
            UfModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from uf_id where (uf_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UfModel
                        {
                            Id = (int)reader["uf_id"],
                            Nome = (string)reader["uf_nome"]
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
                        comando.CommandText = "delete from uf where (uf_id = @id)";

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
                        comando.CommandText = "insert into uf (uf_nome) values (@nome); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update uf set uf_nome=@nome where per_id = @id";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }

                return ret;
            }
        }
    }
}
