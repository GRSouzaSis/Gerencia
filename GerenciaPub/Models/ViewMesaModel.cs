using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class ViewMesaModel
    {
        // public MesaModel Mesa { get; set; }
        // public MovimentoMesaModel MoviMesa { get; set; }
        [Required(ErrorMessage = "Preencha o número da mesa.")]
        public int MesaId { get; set; }

        [Required(ErrorMessage = "Preencha o número da mesa.")]
        public string MesaNome { get; set; }

        public bool MesaAtivo { get; set; }


        public int MoviId { get; set; }

        [DataType(DataType.Date)]
        public DateTime MoviDataEntrada { get; set; }

        [DataType(DataType.Date)]
        public DateTime MoviDataSaida { get; set; }

        public int MoviNroPessoa { get; set; }


        public static List<ViewMesaModel> RecuperarLista(string filtro = "")
        {
            var ret = new List<ViewMesaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(m.mes_descricao) like '%{0}%'", filtro.ToLower());
                    }

                    comando.Connection = conexao;
                    //comando.CommandText = "select * from produto order by pro_nome";
                    comando.CommandText = string.Format(
                        "select m.mes_id as MesaId, m.mes_descricao as MesaNome, m.mes_ativo as MesaAtivo," +
                        "mm.mes_id as MoviId, mm.mmo_dataentrada as MoviDataEntrada, mm.mmo_datasaida as MoviDataSaida,mm.mmo_nropessoa as MoviNroPessoa" +
                        " from mesa m left join movimesa mm on m.mes_id = mm.mes_id" + filtroWhere + " order by m.mes_descricao");

                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        ret.Add(new ViewMesaModel
                        {
                            MesaId = (int)reader["MesaId"],
                            MesaNome = (string)reader["MesaNome"],
                            MesaAtivo = (bool)reader["MesaAtivo"]

                            //  MoviId = (int)reader["MoviId"],
                            //  MoviNroPessoa = (int)reader["MoviNroPessoa"],
                            //   MoviDataEntrada = (DateTime)reader["MoviDataEntrada"],
                            //   MoviDataSaida = (DateTime)reader["MoviDataSaida"]

                        });
                    }
                }
            }

            return ret;
        }

        public static ViewMesaModel VerificaMesa(string mesa)
        {
            ViewMesaModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select mes_descricao from mesa where (mes_descricao = @mesa)";

                    comando.Parameters.Add("@mesa", SqlDbType.Int).Value = mesa;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        return ret;

                        //aqui você colocaria a pagina na qual vai redirecionar caso o usuário exista...
                    }
                    //SENÃO NÃO ENTRA
                    else
                    {
                        return null;
                    }
                }
            }

        }

        public static ViewMesaModel RecuperarPeloId(int id)
        {
            ViewMesaModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from mesa where (mes_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new ViewMesaModel
                        {
                            MesaId = (int)reader["mes_id"],
                            MesaNome = (string)reader["mes_descricao"],
                            MesaAtivo = (bool)reader["mes_ativo"]
                        };
                    }
                }

            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.MesaId);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;


                    if (model == null)
                    {
                        comando.CommandText = "insert into mesa (mes_descricao, mes_ativo) values (@nome, @ativo); select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.MesaNome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.MesaAtivo ? 1 : 0);

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update mesa set mes_descricao=@nome, mes_ativo=@ativo where mes_id = @id";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.MesaNome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.MesaAtivo ? 1 : 0);
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.MesaId;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.MesaId;
                        }
                    }
                }
            }

            return ret;
        }
        /*
                public int Salvar(string mesa)
                {
                    var ret = 0;

                    //var model = RecuperarPeloId(this.Id);

                    using (var conexao = new SqlConnection())
                    {
                        conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                        conexao.Open();
                        using (var comando = new SqlCommand())
                        {
                            comando.Connection = conexao;


                            comando.CommandText = "insert into mesa (mes_id, " +
                                "mes_descricao," +
                                "mes_ativo) " +

                                "values ((select max(mes_id) from mesa)+1," +
                                " @descricao," +
                                "@ativo) ";

                            comando.Parameters.Add("@descricao", SqlDbType.VarChar).Value = mesa;
                            comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = 1;

                            ret = 1;

                        }
                    }

                    return ret;
                }*/
    }
}