SudoFont makes it easy to generate bitmap fonts from any Windows font.

The intent is to make it as easy as possible to add low-memory, fast-loading,
compact fonts to any app or game.


Features:

- Packs the characters into a power-of-two image that is as close to square as possible.

- Includes C# source code to load, layout, and display the bitmap font at runtime,
  assuming you're rendering them in a graphics package like DirectX or OpenGL.

- SudoFont will access both the .NET font APIs (in DotNetFontSystem.cs) as well as the 
  older Win32 APIs (in Win32FontSystem.cs). The latter is necessary to access certain fonts.

- Includes kerning information (and sample code to use it).

- Supports text rendering hints (ClearType, Antialias, etc).

- Can render the text with gradients. (Future work is to allow any filters on the text).

- Hover preview: Renders a preview of the current font to the side of the main window so 
  you can hover it over your software to see how it looks without exporting.



Map:

- DotNetFontSystem.cs and Win32FontSystem.cs: code to access Windows fonts themselves
  (In the program, use the Font System menu to alternate between them).

- RuntimeFont class: code to load a font at runtime

- FontLayout class: code to layout characters


