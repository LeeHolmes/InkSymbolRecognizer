# InkSymbolRecognizer
A tool to generate static assemblies that will recognize glyphs / images drawn by the user.

When you launch the program, you will able able to draw symbols / glyphs, as well as a keyword
that they should map to. SymbolRecognizer then lets you export this set to an assembly, which
supports a static "Recognize()" method. This method takes a Microsoft.Ink.Ink instance, and
returns the keyword for the glyph that most closely matches the user input.