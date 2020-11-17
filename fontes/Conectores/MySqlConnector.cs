using System;
using System.Data;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace GeraClasses.Conectores {
    public class MySqlConnector:IConector {
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
                return String.Format("Data Source={0};Database={1};User Id={2};Password={3};Allow Batch=true;",
                    _Host, _Database, _Login, _Senha);
            }
        }

        public string SQL {
            get { return _SQL; }
            set { _SQL = value; }
        }

        public void Open() {
            _DbConnection = new MySqlConnection(ConnectionString);
            _DbConnection.Open();
        }

        public void Close() {
            _DbConnection.Close();
        }

        public DataSet getDataSet(string strQuery) {
            this.Open();
            DataSet dataSet = new DataSet();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter();
            dataAdapter.SelectCommand = new MySqlCommand(strQuery, (MySqlConnection)_DbConnection);
            dataAdapter.Fill(dataSet);
            this.Close();
            return dataSet;
        }

        public bool execNonQuery(string str) {
            try {
                this.Open();
                MySqlCommand Command = ((MySqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (MySqlConnection)_DbConnection;
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
                MySqlCommand Command = ((MySqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (MySqlConnection)_DbConnection;
                Command.CommandText = SQL;
                MySqlParameter myParameter = new MySqlParameter("?", parametro);
                Command.Parameters.Add(myParameter);
                Command.ExecuteNonQuery();
                this.Close();
            } catch(Exception ex) {
                this.Close();
                throw new Exception("DllConexao. Erro ao comunicar com a base de dados." + ex);
            }
        }

        public string execScalar(string str) {
            string strRetorno = string.Empty;
            try {
                this.Open();
                MySqlCommand Command = ((MySqlConnection)_DbConnection).CreateCommand();
                Command.Connection = (MySqlConnection)_DbConnection;
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
