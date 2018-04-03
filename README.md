[
  ![Inglês](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/doc/img/en-us.png)
](https://github.com/juniorgasparotto/MarkdownGenerator)
[
  ![Português](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/doc/img/pt-br.png)
](https://github.com/juniorgasparotto/MarkdownGenerator/blob/master/readme-pt-br.md)

# Markdown generator

This tool is an executable (with some dependencies) that tries to solve common problems for people who write with markdown documents.

## Installation (Manual)

1. Download the .zip file [by clicking here](https://github.com/juniorgasparotto/MarkdownGenerator/releases/download/1.0.1/MarkdownGenerator.zip)
2. Extract the .zip anywhere
3. Run the file via the command line`MarkdownGenerator.exe`

```
MarkdownGenerator.exe --base-dir "C:\my-doc-folder" --index-file "index.xml" --translator-key [api-key] --verbose error
```

**DOWNLOAD LINK**: https://github.com/juniorgasparotto/MarkdownGenerator/releases/download/1.0.1/MarkdownGenerator.zip

_No need for installation_

## Installation (Chocolatey)

```
choco install markdown-generator
```

URL: https://chocolatey.org/packages/Markdown-Generator

**Update**

```
choco upgrade markdown-generator
```

**Uninstalling**

```
choco uninstall markdown-generator
```

**Comments:**

* This tool is supported only on Windows, but it is possible to port it to other operating systems if you have many requests.

# How to use?

To use, specify the name of the executable at the command prompt, followed by the arguments:

* `--index-file`: This argument is required and must contain the name of the configuration file.
* `--base-dir`: Sets the base directory. If not specified, the current directory will be used.
* `--translator-key`: Sets the authentication key translation API from Microsoft. If not specified, no translation is made.
* `--verbose`: Specify the value `error` to show any failure.
* `help`: Displays help text

Note: to generate an API key, you need to register on the portal of Azure and set up cognitive service. Remember that this service is paid, but there are free plans for experiments. See the link for more information:

https://www.microsoft.com/en-us/translator/getstarted.aspx

**Examples of use:**

```
MarkdownGenerator.exe help
```

```
MarkdownGenerator.exe --base-dir "C:\my-doc-folder" --index-file "index.xml" --translator-key [api-key] --verbose error
```

_The names of the arguments can be omitted:_

```
MarkdownGenerator.exe "index.xml" "C:\my-doc-folder" [api-key] --verbose error
```

# What are the problems that it solves?

Is useful for the creation of large document on MarkDown language and has the following objectives:

* Separate the contents of your document into one or more files, facilitating maintenance. Imagine the situation where you need to move a large block of text from one place to another without any effort or risk.
* Divide your document into two or more pages.
* Creation of anchors to be used in multiple locations while keeping the original text of the moment in which it was created.
* Generate index automatically according to the hierarchy of markdown ("#" to h1 and "##" to h2 and etc).
* Machine translation by using the tool "Microsoft Azure-Translator API".
  * You can create blocks of text that are not traduzivél.
  * You can create text blocks with a custom translation. This is useful in case of many specific texts where no translation can get the expected result.

# The configuration file

This file contains all the settings for your document. It is composed, compulsorily, by tags:

* documentation (only one)
  * page (one or more)
    * languages (only one)
      * language (one or more)
    * content (only one)

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

**Tag`page`**

It is mandatory that there is at least one occurrence of this tag. It is composed by the tags `languages` and `content` . She represents a document that can be translated into other languages. Each language version is equivalent to a physical file.

**Tag`languages`**

To define a new language, add the tag `language` within the tag `languages` . It is mandatory that there is at least one language configured and that language must be the same as your content. It is obligatory the use of the `default=true` attribute for the default language.

Each tag `language` contains the following attributes:

* `name`: Sets the language abbreviation (https://dev.microsofttranslator.com/languages?api-version=1.0)
* `output`: Sets the path where the final file will be saved. The relative folder will always be the folder that executavél this running, but you can change this relative path using the `--base-dir` argument.
* `default`: Defines which tag `language` corresponds to the default language.
* `url-base`: It is not mandatory, but it is interesting there to help in the creation of the anchors with the absolute path, thus avoiding problems by using the method `<anchor-get name="anchor-name" />` . Guaranteed also the creation of anchors that are in other pages: `<anchor-get name="anchor-name-other-page" />` .

**Tag`content`**

Is the content of your document, it is strongly advised that your text will be separated into other files using the tag `include` . However, you can still leave your text directly in that tag, just avoid texts that look like xml tags, for example: `Func<T>` .

This tag is mandatory there.

**Tag`include`**

This tag defines a content that this within another file. By Convention use the `.md` extension to your files, some publishers can interpret this extension as being `markdown` . Use the `href="file.md"` attribute to load a content.

# Anchors

To create an anchor, simply insert the tag `anchor-set` anywhere in your text. This tag has the `name` attribute that will be used to retrieve it when needed. Your text should be within your content.

The tag `anchor-get` is responsible for retrieving this anchor anywhere in your text. This tag has the `name` attribute that must match an anchor that was set in another time. If you set a content, then this text will be used in your link, otherwise the text of the anchor definition will be used. This is very useful for changes to be made in one place.

```xml
Set ancor: <anchor-set name="anchor-name">custom ancor</anchor-set>
Get ancor with original text: <anchor-get name="anchor-name" />
Get ancor with custom text: <anchor-get name="anchor-name">custom text</anchor-get>
```

# Automatic table of contents

To add an index, use the tag `<table-of-contents />` where you want the index to appear. However, for this to work, all your headers should have the tag `header-set` at the end of the line. This tag contains the required attribute `name` that adds this header on list of anchors. So you can use the tag `anchor-get` for a header.

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

# Customized translation

The tag `custom-translation` is useful in case of many specific texts where no translation can get the expected result.

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

# Text that cannot be translated

To create blocks of text that cannot be translated, use the tag `no-translate` . By default, markdown-formatted code blocks are not translated.

```xml
<no-translate>This text can't be translated</no-translate >
```

# Limitations

It is important to note that this tool can't solve every problem and also contains several problems that must be resolved by the user in the best way possible. Consider changing the format of your text when possible.

The main reasons of these problems are:

* She was based on the standard of github, any other pattern will not work as expected.
* Due to the translation process, some steps of conversions can harm the original format and change the end result. These steps are:
  * Converts the content, that is with markdown to html. This is done because the translation API does not support the markdown format. If anyone knows any tools to do this, let us know that we changed the code to support it. (https://github.com/baynezy/Html2Markdown)
  * Translates the html for the specified language. (https://portal.azure.com/)
  * Converts back to markdown (https://github.com/lunet-io/markdig)
* Not been made complex tests

Note: due to some customizations, the package code "Html2Markdown" and "Markdig" were copied into the project. I thank the creators for the great work.