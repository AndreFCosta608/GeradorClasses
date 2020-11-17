using System;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class Arquitetura_Escolar_CodeBehind:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".aspx.cs";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;
                    dados  = "using System;\n";
                    dados += "using BoletoNet;\n";
                    dados += "using System.IO;\n";
                    dados += "using Seguranca;\n";
                    dados += "using System.Web;\n";
                    dados += "using System.Data;\n";
                    dados += "using System.Linq;\n";
                    dados += "using MongoDB.Bson;\n";
                    dados += "using persistencia;\n";
                    dados += "using System.Web.UI;\n";
                    dados += "using System.Configuration;\n";
                    dados += "using System.Web.UI.WebControls;\n";
                    dados += "using System.Collections.Generic;\n\n";
                    dados += "namespace escolar {\n\n";
                    dados += "\tpublic partial class " + formataNomeClasse(tabela) + ":System.Web.UI.Page {\n\n";
                    dados += "\t\t#region variaveis\n\n";
                    dados += "\t\tprivate static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);\n";
                    dados += "\t\tprivate int idInstituicao = 0;\n\n";
                    dados += "\t\t#endregion\n\n";
                    dados += "\t\t#region eventos\n\n";
                    dados += "\t\tprotected void Page_Load(object sender, EventArgs e) {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\t((interno)Page.Master).nivelAcesso = 4;\n";
                    dados += "\t\t\t\t((interno)Page.Master).validaLogado();\n";
                    dados += "\t\t\t\tidInstituicao = int.Parse(Session[\"idInstituicao\"].ToString());\n";
                    dados += "\t\t\t\tif(!IsPostBack) {\n";
                    dados += "\t\t\t\t\tpopula();\n";
                    dados += "\t\t\t\t}\n";
                    dados += "\t\t\t} catch(Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"" + formataNomeClasse(tabela) + ".load => \" + ex.Message + \" Inner =  \" + ex.InnerException);\n";
                    dados += "\t\t\t\tmessageBox.AddMessage(\"Falha ao tentar carregar a pagina.<br>Tente novamente.<br>Se o problema continuar procure o suporte técnico.\", messageBox.enmMessageType.Error);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";
                    
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados += "\t\tprotected void lbtnLimpar_Click(object sender, EventArgs e) {\n";
                    dados += "\t\t\tlimparCampos();\n";
                    dados += "\t\t}\n\n";
                    dados += "\t\tprotected void lbtnSalvar_Click(object sender, EventArgs e) {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tsalvar();\n";
                    dados += "\t\t\t} catch(Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"" + formataNomeClasse(tabela) + ".salvar => \" + ex.Message + \" Inner =  \" + ex.InnerException);\n";
                    dados += "\t\t\t\tmessageBox.AddMessage(\"Falha ao tentar carregar a pagina.<br>Tente novamente.<br>Se o problema continuar procure o suporte técnico.\", messageBox.enmMessageType.Error);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";
                    dados += "\t\tprotected void lbCadastrados_SelectedIndexChanged(object sender, EventArgs e) {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\tpopulaTela();\n";
                    dados += "\t\t\t} catch(Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"" + formataNomeClasse(tabela) + ".lbCadastrados_SelectedIndexChanged => \" + ex.Message + \" Inner =  \" + ex.InnerException);\n";
                    dados += "\t\t\t\tmessageBox.AddMessage(\"Falha ao tentar carregar a pagina.<br>Tente novamente.<br>Se o problema continuar procure o suporte técnico.\", messageBox.enmMessageType.Error);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";

                    dados += "\t\tprotected void lbtnExcluir_Click(object sender, EventArgs e) {\n";
                    dados += "\t\t\ttry {\n";
                    dados += "\t\t\t\texcluir();\n";
                    dados += "\t\t\t} catch(Exception ex) {\n";
                    dados += "\t\t\t\tlog.Error(\"" + formataNomeClasse(tabela) + ".lbCadastrados_SelectedIndexChanged => \" + ex.Message + \" Inner =  \" + ex.InnerException);\n";
                    dados += "\t\t\t\tmessageBox.AddMessage(\"Falha ao tentar carregar a pagina.<br>Tente novamente.<br>Se o problema continuar procure o suporte técnico.\", messageBox.enmMessageType.Error);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";

                    dados += "\t#endregion\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados += "\t\t#region metodos\n\n";
                    dados += "\t\tprivate void popula() {\n";
                    dados += "\t\t\tDataSet retorno = (new " + formataNomeClasse(tabela) + "()).listar(this.idInstituicao);\n";
                    dados += "\t\t\tlbCadastrados.Items.Clear();\n";
                    dados += "\t\t\tlbCadastrados.DataValueField = \"idPessoa\";\n";
                    dados += "\t\t\tlbCadastrados.DataTextField = \"nome\";\n";
                    dados += "\t\t\tlbCadastrados.DataSource = retorno;\n";
                    dados += "\t\t\tlbCadastrados.DataBind();\n";
                    dados += "\t\t\tif(retorno.Tables[0].Rows.Count <= 0) {\n";
                    dados += "\t\t\t\tmessageBox.AddMessage(\"Nenhum " + formataTabela(tabela) + " encontrado.<br>Cadastre um agora...\", messageBox.enmMessageType.Info);\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";
                    dados += "\t\tprivate void limparCampos() {\n";
                    dados += "\t\t\tlbCadastrados.SelectedIndex = -1;\n";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\t\t\ttxt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".Text = string.Empty;\n";
                        }
                    }

                    dados += "\t\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados += "\t\tprivate void populaTela() {\n";
                    dados += "\t\t\tint id = int.Parse(lbCadastrados.SelectedValue.ToString());\n";
                    dados += "\t\t\tDataSet retorno = (new " + formataNomeClasse(tabela) + "()).retorna(id);\n";
                    dados += "\t\t\tif(retorno.Tables[0].Rows.Count > 0) {\n";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\t\t\t\ttxt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".Text = retorno.Tables[0].Rows[0][\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\"].ToString();\n";
                        }
                    }

                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    dados += "\t\tprivate void excluir() {\n";
                    dados += "\t\t\tint id = 0;\n";
                    dados += "\t\t\tif(lbCadastrados.SelectedValue != string.Empty) {\n";
                    dados += "\t\t\t\tid = int.Parse(lbCadastrados.SelectedValue.ToString());\n";
                    dados += "\t\t\t\t(new " + formataNomeClasse(tabela) + "()).excluir(id);\n";
                    dados += "\t\t\t\tlimparCampos();\n";
                    dados += "\t\t\t\tpopula();\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    dados += "\t\tprivate void salvar() {\n";
                    dados += "\t\t\tint id = 0;\n";
                    dados += "\t\t\tif(lbCadastrados.SelectedValue != string.Empty)\n";
                    dados += "\t\t\t\tid = int.Parse(lbCadastrados.SelectedValue.ToString());\n";
                    dados += "\t\t\t\tBsonDocument parametros = new BsonDocument();\n";
                    dados += "\t\t\t\tparametros.Add(\"idInstituicao\", this.idInstituicao);\n";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\t\t\t\tparametros.Add(\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\", caoGuarda.filtraInjection(txt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ".Text));\n";
                        }
                    }                                
                    dados += "\t\t\t\tif(id <= 0) {\n";
                    dados += "\t\t\t\t\t(new " + formataNomeClasse(tabela) + "()).cadastra(parametros);\n";
                    dados += "\t\t\t\t} else {\n";
                    dados += "\t\t\t\t\t(new " + formataNomeClasse(tabela) + "()).atualiza(parametros);\n";
                    dados += "\t\t\t\t}\n";
                    dados += "\t\t\t\tlimparCampos();\n";
                    dados += "\t\t\t\tpopula();\n";
                    dados += "\t\t\t}\n\n";
                    dados += "\t\t#endregion\n\n";
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
