using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MdXaml;

namespace WpfApp.Pages;
/// <summary>
/// Interaction logic for MarkdownEditorPage.xaml
/// </summary>
public partial class MarkdownEditorPage : Page
{
    private readonly string _sampleMarkdown =
        """"

        # Sample Markdown Document
        
        ## Headers
        
        This document includes various Markdown elements to demonstrate formatting.
        
        ### Sub-header Level 3
        
        ## Lists
        
        ### Unordered List
        - Item 1
        - Item 2
          - Subitem 1
          - Subitem 2
        - Item 3
        
        ### Ordered List
        1. First item
        2. Second item
           1. Subitem 1
           2. Subitem 2
        3. Third item
        
        ## Links and Images
        
        This is a [link to Google](https://www.google.com).
        
        ![Sample Image](https://via.placeholder.com/150)
        
        ## Code Blocks
        
        ### Inline Code
        Here is an inline code: `Console.WriteLine("Hello, World!");`.
        
        ### Block of Code
        ```csharp
        using System;
        
        namespace HelloWorld
        {
            class Program
            {
                static void Main(string[] args)
                {
                    Console.WriteLine("Hello, World!");
                }
            }
        }    
        ```           
        """";

    public MarkdownEditorPage()
    {
        InitializeComponent();

        TextEditor.TextChanged += TextEditorOnTextChanged;
        
        var path = System.IO.Path.Combine("Syntax", "MarkDown-Mode.xshd");
        XmlReader reader = XmlReader.Create(path);
        TextEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        TextEditor.Text = _sampleMarkdown;
        
        

        
    }

    void TextEditorOnTextChanged(object? sender, EventArgs e)
    {
        //DocumentViewer.Document = _engine.Transform(TextEditor.Text);
        //FlowDocument document = _engine.Transform(TextEditor.Text);
        DocumentViewer.Markdown = TextEditor.Text;
    }
}
