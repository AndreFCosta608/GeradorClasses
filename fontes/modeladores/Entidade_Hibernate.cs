using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class EntidadeHibernate:Modelador {
        public void GerarArquivosEntidadeHibernate(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();

                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".partial.cs";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "/*\n";
                    dados += objColecao.Assinatura.ToString() + "\n";
                    dados += "*/\n\n";

                    dados += "using System;\n";
                    dados += "using System.Text;\n";
                    dados += "using System.Collections;\n";
                    dados += "using System.Collections.Generic;\n";
                    dados += "using NHibernate.Mapping.Attributes;\n\n";
                    dados += "namespace " + strNameSpace + "\n{\n";

                    dados += "public partial class " + formataNomeClasse(tabela) + "\n";
                    dados += "{\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados = "\n\tprivate " + CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + " _" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";\n";

                        dados += "\tpublic virtual " + CSharpType(detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString()) + " " + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\n";
                        dados += "\t{\n";
                        dados += "\t\tget { return _" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "; }\n";
                        dados += "\t\tset { _" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " = value; }\n";
                        dados += "\t}\n\n";

                        myStreamWriter.Write(dados);
                        myStreamWriter.Flush();
                        dados = string.Empty;
                    }
                    dados = "}\n}";

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
