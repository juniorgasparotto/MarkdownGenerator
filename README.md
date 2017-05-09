# Markdown generator

Essa ferramenta é um executável (com algumas dependências) que tenta resolver problemas comuns para quem escreve documentos no formato markdown.

Baixe, descompacte e use:

https://github.com/juniorgasparotto/MarkdownGenerator/tree/master/final/markdown-generator.zip

```
MarkdownGenerator.exe "index.xml" "C:\doc-root" --verbose error
```

Nota: Essa ferramenta é suportada apenas no Windows, mas é possível porta-la para outros sistemas operacionais caso tenha muitos pedidos.

# Quais são os problemas que ela resolve?

É útil para a criação de grandes documento na linguagem MarkDown e tem os seguintes objetivos:

* Separar o conteúdo do seu documento em um ou mais arquivos, facilitando a manutenção. Imagine a situação onde você precisa mover um grande bloco de texto de um lugar para o outro sem nenhum esforço ou risco. 
* Dividir seu documento em duas ou mais páginas.
* Criação de âncoras para serem utilizadas em diversos locais mantendo o texto original do momento em que foi criada. 
* Gerar índice de forma automática de acordo com a hierarquia do markdown ("#" para h1 e "##" para h2 e etc).
* Tradução automática usando a ferramenta "Microsoft Azure - Translator API".
  * É possível criar blocos de textos que não são traduzivél.
  * É possível criar blocos de textos com uma tradução customizada. Isso é útil em caso de textos muitos especificos onde nenhuma tradução consegue chegar no resultado esperado.

# Separar seu documento em diversos arquivos usando a tag `Include`

# Divisão do documentos em mais de uma página

# Âncoras

# Índice automático

# Tradução automática

## Tag `custom-translation`

```xml
<custom-translation>
    <default>
        
    </default>

    <language name="en-us">
        ---
        <sub>This text was translated by a machine</sub>

        https://github.com/juniorgasparotto/MarkdownGenerator
    </language>
</custom-translation>
```

## Tag `no-translate`

```xml
<no-translate>This text can't be translated</no-translate>
```

# Limitações

É importante ressaltar que essa ferramenta não consegue resolver todos os problemas e também contém diversos problemas que devem ser contornados pelo usuário da melhor forma possível. Considere mudar o formato de seu texto quando possível. 

Os principais motivos desses problemas são:

* Ela foi baseada no padrão do github, qualquer outro padrão não funcionará como esperado.
* Devido ao processo de tradução, algumas etapas de conversões podem prejudicar o formato original e alterar o resultado final. Essas etapas são:
  * Converte o contéudo, que está no formato markdown, para html. Isso é feito, pois a API de tradução não suporta o formato markdown. Caso alguem conheça alguma ferramenta que faça isso, nos informe que alteramos o código para suporta-la. (https://github.com/baynezy/Html2Markdown)
  * Traduz o html para a lingua especificada. (https://portal.azure.com/)
  * Converte de volta para markdown (https://github.com/lunet-io/markdig)
* Não foram feitos testes complexos

Nota: Devido a algumas customizações, o código dos pacotes "Html2Markdown" e "Markdig" foram copiados para o projeto. Agradeço aos criadores pelo ótimo trabalho.