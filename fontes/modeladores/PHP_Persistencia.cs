using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class PHP_Persistencia:Modelador {
        public void GerarArquivosPHP_Persistencia(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataTabela(tabela) + ".inc";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "<?PHP\n";
                    //dados += objColecao.Assinatura.ToString() + "\n";
                    //dados += "*/\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    dados = "\tclass " + formataNomeClasse(tabela) + " {\n\n";                    
                    dados += "\t\tfunction " + formataNomeClasse(tabela) + "() {}\n\n";

                    //retorna lista
                    dados += "\t\tfunction lista($idInstituicao) {\n";
                    dados += "\t\t\t$jsonRetorno = \"\";\n ";
			        dados += "\t\t\ttry {\n";
				    dados += "\t\t\t\t$conec = mysqli_connect (configuracao::$servidor, configuracao::$usuario, configuracao::$senhaBanco, configuracao::$base);\n";
				    dados += "\t\t\t\tif (mysqli_connect_errno()) {\n";
					dados += "\t\t\t\t\treturn(\"Erro no servidor : \" . mysqli_connect_error ());\n";
				    dados += "\t\t\t\t}\n";
                    dados += "\t\t\t\t$sql  = \" select ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1))
                            dados += ", ";
                        else
                            dados += "\";";
                    }
                    dados += "\n\t\t\t\t$sql .= \" FROM " + formataTabela(tabela) + "\";\n"; // where idSobreNos = ". $idInstituicao
                    dados += "\t\t\t\t$jsonRetorno = '{ \"lista\" : [';\n";
                    dados += "\t\t\t\t$result = mysqli_query($conec, $sql);\n";
                    dados += "\t\t\t\twhile ($row = mysqli_fetch_row ($result)) {\n";                                        
                    dados += "\t\t\t\t\t$jsonRetorno .= \'{' . ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "'\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                        dados += "\" : '";
                        string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                        bool plica = false;
                        if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                            plica = true;
                        }
                        if(plica == true) {
                            dados = dados.Remove(dados.Length - 1);
                            dados += "\"'";
                        }                        
                        dados += " . $row [" + subcontador.ToString() + "] . ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                            if(plica == true) {
                                dados += "'\", ' . ";
                            } else {
                                dados += "', ' . ";
                            }
                        }
                        if(subcontador == (detalheTabela.Tables[0].Rows.Count - 1)) {
                            if(plica == true) {
                                dados += " '\"' . ";
                            }
                        }
                    }
                    dados += "'}, ';\n\t\t\t\t}\n";
                    dados += "\t\t\t\t$jsonRetorno .= '] }';\n";
                    dados += "\t\t\t\t$jsonRetorno = str_replace ( ', ]', ' ] ', $jsonRetorno );\n";
                    dados += "\t\t\t\tmysqli_close($conec);\n";
                    dados += "\t\t\t} catch(Exception $ex) {\n";
                    dados += "\t\t\t\treturn($ex->getMessage());\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t\t$jsonRetorno = utf8_encode($jsonRetorno);\n";
                    dados += "\t\t\treturn($jsonRetorno);\n";
                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //retorna item
                    dados = "\n\t\tfunction retorna($id){\n";
                    dados += "\t\t\t$jsonRetorno = \"\";\n";
			        dados += "\t\t\ttry {\n";
				    dados += "\t\t\t\t$conec = mysqli_connect (configuracao::$servidor, configuracao::$usuario, configuracao::$senhaBanco, configuracao::$base);\n";
				    dados += "\t\t\t\tif (mysqli_connect_errno()) {\n";
					dados += "\t\t\t\t\treturn(\"Erro no servidor : \" . mysqli_connect_error ());\n";
				    dados += "\t\t\t\t}\n";
				    dados += "\t\t\t\t$sql  = \" select ";                    
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += "\";";
                    dados += "\n\t\t\t\t$sql .= \" FROM " + formataTabela(tabela) + "\";\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\t\t\t\t$sql .= \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" . $id;";
                        }
                    }
                    dados += "\n\t\t\t\t$result = mysqli_query($conec, $sql);\n";
                    dados += "\t\t\t\t$row = mysqli_fetch_row ($result);\n";
                    dados += "\t\t\t\t$jsonRetorno = '{' . ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            
                            dados += "'\"" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "\" : ' . ";
                            
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            bool plica = false;
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                plica = true;
                            }
                            if(plica == true) {
                                dados = dados.Remove(dados.Length - 4);
                                dados += "\"' . ";
                            }
                            dados += " $row [" + (subcontador - 1).ToString() + "] ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                    dados += " . '\", ' . ";
                                } else {
                                    dados += " . ', ' . ";
                                }
                            }
                            if(subcontador == (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                    dados += " . '\"'";
                                }
                            }
                        }
                    }
                    dados += " . '}';\n";
                    dados += "\t\t\t\tmysqli_close($conec);\n";
                    dados += "\t\t\t} catch(Exception $ex) {\n";
                    dados += "\t\t\t\treturn($ex->getMessage());\n";
                    dados += "\t\t\t}\n";
                    dados += "\t\t\t$jsonRetorno = utf8_encode($jsonRetorno);\n";
                    dados += "\t\t\treturn($jsonRetorno);\n";
                    dados += "\t\t}\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //insert
                    dados = "\n\t\tfunction cadastra($json){\n";
                    dados += "\t\t\ttry {\n";
				    dados += "\t\t\t\t$conec = mysqli_connect (configuracao::$servidor, configuracao::$usuario, configuracao::$senhaBanco, configuracao::$base);\n";
				    dados += "\t\t\t\tif (mysqli_connect_errno()) {\n";
					dados += "\t\t\t\t\treturn(\"Erro no servidor : \" . mysqli_connect_error ());\n";
				    dados += "\t\t\t\t}\n";
				    dados += "\t\t\t\t$sql  = \" insert into " + formataNomeClasse(tabela) + "(";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString());
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += ") values (\";\n";
                    dados += "\t\t\t\t$sql .= ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            bool plica = false;
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                plica = true;
                            }
                            if(plica == true) {
                                dados = dados.Remove(dados.Length - 1);
                                dados += " \"'\" . ";
                            }
                            dados += " $json['" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "'] ";
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                   dados += ". \"', \" . ";
                                } else {
                                    dados += ". \", \" . ";
                                }
                            }
                            if(subcontador == (detalheTabela.Tables[0].Rows.Count - 1)) {
                                if(plica == true) {
                                    dados += " . \"'\" ";
                                }
                            }
                        }
                    }
                    dados += ". \");\";\n";
				    dados += "\t\t\t\t$result = mysqli_query($conec, $sql);\n";
				    dados += "\t\t\t\tmysqli_close($conec);\n";
			        dados += "\t\t\t} catch ( Exception $ex ) {\n";
				    dados += "\t\t\t\treturn($ex->getMessage());\n";
			        dados += "\t\t\t}\n";
		            dados += "\t\t}\n\n";
		
                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    //Atualiza
		            dados = "\t\tfunction atualiza($json){\n";
			        dados += "\t\t\ttry {\n";
				    dados += "\t\t\t\t$conec = mysqli_connect (configuracao::$servidor, configuracao::$usuario, configuracao::$senhaBanco, configuracao::$base);\n";
				    dados += "\t\t\t\tif (mysqli_connect_errno()) {\n";
					dados += "\t\t\t\t\treturn(\"Erro no servidor : \" . mysqli_connect_error ());\n";
				    dados += "\t\t\t\t}\n";
                    dados += "\t\t\t\t$sql  = \" update " + formataNomeClasse(tabela) + " set ";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " = ";
                            string tipo = detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            bool plica = false;
                            if(tipo.Contains("varchar") || tipo.Contains("date") ) {
                                plica = true;
                            }
                            if(plica == true) {
                                dados += "'";
                            }
                            dados += "\" . $json['" + formataTabela(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + "'] . \"";
                            if(plica == true) {
                                dados += "'";
                            }
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += "\";\n";
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\t\t\t\t$sql .= \" WHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = \" . $json['id'] . \";\";\n";
                        }
                    }
                    dados += "\t\t\t\t$result = mysqli_query($conec, $sql);\n";
				    dados += "\t\t\t\tmysqli_close($conec);\n";
			        dados += "\t\t\t} catch ( Exception $ex ) {\n";
				    dados += "\t\t\t\treturn($ex->getMessage());\n";
			        dados += "\t\t\t}\n";
		            dados += "\t\t}\n";
		            dados += "\t}\n";
                    dados += "?>\n";
                    
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
