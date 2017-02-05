# TitleA {{<header-set anchor-name="a" />}} continue
## TitleB {{<header-set anchor-name="titleb" />}} continue
### TitleC {{<header-set anchor-name="titlec" />}} continue
# TitleD {{<header-set anchor-name="b" />}}

Test Test Test Test Test Test Test

## Title {{<anchor-set name="title2" />}} continue

Test Test Test Test Test {{<anchor-set name="test" text="Test Testss" />}}

### Title 3  {{<anchor-set name="title3" text="my title 3" />}} continue continue 

Test Test Test Test Test Test Test {{<anchor-get name="test" />}}

#### Title 4 {{<anchor-set name="title4" text="my title 4" />}} continue

Test Test Test Test Test Test Test

{{<anchor-get name="title3" />}}
{{<anchor-get name="title4" />}}

