using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class DAO_Hibernate:Modelador {
        public void GerarArquivosDAO_Hibernate(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();

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
                    dados += "using System.Web;\n";
                    dados += "using System.Web.Security;\n";
                    dados += "using System.Web.UI;\n";
                    dados += "using System.Web.UI.WebControls;\n";
                    dados += "using System.Web.UI.WebControls.WebParts;\n";
                    dados += "using System.Web.UI.HtmlControls;\n";
                    dados += "using System.Collections;\n";
                    dados += "using NHibernate;\n";
                    dados += "using NHibernate.Mapping;\n";
                    dados += "using NHibernate.Mapping.Attributes;\n\n\n";
                    dados += "namespace " + strNameSpace + "\n{\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "public partial class " + formataNomeClasse(tabela) + "DAO\n\n";
                    dados += "{\n";
                    dados += "\tpublic " + formataNomeClasse(tabela) + "DAO() {}\n\n";

                    dados += "\tpublic static IList Retorna" + formataNomeClasse(tabela) + "(string column, int maximumRows, int startRowIndex)\n";
                    dados += "\t{\n";
                    dados += "\t\tISession session = NHibernateHelper.GetSession();\n\n";
                    dados += "\t\tstring sql = \"FROM " + formataNomeClasse(tabela) + " AS " + formataTabela(tabela) + "\";\n\n";
                    dados += "\t\t// Sorting\n";
                    dados += "\t\tif (!String.IsNullOrEmpty(column))\n";
                    dados += "\t\t{\n";
                    dados += "\t\t\tsql += \" ORDER BY " + formataTabela(tabela) + ".\" + column;\n";
                    dados += "\t\t}\n";
                    dados += "\t\tIQuery query = session.CreateQuery(sql);\n\n";
                    dados += "\t\t// Paging\n";
                    dados += "\t\tquery.SetFirstResult(startRowIndex);\n";
                    dados += "\t\tquery.SetMaxResults(maximumRows);\n";
                    dados += "\t\treturn query.List();\n";
                    dados += "\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tpublic static void Salva" + formataNomeClasse(tabela) + "(" + formataNomeClasse(tabela) + " " + formataTabela(tabela) + ")\n";
                    dados += "\t{\n";
                    dados += "\t\tISession session = NHibernateHelper.GetSession();\n";
                    dados += "\t\tsession.Save(" + formataTabela(tabela) + ");\n";
                    dados += "\t\tsession.Flush();\n";
                    dados += "\t\tsession.Close();\n";
                    dados += "\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tpublic static " + formataNomeClasse(tabela) + " Retorna" + formataNomeClasse(tabela) + "(int id)\n";
                    dados += "\t{\n";
                    dados += "\t\tISession session = NHibernateHelper.GetSession();\n";
                    dados += "\t\treturn (" + formataNomeClasse(tabela) + ")session.Get(typeof(" + formataNomeClasse(tabela) + "), id);\n";
                    dados += "\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados += "\tpublic static void Exclui" + formataNomeClasse(tabela) + "(" + formataNomeClasse(tabela) + " " + formataTabela(tabela) + ")\n";
                    dados += "\t{\n";
                    dados += "\t\tISession session = NHibernateHelper.GetSession();\n";
                    dados += "\t\tsession.Delete(" + formataTabela(tabela) + ");\n";
                    dados += "\t\tsession.Flush();\n";
                    dados += "\t\tsession.Close();\n";
                    dados += "\t}\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tpublic static void Atualiza" + formataNomeClasse(tabela) + "(" + formataNomeClasse(tabela) + " " + formataTabela(tabela) + ")\n";
                    dados += "\t{\n";
                    dados += "\t\tISession session = NHibernateHelper.GetSession();\n";
                    dados += "\t\tsession.Update(" + formataTabela(tabela) + ");\n";
                    dados += "\t\tsession.Flush();\n";
                    dados += "\t\tsession.Close();\n";
                    dados += "\t}\n";
                    dados += "}\n}";

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
