<table-of-contents/>

<header-set name="title1" heading="1">Titulo 1</header-set>
      
<![CDATA[qualquer valor]]>

<unknow-tag
  attr1="value"
  attr2="value"
  attr3="value">
  Tag que faz parte do documento
</unknow-tag>
      
Meu texto em português com uma ancora aqui "<anchor-set name="readme">ancora para **readme**</anchor-set>" e o texto
continua aqui.

Aqui tem o link para a documentação: <anchor-get name="documentation" />
Aqui tem o link para a documentação com o texto customizado: <anchor-get name="documentation">Texto customizado</anchor-get>
Link para o "titulo 2" da documentação: <anchor-get name="doc-title2" />
Inclusão de conteúdo externo 1:
<include href="Sample/Page1.md" />

Inclusão de conteúdo externo SEM TRADUÇÃO:
<no-translation><include href="Sample/Page1_2.md" /></no-translation>