# Grammar Rules
```
any_char
string_char = ["]
number_char = [0-9]
point_char = [\.]
letter_char = [a-zA-Z]
underscore_char = [_]
new_line_char = [\n]
carriage_return_char = [\r]
single_line_comment_char = [#] 

end_of_line
    : new_line_char carriage_return_char
    | new_line_char
    | carriage_return_char
    ;

single_line_comment
    : single_line_comment_char any_char* end_of_line
    ;

string
    : string_char any_char* string_char
    ;
    
integer
    : number_char+
    ;

decimal
    : number_char+ point_char
    | point_char number_char+
    | number_char+ point_char number_char+
    ;  
    
identifier
    : underscore_char+ letter_char*? number_char*?
    | letter_char+ underscore_char*? number_char*?
    ;

boolean
    : 'true'
    | 'false'
    | 'True'
    | 'False'
    ;

keywords
    : boolean
    | 'if'
    | 'else'
    | 'while'
    ;
    

```

