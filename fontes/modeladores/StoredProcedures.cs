using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GeraClasses.Conectores;

namespace GeraClasses.modeladores {
    public class StoredProcedure:Modelador {
        public void GerarArquivosSP(string Caminho, DataSet listaTabela, string strNameSpace, IConector Conector) {
            try {
                colecoes objColecao = new colecoes();
                for(int contador = 0; contador < listaTabela.Tables[0].Rows.Count; contador++) {
                    string tabela = listaTabela.Tables[0].Rows[contador][0].ToString();
                    DataSet detalheTabela = RetornaDescricao(tabela, Conector);

                    StreamWriter myStreamWriter = null;
                    string arquivo = Caminho + formataNomeClasse(tabela) + ".sql";
                    myStreamWriter = File.CreateText(arquivo);

                    string dados = string.Empty;

                    dados = "/*\n";
                    dados += objColecao.Assinatura.ToString() + "\n";
                    dados += "*/\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;

                    int contaChaves = 0;


                    // ----- Select todos
                    dados = "CREATE PROCEDURE USP_" + formataNomeClasse(tabela) + "_SLL";

                    dados += "\nAS \n\tBEGIN \n\t\tSELECT ";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                            dados += ", ";
                        }
                    }
                    dados += "\n\t\tFROM " + formataNomeClasse(tabela);

                    contaChaves = 0;
                    dados += "\n\tEND \nGO\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;



                    // ----- Select where
                    dados = "CREATE PROCEDURE USP_" + formataNomeClasse(tabela) + "_SEL";

                    contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            contaChaves = contaChaves + 1;
                        }

                    }
                    bool somenteChaves = true;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(subcontador < (contaChaves - 1)) {
                                dados += ", ";
                            }
                        }
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == " ") {
                            somenteChaves = false;
                        }
                    }

                    dados += "\nAS \n\tBEGIN \n\t\tSELECT ";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += formataNomeClasse(detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString()) + " ";
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                            dados += ", ";
                        }
                    }
                    dados += "\n\t\tFROM " + formataNomeClasse(tabela);

                    contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            if(contaChaves == 0) {
                                dados += "\n\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            } else {
                                dados += "\n\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            }
                            contaChaves = contaChaves + 1;
                        }
                    }
                    contaChaves = 0;
                    dados += "\n\tEND \nGO\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;



                    // ----- Delete
                    dados = "CREATE PROCEDURE USP_" + formataNomeClasse(tabela) + "_DEL";

                    contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            contaChaves = contaChaves + 1;
                        }
                    }

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(subcontador < (contaChaves - 1)) {
                                dados += ", ";
                            }
                        }
                    }

                    dados += "\nAS \n\tBEGIN \n\t\tDELETE FROM " + formataNomeClasse(tabela);

                    contaChaves = 0;
                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                            if(contaChaves == 0) {
                                dados += "\n\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            } else {
                                dados += "\n\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            }
                            contaChaves = contaChaves + 1;
                        }
                    }
                    contaChaves = 0;
                    dados += "\n\tEND \nGO\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;



                    // ----- Insert
                    dados = "CREATE PROCEDURE USP_" + formataNomeClasse(tabela) + "_INS";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI" && somenteChaves) {
                            dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += "\nAS \n\tBEGIN \n\t\tINSERT INTO " + formataNomeClasse(tabela) + " (";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI" && somenteChaves) {
                            dados += detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += ") VALUES (";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                            dados += "@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                        if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI" && somenteChaves) {
                            dados += "@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }
                    dados += ")\n\tEND \nGO\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;



                    // ----- Update
                    dados = "CREATE PROCEDURE USP_" + formataNomeClasse(tabela) + "_UPD";

                    for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                        dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                        if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                            dados += ", ";
                        }
                    }
                    if(somenteChaves) {
                        dados += ", ";
                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            dados += "\n\t@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "Antigo " + detalheTabela.Tables[0].Rows[subcontador]["Type"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                    }

                    if(somenteChaves) {
                        dados += "\nAS \n\tBEGIN\n\n\tDECLARE @teste int; \n\n\t\tSELECT @teste = 1";
                        dados += "\n\t\tSELECT @teste = 0 \n\t\tFROM " + formataNomeClasse(tabela);

                        contaChaves = 0;
                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                                if(contaChaves == 0) {
                                    dados += "\n\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                                } else {
                                    dados += "\n\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                                }
                                contaChaves = contaChaves + 1;
                            }
                        }

                        dados += "\n\n\t\tSELECT @teste = 1 \n\t\tFROM " + formataNomeClasse(tabela);

                        contaChaves = 0;
                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                                if(contaChaves == 0) {
                                    dados += "\n\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "Antigo";
                                } else {
                                    dados += "\n\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "Antigo";
                                }
                                contaChaves = contaChaves + 1;
                            }
                        }

                        dados += "\n\n\t\tIF(@teste = 1) \n\t\tBEGIN";

                        dados += "\n\t\t\tDELETE FROM " + formataNomeClasse(tabela);

                        contaChaves = 0;
                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                                if(contaChaves == 0) {
                                    dados += "\n\t\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "Antigo";
                                } else {
                                    dados += "\n\t\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + "Antigo";
                                }
                                contaChaves = contaChaves + 1;
                            }
                        }

                        dados += "\n\n\t\t\tINSERT INTO " + formataNomeClasse(tabela) + " (";

                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            dados += detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                        dados += ") VALUES (";

                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            dados += "@" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                            if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                dados += ", ";
                            }
                        }
                        dados += ");\n\n\t\tEND";
                    } else {
                        dados += "\nAS \n\tBEGIN \n\t\tUPDATE " + formataNomeClasse(tabela) + " \n\t\tSET";

                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() != "PRI") {
                                dados += "\n\t\t\t" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                                if(subcontador < (detalheTabela.Tables[0].Rows.Count - 1)) {
                                    dados += ", ";
                                }
                            }
                        }
                        contaChaves = 0;
                        for(int subcontador = 0; subcontador < detalheTabela.Tables[0].Rows.Count; subcontador++) {
                            if(detalheTabela.Tables[0].Rows[subcontador]["Key"].ToString() == "PRI") {
                                if(contaChaves == 0) {
                                    dados += "\n\t\tWHERE " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                                } else {
                                    dados += "\n\t\tAND " + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString() + " = @" + detalheTabela.Tables[0].Rows[subcontador]["Field"].ToString();
                                }
                                contaChaves = contaChaves + 1;
                            }
                        }
                    }
                    dados += "\n\tEND \nGO\n\n";

                    myStreamWriter.Write(dados);
                    myStreamWriter.Flush();
                    dados = string.Empty;


                    myStreamWriter.Close();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Erro na geracao do sp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}