using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class Arquitetura_Escolar_Persistencia:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataTabela(tabela) + ".cs";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "using System;\n";
                    dados += "using MySql.Data;\n";
                    dados += "using System.Data;\n";
                    dados += "using System.Linq;\n";
                    dados += "using MongoDB.Bson;\n";
                    dados += "using System.Data.OleDb;\n";
                    dados += "using System.Data.Common;\n";
                    dados += "using MySql.Data.MySqlClient;\n\n";
                    dados += "namespace persistencia {\n\n";
                    dados += "\tpublic partial class " + formataNomeClasse(tabela) + " {\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                                        
                    dados += "\t\tprivate MySqlConnector conector;\n";
                    dados += "\t\tprivate static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);\n\n";
                    dados += "\t\tpublic " + formataNomeClasse(tabela) + "() {\n";
                    dados += "\t\t\tconector = SingletonMySql.getInstance();\n";
                    dados += "\t\t}\n\n";
            
                    //retorna lista
                    dados += "\t\tpublic DataSet listar(int idInstituicao) {\n";
                    dados += "\t\t\tstring sql = string.Empty;\n ";
                    
                    
                    dados += "\t\t\ttry {\n";

                    
                    dados += "\t\t\t\tsql = \"select ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                            dados += ", ";
                        else
                            dados += "\";";
                    }
                    dados += "\n\t\t\t\tsql += \" FROM " + formataTabela(tabela) + "\";\n";
                    dados += "\t\t\t\treturn (conector.getDataSet(sql));\n";

             
                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".listar SQL => \" + sql);\n";
                    dados += "\t\t\t\tthrow new Exception(ex.Message);\n";
                    dados += "\t\t\t}\n";

                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //retorna item
                    dados = "\n\t\tpublic DataSet retorna(int id) {\n";
                    dados += "\t\t\tstring sql = string.Empty;\n";

                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tsql = \" select ";                    
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += "\";";
                    dados += "\n\t\t\t\tsql += \" FROM " + formataTabela(tabela) + "\";\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\t\t\t\tsql += \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + id;";
                        }
                    }
                    dados += "\n\t\t\t\treturn (conector.getDataSet(sql));\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".retorna SQL => \" + sql);\n";
                    dados += "\t\t\t\tthrow new Exception(ex.Message);\n";
                    dados += "\t\t\t}\n";

                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //insert
                    dados = "\n\t\tpublic void cadastra(BsonDocument json) {\n";
				    dados += "\t\t\tstring sql = string.Empty;\n";
                    
                    dados += "\t\t\ttry {\n";
                    
                    dados += "\t\t\t\tsql  = \" insert into " + formataNomeClasse(tabela) + "(";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += ") values (\";\n";
                    dados += "\t\t\t\tsql += ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            bool plica = false;
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                plica = true;
                            }
                            if(plica == true) {
                                dados = dados.Remove(dados.Length - 1);
                                dados += " \"'\" + ";
                            }
                            dados += " json[\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"] ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                   dados += "+ \"', \" + ";
                                } else {
                                    dados += "+ \", \" + ";
                                }
                            }
                            if(subcontador == (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                    dados += " + \"'\" ";
                                }
                            }
                        }
                    }
                    dados += "+ \");\";\n";
                    dados += "\t\t\t\tconector.execNonQuery(sql);\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".cadastra SQL => \" + sql);\n";
                    dados += "\t\t\t\tthrow new Exception(ex.Message);\n";
                    dados += "\t\t\t}\n";

		            dados += "\t\t}\n\n";
		
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                    
                    //Atualiza
		            dados = "\t\tpublic void atualiza(BsonDocument json) {\n";
                    dados += "\t\t\tstring sql = string.Empty;";
                    dados += "\n\t\t\ttry {\n";
                    dados += "\t\t\t\tsql  = \" update " + formataNomeClasse(tabela) + " set ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " = ";
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            bool plica = false;
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                plica = true;
                            }
                            if(plica == true) {
                                dados += "'";
                            }
                            dados += "\" + json[\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"] + \"";
                            if(plica == true) {
                                dados += "'";
                            }
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += "\";\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\t\t\t\tsql += \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            dados += " = \" + json[\"" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "\"] + \";\";\n";
                        }
                    }
                    dados += "\t\t\t\tconector.execNonQuery(sql);\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".atualiza SQL => \" + sql);\n";
                    dados += "\t\t\t\tthrow new Exception(ex.Message);\n";
                    dados += "\t\t\t}\n";

			        dados += "\t\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    //deleta item
                    dados = "\n\t\tpublic void deleta(int id) {\n";
                    dados += "\t\t\tstring sql = string.Empty;\n";

                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tsql = \" DELETE FROM "  + formataTabela(tabela) + " WHERE ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" + id;";
                        }
                    }
                    dados += "\n\t\t\t\tconector.execNonQuery(sql);\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".retorna SQL => \" + sql);\n";
                    dados += "\t\t\t\tthrow new Exception(ex.Message);\n";
                    dados += "\t\t\t}\n";

                    dados += "\t\t}\n\n";
		            dados += "\t}\n";
		            dados += "}\n";
                    
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    myStreamWriter.Close();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Erro na geracao do Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
