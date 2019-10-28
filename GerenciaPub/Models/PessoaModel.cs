using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class PessoaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o Cpf")]
       // [StringLength(14)]
        [CPF]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Informe o endereço")]
      //  [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Informe o numero")]
        [Range(1,100000)]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe o Bairro/Complemento")]
       // [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Bairro { get; set; }
        
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$", ErrorMessage = "Digite um email válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe o telefone")]
       // [StringLength(16)]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Informe o Celular")]
        //[StringLength(16)]
        public string Celular { get; set; }

        public bool Ativo { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string Obs { get; set; }

        [Required(ErrorMessage = "Informe o Nome")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Selecione a Cidade")]
        public int IdCidade { get; set; }



        public static List<PessoaModel> RecuperarLista(string filtro="")
        {
            var ret = new List<PessoaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(pes_nome) like '%{0}%'", filtro.ToLower());
                    }

                    comando.Connection = conexao;                    
                    comando.CommandText = string.Format("select * from pessoa"+ filtroWhere + " order by pes_nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PessoaModel
                        {
                            Id = (int)reader["pes_id"],
                            Nome = (string)reader["pes_nome"],
                            Cpf = (string)reader["pes_cpf"],
                            Endereco = (string)reader["pes_Endereco"],
                            Numero = (string)reader["pes_numero"],
                            Bairro = (string)reader["pes_Bairro"],
                            Email = (string)reader["pes_email"],
                            Telefone = (string)reader["pes_telefone"],
                            Celular = (string)reader["pes_celular"],
                            Obs = (string)reader["pes_obs"],
                            Ativo = (bool)reader["pes_ativo"],
                            IdCidade = (int)reader["cid_id"]

                        });
                    }
                }
            }

            return ret;
        }

        public static List<PessoaModel> RecuperarListaAtivos()
        {
            var ret = new List<PessoaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from pessoa where pes_ativo=1 order by pes_nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new PessoaModel
                        {
                            Id = (int)reader["pes_id"],
                            Nome = (string)reader["pes_nome"],
                            Cpf = (string)reader["pes_cpf"],
                            Endereco = (string)reader["pes_Endereco"],
                            Numero = (string)reader["pes_numero"],
                            Bairro = (string)reader["pes_Bairro"],
                            Email = (string)reader["pes_email"],
                            Telefone = (string)reader["pes_telefone"],
                            Celular = (string)reader["pes_celular"],
                            Obs = (string)reader["pes_obs"],
                            Ativo = (bool)reader["pes_ativo"],
                            IdCidade = (int)reader["cid_id"]
                        });
                    }
                }
            }

            return ret;
        }

        public static PessoaModel RecuperarPeloId(int id)
        {
            PessoaModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from pessoa where (pes_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new PessoaModel
                        {
                            Id = (int)reader["pes_id"],
                            Nome = (string)reader["pes_nome"],
                            Cpf = (string)reader["pes_cpf"],
                            Endereco = (string)reader["pes_Endereco"],
                            Numero = (string)reader["pes_numero"],
                            Bairro = (string)reader["pes_Bairro"],
                            Email = (string)reader["pes_email"],
                            Telefone = (string)reader["pes_telefone"],
                            Celular = (string)reader["pes_celular"],
                            Obs = (string)reader["pes_obs"],
                            Ativo = (bool)reader["pes_ativo"],
                            IdCidade = (int)reader["cid_id"]
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
                        comando.CommandText = "delete from pessoa where (pes_id = @id)";

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
                        comando.CommandText = "insert into pessoa (pes_nome, pes_ativo,pes_cpf,pes_endereco," +
                            "pes_numero,pes_bairro,pes_email,pes_obs,pes_telefone,cid_id,pes_celular) " +
                            "values (@nome, @ativo,@cpf,@endereco,@numero,@bairro,@email,@obs,@telefone,@cidia,@celular);" +
                            " select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.Cpf;
                        comando.Parameters.Add("@endereco", SqlDbType.VarChar).Value = this.Endereco;
                        comando.Parameters.Add("@numero", SqlDbType.VarChar).Value = this.Numero;
                        comando.Parameters.Add("@bairro", SqlDbType.VarChar).Value = this.Bairro;
                        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@obs", SqlDbType.VarChar).Value = this.Obs;
                        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone;
                        comando.Parameters.Add("@cidia", SqlDbType.Int).Value = this.IdCidade;
                        comando.Parameters.Add("@celular", SqlDbType.VarChar).Value = this.Celular;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update pessoa set pes_nome=@nome, pes_ativo=@ativo," +
                            "pes_cpf=@cpf, pes_endereco=@endereco, pes_numero=@numero, pes_bairro=@bairro," +
                            "pes_email=@email, pes_obs=@obs, pes_telefone=@telefone, cid_id=@cidia,pes_celular=@celular" +
                            " where pes_id = @id";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.Cpf;
                        comando.Parameters.Add("@endereco", SqlDbType.VarChar).Value = this.Endereco;
                        comando.Parameters.Add("@numero", SqlDbType.VarChar).Value = this.Numero;
                        comando.Parameters.Add("@bairro", SqlDbType.VarChar).Value = this.Bairro;
                        comando.Parameters.Add("@email", SqlDbType.VarChar).Value = this.Email;
                        comando.Parameters.Add("@obs", SqlDbType.VarChar).Value = this.Obs;
                        comando.Parameters.Add("@telefone", SqlDbType.VarChar).Value = this.Telefone;
                        comando.Parameters.Add("@cidia", SqlDbType.Int).Value = this.IdCidade;
                        comando.Parameters.Add("@celular", SqlDbType.VarChar).Value = this.Celular;

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