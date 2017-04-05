<table-of-contents/>

<h1 name="title1">Titulo 1</h1>
      
<unknow-tag
  attr1="value"
  attr2="value"
  attr3="value">
  Tag que faz parte do documento
</unknow-tag>

<p>Meu texto em português com uma ancora aqui "<anchor-set name="readme">ancora para **readme**</anchor-set>" e o texto
continua aqui.</p>

<p>Aqui tem o link para a documentação: <anchor-get name="documentation" /></p>
<p>Aqui tem o link para a documentação com o texto customizado: <anchor-get name="documentation">Texto customizado</anchor-get></p>
<p>Link para o "titulo 2" da documentação: <anchor-get name="doc-title2" /></p>
<p>Inclusão de conteúdo externo 1:</p>
<include href="Sample/Page1.md" />

<p>Inclusão de conteúdo externo SEM TRADUÇÃO:</p>
<no-translation>
  <include href="Sample/Page1_2.md" />
</no-translation>