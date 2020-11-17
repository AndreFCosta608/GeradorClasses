using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
//using Npgsql;
//using NpgsqlTypes;

namespace GeraClasses.Conectores {
    public class PostgreConnector:IConector {
        protected string _Host;
        protected string _Database;
        protected string _Login;
        protected string _Senha;
        private string _SQL = string.Empty;
        //private NpgsqlConnection _DbConnection;

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
                return String.Format("Server={0};Database={1};User Id={2};Password={3};Port=5432;",
                    _Host, _Database, _Login, _Senha);
            }
        }

        public string SQL {
            get { return _SQL; }
            set { _SQL = value; }
        }

        public void Open() {
            //_DbConnection = new NpgsqlConnection(ConnectionString);
            //_DbConnection.Open();
        }

        public void Close() {
            //_DbConnection.Close();
        }

        public DataSet getDataSet(string strQuery) {
            this.Open();
            DataSet dataSet = new DataSet();
            //NpgsqlCommand command = new NpgsqlCommand(strQuery, _DbConnection);
            //command.CommandType = CommandType.Text;
            //NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command);
            //dataAdapter.Fill(dataSet);
            this.Close();
            return dataSet;
        }

        public bool execNonQuery(string str) {
            try {
                this.Open();
                //NpgsqlCommand command = new NpgsqlCommand(str, _DbConnection);
                //command.ExecuteNonQuery();
                this.Close();
            } catch(Exception ex) {
                this.Close();
                throw new Exception("Erro ao comunicar com a base de dados." + ex);
            }
            return (true);
        }

        public string execScalar(string str) {
            string strRetorno = string.Empty;
            try {
                this.Open();
                //NpgsqlCommand command = new NpgsqlCommand(str, _DbConnection);
                //strRetorno = (String)command.ExecuteScalar();
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
