namespace GeraClasses
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.btnGerador = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtSchema = new System.Windows.Forms.TextBox();
            this.txtDiretorio = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLocal = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbTipoClasse = new System.Windows.Forms.ComboBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ddlServidorBanco = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSair = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGerador
            // 
            resources.ApplyResources(this.btnGerador, "btnGerador");
            this.btnGerador.Name = "btnGerador";
            this.btnGerador.UseVisualStyleBackColor = true;
            this.btnGerador.Click += new System.EventHandler(this.btnGerador_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtServidor
            // 
            resources.ApplyResources(this.txtServidor, "txtServidor");
            this.txtServidor.Name = "txtServidor";
            // 
            // txtSenha
            // 
            resources.ApplyResources(this.txtSenha, "txtSenha");
            this.txtSenha.Name = "txtSenha";
            // 
            // txtUsuario
            // 
            resources.ApplyResources(this.txtUsuario, "txtUsuario");
            this.txtUsuario.Name = "txtUsuario";
            // 
            // txtSchema
            // 
            resources.ApplyResources(this.txtSchema, "txtSchema");
            this.txtSchema.Name = "txtSchema";
            // 
            // txtDiretorio
            // 
            resources.ApplyResources(this.txtDiretorio, "txtDiretorio");
            this.txtDiretorio.Name = "txtDiretorio";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // btnLocal
            // 
            resources.ApplyResources(this.btnLocal, "btnLocal");
            this.btnLocal.Name = "btnLocal";
            this.btnLocal.UseVisualStyleBackColor = true;
            this.btnLocal.Click += new System.EventHandler(this.btnLocal_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbTipoClasse
            // 
            resources.ApplyResources(this.cmbTipoClasse, "cmbTipoClasse");
            this.cmbTipoClasse.Name = "cmbTipoClasse";
            // 
            // txtNameSpace
            // 
            resources.ApplyResources(this.txtNameSpace, "txtNameSpace");
            this.txtNameSpace.Name = "txtNameSpace";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // ddlServidorBanco
            // 
            resources.ApplyResources(this.ddlServidorBanco, "ddlServidorBanco");
            this.ddlServidorBanco.Name = "ddlServidorBanco";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnSair
            // 
            resources.ApplyResources(this.btnSair, "btnSair");
            this.btnSair.Name = "btnSair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // frmPrincipal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.ddlServidorBanco);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtNameSpace);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbTipoClasse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnLocal);
            this.Controls.Add(this.txtDiretorio);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSchema);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.txtServidor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGerador);
            this.IsMdiContainer = true;
            this.Name = "frmPrincipal";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGerador;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtSchema;
        private System.Windows.Forms.TextBox txtDiretorio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLocal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbTipoClasse;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddlServidorBanco;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FolderBrowserDialog dirDialog;
        private System.Windows.Forms.Button btnSair;
    }
}

