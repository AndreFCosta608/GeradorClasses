using System;
using System.IO;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using GeraClasses.Conectores;
using GeraClasses.modeladores;
using System.Collections.Generic;

namespace GeraClasses {
    public partial class frmPrincipal:Form {
        public frmPrincipal() {
            InitializeComponent();
        }

        public IConector Conector;
        public DataSet listaTabelas;

        private void frmPrincipal_Load(object sender, EventArgs e) {
            string[] names = Enum.GetNames(typeof(colecoes.classesDisponiveis));
            cmbTipoClasse.DataSource = names;

            string[] servidores = Enum.GetNames(typeof(colecoes.Servidores));
            ddlServidorBanco.DataSource = servidores;
            txtDiretorio.Text = @"C:\\Deposito\\Downloads\\WindNet\\";
            txtSchema.Text = "biomassa";
            txtSenha.Text = "graboa";
            txtServidor.Text = "127.0.0.1";
            txtUsuario.Text = "root";

        }

        private void gerarArquivos(string schema, string caminho, string nameSpace, IConector Conector) {
            try {
                modeladores.Modelador objModelador = new modeladores.Modelador();
                listaTabelas = objModelador.RetornaDetalhesTabela(Conector, schema);
                if(!Directory.Exists(caminho)) {
                    Directory.CreateDirectory(caminho);
                }
                switch(cmbTipoClasse.SelectedValue.ToString()) {
                    case "DAO_Hibernate":
                    (new GeraClasses.modeladores.DAO_Hibernate()).GerarArquivosDAO_Hibernate(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "DAO":
                    (new GeraClasses.modeladores.DAO()).GerarArquivosDAO(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "EntidadeHibernate":
                    (new GeraClasses.modeladores.EntidadeHibernate()).GerarArquivosEntidadeHibernate(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "Entidade":
                    (new GeraClasses.modeladores.Entidade()).GerarArquivosEntidade(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "BLL":
                    (new GeraClasses.modeladores.BLL()).GerarArquivosBLL(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "MapeamentoHibernate": //"XML_HBM":
                    (new GeraClasses.modeladores.XML_HBM()).GerarArquivosXML_HBM(caminho, listaTabelas, Conector, schema);
                    break;

                    case "StoredProcedure":
                    (new GeraClasses.modeladores.StoredProcedure()).GerarArquivosSP(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "PHP_Persitência":
                    (new GeraClasses.modeladores.PHP_Persistencia()).GerarArquivosPHP_Persistencia(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "PHP_Ajax":
                    (new GeraClasses.modeladores.PHP_Ajax()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "PHP_GradeTela":
                    (new GeraClasses.modeladores.PHP_GradeTela()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "Arquitetura_Escolar_Persistencia":
                    (new GeraClasses.modeladores.Arquitetura_Escolar_Persistencia()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "Arquitetura_Escolar_Tela":
                    (new GeraClasses.modeladores.Arquitetura_Escolar_Tela()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "Arquitetura_Escolar_CodeBehind":
                    (new GeraClasses.modeladores.Arquitetura_Escolar_CodeBehind()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "Arquitetura_Escolar_Designer":
                    (new GeraClasses.modeladores.Arquitetura_Escolar_Designer()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                    case "CamadaControlePersistencia":
                    (new GeraClasses.modeladores.CamadaControlePersistencia()).GerarArquivos(caminho, listaTabelas, nameSpace, Conector);
                    break;

                }
                MessageBox.Show("Arquivos criados com Exito.", "Tudo correto.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message, "Campos sem preencher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLocal_Click(object sender, EventArgs e) {
            if(dirDialog.ShowDialog() == DialogResult.OK) {
                this.txtDiretorio.Text = dirDialog.SelectedPath;
            }
        }

        private void btnGerador_Click(object sender, EventArgs e) {
            string mensagem = string.Empty;
            if(txtSchema.Text == string.Empty)
                mensagem = "Selecione o Schema.\n";

            if(txtSenha.Text == string.Empty)
                mensagem = "Selecione a Senha.\n";

            if(txtServidor.Text == string.Empty)
                mensagem = "Selecione o Servidor.\n";

            if(txtUsuario.Text == string.Empty)
                mensagem = "Selecione o Usuario.\n";

            if(txtDiretorio.Text == string.Empty)
                mensagem = "Selecione o Diretorio Destino.\n";

            if(mensagem != string.Empty) {
                MessageBox.Show(mensagem, "Campos sem preencher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                if(Conector == null) {
                    switch(ddlServidorBanco.SelectedValue.ToString()) {
                        case "Mysql_5": {
                            Conector = new MySqlConnector();
                            break;
                        }
                        case "SQL_SERVER_2005": {
                            Conector = new MSSQLSERVERConnector();
                            break;
                        }
                        case "PostGre_8": {
                            Conector = new PostgreConnector();
                            break;
                        }
                    }
                    Conector.SetaPropert(txtServidor.Text, txtSchema.Text, txtUsuario.Text, txtSenha.Text);
                }
                string nameSpace = ((txtNameSpace.Text.ToString() == string.Empty) ? txtSchema.Text.ToString() : txtNameSpace.Text.ToString());
                gerarArquivos(txtSchema.Text.ToString(), txtDiretorio.Text.ToString(), nameSpace, Conector);
            }

        }

        private void btnSair_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
