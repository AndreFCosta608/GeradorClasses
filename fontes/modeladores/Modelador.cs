using System;
using System.Text;
using System.Data;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class Modelador {
        public DataSet RetornaDescricao(string tabela, IConector Conector) {
            string sql = string.Empty;
            if(Conector.GetType() == typeof(MySqlConnector)) {
                sql = " DESCRIBE " + tabela + "; ";
            } else if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                sql = " SELECT cols.name as Field,  ";
                sql += " typs.name as Type,  ";
                sql += " cols.Length,  ";
                sql += " cols.prec as Precision,  ";
                sql += " cols.Scale,  ";
                sql += " Allownulls as [Allow Nulls],  ";
                sql += " case when cols.name IN (SELECT ccu.column_name ";
                sql += " FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc  ";
                sql += " JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu  ";
                sql += " ON tc.CONSTRAINT_NAME = ccu.Constraint_name  ";
                sql += " WHERE tc.CONSTRAINT_TYPE = 'Primary Key' ";
                sql += " and tc.table_name = '" + tabela + "')  ";
                sql += " then 'PRI' ";
                sql += " else ' ' ";
                sql += " end [Key] ";
                sql += " FROM syscolumns cols  ";
                sql += " INNER JOIN systypes typs  ";
                sql += " ON cols.xusertype = typs.xusertype  ";
                sql += " INNER JOIN sysobjects obj ";
                sql += " ON cols.id = obj.id ";
                sql += " INNER JOIN sys.key_constraints  keys ";
                sql += " ON keys.parent_object_id = obj.id ";
                sql += " WHERE obj.name = '" + tabela + "' ";
            } else {
                //TODO: Ajustar ao postgre
            }
            return (Conector.getDataSet(sql));
        }

        public DataSet RetornaDetalhesTabela(IConector Conector, string schema) {
            string sql = string.Empty;
            if(Conector.GetType() == typeof(MySqlConnector)) {
                sql = " SHOW TABLES; ";
            } else if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                sql = " use " + schema + " ; ";
                sql += " select TABLE_NAME from information_schema.tables; ";
            } else {//PostGre
                //TODO: Ajustar ao postgre
            }
            return (Conector.getDataSet(sql));
        }

        public bool RetornaAutoIncrement(IConector Conector, string tabela, string campo) {
            string sql = string.Empty;
            if(Conector.GetType() == typeof(MySqlConnector)) {
                sql = " ";
            } else if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                sql = " select COLUMN_NAME coluna from INFORMATION_SCHEMA.COLUMNS ";
                sql += " where TABLE_SCHEMA = 'dbo' ";
                sql += " and COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 ";
                sql += " and TABLE_NAME = '" + tabela + "' ";
            } else {
                //TODO: Não implementado
            }
            DataSet lista = Conector.getDataSet(sql);
            bool retorno = false;
            for(int subcontador = 0; subcontador < lista.Tables[0].Rows.Count; subcontador++) {
                string teste = lista.Tables[0].Rows[subcontador]["coluna"].ToString();
                if(teste.ToLower() == campo.ToLower()) {
                    retorno = true;
                }
            }
            return (retorno);
        }

        public DataSet RetornaRelacionamentos(string schema, string tabela, string campo, IConector Conector) {
            string sql = string.Empty;
            if(Conector.GetType() == typeof(MySqlConnector)) {
                sql = "  SELECT REFERENCED_TABLE_NAME AS TABELA, REFERENCED_COLUMN_NAME AS COLUNA ";
                sql += " FROM information_schema.KEY_COLUMN_USAGE WHERE CONSTRAINT_NAME <> 'PRIMARY' ";
                sql += " AND REFERENCED_TABLE_SCHEMA = '" + schema + "'";
                sql += " AND TABLE_NAME = '" + tabela + "'";
                sql += " AND COLUMN_NAME = '" + campo + "';";
            } else {
                //TODO: Ajustar ao postgre
            }
            return (Conector.getDataSet(sql));
        }

        public static string formataNomeClasse(string nome) {
            string primeiraLetra = nome.Substring(0, 1).ToUpper();
            string restante = nome.Substring(1, (nome.Length - 1));
            return (primeiraLetra + restante);
        }

        public static string formataTabela(string nome) {
            string primeiraLetra = nome.Substring(0, 1).ToLower();
            string restante = nome.Substring(1, (nome.Length - 1));
            return (primeiraLetra + restante);
        }

        public static string CSharpType(string nome) {
            //string temp = nome.Substring(0, 4);
            int tamanho = nome.Length;

            if((tamanho >= 3) && (nome.Substring(0, 3) == "char"))
                return "char";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "enum"))
                return "enum";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "text"))
                return "string";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "int"))
                return "Int32";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "bit"))
                return "bool";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "date"))
                return "DateTime";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "blob"))
                return "Byte[]";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "float"))
                return "float";
            else if((tamanho >= 5) && (nome.Substring(0, 5) == "double"))
                return "double";
            else if((tamanho >= 6) && (nome.Substring(0, 6) == "bigint"))
                return "Int64";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "decimal"))
                return "decimal";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "varchar"))
                return "string";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "datetime"))
                return "DateTime";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "interger"))
                return "Int32";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "longblob"))
                return "Byte[]";
            return "string";
        }

        public static string NHibernateType(string nome) {
            string temp = nome.Substring(0, 4);
            int tamanho = nome.Length;

            if((tamanho >= 3) && (nome.Substring(0, 3) == "char"))
                return "char";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "enum"))
                return "enum";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "text"))
                return "string";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "int"))
                return "Int32";
            else if((tamanho >= 3) && (nome.Substring(0, 3) == "bit"))
                return "boolean";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "date"))
                return "DateTime";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "blob"))
                return "Byte[]";
            else if((tamanho >= 4) && (nome.Substring(0, 4) == "float"))
                return "float";
            else if((tamanho >= 5) && (nome.Substring(0, 5) == "double"))
                return "double";
            else if((tamanho >= 6) && (nome.Substring(0, 6) == "bigint"))
                return "Int64";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "decimal"))
                return "float";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "varchar"))
                return "string";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "datetime"))
                return "DateTime";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "interger"))
                return "Int32";
            else if((tamanho >= 7) && (nome.Substring(0, 7) == "longblob"))
                return "Byte[]";
            return "string";
        }

        public static string somenteNumericos(string nome) {
            string temp = string.Empty;
            foreach(char caracter in nome) {
                if(caracter == '0')
                    temp += '0';
                if(caracter == '1')
                    temp += '1';
                if(caracter == '2')
                    temp += '2';
                if(caracter == '3')
                    temp += '3';
                if(caracter == '4')
                    temp += '4';
                if(caracter == '5')
                    temp += '5';
                if(caracter == '6')
                    temp += '6';
                if(caracter == '7')
                    temp += '7';
                if(caracter == '8')
                    temp += '8';
                if(caracter == '9')
                    temp += '9';
                if(caracter == ',')
                    temp += ',';
            }
            return temp;
        }

        public static int retornaTamanho(IConector Conector, string tabela, string campo, string tipo) { 
            int retorno = 0;
            if(Conector.GetType() == typeof(MySqlConnector)) {
                string[] temporario = tipo.Split('(');
                string temp = temporario[1].Replace(")", string.Empty);
                retorno = int.Parse(temp);
            } else if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                //TODO: Não implementado
            } else if(Conector.GetType() == typeof(PostgreConnector)) {
                //TODO: Não implementado
            }
            return(retorno);
        }

    }
}
