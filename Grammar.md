## SIGMA Grammar Notation
This is the grammar definition document for the Sigma scripting language

The notation of sigma is as follows.
- N : `Non Terminal` represent rules in the grammar
- T : `Terminal` represent the Tokens
- | : Either / Or

### Declarations
There are 

* #### Variable declarations \
 `(T)IDENTIFIER | [(T)CommaToken] :`
                ` (T)EQToken (N)EXP | [(T)CommaToken]`

* ####  Function declaration \
 `(T)func (T)IDENTIFIER ( OPTIONAL(T)IDENTIFIER ) :`
                `(N)STATEMENT `

* #### Type declaration \
 `(T)type (T)IDENTIFIER {` \
    `(T)IDENTIFIER: (N)EXPRESSION  }`

### Statements

- #### IF STATEMENT : \ 
    `(T) if (N)EXPRESSION { (N)STATEMENT } ` \
    `| (T) elif  (N)EXPRESSION { (N)STATEMENT }  ` \
    `| (T) else { (N)STATEMENT }`
- #### FOR STATEMENT : \
    `(T) for (T) IDENTIFIER (T)EQToken (N)Literal ; (N) EXPRESSION ; (T) EXPRESSION ` \
    `(T) for (T) IDENTIFIER (T)EQToken (N)Literal ; (T) RANGE (N) LIST | MAP ` \
    `(T) for (N) EXPRESSION `
- #### PRINT STATEMENT : \
    `(T) print (N) EXPRESSION`

- #### RETURN STATEMENT : \
    `(T) return (N) EXPRESSION`


### Expressions
- Expression : Any activity be it; Unary or Assignment

##### Author
- John Nyingi