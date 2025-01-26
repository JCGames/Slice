```antlr
text : .* ;
 
end_of_line
    : \r\n
    | \r
    | \n
    | ';'
    ;

single_line_comment
    : '//' text?
    ;

type
    : 'int'
    | 'decimal'
    ;
    
boolean
    : 'true'
    | 'false'
    | 'True'
    | 'False'
    ;

keywords
    : 'if'
    | 'else'
    | 'while'
    | type
    | boolean
    ;
    
identifier
    : [a-zA-Z_]+[a-zA-Z0-9_]*
    | keywords
    ;

string
    : '"' .* '"'
    ;

integer
    : [0-9]+
    ;
    
decimal
    : ([0-9]+\.*|\.[0-9]+)
    ;
```

