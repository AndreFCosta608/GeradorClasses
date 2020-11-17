using System;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using GeraClasses.Conectores;
using System.Collections.Generic;

namespace GeraClasses.modeladores {
    public class PHP_Ajax:Modelador {
        public void GerarArquivos(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                
                StreamWriter myStreamWriter = null;
                string arquivo = Caminho + "ajaxPrincipal.js";
                myStreamWriter = File.CreateText(arquivo);

                string dados = string.Empty;
                //dados = "/*\n";
                //dados += objColecao.Assinatura.ToString() + "\n";
                //dados += "*/\n\n";

                myStreamWriter.Write(dados);
                myStreamWriter.Flush();
                dados = string.Empty;

                dados  = " var modoDebug = 0; //1 pra ligado\n\n";
                dados += " // ---- Master Inicio ----\n\n";
                dados += " function logout() {\n ";
	            dados += " \t$.post(\"servico.php\", { acao: 0 }) \n";
		        dados += " \t\t.done(function(data) {\n";
			    dados += " \t\t\twindow.location.assign(\"index.php\");\n";
	            dados += " \t});\n";
                dados += " }\n\n";

                myStreamWriter.Write(dados);
                myStreamWriter.Flush();
                dados = string.Empty;

                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    dados  = " // ---- " + formataNomeClasse(tabela) + " Inicio ----\n";
                    dados += " function populaLista" + formataNomeClasse(tabela) + "() {\n";
	                dados += " \t$.post(\"servico.php\", { acao: 2 })\n";
	                dados += " \t\t.done(function(data) {\n";
		            dados += " \t\t\tif(modoDebug == 1){\n";
                    dados += " \t\t\t\talert('data populaLista" + formataNomeClasse(tabela) + " = ' + data);\n";
		            dados += " \t\t\t}\n";
		            dados += " \t\t\tvar objJson = JSON.parse(data);\n";
		            dados += " \t\t\tvar itens = objJson.lista;\n";
	                dados += " \t\t\tvar lista = document.getElementById(\"lst" + formataNomeClasse(tabela) + "\");\n";
	                dados += " \t\t\twhile(lista.options.length > 0) {\n";
	    	        dados += " \t\t\t\tlista.remove(i);\n";
	                dados += " \t\t\t}\n";
	                dados += " \t\t\tfor(var i = 0; i < itens.length; i++){\n";
	                dados += " \t\t\t\tvar item = itens[i];\n";
	                dados += " \t\t\t\tvar option = document.createElement(\"option\");\n";
	                dados += " \t\t\t\toption.text = decode_utf8(item.descricao);\n";
                    for(int subcontadorA = 0; subcontadorA < detalheTabela.Tables[0].Rows.Count; subcontadorA++) {
                        if(detalheTabela.Tables[0].Rows[subcontadorA]["Key"].ToString() == "PRI") {
                            dados += " \t\t\t\toption.value = item." + formataTabela(detalheTabela.Tables[0].Rows[subcontadorA]["Field"].ToString()) + ";\n";
                        }
                    }
                    dados += " \t\t\t\tlista.add(option);\n";
	                dados += " \t\t\t}\n";
	                dados += " \t\t\tif(lista.length == 0){\n";
	    	        dados += " \t\t\talert('Nenhum registro encontrado.');\n";
	                dados += " \t\t}\n";
	                dados += " \t});\n";
                    dados += " }\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados  = " function carregaMenu" + formataNomeClasse(tabela) + "(){\n";
	                dados += " \t$(\"#divConteudo\").load(\"apresentacao/cadastro_" + formataTabela(tabela) + ".html\", function(){\n";
		            dados += " \t\tpopulaLista" + formataNomeClasse(tabela) + "();\n";
	                dados += " \t});\n";
                    dados += " }\n\n";
                    dados += " function limparTela" + formataNomeClasse(tabela) + "(){\n";
                    dados += " \tvar lista = document.getElementById(\"lst" + formataNomeClasse(tabela) + "\");\n";
                    dados += " \tvar elements = lista.options;\n";
                    dados += " \tif(lista.length > 0) {\n";
                    dados += " \t\tfor(var i = 0; (elements.length > i); i++) {\n";
                    dados += " \t\t\telements[i].selected = false;\n";
                    dados += " \t\t}\n";        
                    dados += " \t}\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                dados += " \tdocument.getElementById(\"txt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\").value = '';\n";
                            }
                            if(tipo.Contains("bit")) {
                                dados += " \tdocument.getElementById(\"chk" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\").checked = 0;\n";
                            }
                        }
                    }
                    dados += " \t}\n\n";                    
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados  = " function salva" + formataNomeClasse(tabela) + "(){\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += " \tvar " + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " = document.getElementById(\"txt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\");\n";                            
                        }
                    }
                    dados += "\tvar mensagem = '';\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(!tipo.Contains("bit")) {
                                dados += " \tif(" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                                if(tipo.Contains("varchar") || tipo.Contains("date")) {
                                    dados += ".value == ''){ \n";
                                }
                                if(tipo.Contains("int") || tipo.Contains("decimal")) {
                                    dados += " == 0){ \n";
                                }
                                dados += "\t\tmensagem += 'Digite o " + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " do " + formataTabela(tabela) + ".\\n';\n";
                                dados += "\t}\n";
                            }
                        }
                    }                    
                    dados += "\tif(mensagem != '') {\n";
    	            dados += "\t\talert(mensagem);\n";
    	            dados += "\t\treturn;\n";
                    dados += "\t}\n";                    
                    dados += "\tvar obj = { ";                
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += " \"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" : " + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(tipo.Contains("varchar") || tipo.Contains("date")) {
                                dados += ".value ";
                            }
                            if(tipo.Contains("bit")) {
                                dados += ".checked ";
                            }
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                                dados += ", ";
                            else
                                dados += "};\n";
                        }                        
                    }
                    dados += " \tvar lista = document.getElementById('lst" + formataNomeClasse(tabela) + "');\n";
                    dados += " \tif(lista.selectedIndex != -1) {\n";
    	            dados += " \t\tvar idSelecionado = lista.options[lista.selectedIndex].value;\n";
    	            dados += " \t\tobj.id = idSelecionado;\n";
                    dados += " \t}\n";
                    dados += " \tvar str = JSON.stringify(obj);\n";
                    dados += " \t$.post(\"servico.php\", { acao: 3, strJson: str })\n";
    	            dados += " \t\t.done(function(data) {\n";
    		        dados += " \t\t\tif(modoDebug == 1) {\n";
                    dados += " \t\t\t\talert('data populaLista" + formataNomeClasse(tabela) + " = ' + data);\n";
			        dados += " \t\t\t}\n";
			        dados += " \t\t\tif(data == '') {\n";
                    dados += " \t\t\tpopulaLista" + formataNomeClasse(tabela) + "();\n";
                    dados += " \t\t\t\tlimparTela" + formataNomeClasse(tabela) + "();\n";
                    dados += " \t\t\t\talert('Registro salvo com sucesso.');\n";
                    dados += " \t\t\t} else {\n";
                    dados += " \t\t\t\talert('Falha ao salvar os dads.');\n";
                    dados += " \t\t\t}\n";
                    dados += " \t});\n";
                    dados += " }\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados += " function carrega" + formataNomeClasse(tabela) + "() {\n";
                    dados += " \tvar lista = document.getElementById('lst" + formataNomeClasse(tabela) + "');\n";
                    dados += " \tif(lista.selectedIndex != -1) {\n";
		            dados += " \t\tvar idSelecionado = lista.options[lista.selectedIndex].value;\n";
    	            dados += " \t\t$.post(\"servico.php\", { acao: 4, id: idSelecionado })\n";
    		        dados += " \t\t\t.done(function(data) {\n";
    			    dados += " \t\t\t\tif(modoDebug == 1) {\n";
                    dados += " \t\t\t\t\talert('data populaLista" + formataNomeClasse(tabela) + " = ' + data);\n";
    			    dados += " \t\t\t\t}\n";
    			    dados += " \t\t\t\tif(data != '') {\n";
    				dados += " \t\t\t\t\tdata = decode_utf8(data);\n";
    				dados += " \t\t\t\t\tvar objJson = JSON.parse(data);\n";
    				
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        string campo = "value";
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(tipo.Contains("bit")) {
                                campo = "checked";
                            }
                            dados += " \t\t\t\t\tdocument.getElementById(\"txt" + formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\")." + campo + " = objJson." + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + ";\n";
                        }
                    }
                    dados += " \t\t\t\t}\n \t\t});\n \t}\n }\n // ---- " + formataNomeClasse(tabela) + " Fim ----\n\n";
                    
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;
                }
                myStreamWriter.Flush();
                myStreamWriter.Close();

            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Erro na geracao do Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
