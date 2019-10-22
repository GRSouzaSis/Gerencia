using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class CidadeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(30, ErrorMessage = "Nome não pode ter mais que 30 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o estado")]
        public int IdEstado { get; set; }

        public string Uf { get; set; }

        public static List<CidadeModel> RecuperarLista()
        {
            var ret = new List<CidadeModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                     //comando.CommandText = "select * from cidade order by cid_nome";
                     comando.CommandText = "select cid.cid_id, cid.cid_nome,cid.uf_id, u.uf_nome " +
                        "from cidade cid " +
                        "inner join uf u on u.uf_id = cid.uf_id";
                 
                     var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new CidadeModel
                        {
                            Id = (int)reader["cid_id"],
                            Nome = (string)reader["cid_nome"],
                            IdEstado = (int)reader["uf_id"],
                            Uf = (string)reader["uf_nome"]
                        });
                    }
                }
            }

            return ret;
        }
        
        
       /* public static List<CidadeModel> RecuperarListaA()
        {
            var ret = new List<CidadeModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from cidade order by cid_nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new CidadeModel
                        {
                            Id = (int)reader["cid_id"],
                            Nome = (string)reader["cid_nome"],
                            IdUf = (int)reader["uf"]
                        });
                    }
                }
            }

            return ret;
        }*/
        

        public static CidadeModel RecuperarPeloId(int id)
        {
            CidadeModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from cidade where (cid_id = @id)";
                    //comando.CommandText = "select c.*, e.uf from cidade c, uf e where (c.cid_id = @id) and (c.uf = e.uf)";
                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new CidadeModel
                        {
                            Id = (int)reader["cid_id"],
                            Nome = (string)reader["cid_nome"],
                            IdEstado = (int)reader["uf_id"]
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
                        comando.CommandText = "delete from cidade where (cid_id = @id)";

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
                        comando.CommandText = "insert into cidade (cid_nome, uf_id) values (@nome, @uf_id); select convert(int, scope_identity())";
                        //comando.CommandText = "insert into cidade (cid_nome, uf) values (@nome, @ufnome); select convert(int, scope_identity())";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@uf_id", SqlDbType.VarChar).Value = this.IdEstado;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update cidade set cid_nome=@nome, uf_id=@ufnome where cid_id = @id";
                        //comando.CommandText = "update cidade set cid_nome=@nome, uf=@ufnome where cid_id = @id";
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ufnome", SqlDbType.VarChar).Value = this.IdEstado;
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
