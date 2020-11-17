using System;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace GeraClasses.Conectores {
    public class MSSQLSERVERConnector:IConector {
        protected string _Host;
        protected string _Database;
        protected string _Login;
        protected string _Senha;
        private string _SQL = string.Empty;
        private IDbConnection _DbConnection;

        public string Host {
            get { return _Host; }
            set { _Host = value; }
        }

        public string Database {
            get { return _Database; }
            set { _Database = value; }
        }

        public string Login {
            get { return _Login; }
            set { _Login = value; }
        }

        public string Senha {
            get { return _Senha; }
            set { _Senha = value; }
        }

        public void SetaPropert(string servidor, string schema, string usuario, string senha) {
            this.Host = servidor;
            this.Database = schema;
            this.Login = usuario;
            this.Senha = senha;
        }

        private string ConnectionString {
            get {
                return String.Format("Data Source={0};Initial Catalog = {1};User Id={2};Password={3};",
                    _Host, _Database, _Login, _Senha);
            }
        }

        public string SQL {
            get { return _SQL; }
            set { _SQL = value; }
        }

        public void Open() {
            _DbConnection = new SqlConnection(ConnectionString);
            _DbConnection.Open();
        }

        public void Close() {
            _DbConnection.Close();
        }

        public DataSet getDataSet(string strQuery) {
            this.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = new SqlCommand(strQuery, (SqlConnection)_DbConnection);
            dataAdapter.Fill(dataSet);
            this.Close();
            return dataSet;
        }

        public bool execNonQuery(string str) {
            try {
                this.Open();
                SqlCommand Command = ((SqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (SqlConnection)_DbConnection;
                Command.CommandText = str;
                Command.ExecuteNonQuery();
                this.Close();
            } catch(Exception ex) {
                this.Close();
                throw new Exception("Erro ao comunicar com a base de dados." + ex);
            }
            return (true);
        }

        public void execNonQuery(string SQL, byte[] parametro) {
            try {
                this.Open();
                SqlCommand Command = ((SqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (SqlConnection)_DbConnection;
                Command.CommandText = SQL;
                SqlParameter myParameter = new SqlParameter("?", parametro);
                Command.Parameters.Add(myParameter);
                Command.ExecuteNonQuery();
                this.Close();
            } catch(Exception ex) {
                this.Close();
                throw new Exception("DllConexao. Erro ao comunicar com a base de dados." + ex);
            }
        }

        public string execScalar(string str) {
            string strRetorno;
            try {
                this.Open();
                SqlCommand Command = ((SqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (SqlConnection)_DbConnection;
                Command.CommandText = str;
                strRetorno = Convert.ToString(Command.ExecuteScalar());
                this.Close();
            } catch(Exception ex) {
                this.Close();
                strRetorno = string.Empty;
                throw new Exception("Erro ao comunicar com a base de dados." + ex);
            }
            return strRetorno;
        }

    }
}
