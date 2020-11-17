# GeradorClasses
   Um pequeno utilitário para geração de código a partir de bases de dados.
   Inspirado no “MyGeneration”.
   Desenvolvido em C#.

   Atualmente com suporte aos SGDBs MySql, MS-Sql Server. Suporte parcial ao PostgreSQL(Faltando implementar algumas funções).

   A vantagem em relação ao MyGeneration é que o código de gabarito é escrito em puro C#.
   Funciona se conectando ao banco de dados. Aonde explora tudo que é tabela e gera o código correspondente a cada tabela seguindo 
o gabarito. 
   Gerando desde objetos de transferência, mapeamento O/R, código SQL de CRUD, Telas com validações, entre outros.
