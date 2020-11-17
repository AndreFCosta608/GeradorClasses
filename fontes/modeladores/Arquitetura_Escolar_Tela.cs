using System;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class Arquitetura_Escolar_Tela:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".aspx";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados  = "<%@ Page Title=\"\" Language=\"C#\" MasterPageFile=\"~/interno/interno.Master\" AutoEventWireup=\"true\" CodeBehind=\"" + formataTabela(tabela) + ".aspx.cs\" Inherits=\"escolar." + formataTabela(tabela) + "\" %>\n";
                    dados += "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"cc1\" %>\n";
                    dados += "<%@ Register Assembly=\"Boleto.Net\" Namespace=\"BoletoNet\" TagPrefix=\"bn\" %>\n";
                    dados += "<%@ Register Src=\"~/controles/messageBox.ascx\" TagPrefix=\"uc1\" TagName=\"messageBox\" %>\n\n";
                    dados += "<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"head\" runat=\"server\">\n";
                    dados += "</asp:Content>\n\n";
                    dados += "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentPlaceHolder1\" runat=\"server\">\n";
                    dados += "\t\t<uc1:messageBox runat=\"server\" ID=\"messageBox\" />\n\n";
                    dados += "\t\t\t<h2>" + formataNomeClasse(tabela) + "</h2><br />\n\n";

                    dados += "\t\t\t<asp:Label ID=\"lblCadastrados\" runat=\"server\" Text=\""+ formataTabela(tabela) +" Cadastrados:\"></asp:Label>\n";
                    dados += "\t\t\t<asp:ListBox ID=\"lbCadastrados\" runat=\"server\" SelectionMode=\"Single\" OnSelectedIndexChanged=\"lbCadastrados_SelectedIndexChanged\" AutoPostBack=\"true\"></asp:ListBox>\n\n";
                    dados += "\t\t\t<br />\n";
                    dados += "\t\t\t<br />\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                    
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            
                            dados += "\t\t\t<asp:Label ID=\"lbl" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" runat=\"server\" Text=\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " " + formataNomeClasse(tabela) + ":\"></asp:Label>\n";

                            //string type = "text";
                            string prefixo = "txt";
                            int tamanho = 0;
                            string max = "maxlength=\"" + tamanho + "\"";
                            if(tipo.Contains("varchar")) {
                                //type = "text";
                                prefixo = "txt";
                                tamanho = retornaTamanho(Conector, string.Empty, string.Empty, tipo);
                                max = "maxlength=\"" + tamanho + "\"";
                            }
                            if(tipo.Contains("bit")) {
                                //type = "checkbox";
                                prefixo = "chk";
                                max = string.Empty;
                            }
                            if(tipo.Contains("int")) {
                                //type = "number";
                                prefixo = "txt";
                                tamanho = retornaTamanho(Conector, string.Empty, string.Empty, tipo);
                                max = "maxlength=\"" + tamanho + "\"";
                            }
                            dados += "\t\t\t<asp:TextBox ID=\"" + prefixo + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" MaxLength=\"" + tamanho + "\" runat=\"server\" ></asp:TextBox>\n";
                            dados += "\t\t\t<asp:RequiredFieldValidator ID=\"rfv" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" ControlToValidate=\"" + prefixo + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString())  + "\" runat=\"server\" ErrorMessage=\"*\" ></asp:RequiredFieldValidator>\n";
                            
                            dados += "\t\t\t<br />\n";
                            dados += "\t\t\t<br />\n\n";
		                    
                            myStreamWriter.Write(dados);
                            myStreamWriter.Flush();
                            dados = string.Empty;
                        }
                    }
                    
                    dados += "\t\t\t<asp:LinkButton ID=\"lbtnLimpar\" runat=\"server\" Text=\"Limpar\" OnClick=\"lbtnLimpar_Click\" ValidationGroup=\"executar\"></asp:LinkButton>\n";
                    dados += "\t\t\t<asp:LinkButton ID=\"lbtnExcluir\" runat=\"server\" Text=\"Excluir\" OnClick=\"lbtnExcluir_Click\" ValidationGroup=\"executar\"></asp:LinkButton>\n";
                    dados += "\t\t\t<asp:LinkButton ID=\"lbtnSalvar\" runat=\"server\" Text=\"Salvar\" OnClick=\"lbtnSalvar_Click\"></asp:LinkButton>\n\n";

                    dados += "</center>\n";
                    
                    dados += "</asp:Content>\n";
                    
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
