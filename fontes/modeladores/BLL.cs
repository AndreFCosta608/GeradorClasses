using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class BLL:Modelador {
        public void GerarArquivosBLL(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + "BLL.partial.cs";
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
                    dados += "using System.Collections.Generic;\n\n";

                    dados += "namespace " + strNameSpace + "\n{\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tpublic partial class " + formataNomeClasse(tabela) + "BLL\n";
                    dados += "\t{\n";

                    dados += "\t\tprivate " + formataNomeClasse(tabela) + "DAO objDAO;\n\n";
                    dados += "\t\tpublic " + formataNomeClasse(tabela) + "BLL()\n\t\t{\n";
                    dados += "\t\t\tif (objDAO == null)\n\t\t\t\tobjDAO = new " + formataNomeClasse(tabela) + "DAO();\n\t\t}";

                    //select objeto
                    dados += "\n\n\t\tpublic " + formataNomeClasse(tabela) + " Retorna" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{\n";
                    dados += "\t\t\treturn(objDAO.Retorna" + formataNomeClasse(tabela) + "(obj" + formataNomeClasse(tabela) + "));\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //select lista
                    dados += "\n\n\t\tpublic List<" + formataNomeClasse(tabela) + "> Retorna" + formataNomeClasse(tabela) + "()\n\t\t{";
                    dados += "\n\t\t\treturn(objDAO.Retorna" + formataNomeClasse(tabela) + "());\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //select lista
                    dados += "\n\n\t\tpublic List<" + formataNomeClasse(tabela) + "> Retorna" + formataNomeClasse(tabela) + "(string filtro)\n\t\t{";
                    dados += "\n\t\t\treturn(objDAO.Retorna" + formataNomeClasse(tabela) + "(filtro));\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //insert
                    dados += "\n\n\t\tpublic int insere" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";
                    dados += "\n\t\t\treturn(objDAO.insere" + formataNomeClasse(tabela) + "(obj" + formataNomeClasse(tabela) + "));\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //Deleta
                    dados += "\n\n\t\tpublic void exclui" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";
                    dados += "\n\t\t\tobjDAO.exclui" + formataNomeClasse(tabela) + "(obj" + formataNomeClasse(tabela) + ");\n\t\t}";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //Update
                    dados += "\n\n\t\tpublic void atualisa" + formataNomeClasse(tabela) + "(";
                    dados += formataNomeClasse(tabela) + " obj" + formataNomeClasse(tabela) + ")\n\t\t{";
                    dados += "\n\t\t\tobjDAO.atualisa" + formataNomeClasse(tabela) + "(obj" + formataNomeClasse(tabela) + ");\n\t\t}";

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
