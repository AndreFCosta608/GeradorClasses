using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class CamadaControlePersistencia : Modelador {
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
                    dados += "using log4net;\n";
                    dados += "using Seguranca;\n";
                    dados += "using MySql.Data;\n";
                    dados += "using System.Web;\n";
                    dados += "using System.Data;\n";
                    dados += "using System.Linq;\n";
                    dados += "using persistencia;\n";
                    dados += "using MongoDB.Bson;\n";
                    dados += "using System.Collections.Generic;\n\n";
                    dados += "namespace nucleo {\n\n";
                    dados += "\tpublic partial class " + formataNomeClasse(tabela) + " {\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                                        
                    dados += "\t\tprivate static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);\n\n";
                    
                    //Listar
                    dados += "\t\tpublic BsonArray listar() {\n";
                    dados += "\t\t\tBsonArray lista = new BsonArray();\n ";
                    dados += "\t\t\tBsonDocument item;\n ";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tDataSet ds = (new persistencia." + formataNomeClasse(tabela) + "()).listar();\n";
                    dados += "\t\t\t\tforeach(DataRow dr in ds.Tables[0].Rows) {\n";
                    dados += "\t\t\t\t\titem = new BsonDocument();\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\t\t\t\t\titem.Add(\"";
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        dados += "\", dr[\"";
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        dados += "\"].ToString());\n";
                    }
                    dados += "\t\t\t\t\tlista.Add(item);\n";
                    dados += "\t\t\t\t}\n";
                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".listar Erro => \" + ex.Message);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t\treturn (lista);\n";
                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //retorna item
                    dados = "\n\t\tpublic BsonDocument retorna(int id) {\n";
                    dados += "\t\t\tBsonDocument item = new BsonDocument();\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tDataSet ds = (new persistencia." + formataNomeClasse(tabela) + "()).retorna(id);\n";                    
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\t\t\t\titem.Add(\"";
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            dados += "\", ds.Tables[0].Rows[0][\"";
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            dados += "\"].ToString());\n";
                        }
                    }
                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".retorna Erro => \" + ex.Message);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t\treturn (item);\n";
                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //cadastro
                    dados = "\n\t\tpublic bool salvar(";
				    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                        dados += " ";
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                    }
                    dados += ") {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tBsonDocument item = new BsonDocument();\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\t\t\t\titem.Add(\"";
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        dados += "\", ";
                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            dados += "caoGuarda.filtraInjection(";
                        }
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        if(CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) == "string") {
                            dados += ")";
                        }
                        dados += ");\n";
                    }                    
                    dados += "\t\t\t\tif(";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        }    
                    }
                    dados += " <= 0)\n";
                    dados += "\t\t\t\t\t(new persistencia." + formataNomeClasse(tabela) + "()).cadastra(item);\n";
                    dados += "\t\t\t\telse\n";
                    dados += "\t\t\t\t\t(new persistencia." + formataNomeClasse(tabela) + "()).atualiza(item);\n";
                    dados += "\t\t\t\treturn(true);\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".cadastra Erro => \" + ex.Message);\n";
                    dados += "\t\t\t\treturn(false);\n";
                    dados += "\t\t\t}\n";

		            dados += "\t\t}\n";
		
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                    
                    //deleta item
                    dados = "\n\t\tpublic void excluir(int id) {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\t(new persistencia."  + formataNomeClasse(tabela) + "()).deleta(id);\n";

                    dados += "\t\t\t} catch (Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"persistencia." + formataNomeClasse(tabela) + ".retorna Erro => \" + ex.Message);\n";
                    dados += "\t\t\t}\n";

                    dados += "\t\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

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
