using System;
using System.Collections.Generic;
using System.Text;

namespace GeraClasses {
    public class colecoes {
        public enum classesDisponiveis {
            BLL,
            Entidade,
            EntidadeHibernate,
            DAO_Hibernate,
            DAO,
            MapeamentoHibernate,
            StoredProcedure,            
            PHP_Ajax,
            PHP_GradeTela,
            PHP_Persitência,
            Arquitetura_Escolar_Persistencia,
            Arquitetura_Escolar_Tela,
            Arquitetura_Escolar_CodeBehind,
            Arquitetura_Escolar_Designer,
            CamadaControlePersistencia
        }

        public enum Servidores {
            Mysql_5 = 1,
            PostGre_8 = 2,
            SQL_SERVER_2005 = 3
        }

        public string versao {
            get { return "0.8"; }
        }

        public string Assinatura {
            get {
                string assinatura = "\t\tAndréCosta -- WindNet\n";
                assinatura += "\t\tWind Net Consultoria em Automação e TI.\n";
                assinatura += "\t\tGerador Classes a partir de base de dados.\n";
                assinatura += "\t\tVersao: " + versao + "\n";
                assinatura += "\t\tAndre Costa -- andre.costa@wind.net.br\n";
                return assinatura;
            }
        }

    }
}
