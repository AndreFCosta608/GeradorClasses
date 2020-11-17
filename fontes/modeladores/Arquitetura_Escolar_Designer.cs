using System;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class Arquitetura_Escolar_Designer:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".aspx.designer.cs";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;
                    dados = "\n\nnamespace persistencia {\n\n";
                    dados += "\tpublic partial class " + formataNomeClasse(tabela) + " {\n\n";

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
