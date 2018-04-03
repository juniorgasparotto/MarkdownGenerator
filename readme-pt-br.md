[
  ![Inglês](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/MarkdownGenerator)
[
  ![Português](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/readme-pt-br.md)

# Markdown generator

Essa ferramenta é um executável (com algumas dependências) que tenta resolver problemas comuns para quem escreve documentos no formato markdown.

## Instalação (Manual)

1. Fazer o download do .zip [clicando aqui](https://github.com/juniorgasparotto/MarkdownGenerator/releases/download/1.0.1/MarkdownGenerator.zip)
2. Extrair o .zip em qualquer local
3. Executar o arquivo via linha de comando `MarkdownGenerator.exe`

```
MarkdownGenerator.exe --base-dir "C:\my-doc-folder" --index-file "index.xml" --translator-key [api-key] --verbose error
```

**DOWNLOAD LINK**: https://github.com/juniorgasparotto/MarkdownGenerator/releases/download/1.0.1/MarkdownGenerator.zip

_Não precisa de instalação_

## Instalação (Chocolatey)

```
choco install markdown-generator
```

Url: https://chocolatey.org/packages/Markdown-Generator

**Atualização**

```
choco upgrade markdown-generator
```

**Desinstalação**

```
choco uninstall markdown-generator
```

**Observações:**

* Essa ferramenta é suportada apenas no Windows, mas é possível porta-la para outros sistemas operacionais caso tenha muitos pedidos.

# Como usar?

Para usar, especifique o nome do executável no prompt de comando seguido dos argumentos:

* `--index-file`: Esse argumento é obrigatório e deve conter o nome do arquivo de configuração.
* `--base-dir`: Define o diretório base. Caso não seja especificado a pasta corrente será utilizada.
* `--translator-key`: Define a chave de autenticação da API de tradução da Microsoft. Caso não seja especificado, nenhuma tradução será feita.
* `--verbose`: Especifique o valor `error` para mostrar qualquer falha.
* `help`: Exibe o texto de ajuda

Nota: Para gerar uma chave da API de tradução, você precisa se cadastrar no portal do Azure e configurar o serviço cognitivo de tradução. Vale lembrar que esse serviço é pago, mas existem planos gratuítos para experimentos. Consulte o link para mais informações:

https://www.microsoft.com/en-us/translator/getstarted.aspx

**Exemplos de uso:**

```
MarkdownGenerator.exe help
```

```
MarkdownGenerator.exe --base-dir "C:\my-doc-folder" --index-file "index.xml" --translator-key [api-key] --verbose error
```

_Os nomes dos argumentos podem ser omitidos:_

```
MarkdownGenerator.exe "index.xml" "C:\my-doc-folder" [api-key] --verbose error
```

# Quais são os problemas que ela resolve?

É útil para a criação de grandes documento na linguagem MarkDown e tem os seguintes objetivos:

* Separar o conteúdo do seu documento em um ou mais arquivos, facilitando a manutenção. Imagine a situação onde você precisa mover um grande bloco de texto de um lugar para o outro sem nenhum esforço ou risco.
* Dividir seu documento em duas ou mais páginas.
* Criação de âncoras para serem utilizadas em diversos locais mantendo o texto original do momento em que foi criada.
* Gerar índice de forma automática de acordo com a hierarquia do markdown ("#" para h1 e "##" para h2 e etc).
* Tradução automática usando a ferramenta "Microsoft Azure - Translator API".
  * É possível criar blocos de textos que não são traduzivél.
  * É possível criar blocos de textos com uma tradução customizada. Isso é útil em caso de textos muitos especificos onde nenhuma tradução consegue chegar no resultado esperado.

# Arquivo de configuração

Esse arquivo contém todas as configurações do seu documento. Ele é composto, obrigatóriamente, pelas tags:

* documentation (apenas uma)
  * page (uma ou mais)
    * languages (apenas uma)
      * language (uma ou mais)
    * content (apenas uma)

```xml
<documentation>
  <page>
    <languages>
      <language name="pt-br" output="readme-pt-br.md" default="true" url-base="https://github.com/user/project/blob/master/readme-pt-br.md" />
      <language name="en-us" output="readme.md" url-base="https://github.com/user/project" />
    </languages>
    <content>
        <include href="doc/pages/readme/description.md" />
        <include href="doc/pages/readme/install.md" />
        <include href="doc/pages/readme/licence.md" />
    </content>
  </page>

  <page>
    <languages>
      <language name="pt-br" output="doc/pt-br.md" default="true" url-base="https://github.com/user/project/blob/master/doc/pt-br.md" />
      <language name="en-us" output="doc/en.md" url-base="https://github.com/user/project/blob/master/doc/en.md" />
    </languages>
    <content>
        <include href="doc/pages/suject1.md" />
        <include href="doc/pages/suject2.md" />
        <include href="doc/pages/suject3.md" />
    </content>
  </page>
</documentation>
```

**Tag `page`**

É obrigatório que exista no mínimo uma ocorrência dessa tag. Ela é composta pelas tags `languages` e `content`. Ela representa um documento que pode ser traduzido para outros idiomas. Cada versão de idioma equivale a um arquivo físico.

**Tag `languages`**

Para definir um novo idioma, adicione a tag `language` dentro da tag `languages`. É obrigatório que exista no mínimo um idioma configurado e esse idioma deve ser o mesmo do seu contéudo. É obrigatório o uso do atributo `default=true` para o idioma padrão.

Cada tag `language` contem os seguintes atributos:

* `name`: Define a sigla do idioma (https://dev.microsofttranslator.com/languages?api-version=1.0)
* `output`: Define o caminho onde o arquivo final será salvo. A pasta relativa será sempre a pasta que o executavél esta sendo executado, mas você pode mudar esse caminho relativo usando o argumento `--base-dir`.
* `default`: Define qual tag `language` corresponde ao idioma padrão.
* `url-base`: Não é obrigatório, mas é interessante existir para ajudar na criação das âncoras com o caminho absoluto, evitando assim problemas ao usar o método `<anchor-get name="anchor-name" />`. Garantido também a criação de âncoras que estão em outras páginas: `<anchor-get name="anchor-name-other-page" />`.

**Tag `content`**

É o contéudo do seu documento, é altamente aconselhado que seu texto seja separado em outros arquivos usando a tag `include`. Contudo, você ainda pode deixar seu texto de forma direta nessa tag, basta evitar textos que se pareçam com tags de xml, como por exemplo: `Func<T>`.

Essa tag é obrigatório existir.

**Tag `include`**

Essa tag define um conteúdo que esta dentro de outro arquivo. Por convensão use a extensão `.md` para seus arquivos, alguns editores conseguem interpretar essa extensão como sendo `markdown`. Use o atributo `href="file.md"` para carregar um contéudo.

# Âncoras

Para criar uma âncora, basta inserir a tag `anchor-set` em qualquer lugar do seu texto. Essa tag tem o atributo `name` que será utilizado para recupera-la quando necessário. Seu texto deve estar dentro de seu conteúdo.

A tag `anchor-get` é a responsável por recuperar essa âncora em qualquer lugar do seu texto. Essa tag tem o atributo `name` que deve corresponder a uma âncora que foi definida em outro momento. Caso você defina um contéudo, então esse texto será usado em seu link, do contrário o texto da definição da âncora será utilizado. Isso é muito útil para que mudanças sejam feitas em um só lugar.

```xml
Set ancor: <anchor-set name="anchor-name">custom ancor</anchor-set>
Get ancor with original text: <anchor-get name="anchor-name" />
Get ancor with custom text: <anchor-get name="anchor-name">custom text</anchor-get>
```

# Índice automático

Para adicionar um índice, use a tag `<table-of-contents />` no local onde você deseja que o índice apareça. Porém, para que isso funcione, todos os seus cabeçalhos devem ter a tag `header-set` no final da linha. Essa tag contém o atributo obrigatório `name` que adiciona esse cabeçalho na lista de âncoras. Assim você pode usar a tag `anchor-get` para um cabeçalho.

```xml
<table-of-contents />

# Header H1 <header-set name="header-1" />
Text
## Header H2 <header-set name="header-2" />
Text
### Header H3 <header-set name="header-3" />
Text
#### Header H4 <header-set name="header-4" />
Text
##### Header H5 <header-set name="header-5" />
Text
###### Header H6 <header-set name="header-6" />
Text

Get header anchor: <anchor-get name="header-1" />
```

# Tradução customizada

A tag `custom-translation` é útil em caso de textos muitos especificos onde nenhuma tradução consegue chegar no resultado esperado.

```xml
<custom-translation>
    <default>
        Esse texto está em português
    </default>

    <language name="en-us">
        This text is customized for english
    </language>
</custom-translation >
```

# Texto que não pode ser traduzido

Para criar blocos de textos que não podem ser traduzidos, utilize a tag `no-translate`. Por padrão, blocos de código no formato markdown já não são traduzidos.

```xml
<no-translate>This text can't be translated</no-translate >
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