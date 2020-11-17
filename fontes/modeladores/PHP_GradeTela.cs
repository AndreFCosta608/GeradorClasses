using System;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class PHP_GradeTela:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + "cadastro_" + formataTabela(tabela) + ".html";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados  = "<center>\n";
                    dados += "\t<b><i><h3>Cadastro de " + formataNomeClasse(tabela) + "</h3></i></b>\n";
	                dados += "\t<hr>\n";
                    dados += "\t<table>\n";
                    dados += "\t\t<tr>\n";
			        dados += "\t\t\t<td>\n";
                    dados += "\t\t\t\t" + formataNomeClasse(tabela) + " Cadastrados:\n";
			        dados += "\t\t\t</td>\n";
			        dados += "\t\t\t<td>\n";
                    dados += "\t\t\t\t<select id=\"lst" + formataNomeClasse(tabela) + "\" size=\"5\" onchange=\"carrega" + formataNomeClasse(tabela) + "();\"/>\n";
			        dados += "\t\t\t</td>\n";
		            dados += "\t\t</tr>\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            dados += "\t\t<tr>\n";
			                dados += "\t\t\t<td>\n";
                            dados += "\t\t\t\t" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " " + formataNomeClasse(tabela) + ":";
                            
                            string type = "text";
                            string prefixo = "txt";
                            int tamanho = 0;
                            string max = "maxlength=\"" + tamanho + "\"";
                            if(tipo.Contains("varchar")) {
                                type = "text";
                                prefixo = "txt";
                                tamanho = retornaTamanho(Conector, string.Empty, string.Empty, tipo);
                                max = "maxlength=\"" + tamanho + "\"";
                            }
                            if(tipo.Contains("bit")) {
                                type = "checkbox";
                                prefixo = "chk";
                                max = string.Empty;
                            }
                            if(tipo.Contains("int")) {
                                type = "number";
                                prefixo = "txt";
                                tamanho = retornaTamanho(Conector, string.Empty, string.Empty, tipo);
                                max = "maxlength=\"" + tamanho + "\"";
                            }
                            dados += "<input type=\"" + type + "\" id=\"" + prefixo + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" " + max + " />\n";
			                dados += "\t\t\t</td>\n";
		                    dados += "\t\t</tr>\n";
                            myStreamWriter.Write(dados);
                            myStreamWriter.Flush();
                            dados = string.Empty;
                        }
                    }
                    dados += "\t\t<tr>\n\t\t\t<td>\n";
                    dados += "\t\t\t\t<input type=\"button\" id=\"btnLimpar\" value=\"Limpar\" onclick=\"limparTela" + formataNomeClasse(tabela) + "();\" />\n";
			        dados += "\t\t\t</td>\n\t\t\t<td>\n";
                    dados += "\t\t\t\t<input type=\"button\" id=\"btnSalvar\" value=\"Salvar\" onclick=\"salva" + formataNomeClasse(tabela) + "();\" />\n";
                    dados += "\t\t\t</td>\n\t\t</tr>\n";
                    dados += "\t</table>\n";
                    dados += "</center>\n";
                    
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
