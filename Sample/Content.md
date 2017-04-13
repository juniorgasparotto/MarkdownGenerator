# Titulo 1 <header-set anchor-name="title1" />

Meu texto do titulo 1.

_Código em linha:_

`meu código`

*Código em bloco:*

```csharp
meu código em bloco
```

Texto da ancora 6: <anchor-get name="anchor-title6" />

## Titulo 2 <header-set anchor-name="title2" />

Meu texto do titulo 2

Texto do cabeçalho 1: <anchor-get name="title1">ancora customizada</anchor-get>

### Titulo 3 <header-set anchor-name="title3" />

<no-translate>
  # Meu texto que não pode ser traduzido! <header-set anchor-name="no-translate"/>
  ## Sub-titulo <header-set anchor-name="no-translate-sub"/>
  
  `meu código`
  
  ### Sub-titulo 1 <header-set anchor-name="no-translate-sub1"/>
  
  `meu código`

  * Ancora customizado por lingua: <anchor-get name="doc-title2"/>
  * Ancora customizado por lingua com texto customizado: <anchor-get name="doc-title2">texto customizado</anchor-get>
</no-translate>

#### Titulo 4 <header-set anchor-name="title4" />

Meu texto do titulo 4

<custom-translation>
  <default>
    # Aqui tem um texto em Portugues <header-set anchor-name="doc-title2"/>
    * Código `my code`
  </default>

  <language name="en-us">
    # Here is a custom text in English! <header-set anchor-name="doc-title2"/>
  </language>

  <language name="fr">
    # Franch <header-set anchor-name="doc-title2"/>
  </language>
</custom-translation>

## Titulo 2.2 <header-set anchor-name="title5" />

Meu texto do titulo 2.2.

### Titulo 2.3 <header-set anchor-name="title6" />

Meu texto do titulo "2.3". <anchor-set name="anchor-title6">e uma ancora</anchor-set>