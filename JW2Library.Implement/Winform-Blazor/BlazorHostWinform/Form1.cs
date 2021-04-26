using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JWLibrary.Utils;
using Microsoft.Web.WebView2.WinForms;

namespace BlazorHostWinform {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            ProcessSimpleHandler psh = new ProcessSimpleHandler();
            psh.Run("dotnet run", "", @"F:\workspace\JW2Library\JW2Library.Implement\Winform-Blazor\BlazorServer");
            
            var webview2 = new WebView2();
            webview2.Dock = DockStyle.Fill;
            webview2.Source = new Uri("https://localhost:5001/");
            //webview2.NavigateToString();
            this.Controls.Add(webview2);

            this.Load += (s, e) => {

            };

            this.Closed += (s, e) => {
                psh.Stop();
            };            
        }
    }
}