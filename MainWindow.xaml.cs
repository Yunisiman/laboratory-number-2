using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Diagnostics;

import requests
from bs4 import BeautifulSoup
import json

def parse_central_bank():
    url = "https://www.cbr.ru/eng/market/forex/"
    response = requests.get(url)
    soup = BeautifulSoup(response.content, 'html.parser')

    # Получение курсов валют
    rates = { }
for row in soup.select('table.data tr'):

    cells = row.find_all('td')
        if len(cells) > 0:
            currency = cells[0].text.strip()
            rate = cells[1].text.strip()
            rates[currency] = rate


    return rates

if __name__ == "__main__":
    rates = parse_central_bank()
    print(json.dumps(rates))
< Window x: Class = "CurrencyRates.MainWindow"
        xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"
        Title = "Курсы валют" Height = "350" Width = "525" >
    < Grid >
        < ListBox Name = "CurrencyListBox" />
    </ Grid >
</ Window >
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Newtonsoft.Json;

namespace CurrencyRates
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCurrencyRates();
        }

        private void LoadCurrencyRates()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python"; // Убедитесь, что Python добавлен в PATH
            start.Arguments = "path_to_your_script/parse_cbr.py"; // Укажите путь к вашему скрипту
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            using (Process process = Process.Start(start))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    DisplayRates(result);
                }
            }
        }

        private void DisplayRates(string output)
        {
            var rates = JsonConvert.DeserializeObject<Dictionary<string, string>>(output);
            foreach (var rate in rates)
            {
                CurrencyListBox.Items.Add($"{rate.Key}: {rate.Value}");
            }
        }
    }
}


