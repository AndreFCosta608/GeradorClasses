using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class DAO:Modelador {

        public void GerarArquivosDAO(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
        
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + "DAO.partial.cs";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "/*\n";
                    dados += objColecao.Assinatura.ToString() + "\n";
                    dados += "*/\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "using System;\n";
                    dados += "using System.Data;\n";
                    dados += "using System.Configuration;\n";
                    dados += "using System.Collections;\n";
                    dados += "using System.Collections.Generic;\n";

                    if(Conector.GetType() == typeof(MySqlConnector)) {
                        dados += "using MySql.Data;\n";
                        dados += "using MySql.Data.MySqlClient;\n\n\n";
                    }
                    if(Conector.GetType() == typeof(PostgreConnector)) {
                        dados += "using Npgsql;\n";
                        dados += "using NpgsqlTypes;\n\n\n";
                    }
                    if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                        dados += "using System.Data.Sql;\n";
                        dados += "using System.Data.SqlClient;\n\n\n";
                    }

                    dados += "namespace " + strNameSpace + "\n{\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tpublic partial class " + formataNomeClasse(tabela) + "DAO\n";
                    dados += "\t{\n";

                    string[] tipoConector = Conector.GetType().ToString().Split('.');

                    dados += "\t\tprivate " + tipoConector[tipoConector.Length - 1] + " objConexao;\n\n";
                    dados += "\t\tpublic  " + formataNomeClasse(tabela) + "DAO()\n\t\t{\n";
                    dados += "\t\t\tif (objConexao == null)\n\t\t\t\tobjConexao = new " + tipoConector[tipoConector.Length - 1] + "();\n\t\t}";

                    //select objeto
                    dados += "\n\n\t\tpublic " + formataNomeClasse(tabela) + " Retorna" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{\n";

                    dados += "\t\t\tstring sql = \"SELECT ";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                            dados += ", ";
                        else
                            dados += "\";";
                    }
                    dados += "\n\t\t\tsql += \" FROM " + formataTabela(tabela) + "\";\n";

                    int contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            if(contaChaves == 0) {
                                dados += "\t\t\tsql += \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            } else {
                                dados += "\n\t\t\tsql += \" AND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            }
                            contaChaves = contaChaves + 1;
                        }
                    }
                    contaChaves = 0;

                    dados += "\n\n\t\t\tDataSet ds = objConexao.getDataSet(sql);\n\n\t\t\t";
                    dados += formataNomeClasse(tabela) + " item = new " + formataNomeClasse(tabela) + "();";
                    dados += "\n\n\t\t\tif (ds.Tables[0].Rows.Count > 0)\n\t\t\t{";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\n\t\t\t\titem." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());

                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            dados += " = ds.Tables[0].Rows[0][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"].ToString();";
                        } else {
                            dados += " = ((" + CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + ")ds.Tables[0].Rows[0][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"]);";
                        }
                    }

                    dados += "\n\t\t\t}\n\n\t\t\treturn(item);\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    //select lista
                    dados += "\n\n\t\tpublic List<" + formataNomeClasse(tabela) + "> Retorna" + formataNomeClasse(tabela) + "()\n\t\t{";
                    dados += "\n\t\t\tstring sql = \"SELECT ";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                            dados += ", ";
                        else
                            dados += "\";";
                    }
                    dados += "\n\t\t\tsql += \" FROM " + formataTabela(tabela) + "\";\n";
                    contaChaves = 0;
                    dados += "\n\n\t\t\tDataSet ds = objConexao.getDataSet(sql);\n\n\t\t\t";

                    dados += formataNomeClasse(tabela) + " item;";
                    dados += "\n\t\t\tList < " + formataNomeClasse(tabela) + " > lista" + formataNomeClasse(tabela) + " = new List<" + formataNomeClasse(tabela) + ">();";

                    dados += "\n\n\t\t\tint linhas = ds.Tables[0].Rows.Count;";
                    dados += "\n\n\t\t\tfor (int contador = 0; contador < linhas; contador++)\n\t\t\t{\n";
                    dados += "\t\t\t\titem = new " + formataNomeClasse(tabela) + "();";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\n\t\t\t\titem." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());

                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            dados += " = ds.Tables[0].Rows[contador][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"].ToString();";
                        } else {
                            dados += " = ((" + CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + ")ds.Tables[0].Rows[contador][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"]);";
                        }
                    }
                    dados += "\n\t\t\t\tlista" + formataNomeClasse(tabela) + ".Add(item);";
                    dados += "\n\t\t\t}\n\t\t\treturn (lista" + formataNomeClasse(tabela) + ");\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //select lista com like
                    dados += "\n\n\t\tpublic List<" + formataNomeClasse(tabela) + "> Retorna" + formataNomeClasse(tabela) + "(string filtro)\n\t\t{";
                    dados += "\n\t\t\tstring sql = \"SELECT ";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                            dados += ", ";
                        else
                            dados += "\";";
                    }
                    dados += "\n\t\t\tsql += \" FROM " + formataTabela(tabela) + "\";\n";


                    contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            if(contaChaves == 0) {
                                dados += "\t\t\tsql += \" WHERE " + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " LIKE '%\" + filtro + \"%'\";";
                                contaChaves = contaChaves + 1;
                            } else {
                                dados += "\n\t\t\tsql += \" OR " + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " LIKE '%\" + filtro + \"%'\";";
                                contaChaves = contaChaves + 1;
                            }
                        }
                    }
                    contaChaves = 0;
                    dados += "\n\n\t\t\tDataSet ds = objConexao.getDataSet(sql);\n\n\t\t\t";

                    dados += formataNomeClasse(tabela) + " item;";
                    dados += "\n\t\t\tList < " + formataNomeClasse(tabela) + " > lista" + formataNomeClasse(tabela) + " = new List<" + formataNomeClasse(tabela) + ">();";

                    dados += "\n\n\t\t\tint linhas = ds.Tables[0].Rows.Count;";
                    dados += "\n\n\t\t\tfor (int contador = 0; contador < linhas; contador++)\n\t\t\t{\n";
                    dados += "\t\t\t\titem = new " + formataNomeClasse(tabela) + "();";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\n\t\t\t\titem." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());

                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            dados += " = ds.Tables[0].Rows[contador][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"].ToString();";
                        } else {
                            dados += " = ((" + CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + ")ds.Tables[0].Rows[contador][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"]);";
                        }
                    }
                    dados += "\n\t\t\t\tlista" + formataNomeClasse(tabela) + ".Add(item);";
                    dados += "\n\t\t\t}\n\t\t\treturn (lista" + formataNomeClasse(tabela) + ");\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //insert
                    dados += "\n\n\t\tpublic int insere" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";

                    dados += "\n\n\t\t\tstring sql = \"INSERT INTO " + formataTabela(tabela) + "(";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(!(RetornaAutoIncrement(Conector, tabela, detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()))) {
                            dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                                dados += ", ";
                            else
                                dados += ")\";";
                        }
                    }
                    dados += "\n\t\t\tsql += \" VALUES (";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(!(RetornaAutoIncrement(Conector, tabela, detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()))) {
                            string tipo = CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                            if((tipo == "string") || (tipo == "DateTime")) {
                                dados += "\'\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " + \"\'";
                            } else if(tipo == "bool") {
                                dados += "\" + ((obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".ToString().ToLower() == \"false\") ? 0 : 1 ) + \"";
                            } else if(tipo == "decimal") {
                                dados += "\'\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".ToString().Replace(\",\",\".\") + \"\'";
                            } else {
                                dados += "\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " + \"";
                            }
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                                dados += ", ";
                            else
                                dados += ");\";";
                        }
                    }
                    if(Conector.GetType() == typeof(MySqlConnector)) {
                        dados += " \n\t\t\tsql += \" SELECT @@identity; \"; ";
                    }
                    if(Conector.GetType() == typeof(MSSQLSERVERConnector)) {
                        dados += " \n\t\t\tsql += \" SELECT @@identity; \"; ";
                    }
                    dados += "\n\n\t\t\t return(Convert.ToInt32(objConexao.execScalar(sql))); \n\n\t\t}";
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    //Deleta
                    dados += "\n\n\t\tpublic void exclui" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";

                    dados += "\n\n\t\t\tstring sql = \"DELETE FROM " + formataTabela(tabela) + "\";";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            if(contaChaves == 0) {
                                dados += "\n\t\t\tsql += \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            } else {
                                dados += "\n\t\t\tsql += \" AND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            }
                            contaChaves = contaChaves + 1;
                        }
                    }
                    contaChaves = 0;
                    dados += "\n\n\t\t\tobjConexao.execNonQuery(sql);\n\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    //Update
                    dados += "\n\n\t\tpublic void atualisa" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";

                    dados += "\n\n\t\t\tstring sql = \"UPDATE " + formataTabela(tabela) + " SET \";";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\n\t\t\tsql += \"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " = ";
                            string tipo = CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                            if((tipo == "string") || (tipo == "DateTime")) {
                                dados += "\'\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " + \"\'\"";
                            } else if(tipo == "bool") {
                                dados += "\" + ((obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".ToString().ToLower() == \"false\") ? 0 : 1 ) ";
                            } else if(tipo == "decimal") {
                                dados += "\'\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".ToString().Replace(\",\",\".\") + \"\'\"";
                            } else {
                                dados += "\" + obj" + formataNomeClasse(tabela) + "." + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            }

                            if(subcontador < detalheTabela.Tables[0].Rows.Count - 1)
                                dados += " + \", \";";
                            else
                                dados += ";";
                        }
                    }
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            if(contaChaves == 0) {
                                dados += "\n\t\t\tsql += \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            } else {
                                dados += "\n\t\t\tsql += \" AND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + obj" + formataNomeClasse(tabela) + ".";
                                dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";";
                            }
                            contaChaves = contaChaves + 1;
                        }
                    }
                    contaChaves = 0;

                    dados += "\n\n\t\t\tobjConexao.execNonQuery(sql);\n\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    dados = "\n\n\t}\n}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();

                    myStreamWriter.Close();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Erro na geracao do DAO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
