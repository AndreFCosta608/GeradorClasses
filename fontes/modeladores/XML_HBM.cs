using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {

    public class XML_HBM:Modelador {

        public void GerarArquivosXML_HBM(string Caminho, DataSet listaTabela, IConector Conector, string schema) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();

                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".hbm.xml";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\n";

                    //dados += "<Assinatura>\n";
                    //dados += objColecao.Assinatura.ToString() + "\n";
                    //dados += "</Assinatura>\n\n";

                    //dados += "<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\">\n";
                    dados += "<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.0\">\n";
                    dados += "\t<class name=\"" + formataNomeClasse(tabela) + ", MapeamentoOR\" table=\"" + formataTabela(tabela) + "\" lazy=\"true\">\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados = "\t\t<id name=\"" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            dados += "\" column=\"" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            dados += "\" type=\"" + NHibernateType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                            dados += "\" unsaved-value=\"0\">\n";
                            dados += "\t\t\t<generator class=\"assigned\"/>\n";
                            dados += "\t\t</id>\n";

                            myStreamWriter.Write(dados);
                            myStreamWriter.Flush();
                            dados = string.Empty;
                        } else if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "MUL") {
                            DataSet constraints = RetornaRelacionamentos(schema, tabela, detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString(), Conector);

                            //Many to One
                            dados = "\n\t\t<many-to-one name=\"" + formataNomeClasse(constraints.Tables[0].Rows[0]["TABELA"].ToString()) + "\"";
                            dados += " class=\"" + formataNomeClasse(constraints.Tables[0].Rows[0]["TABELA"].ToString());
                            dados += "\" column=\"" + formataNomeClasse(constraints.Tables[0].Rows[0]["COLUNA"].ToString());
                            dados += "\" not-null=\"true\" />";

                            //myStreamWriter.Write(dados);
                            //myStreamWriter.Flush();
                            //dados = string.Empty;
                        } else {
                            dados = "\t\t<property column=\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            dados += "\" type=\"" + NHibernateType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                            dados += "\" name=\"" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" ";

                            string tipo = NHibernateType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString());
                            if((tipo != "DateTime") && (tipo != "decimal")) {
                                dados += "length=\"" + somenteNumericos(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + "\"";
                            }
                            dados += " />\n";

                            myStreamWriter.Write(dados);
                            myStreamWriter.Flush();
                            dados = string.Empty;
                        }
                    }
                    dados = "\n\t</class>\n";
                    dados += "</hibernate-mapping>\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();

                    myStreamWriter.Close();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Erro na geracao do DAO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception(ex.Message);
            }
        }
    }
}
