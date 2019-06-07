using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TicTacToeGameWPFClient
{
    public static class RichTextBoxExtention
    {
        public static void AddText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        public static void AddRangeText(this RichTextBox richTextBox, List<string> users)
        {
            if (users.Count>0)
            {
                List<Paragraph> paragraphs = new List<Paragraph>();
                for (int i = 0; i < users.Count; i++)
                {
                    paragraphs.Add(new Paragraph(new Run(users[i])));
                }
                richTextBox.Document.Blocks.AddRange(paragraphs);
            }
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,
                richTextBox.Document.ContentEnd).Text;
        }
    }
}
