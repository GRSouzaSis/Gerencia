using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class FuncionarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione a pessoa")]
        public int IdPessoa { get; set; }

        public static List<FuncionarioModel> RecuperarLista()
        {
            var ret = new List<FuncionarioModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from funcionario order by fun_id";
                    //    "select f.*, p.pes_id from funcionario f, pessoa p where (f.fun_id = @id) and " +
                    //   "(f.pes_id = p.pes_id) order by f.fun_id";
                    //  "select c.*, e.id_pais from cidade c, estado e where (c.id = @id) and (c.id_estado = e.id)";

                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new FuncionarioModel
                        {
                            Id = (int)reader["fun_id"],
                            IdPessoa = (int)reader["pes_id"]
                        });
                    }
                }
            }

            return ret;
        }

        public static FuncionarioModel RecuperarPeloId(int id)
        {
            FuncionarioModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from funcionario where (fun_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new FuncionarioModel
                        {
                            Id = (int)reader["fun_id"],
                            IdPessoa = (int)reader["pes_id"]
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
                        comando.CommandText = "delete from funcionario where (fun_id = @id)";

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
                        comando.CommandText = "insert into funcionario (pes_id) values (@pesid); select convert(int, scope_identity())";
                        comando.Parameters.Add("@pesid", SqlDbType.Int).Value = this.IdPessoa;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText =
                            "update funcionario set pes_id=@perid where fun_id = @id";

                        comando.Parameters.Add("@pesid", SqlDbType.Int).Value = this.IdPessoa;

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