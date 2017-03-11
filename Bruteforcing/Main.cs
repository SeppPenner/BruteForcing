using System;
using System.Windows.Forms;

public partial class Main : Form
{
    public Main()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        IBruteforcing bruteforce = new Bruteforcing("abcdefghijklmnopqrstuvwxyz0123456789", 1, 3);

        foreach (string result in bruteforce)
            richTextBox1.Text = richTextBox1.Text + Environment.NewLine + result;
    }
}